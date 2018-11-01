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
            if (args.Length == 0) {
                BenchmarkRunner.Run<BinaryTrees>();
                BenchmarkRunner.Run<Fannkuchredux>();
                BenchmarkRunner.Run<Fasta>();
                BenchmarkRunner.Run<KNucleotide>();
                BenchmarkRunner.Run<Mandelbrot>();
                BenchmarkRunner.Run<RegexRedux> ();
                BenchmarkRunner.Run<RevComp>();
                BenchmarkRunner.Run<SpectralNorm>();
                BenchmarkRunner.Run<NBodyTest>();
                return;
            }
            switch (args [0]) {
                case "BinaryTrees":
                    BenchmarkRunner.Run<BinaryTrees>();
                    break;
                case "Fannkuchredux":
                    BenchmarkRunner.Run<Fannkuchredux>();
                    break;
                case "Fasta":
                    BenchmarkRunner.Run<Fasta>();
                    break;
                case "KNucleotide":
                    BenchmarkRunner.Run<KNucleotide>();
                    break;
                case "Mandelbrot":
                    BenchmarkRunner.Run<Mandelbrot>();
                    break;
                case "RegexRedux":
                    BenchmarkRunner.Run<RegexRedux>();
                    break;
                case "RevComp":
                    BenchmarkRunner.Run<RevComp>();
                    break;
                case "SpectralNorm":
                    BenchmarkRunner.Run<SpectralNorm>();
                    break;
                case "NBodyTest":
                    BenchmarkRunner.Run<NBodyTest>();
                    break;
                default:
                    Console.WriteLine ("No such microbenchmark");
                    break;
            }
        }
    }
}
