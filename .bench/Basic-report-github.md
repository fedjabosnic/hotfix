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
 |               Noop | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns |       Infinity |      0 kB |
 |  IndexingFirstItem | 0.0604 ns | 0.0638 ns | 0.0422 ns | 0.0133 ns | 0.0664 ns | 0.0060 ns | 0.0060 ns | 0.0966 ns | 0.1268 ns | 16564024925.54 |      0 kB |
 | IndexingMiddleItem | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns |       Infinity |      0 kB |
 |   IndexingLastItem | 0.0402 ns | 0.0419 ns | 0.0277 ns | 0.0088 ns | 0.0493 ns | 0.0040 ns | 0.0040 ns | 0.0644 ns | 0.0644 ns | 24845831614.83 |      0 kB |
