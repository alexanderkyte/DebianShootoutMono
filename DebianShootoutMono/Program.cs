using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

using System;

namespace BenchmarkDebianShootout
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<BinaryTrees>();
            BenchmarkRunner.Run<Fannkuchredux>();
            BenchmarkRunner.Run<Fasta>();
            BenchmarkRunner.Run<KNucleotide>();
            BenchmarkRunner.Run<Mandelbrot>();
            BenchmarkRunner.Run<RegexRedux> ();
            BenchmarkRunner.Run<RevComp>();
            BenchmarkRunner.Run<SpectralNorm>();
            BenchmarkRunner.Run<NBodyTest>();

        }
    }
}
