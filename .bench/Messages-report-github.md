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
 |                    ParseHeartbeat |   368.9439 ns | 20.8808 ns | 13.8113 ns |  4.3675 ns |   357.5034 ns |   358.4090 ns |   364.7481 ns |   376.8225 ns |   402.1788 ns | 2710439.09 |      0 kB |
 |              ParseExecutionReport | 1,225.5535 ns | 49.4795 ns | 32.7276 ns | 10.3494 ns | 1,183.9571 ns | 1,209.0115 ns | 1,212.4829 ns | 1,267.5724 ns | 1,272.4022 ns |  815957.88 |      0 kB |
 | ParseMarketDataIncrementalRefresh | 3,206.6737 ns | 50.6993 ns | 33.5345 ns | 10.6045 ns | 3,160.5796 ns | 3,183.2191 ns | 3,196.5010 ns | 3,230.6112 ns | 3,271.6643 ns |  311849.63 |      0 kB |
