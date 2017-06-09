``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3312789 Hz, Resolution=301.8605 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  Job-LJQWSI : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0

InvocationCount=1000  LaunchCount=1  RunStrategy=Throughput  
TargetCount=10  UnrollFactor=1  WarmupCount=5  

```
 |   Method |        Mean |     Error |    StdDev |    StdErr |         Min |          Q1 |      Median |          Q3 |         Max |        Op/s | Scaled | ScaledSD | Allocated |
 |--------- |------------:|----------:|----------:|----------:|------------:|------------:|------------:|------------:|------------:|------------:|-------:|---------:|----------:|
 | Standard | 154.7488 ns | 3.9010 ns | 2.5803 ns | 0.8160 ns | 151.9717 ns | 152.5754 ns | 153.9337 ns | 156.8014 ns | 159.2163 ns |  6462087.15 |   1.00 |     0.00 |      0 kB |
 |   Hotfix |  22.5490 ns | 0.6820 ns | 0.4511 ns | 0.1427 ns |  22.1566 ns |  22.1566 ns |  22.4584 ns |  23.0621 ns |  23.3640 ns | 44347907.92 |   0.15 |     0.00 |      0 kB |
