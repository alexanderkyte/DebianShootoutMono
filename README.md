
Purpose:

These microbenchmarks are good indicators of performance problems which are due to fundamental
bits of the language infrastructure. They are also a tongue-in-cheek method of comparing languages
that otherwise would be difficult to compare.

We hope to use it to identify areas where our use of LLVM generates code which is inferior to what
another llvm-using language can generate.

Attribution:

The benchmarks in questions are taken from submissions to a set of common microbenchmarks known as
the "Debian shootout".

It may be periodically worthwhile to check that we have the fastest submission to each benchmark
from the below source:

https://benchmarksgame-team.pages.debian.net/benchmarksgame/measurements/csharpcore.html
