using System;
using System.IO;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Diagnosers;
using TheTechIdea.Beep.Winform.Controls;

// Benchmark measures the cost of ApplyThemeToSvg on a BeepImage with a loaded SVG
[SimpleJob(RuntimeMoniker.Net80)]
[MemoryDiagnoser]
public class BeepImageBenchmark
{
    private BeepImage _img;
    private string _tempSvgPath;

    [GlobalSetup]
    public void Setup()
    {
        _img = new BeepImage();
        // Create a moderately complex SVG content for the benchmark
        string svgContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" + "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"200\" height=\"200\">\n" + " <rect x=\"10\" y=\"10\" width=\"180\" height=\"180\" fill=\"#cccccc\" stroke=\"#333\" stroke-width=\"2\"/>\n" + " <circle cx=\"100\" cy=\"100\" r=\"60\" fill=\"#ff0000\" />\n" + " <g>\n" + " <path d=\"M20,180 L180,20\" stroke=\"#00ff00\" stroke-width=\"4\" />\n" + " <text x=\"100\" y=\"100\" font-size=\"14\" fill=\"#000\">Hello</text>\n" + " </g>\n" + "</svg>\n";
        // Write to a temp file because BeepImage.LoadSvg expects a file path
        _tempSvgPath = Path.Combine(Path.GetTempPath(), $"beep_benchmark_{Guid.NewGuid()}.svg");
        File.WriteAllText(_tempSvgPath, svgContent);
        // Load the svg into the control instance
        _img.LoadSvg(_tempSvgPath);
        // Ensure ApplyThemeOnImage is enabled to exercise the theming path
        _img.ApplyThemeOnImage = true;
        // Also set some theme-like properties to make the method exercise color branches
        _img.ForeColor = System.Drawing.Color.DarkBlue;
        _img.BackColor = System.Drawing.Color.White;
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        try
        {
            if (File.Exists(_tempSvgPath))
                File.Delete(_tempSvgPath);
        }
        catch
        {
        }
    }

    [Benchmark]
    public void ApplyThemeToSvg_Baseline()
    {
        _img.ApplyThemeToSvg();
    }
}