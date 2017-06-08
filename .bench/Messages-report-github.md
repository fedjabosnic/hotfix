``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3316022 Hz, Resolution=301.5662 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  Job-LJQWSI : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0

InvocationCount=1000  LaunchCount=1  RunStrategy=Throughput  
TargetCount=10  UnrollFactor=1  WarmupCount=5  

```
 |                            Method |          Mean |       Error |     StdDev |     StdErr |           Min |            Q1 |        Median |            Q3 |           Max |       Op/s | Allocated |
 |---------------------------------- |--------------:|------------:|-----------:|-----------:|--------------:|--------------:|--------------:|--------------:|--------------:|-----------:|----------:|
 |                    ParseHeartbeat |   420.1520 ns |  12.8292 ns |  8.4857 ns |  2.6834 ns |   405.4658 ns |   417.5284 ns |   420.6948 ns |   427.1785 ns |   430.1942 ns | 2380090.91 |      0 kB |
 |              ParseExecutionReport | 1,204.2877 ns |  31.3471 ns | 18.6542 ns |  6.2181 ns | 1,181.9383 ns | 1,188.2712 ns | 1,200.3338 ns | 1,223.5544 ns | 1,234.4108 ns |  830366.38 |      0 kB |
 | ParseMarketDataIncrementalRefresh | 3,198.0890 ns | 126.9209 ns | 83.9503 ns | 26.5474 ns | 3,127.5225 ns | 3,134.7601 ns | 3,168.2339 ns | 3,236.6894 ns | 3,378.1240 ns |  312686.74 |      0 kB |
