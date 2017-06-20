``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3312788 Hz, Resolution=301.8605 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 |                            Method |          Mean |      Error |     StdDev |    StdErr |           Min |            Q1 |        Median |            Q3 |           Max |       Op/s | Allocated |
 |---------------------------------- |--------------:|-----------:|-----------:|----------:|--------------:|--------------:|--------------:|--------------:|--------------:|-----------:|----------:|
 |                    ParseHeartbeat |   418.6969 ns |  2.8692 ns |  2.6838 ns | 0.6930 ns |   415.1239 ns |   416.5496 ns |   418.8060 ns |   420.6117 ns |   424.9473 ns | 2388362.51 |      0 kB |
 |              ParseExecutionReport | 1,280.9987 ns |  7.9524 ns |  6.6406 ns | 1.8418 ns | 1,271.1018 ns | 1,276.8219 ns | 1,280.2972 ns | 1,284.8122 ns | 1,297.0136 ns |  780640.89 |      0 kB |
 | ParseMarketDataIncrementalRefresh | 3,430.7729 ns | 27.5480 ns | 25.7684 ns | 6.6534 ns | 3,397.1900 ns | 3,415.5450 ns | 3,423.4973 ns | 3,447.3945 ns | 3,477.1702 ns |  291479.51 |      0 kB |
