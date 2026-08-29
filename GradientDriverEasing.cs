using System.Reflection;
using Elements.Core;
using FrooxEngine;
using FrooxEngine.UIX;
using HarmonyLib;
using ResoniteModLoader;

namespace GradientDriverEasing;

public class GradientDriverEasing : ResoniteMod
{
    public override string Name => "GradientDriverEasing";
    public override string Author => "djsime1 / Zenuru";
    public override string Version => "1.3.0";
    public override string Link => "https://github.com/djsime1/GradientDriverEasing";

    public static ModConfiguration Config;

    [AutoRegisterConfigKey]
    private static readonly ModConfigurationKey<bool> enable = new("Enable", "Enable/disable the mod", () => true);
    public static bool Config_Enable => Config.GetValue(enable);

    [AutoRegisterConfigKey]
    private static readonly ModConfigurationKey<bool> useUnclampedLerp = new("UseUnclampedLerp", "Use unclamped interpolation calculations", () => true);
    public static bool Config_UseUnclampedLerp => Config.GetValue(useUnclampedLerp);

    [AutoRegisterConfigKey]
    private static readonly ModConfigurationKey<bool> lerpColorByHSV = new("LerpColorByHSV", "Interpolate colors by HSV values instead of RGB", () => false);
    public static bool Config_LerpColorByHSV => Config.GetValue(lerpColorByHSV);

    [AutoRegisterConfigKey]
    private static readonly ModConfigurationKey<bool> collapseInspectorSections = new("CollapseInspectorSections", "Collapse injected inspector sections by default", () => false);
    public static bool Config_CollapseInspectorSections => Config.GetValue(collapseInspectorSections);

    public override void OnEngineInit()
    {
        Harmony harmony = new("je.dj.GradientDriverEasing");
        Config = GetConfiguration()!;
        Config.Save(true);
        harmony.PatchAll();
    }
    
    static readonly MethodInfo BuildInspectorUIMethod = AccessTools.DeclaredMethod(typeof(GradientDriverEasing), nameof(BuildInspectorUI));

    [HarmonyPatch(typeof(WorkerInspector))]
    class GradientDriverEasingPatches
    {
        [HarmonyPatch(nameof(WorkerInspector.BuildInspectorUI))]
        [HarmonyPostfix]
        public static void WorkerInspector_BuildInspectorUI_Postfix(Worker worker, UIBuilder ui)
        {
            var workerType = worker.GetType();
            if (!Config_Enable ||
                !workerType.IsConstructedGenericType ||
                workerType.GetGenericTypeDefinition() != typeof(ValueGradientDriver<>)
            ) return;

            var genericType = workerType.GetGenericArguments()[0];
            BuildInspectorUIMethod.MakeGenericMethod(genericType).Invoke(null, [worker, ui]);
            // ValueGradientDriver.IsValidGenericType constrains to lerp-able types already
        }
    }

    internal static void UISection(UIBuilder ui, string label) {
            ui.Style.MinHeight = 24f;
            ui.Text(label).Color.Value = RadiantUI_Constants.Hero.CYAN;
            ui.Style.MinHeight = 2f;
            ui.Image(RadiantUI_Constants.Hero.CYAN);
            ui.Style.MinHeight = 24f;
    }

    internal static void BuildInspectorUI<T>(ValueGradientDriver<T> instance, UIBuilder ui)
    {
        if (Config_CollapseInspectorSections)
        {
            ui.Style.SupressLayoutElement = true;
            var header = ui.VerticalLayout(6f);
            ui.Style.SupressLayoutElement = false;
            UISection(ui, "Gradient Driver Easing (Mod)");
            ui.Button("Show easing utilities and functions").SetupToggle(header.Slot.ActiveSelf_Field, null, null);
            
            ui.NestOut();
            ui.Style.SupressLayoutElement = true;
            var collapse = ui.VerticalLayout(6f);
            ui.Style.SupressLayoutElement = false;
            collapse.Slot.ActiveSelf_Field.DriveInverted(header.Slot.ActiveSelf_Field);
        }
        
        UISection(ui, "Easing utilities");

        // First row
        ui.HorizontalLayout(4f);
        var minF = ui.HorizontalElementWithLabel("Min position", 0.66f, () => ui.FloatField());
        var maxF = ui.HorizontalElementWithLabel("Max position", 0.66f, () => ui.FloatField());
        ui.HorizontalLayout(4f);
        ui.Button("01").LocalPressed += (_, _) =>
        {
            minF.ParsedValue.Value = 0f;
            maxF.ParsedValue.Value = 1f;
        };
        ui.Button("Auto").LocalPressed += (_, _) =>
        {
            minF.ParsedValue.Value = instance.Points.Min((p) => p.Position.Value);
            maxF.ParsedValue.Value = instance.Points.Max((p) => p.Position.Value);
        };
        ui.Button("Swap").LocalPressed += (_, _) =>
        {
            var oldMin = minF.ParsedValue.Value;
            var oldMax = maxF.ParsedValue.Value;
            minF.ParsedValue.Value = oldMax;
            maxF.ParsedValue.Value = oldMin;
        };
        ui.NestOut();
        if (instance.Points.Count == 0)
        {
            minF.ParsedValue.Value = 0f;
            maxF.ParsedValue.Value = 1f;
        }
        else
        {
            minF.ParsedValue.Value = instance.Points.Min((p) => p.Position.Value);
            maxF.ParsedValue.Value = instance.Points.Max((p) => p.Position.Value);
        }
        ui.NestOut();

        // Second row
        ui.HorizontalLayout(4f);
        var temp = new List<(float, T)>();
        ui.Button("Sort position").LocalPressed += (_, _) =>
        {
            temp.Clear();
            foreach (var p in instance.Points) temp.Add((p.Position.Value, p.Value.Value));
            temp.Sort((a, b) => a.Item1.CompareTo(b.Item1));
            instance.Points.Clear();
            temp.ForEach((p) => instance.AddPoint(p.Item1, p.Item2));
        };
        ui.Button("-Position").LocalPressed += (_, _) =>
        {
            var max = instance.Points.Max((p) => p.Position.Value);
            foreach (var p in instance.Points) p.Position.Value = max - p.Position.Value;
        };
        ui.Button("Subdivide").LocalPressed += (_, _) =>
        {
            if (instance.Points.Count < 2) { return; }
            temp.Clear();
            for (int i = 0; i < instance.Points.Count - 1; i++)
            {
                var p1 = instance.Points[i];
                var p2 = instance.Points[i + 1];
                var p3 = (MathX.Lerp(p1.Position.Value, p2.Position.Value, 0.5f), ConfiguredLerp(p1.Value.Value, p2.Value.Value, 0.5f));
                temp.Add(p3);
            }
            foreach (var p in instance.Points) temp.Add((p.Position.Value, p.Value.Value));
            temp.Sort((a, b) => a.Item1.CompareTo(b.Item1));
            instance.Points.Clear();
            temp.ForEach((p) => instance.AddPoint(p.Item1, p.Item2));
        };
        ui.NestOut();

        UISection(ui, "Easing functions");

        // Third row
        var radio = ui.HorizontalLayout(32f);
        var boolSwitcher = radio.Slot.AttachComponent<BooleanSwitcher>();
        ui.Text("Target field", true, Alignment.MiddleLeft);
        ui.ValueRadio("Position", boolSwitcher.ActiveIndex, 0);
        if (!IsFakeLerpType<T>()) ui.ValueRadio("Value", boolSwitcher.ActiveIndex, 1);
        ui.NestOut();

        // Function buttons
        ui.Style.SupressLayoutElement = true;
        var funcContainer = ui.OverlappingLayout();

        // Position buttons
        var posButtons = ui.VerticalLayout(4f);
        ui.Style.SupressLayoutElement = false;
        CreatePositionEasingButton(instance, ui, EasingFunction.EaseType.Linear, minF.ParsedValue, maxF.ParsedValue);
        ui.Style.SupressLayoutElement = true;
        ui.GridLayout(new float2(240f, ui.Style.MinHeight), float2.One * 4f).ExpandWidthToFit.Value = true;
        foreach (EasingFunction.EaseType easing in PositionEasings) CreatePositionEasingButton(instance, ui, easing, minF.ParsedValue, maxF.ParsedValue);
        ui.NestOut();
        ui.NestOut();

        // Value buttons
        var valButtons = ui.VerticalLayout(4f);
        ui.HorizontalLayout(4f);
        ui.Style.SupressLayoutElement = false;
        CreateValueEasingButton(instance, ui, EasingFunction.EaseType.Linear, minF.ParsedValue, maxF.ParsedValue);
        CreateValueEasingButton(instance, ui, EasingFunction.EaseType.Spring, minF.ParsedValue, maxF.ParsedValue);
        ui.Style.SupressLayoutElement = true;
        ui.NestOut();
        ui.GridLayout(new float2(240f, ui.Style.MinHeight), float2.One * 4f).ExpandWidthToFit.Value = true;
        foreach (EasingFunction.EaseType easing in ValueEasings) CreateValueEasingButton(instance, ui, easing, minF.ParsedValue, maxF.ParsedValue);
        ui.NestOut();
        ui.NestOut();

        boolSwitcher.Targets.Add().Target = posButtons.Slot.ActiveSelf_Field;
        boolSwitcher.Targets.Add().Target = valButtons.Slot.ActiveSelf_Field;
        ui.Style.SupressLayoutElement = false;

        if (Config_CollapseInspectorSections)
        {
            ui.NestOut();
        }

        ui.Spacer(8f);
    }

    internal static Button CreatePositionEasingButton<T>(ValueGradientDriver<T> instance, UIBuilder ui, EasingFunction.EaseType easing, Sync<float> min, Sync<float> max)
    {
        string fName = Enum.GetName(easing)!;
        EasingFunction.Function fFunc = EasingFunction.GetEasingFunction(easing);
        Button btn = ui.Button(fName);
        btn.LocalPressed += (_, _) =>
        {
            int pCount = instance.Points.Count;
            for (int i = 0; i < pCount; i++) instance.Points[i].Position.Value = fFunc(min.Value, max.Value, (float)i / (pCount - 1));
        };
        return btn;
    }

    internal static Button CreateValueEasingButton<T>(ValueGradientDriver<T> instance, UIBuilder ui, EasingFunction.EaseType easing, Sync<float> min, Sync<float> max)
    {
        string fName = Enum.GetName(easing)!;
        EasingFunction.Function fFunc = EasingFunction.GetEasingFunction(easing);
        Button btn = ui.Button(fName);
        btn.LocalPressed += (_, _) =>
        {
            int pCount = instance.Points.Count;
            for (int i = 0; i < pCount; i++)
            {
                instance.Points[i].Value.Value = ConfiguredLerp(
                    instance.Points[0].Value.Value,
                    instance.Points[^1].Value.Value,
                    fFunc(min.Value, max.Value, instance.Points[i].Position.Value)
                );
            }
        };
        return btn;
    }

    internal static T ConfiguredLerp<T>(T a, T b, float ratio)
    {
        if (Config_LerpColorByHSV && (typeof(T) == typeof(colorX) || typeof(T) == typeof(color)))
        {
            switch (a)
            {
                case colorX colorXa when b is colorX colorXb:
                    return (T)(object)HSVLerp(colorXa, colorXb, ratio);
                case color colora when b is color colorb:
                    return (T)(object)HSVLerp(colora, colorb, ratio);
            }
        }

        return Config_UseUnclampedLerp ? Coder<T>.LerpUnclamped(a, b, ratio) : Coder<T>.Lerp(a, b, ratio);
    }

    internal static color HSVLerp(color a, color b, float ratio)
    {
        var hsva = new ColorHSV(a);
        var hsvb = new ColorHSV(b);
        var floata = new float4(hsva.H, hsva.S, hsva.V, hsva.A);
        var floatb = new float4(MathX.Abs(hsvb.H - hsva.H) > 0.5f ? 1 - hsvb.H : hsvb.H, hsvb.S, hsvb.V, hsvb.A);
        var floatc = ConfiguredLerp(floata, floatb, ratio);
        var hsvc = new ColorHSV(MathX.Repeat01(floatc.X), floatc.Y, floatc.Z, floatc.W);
        return hsvc.ToRGB();
    }

    internal static colorX HSVLerp(colorX a, colorX b, float ratio) => new colorX(HSVLerp((color)a, (color)b, ratio)).SetProfile(a.Profile);

    internal static bool IsFakeLerpType<T>() => FakeLerpTypes.Contains(typeof(T));

    internal static Type[] FakeLerpTypes =
    [
        typeof(bool),
        typeof(char),
        typeof(string),
        typeof(bool2),
        typeof(bool3),
        typeof(bool4)
    ];

    internal static EasingFunction.EaseType[] PositionEasings =
    [
        EasingFunction.EaseType.EaseInQuad,
        EasingFunction.EaseType.EaseOutQuad,
        EasingFunction.EaseType.EaseInOutQuad,
        EasingFunction.EaseType.EaseInCubic,
        EasingFunction.EaseType.EaseOutCubic,
        EasingFunction.EaseType.EaseInOutCubic,
        EasingFunction.EaseType.EaseInQuart,
        EasingFunction.EaseType.EaseOutQuart,
        EasingFunction.EaseType.EaseInOutQuart,
        EasingFunction.EaseType.EaseInQuint,
        EasingFunction.EaseType.EaseOutQuint,
        EasingFunction.EaseType.EaseInOutQuint,
        EasingFunction.EaseType.EaseInSine,
        EasingFunction.EaseType.EaseOutSine,
        EasingFunction.EaseType.EaseInOutSine,
        EasingFunction.EaseType.EaseInExpo,
        EasingFunction.EaseType.EaseOutExpo,
        EasingFunction.EaseType.EaseInOutExpo,
        EasingFunction.EaseType.EaseInCirc,
        EasingFunction.EaseType.EaseOutCirc,
        EasingFunction.EaseType.EaseInOutCirc
    ];

    internal static EasingFunction.EaseType[] ValueEasings =
    [
        EasingFunction.EaseType.EaseInQuad,
        EasingFunction.EaseType.EaseOutQuad,
        EasingFunction.EaseType.EaseInOutQuad,
        EasingFunction.EaseType.EaseInCubic,
        EasingFunction.EaseType.EaseOutCubic,
        EasingFunction.EaseType.EaseInOutCubic,
        EasingFunction.EaseType.EaseInQuart,
        EasingFunction.EaseType.EaseOutQuart,
        EasingFunction.EaseType.EaseInOutQuart,
        EasingFunction.EaseType.EaseInQuint,
        EasingFunction.EaseType.EaseOutQuint,
        EasingFunction.EaseType.EaseInOutQuint,
        EasingFunction.EaseType.EaseInSine,
        EasingFunction.EaseType.EaseOutSine,
        EasingFunction.EaseType.EaseInOutSine,
        EasingFunction.EaseType.EaseInExpo,
        EasingFunction.EaseType.EaseOutExpo,
        EasingFunction.EaseType.EaseInOutExpo,
        EasingFunction.EaseType.EaseInCirc,
        EasingFunction.EaseType.EaseOutCirc,
        EasingFunction.EaseType.EaseInOutCirc,
        EasingFunction.EaseType.EaseInBounce,
        EasingFunction.EaseType.EaseOutBounce,
        EasingFunction.EaseType.EaseInOutBounce,
        EasingFunction.EaseType.EaseInBack,
        EasingFunction.EaseType.EaseOutBack,
        EasingFunction.EaseType.EaseInOutBack,
        EasingFunction.EaseType.EaseInElastic,
        EasingFunction.EaseType.EaseOutElastic,
        EasingFunction.EaseType.EaseInOutElastic,
    ];
}