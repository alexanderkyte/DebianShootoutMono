#!/bin/bash

git submodule update --init --recursive

cd external/BenchmarkDotNet && bash build.sh --skiptests=true && cd -

nuget restore

rm -rf release build

mkdir build

msbuild /property:Configuration=Release /property:OutputPath=`pwd`/build DebianShootoutMono.sln

