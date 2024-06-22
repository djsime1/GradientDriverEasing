using HarmonyLib;
using ResoniteModLoader;
using System;
using System.Reflection;
using FrooxEngine;
using Elements.Core;
using FrooxEngine.UIX;
using System.Reflection.Emit;
using CustomUILib;
using Elements.Assets;

namespace GradientDriverEasing;

public class GradientDriverEasing : ResoniteMod
{
    public override string Name => "GradientDriverEasing";
    public override string Author => "djsime1";
    public override string Version => "1.0.0";
    public override string Link => "https://github.com/djsime1/GradientDriverEasing";

    public override void OnEngineInit()
    {
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
        ui.Button("01").LocalPressed += (_, _) => {
            minF.ParsedValue.Value = 0f;
            maxF.ParsedValue.Value = 1f;
        };
        ui.Button("Auto").LocalPressed += (_, _) => {
            minF.ParsedValue.Value = instance.Points.Min((p) => p.Position.Value);
            maxF.ParsedValue.Value = instance.Points.Max((p) => p.Position.Value);
        };
        ui.Button("Swap").LocalPressed += (_, _) => {
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
        ui.Button("Sort position").LocalPressed += (_, _) => {
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
            temp.ForEach((p) => instance.AddPoint(p.Position, p.Value));
        };
        ui.Button("-Position").LocalPressed += (_, _) => {
            var max = instance.Points.Max((p) => p.Position.Value);
            foreach (var p in instance.Points)
            {
                p.Position.Value = max - p.Position.Value;
            }
        };
        ui.Button("Subdivide").LocalPressed += (_, _) => {
            if (instance.Points.Count < 2) { return; }
            temp.Clear();
            for (int i = 0; i < instance.Points.Count - 1; i++)
            {
                var p1 = instance.Points[i];
                var p2 = instance.Points[i + 1];
                var p3 = new GradientPoint<T>();
                p3.Position = MathX.Lerp(p1.Position.Value, p2.Position.Value, 0.5f);
                p3.Value = Coder<T>.Lerp(p1.Value.Value, p2.Value.Value, 0.5f);
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
            temp.ForEach((p) => instance.AddPoint(p.Position, p.Value));
        };
        ui.NestOut();

        ui.Text("--- Easing functions ---");

        // Third row
        var radio = ui.HorizontalLayout(32f);
        var targetValues = radio.Slot.AttachComponent<ValueField<int>>();
        ui.Text("Target field").HorizontalAlign.Value = TextHorizontalAlignment.Left;
        ui.ValueRadio("Position", targetValues.Value, 0);
        ui.ValueRadio("Value", targetValues.Value, 1);
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

        var boolSwitcher = funcContainer.Slot.AttachComponent<BooleanSwitcher>();
        boolSwitcher.Targets.Add().Target = posButtons.Slot.ActiveSelf_Field;
        boolSwitcher.Targets.Add().Target = valButtons.Slot.ActiveSelf_Field;
        boolSwitcher.ActiveIndex.DriveFrom(targetValues.Value);
        ui.Style.SupressLayoutElement = false;

        ui.Spacer(8f);
    }

    private static Button CreatePositionEasingButton<T>(ValueGradientDriver<T> instance, UIBuilder ui, EasingFunction.EaseType easing, Sync<float> min, Sync<float> max) {
            string fName = Enum.GetName(typeof(EasingFunction.EaseType), easing);
            EasingFunction.Function fFunc = EasingFunction.GetEasingFunction(easing);
            Button btn = ui.Button(fName);
            btn.LocalPressed += (_, _) => {
                int pCount = instance.Points.Count;
                for (int i = 0; i < pCount; i++)
                {
                    instance.Points[i].Position.Value = fFunc(min.Value, max.Value, (float)i/(pCount-1));
                }
            };
            return btn;
    }

    private static Button CreateValueEasingButton<T>(ValueGradientDriver<T> instance, UIBuilder ui, EasingFunction.EaseType easing, Sync<float> min, Sync<float> max) {
            string fName = Enum.GetName(typeof(EasingFunction.EaseType), easing);
            EasingFunction.Function fFunc = EasingFunction.GetEasingFunction(easing);
            Button btn = ui.Button(fName);
            btn.LocalPressed += (_, _) => {
                int pCount = instance.Points.Count;
                for (int i = 0; i < pCount; i++)
                {
                    instance.Points[i].Value.Value = Coder<T>.Lerp(
                        instance.Points.First().Value.Value,
                        instance.Points.Last().Value.Value,
                        fFunc(min.Value, max.Value, instance.Points[i].Position.Value)
                    );
                }
            };
            return btn;
    }

    private static EasingFunction.EaseType[] PositionEasings = new EasingFunction.EaseType[]
    {
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
    };

    private static EasingFunction.EaseType[] ValueEasings = new EasingFunction.EaseType[]
    {
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
    };
}

class GradientPoint<T>
{
    public float Position;
    public T Value;
}