``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3312789 Hz, Resolution=301.8605 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  Job-YPJJZU : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0

InvocationCount=10000  LaunchCount=1  RunStrategy=Throughput  
TargetCount=10  WarmupCount=5  

```
 |             Method |      Mean |     Error |    StdDev |    StdErr |    Median |       Min |        Q1 |        Q3 |       Max |            Op/s | Allocated |
 |------------------- |----------:|----------:|----------:|----------:|----------:|----------:|----------:|----------:|----------:|----------------:|----------:|
 |               Noop | 0.0211 ns | 0.0376 ns | 0.0249 ns | 0.0079 ns | 0.0121 ns | 0.0000 ns | 0.0000 ns | 0.0543 ns | 0.0543 ns |  47325404750.52 |      0 kB |
 |  IndexingFirstItem | 0.0010 ns | 0.0048 ns | 0.0032 ns | 0.0010 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0101 ns | 993838203140.53 |      0 kB |
 | IndexingMiddleItem | 0.0483 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0483 ns | 0.0483 ns | 0.0483 ns | 0.0483 ns | 0.0483 ns |  20704791088.66 |      0 kB |
 |   IndexingLastItem | 0.0332 ns | 0.0454 ns | 0.0300 ns | 0.0095 ns | 0.0302 ns | 0.0000 ns | 0.0000 ns | 0.0604 ns | 0.0906 ns |  30116309186.08 |      0 kB |
