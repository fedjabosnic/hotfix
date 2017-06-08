``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3316022 Hz, Resolution=301.5662 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  Job-LJQWSI : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0

InvocationCount=1000  LaunchCount=1  RunStrategy=Throughput  
TargetCount=10  UnrollFactor=1  WarmupCount=5  

```
 |   Method |        Mean |      Error |    StdDev |    StdErr |         Min |          Q1 |      Median |          Q3 |         Max |        Op/s | Scaled | ScaledSD | Allocated |
 |--------- |------------:|-----------:|----------:|----------:|------------:|------------:|------------:|------------:|------------:|------------:|-------:|---------:|----------:|
 | Standard | 160.4332 ns | 10.7965 ns | 7.1412 ns | 2.2583 ns | 153.8289 ns | 154.7336 ns | 157.5985 ns | 164.9868 ns | 174.6370 ns |  6233124.13 |   1.00 |     0.00 |      0 kB |
 |   Hotfix |  22.5370 ns |  0.6704 ns | 0.3989 ns | 0.1330 ns |  22.1350 ns |  22.1350 ns |  22.4365 ns |  22.7381 ns |  23.3412 ns | 44371388.32 |   0.14 |     0.01 |      0 kB |
