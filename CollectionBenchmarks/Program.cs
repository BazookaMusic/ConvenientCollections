﻿// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using ConvenientCollections.Benchmarks;

var summary = BenchmarkRunner.Run<LazySortedListBenchmark>();
