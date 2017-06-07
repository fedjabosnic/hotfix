``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3312789 Hz, Resolution=301.8605 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  Job-YPJJZU : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0

InvocationCount=10000  LaunchCount=1  RunStrategy=Throughput  
TargetCount=10  WarmupCount=5  

```
 |             Method |      Mean |     Error |    StdDev |    StdErr |    Median |       Min |        Q1 |        Q3 |       Max |           Op/s | Allocated |
 |------------------- |----------:|----------:|----------:|----------:|----------:|----------:|----------:|----------:|----------:|---------------:|----------:|
 |               Noop | 0.0231 ns | 0.0471 ns | 0.0311 ns | 0.0098 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0503 ns | 0.0805 ns | 43210356658.28 |      0 kB |
 |  IndexingFirstItem | 0.0586 ns | 0.0482 ns | 0.0252 ns | 0.0089 ns | 0.0624 ns | 0.0322 ns | 0.0322 ns | 0.0775 ns | 0.0926 ns | 17061643718.76 |      0 kB |
 | IndexingMiddleItem | 0.1771 ns | 0.4408 ns | 0.2623 ns | 0.0874 ns | 0.0463 ns | 0.0161 ns | 0.0161 ns | 0.3481 ns | 0.6802 ns |   5646807263.8 |      0 kB |
 |   IndexingLastItem | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns |       Infinity |      0 kB |
