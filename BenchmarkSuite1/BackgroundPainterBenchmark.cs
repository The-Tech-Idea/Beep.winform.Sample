using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using TheTechIdea.Beep.Winform.Controls.Styling.BackgroundPainters;
using TheTechIdea.Beep.Winform.Controls.Common;

[SimpleJob(RuntimeMoniker.Net80)]
[MemoryDiagnoser]
public sealed class BackgroundPainterBenchmark
{
    private Bitmap _bmp;
    private Graphics _g;
    private GraphicsPath _path;
    private const int Width = 800;
    private const int Height = 600;

    private MethodInfo _paintMethod;
    private Type _controlStateType;

    [GlobalSetup]
    public void Setup()
    {
        _bmp = new Bitmap(Width, Height);
        _g = Graphics.FromImage(_bmp);
        _g.SmoothingMode = SmoothingMode.AntiAlias;
        _g.InterpolationMode = InterpolationMode.HighQualityBicubic;

        int pad = 20;
        var rect = new Rectangle(pad, pad, Width - pad * 2, Height - pad * 2);
        _path = new GraphicsPath();
        int radius = 20;
        _path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
        _path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
        _path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
        _path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
        _path.CloseFigure();

        // Use reflection to find the Paint method and ControlState enum type in the same assembly
        var asm = typeof(AntDesignBackgroundPainter).Assembly;
        _paintMethod = typeof(AntDesignBackgroundPainter).GetMethod("Paint", BindingFlags.Public | BindingFlags.Static);
        _controlStateType = Array.Find(asm.GetTypes(), t => t.Name == "ControlState");
        if (_paintMethod == null || _controlStateType == null)
            throw new InvalidOperationException("Unable to locate Paint method or ControlState enum via reflection.");
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _g?.Dispose();
        _bmp?.Dispose();
        _path?.Dispose();
    }

    [Benchmark(Baseline = true)]
    public void Paint_AntDesign_Normal_Baseline()
    {
        _paintMethod.Invoke(null, new object[] { _g, _path, BeepControlStyle.AntDesign, null, false, Enum.ToObject(_controlStateType, 0) });
    }

    [Benchmark]
    public void Paint_AntDesign_Hovered()
    {
        _paintMethod.Invoke(null, new object[] { _g, _path, BeepControlStyle.AntDesign, null, false, Enum.ToObject(_controlStateType, 1) });
    }

    [Benchmark]
    public void Paint_AntDesign_Pressed()
    {
        _paintMethod.Invoke(null, new object[] { _g, _path, BeepControlStyle.AntDesign, null, false, Enum.ToObject(_controlStateType, 2) });
    }
}