#!/bin/bash

mkdir release

cd build && \
cp System.Diagnostics.StackTrace.dll System.Xml.XmlDocument.dll System.Xml.XPath.dll System.Linq.Expressions.dll System.Collections.Immutable.dll System.Security.SecureString.dll System.Net.Sockets.dll System.Reflection.dll Microsoft.CodeAnalysis.dll System.ValueTuple.dll System.Diagnostics.Tracing.dll BenchmarkDotNet.dll System.Linq.dll System.Runtime.dll System.Reflection.TypeExtensions.dll System.Runtime.Extensions.dll DebianShootoutMono.exe Microsoft.DotNet.PlatformAbstractions.dll System.Reflection.Metadata.dll System.IO.FileSystem.Primitives.dll System.Xml.ReaderWriter.dll System.Console.dll System.Numerics.Vectors.dll System.IO.FileSystem.dll System.AppContext.dll System.Xml.XPath.XDocument.dll Microsoft.CodeAnalysis.CSharp.dll System.Threading.Tasks.Extensions.dll System.Runtime.InteropServices.dll System.IO.dll System.Security.Principal.Windows.dll Microsoft.DotNet.InternalAbstractions.dll System.Xml.XPath.XmlDocument.dll System.Runtime.CompilerServices.Unsafe.dll System.Data.Common.dll CommandLine.dll System.Threading.Thread.dll System.Diagnostics.FileVersionInfo.dll Microsoft.Win32.Registry.dll System.Runtime.Serialization.Primitives.dll System.Security.AccessControl.dll ../release && \
cd -

cp -r external/FlameGraph ./

# leave release, fixtures, and FlameGraph
rm -rf build build.sh DebianShootoutMono DebianShootoutMono.sln external packages README.md release.sh
