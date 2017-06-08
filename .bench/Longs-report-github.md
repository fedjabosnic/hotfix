``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3316022 Hz, Resolution=301.5662 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  Job-LJQWSI : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0

InvocationCount=1000  LaunchCount=1  RunStrategy=Throughput  
TargetCount=10  UnrollFactor=1  WarmupCount=5  

```
 |   Method |        Mean |     Error |    StdDev |    StdErr |         Min |          Q1 |      Median |          Q3 |         Max |        Op/s | Scaled | ScaledSD | Allocated |
 |--------- |------------:|----------:|----------:|----------:|------------:|------------:|------------:|------------:|------------:|------------:|-------:|---------:|----------:|
 | Standard | 128.2678 ns | 3.3370 ns | 1.9858 ns | 0.6619 ns | 125.5872 ns | 125.8888 ns | 128.9045 ns | 129.8092 ns | 130.7139 ns |  7796188.24 |   1.00 |     0.00 |      0 kB |
 |   Hotfix |  14.4903 ns | 1.6375 ns | 1.0831 ns | 0.3425 ns |  13.0729 ns |  13.0729 ns |  14.5807 ns |  15.4854 ns |  15.7870 ns | 69011912.28 |   0.11 |     0.01 |      0 kB |
