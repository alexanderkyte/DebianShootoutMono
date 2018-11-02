#!/bin/bash

git submodule update --init --recursive

cd external/BenchmarkDotNet && bash build.sh --skiptests=true && cd -

nuget restore

rm -rf release build

mkdir build

msbuild /property:Configuration=Release /property:OutputPath=`pwd`/build DebianShootoutMono.sln

mkdir release

cd build && cp CommandLine.dll BenchmarkDotNet.dll DebianShootoutMono.exe Microsoft.CodeAnalysis.CSharp.dll Microsoft.CodeAnalysis.dll Microsoft.DotNet.InternalAbstractions.dll Microsoft.DotNet.PlatformAbstractions.dll System.Collections.Immutable.dll System.Numerics.Vectors.dll System.Reflection.Metadata.dll System.Reflection.TypeExtensions.dll System.Reflection.dll System.Runtime.CompilerServices.Unsafe.dll System.Runtime.Extensions.dll System.Runtime.InteropServices.dll System.Runtime.dll System.Threading.Tasks.Extensions.dll System.Threading.Thread.dll System.ValueTuple.dll ../release  && cd -


