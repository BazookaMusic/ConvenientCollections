``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19043.1586 (21H1/May2021Update)
AMD Ryzen 7 5800H with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK=6.0.100
  [Host]     : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT
  Job-SFZFDD : .NET 6.0.0 (6.0.21.52210), X64 RyuJIT

InvocationCount=1  UnrollFactor=1  

```
|                    Method |     N |         Mean |      Error |     StdDev |
|-------------------------- |------ |-------------:|-----------:|-----------:|
|       **SystemSortedListGet** |  **1000** |    **29.732 μs** |  **0.8799 μs** |  **2.5387 μs** |
|         LazySortedListGet |  1000 |    26.497 μs |  0.8820 μs |  2.5587 μs |
|       SystemSortedListAdd |  1000 |    21.100 μs |  0.1656 μs |  0.1468 μs |
|         LazySortedListAdd |  1000 |     1.317 μs |  0.0430 μs |  0.1262 μs |
|    SystemSortedListRemove |  1000 |    82.700 μs |  1.1853 μs |  0.9254 μs |
|      LazySortedListRemove |  1000 |    56.582 μs |  1.0927 μs |  1.1221 μs |
| SystemSortedListAddGetMix |  1000 |    52.794 μs |  1.0285 μs |  1.6012 μs |
|   LazySortedListAddGetMix |  1000 |    23.503 μs |  0.9374 μs |  2.7343 μs |
|       **SystemSortedListGet** |  **5000** |   **197.700 μs** |  **3.7754 μs** |  **5.4145 μs** |
|         LazySortedListGet |  5000 |   161.718 μs |  3.1378 μs |  3.4876 μs |
|       SystemSortedListAdd |  5000 |   117.327 μs |  0.8325 μs |  0.7787 μs |
|         LazySortedListAdd |  5000 |     6.214 μs |  0.1240 μs |  0.1099 μs |
|    SystemSortedListRemove |  5000 | 1,366.660 μs | 12.7970 μs | 11.9704 μs |
|      LazySortedListRemove |  5000 | 1,285.000 μs |  7.2826 μs |  6.8122 μs |
| SystemSortedListAddGetMix |  5000 |   280.133 μs |  5.3543 μs |  5.0084 μs |
|   LazySortedListAddGetMix |  5000 |   148.986 μs |  2.7833 μs |  2.4673 μs |
|       **SystemSortedListGet** | **10000** |   **401.444 μs** |  **8.0111 μs** | **14.4457 μs** |
|         LazySortedListGet | 10000 |   316.233 μs |  6.2549 μs |  7.4460 μs |
|       SystemSortedListAdd | 10000 |   243.375 μs |  1.0393 μs |  0.8114 μs |
|         LazySortedListAdd | 10000 |    12.069 μs |  0.1918 μs |  0.1601 μs |
|    SystemSortedListRemove | 10000 | 5,430.029 μs | 23.9472 μs | 21.2286 μs |
|      LazySortedListRemove | 10000 | 5,168.136 μs | 85.8415 μs | 76.0962 μs |
| SystemSortedListAddGetMix | 10000 |   582.930 μs | 11.5341 μs | 16.1692 μs |
|   LazySortedListAddGetMix | 10000 |   329.536 μs |  6.5805 μs |  8.7847 μs |
