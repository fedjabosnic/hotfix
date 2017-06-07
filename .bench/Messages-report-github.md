``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3312789 Hz, Resolution=301.8605 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  Job-LJQWSI : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0

InvocationCount=1000  LaunchCount=1  RunStrategy=Throughput  
TargetCount=10  UnrollFactor=1  WarmupCount=5  

```
 |                            Method |          Mean |      Error |     StdDev |    StdErr |           Min |            Q1 |        Median |            Q3 |           Max |       Op/s | Allocated |
 |---------------------------------- |--------------:|-----------:|-----------:|----------:|--------------:|--------------:|--------------:|--------------:|--------------:|-----------:|----------:|
 |                    ParseHeartbeat |   228.7876 ns |  5.5482 ns |  2.9018 ns | 1.0259 ns |   224.8257 ns |   225.8822 ns |   229.6554 ns |   231.4666 ns |   231.4666 ns | 4370866.49 |      0 kB |
 |              ParseExecutionReport |   877.0052 ns | 11.4667 ns |  6.8236 ns | 2.2745 ns |   871.7730 ns |   872.0749 ns |   874.1879 ns |   880.8288 ns |   892.6014 ns | 1140244.04 |      0 kB |
 | ParseMarketDataIncrementalRefresh | 2,372.7453 ns | 33.0964 ns | 17.3101 ns | 6.1200 ns | 2,353.1621 ns | 2,361.9160 ns | 2,366.8967 ns | 2,385.0083 ns | 2,401.1579 ns |  421452.74 |      0 kB |
