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

            if (args [0] == "Run") {
                switch (args [1]) {
                    case "BinaryTrees":
                        new BinaryTrees ().Run ();
                        break;
                    case "Fannkuchredux":
                        new Fannkuchredux ().Run ();
                        break;
                    case "Fasta":
                        new Fasta ().Run ();
                        break;
                    case "KNucleotide":
                        new KNucleotide ().Run ();
                        break;
                    case "Mandelbrot":
                        new Mandelbrot ().Run ();
                        break;
                    case "RegexRedux":
                        new RegexRedux ().Run ();
                        break;
                    case "RevComp":
                        new RevComp ().Run ();
                        break;
                    case "SpectralNorm":
                        new SpectralNorm ().Run ();
                        break;
                    case "NBodyTest":
                        new NBodyTest ().Run ();
                        break;
                    default:
                        Console.WriteLine ("No such microbenchmark");
                        break;
                }
            } else {
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
                    case "GistBenchmark":
                        BenchmarkRunner.RunUrl (args [1]);
                        break;
                    default:
                        Console.WriteLine ("No such microbenchmark");
                        break;
                }
            }
        }
    }
}
