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
 |                    ParseHeartbeat |   232.1106 ns | 15.5865 ns | 10.3095 ns |  3.2602 ns |   223.6585 ns |   224.5641 ns |   225.3187 ns |   244.7887 ns |   248.1092 ns | 4308291.56 |      0 kB |
 |              ParseExecutionReport |   907.5669 ns | 38.1462 ns | 22.7002 ns |  7.5667 ns |   860.2419 ns |   894.5031 ns |   909.7470 ns |   925.8966 ns |   934.4996 ns |  1101847.1 |      0 kB |
 | ParseMarketDataIncrementalRefresh | 2,407.9308 ns | 74.2416 ns | 49.1062 ns | 15.5288 ns | 2,349.4001 ns | 2,368.7191 ns | 2,400.5654 ns | 2,429.3931 ns | 2,509.3861 ns |  415294.33 |      0 kB |
