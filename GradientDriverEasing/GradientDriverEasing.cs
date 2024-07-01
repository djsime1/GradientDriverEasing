using ResoniteModLoader;
using FrooxEngine;
using Elements.Core;
using FrooxEngine.UIX;
using Elements.Assets;

namespace GradientDriverEasing;

public class GradientDriverEasing : ResoniteMod
{
    public override string Name => "GradientDriverEasing";
    public override string Author => "djsime1 / Zenuru";
    public override string Version => "1.1.0";
    public override string Link => "https://github.com/djsime1/GradientDriverEasing";

    public static ModConfiguration? Config;

    [AutoRegisterConfigKey]
    private static readonly ModConfigurationKey<bool> useUnclampedLerp = new("UseUnclampedLerp", "Use unclamped interpolation calculations", () => true);

    [AutoRegisterConfigKey]
    private static readonly ModConfigurationKey<bool> lerpColorByHSV = new("LerpColorByHSV", "Interpolate colors by HSV values instead of RGB", () => false);

    public static bool UseUnclampedLerp => Config!.GetValue(useUnclampedLerp);
    public static bool LerpColorByHSV => Config!.GetValue(lerpColorByHSV);

    public override void OnEngineInit()
    {
        Config = GetConfiguration();
        Config?.Save(true);

        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<bool>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<byte>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<ushort>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<uint>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<ulong>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<sbyte>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<short>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<int>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<long>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<float>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<double>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<decimal>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<char>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<string>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<bool2>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<uint2>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<ulong2>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<int2>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<long2>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<float2>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<double2>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<bool3>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<uint3>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<ulong3>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<int3>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<long3>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<float3>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<double3>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<bool4>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<uint4>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<ulong4>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<int4>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<long4>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<float4>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<double4>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<float2x2>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<double2x2>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<float3x3>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<double3x3>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<float4x4>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<double4x4>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<floatQ>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<doubleQ>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<DateTime>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<TimeSpan>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<color>>(BuildInspectorUI);
        CustomUILib.CustomUILib.AddCustomInspectorAfter<ValueGradientDriver<colorX>>(BuildInspectorUI);
    }

    private static void BuildInspectorUI<T>(ValueGradientDriver<T> instance, UIBuilder ui)
    {
        ui.Text("--- Easing utilities ---");

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
        var temp = new List<GradientPoint<T>>();
        ui.Button("Sort position").LocalPressed += (_, _) =>
        {
            temp.Clear();
            foreach (var p in instance.Points)
            {
                var np = new GradientPoint<T>();
                np.Position = p.Position.Value;
                np.Value = p.Value.Value;
                temp.Add(np);
            }
            temp.Sort((a, b) => a.Position.CompareTo(b.Position));
            instance.Points.Clear();
            temp.ForEach((p) => instance.AddPoint(p.Position, p.Value!));
        };
        ui.Button("-Position").LocalPressed += (_, _) =>
        {
            var max = instance.Points.Max((p) => p.Position.Value);
            foreach (var p in instance.Points)
            {
                p.Position.Value = max - p.Position.Value;
            }
        };
        ui.Button("Subdivide").LocalPressed += (_, _) =>
        {
            if (instance.Points.Count < 2) { return; }
            temp.Clear();
            for (int i = 0; i < instance.Points.Count - 1; i++)
            {
                var p1 = instance.Points[i];
                var p2 = instance.Points[i + 1];
                var p3 = new GradientPoint<T>();
                p3.Position = MathX.Lerp(p1.Position.Value, p2.Position.Value, 0.5f);
                p3.Value = ConfiguredLerp(p1.Value.Value, p2.Value.Value, 0.5f);
                temp.Add(p3);
            }
            foreach (var p in instance.Points)
            {
                var np = new GradientPoint<T>();
                np.Position = p.Position.Value;
                np.Value = p.Value.Value;
                temp.Add(np);
            }
            temp.Sort((a, b) => a.Position.CompareTo(b.Position));
            instance.Points.Clear();
            temp.ForEach((p) => instance.AddPoint(p.Position, p.Value!));
        };
        ui.NestOut();

        ui.Text("--- Easing functions ---");

        // Third row
        var radio = ui.HorizontalLayout(32f);
        var boolSwitcher = radio.Slot.AttachComponent<BooleanSwitcher>();
        ui.Text("Target field").HorizontalAlign.Value = TextHorizontalAlignment.Left;
        ui.ValueRadio("Position", boolSwitcher.ActiveIndex, 0);
        ui.ValueRadio("Value", boolSwitcher.ActiveIndex, 1);
        ui.NestOut();

        // Function buttons
        ui.Style.SupressLayoutElement = true;
        var funcContainer = ui.OverlappingLayout();

        // Position buttons
        var posButtons = ui.VerticalLayout(4f);
        ui.Style.SupressLayoutElement = false;
        CreatePositionEasingButton<T>(instance, ui, EasingFunction.EaseType.Linear, minF.ParsedValue, maxF.ParsedValue);
        ui.Style.SupressLayoutElement = true;
        ui.GridLayout(new float2(240f, ui.Style.MinHeight), float2.One * 4f).ExpandWidthToFit.Value = true;
        foreach (EasingFunction.EaseType easing in PositionEasings)
        {
            CreatePositionEasingButton<T>(instance, ui, easing, minF.ParsedValue, maxF.ParsedValue);
        }
        ui.NestOut();
        ui.NestOut();

        // Value buttons
        var valButtons = ui.VerticalLayout(4f);
        ui.HorizontalLayout(4f);
        ui.Style.SupressLayoutElement = false;
        CreateValueEasingButton<T>(instance, ui, EasingFunction.EaseType.Linear, minF.ParsedValue, maxF.ParsedValue);
        CreateValueEasingButton<T>(instance, ui, EasingFunction.EaseType.Spring, minF.ParsedValue, maxF.ParsedValue);
        ui.Style.SupressLayoutElement = true;
        ui.NestOut();
        ui.GridLayout(new float2(240f, ui.Style.MinHeight), float2.One * 4f).ExpandWidthToFit.Value = true;
        foreach (EasingFunction.EaseType easing in ValueEasings)
        {
            CreateValueEasingButton<T>(instance, ui, easing, minF.ParsedValue, maxF.ParsedValue);
        }
        ui.NestOut();
        ui.NestOut();

        boolSwitcher.Targets.Add().Target = posButtons.Slot.ActiveSelf_Field;
        boolSwitcher.Targets.Add().Target = valButtons.Slot.ActiveSelf_Field;
        ui.Style.SupressLayoutElement = false;

        ui.Spacer(8f);
    }

    private static Button CreatePositionEasingButton<T>(ValueGradientDriver<T> instance, UIBuilder ui, EasingFunction.EaseType easing, Sync<float> min, Sync<float> max)
    {
        string fName = Enum.GetName(typeof(EasingFunction.EaseType), easing);
        EasingFunction.Function fFunc = EasingFunction.GetEasingFunction(easing);
        Button btn = ui.Button(fName);
        btn.LocalPressed += (_, _) =>
        {
            int pCount = instance.Points.Count;
            for (int i = 0; i < pCount; i++)
            {
                instance.Points[i].Position.Value = fFunc(min.Value, max.Value, (float)i / (pCount - 1));
            }
        };
        return btn;
    }

    private static Button CreateValueEasingButton<T>(ValueGradientDriver<T> instance, UIBuilder ui, EasingFunction.EaseType easing, Sync<float> min, Sync<float> max)
    {
        string fName = Enum.GetName(typeof(EasingFunction.EaseType), easing);
        EasingFunction.Function fFunc = EasingFunction.GetEasingFunction(easing);
        Button btn = ui.Button(fName);
        btn.LocalPressed += (_, _) =>
        {
            int pCount = instance.Points.Count;
            for (int i = 0; i < pCount; i++)
            {
                instance.Points[i].Value.Value = ConfiguredLerp(
                    instance.Points.First().Value.Value,
                    instance.Points.Last().Value.Value,
                    fFunc(min.Value, max.Value, instance.Points[i].Position.Value)
                );
            }
        };
        return btn;
    }

    private static T ConfiguredLerp<T>(T a, T b, float ratio)
    {
        if ((typeof(T) == typeof(colorX) || typeof(T) == typeof(color)) && LerpColorByHSV)
        {
            if (a is colorX colorXa && b is colorX colorXb)
            {
                return (T)Convert.ChangeType(HSVLerp(colorXa, colorXb, ratio), typeof(T));
            }
            else if (a is color colora && b is color colorb)
            {
                return (T)Convert.ChangeType(HSVLerp(colora, colorb, ratio), typeof(T));
            }
        }

        if (UseUnclampedLerp)
        {
            return Coder<T>.LerpUnclamped(a, b, ratio);
        }
        else
        {
            return Coder<T>.Lerp(a, b, ratio);
        }
    }

    // TODO: underflow hue if delta is > 180
    private static color HSVLerp(color a, color b, float ratio)
    {
        var hsva = new ColorHSV(a);
        var hsvb = new ColorHSV(b);
        var floata = new float4(hsva.H, hsva.S, hsva.V, hsva.A);
        var floatb = new float4(hsvb.H, hsvb.S, hsvb.V, hsvb.A);
        var floatc = ConfiguredLerp(floata, floatb, ratio);
        var hsvc = new ColorHSV(floatc.X, floatc.Y, floatc.Z, floatc.W);
        return hsvc.ToRGB();
    }

    private static colorX HSVLerp(colorX a, colorX b, float ratio)
    {
        return new colorX(HSVLerp((color)a, (color)b, ratio)).SetProfile(a.Profile);
    }

    private static EasingFunction.EaseType[] PositionEasings =
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

    private static EasingFunction.EaseType[] ValueEasings =
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

class GradientPoint<T>
{
    public float Position;
    public T? Value;
}