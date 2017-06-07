``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3312789 Hz, Resolution=301.8605 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  Job-LJQWSI : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0

InvocationCount=1000  LaunchCount=1  RunStrategy=Throughput  
TargetCount=10  UnrollFactor=1  WarmupCount=5  

```
 |                            Method |          Mean |      Error |     StdDev |     StdErr |           Min |            Q1 |        Median |            Q3 |           Max |       Op/s | Allocated |
 |---------------------------------- |--------------:|-----------:|-----------:|-----------:|--------------:|--------------:|--------------:|--------------:|--------------:|-----------:|----------:|
 |                    ParseHeartbeat |   230.1887 ns |  6.1577 ns |  4.0729 ns |  1.2880 ns |   224.3024 ns |   225.8118 ns |   230.9434 ns |   233.3583 ns |   236.3769 ns | 4344261.49 |      0 kB |
 |              ParseExecutionReport |   880.7819 ns | 37.6233 ns | 22.3890 ns |  7.4630 ns |   854.8889 ns |   867.8689 ns |   872.6987 ns |   894.7345 ns |   929.7503 ns |  1135354.9 |      0 kB |
 | ParseMarketDataIncrementalRefresh | 2,378.5698 ns | 67.3748 ns | 44.5643 ns | 14.0925 ns | 2,297.3996 ns | 2,345.3954 ns | 2,385.5428 ns | 2,409.0879 ns | 2,435.6517 ns |   420420.7 |      0 kB |
