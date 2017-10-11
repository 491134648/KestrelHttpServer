// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Reflection;
using BenchmarkDotNet.Running;

namespace Microsoft.AspNetCore.Server.Kestrel.Performance
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //BenchmarkSwitcher.FromAssembly(typeof(Program).GetTypeInfo().Assembly).Run(args);

            var benchmark = new RequestParsingBenchmark();
            benchmark.Setup();
            for(var i = 0; i < 10240; i += 1)
            {
                benchmark.PlaintextTechEmpower();
            }
        }
    }
}
