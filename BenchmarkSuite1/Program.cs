using System;
using BenchmarkDotNet.Running;

namespace BenchmarkSuite1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Run TreesRenderBenchmark when provided as argument
            if (args.Length > 0 && args[0].IndexOf("TreesRenderBenchmark", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                BenchmarkRunner.Run<TreesRenderBenchmark>();
                return;
            }

            var summary = BenchmarkRunner.Run<BeepImageBenchmark>();
        }
    }
}
