``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3312789 Hz, Resolution=301.8605 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  Job-LJQWSI : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0

InvocationCount=1000  LaunchCount=1  RunStrategy=Throughput  
TargetCount=10  UnrollFactor=1  WarmupCount=5  

```
 |   Method |        Mean |     Error |    StdDev |    StdErr |         Min |          Q1 |      Median |          Q3 |         Max |       Op/s | Scaled | ScaledSD | Allocated |
 |--------- |------------:|----------:|----------:|----------:|------------:|------------:|------------:|------------:|------------:|-----------:|-------:|---------:|----------:|
 | Standard | 762.9448 ns | 9.6892 ns | 5.0677 ns | 1.7917 ns | 758.7564 ns | 759.9639 ns | 760.8695 ns | 764.6427 ns | 773.8495 ns | 1310710.89 |   1.00 |     0.00 |      0 kB |
 |   Hotfix |  58.1786 ns | 1.4121 ns | 0.8403 ns | 0.2801 ns |  57.0718 ns |  57.2227 ns |  58.2792 ns |  58.8829 ns |  59.1848 ns | 17188459.6 |   0.08 |     0.00 |      0 kB |
