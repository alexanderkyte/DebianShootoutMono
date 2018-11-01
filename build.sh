#!/bin/bash

git submodule update --init --recursive

cd external/BenchmarkDotNet && bash build.sh --skiptests=true && cd -

nuget restore

rm -rf release

mkdir release

msbuild /property:Configuration=Release /property:OutputPath=`pwd`/release DebianShootoutMono.sln
