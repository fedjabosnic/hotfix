``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3316022 Hz, Resolution=301.5662 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  Job-LJQWSI : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0

InvocationCount=1000  LaunchCount=1  RunStrategy=Throughput  
TargetCount=10  UnrollFactor=1  WarmupCount=5  

```
 |   Method |       Mean |     Error |    StdDev |    StdErr |        Min |         Q1 |     Median |         Q3 |        Max |         Op/s | Scaled | ScaledSD | Allocated |
 |--------- |-----------:|----------:|----------:|----------:|-----------:|-----------:|-----------:|-----------:|-----------:|-------------:|-------:|---------:|----------:|
 | Standard | 97.5717 ns | 1.7599 ns | 1.1641 ns | 0.3681 ns | 96.2750 ns | 96.8781 ns | 97.1797 ns | 98.6875 ns | 99.5922 ns |  10248870.35 |   1.00 |     0.00 |      0 kB |
 |   Hotfix |  9.2882 ns | 1.7361 ns | 1.1483 ns | 0.3631 ns |  7.6296 ns |  8.2328 ns |  9.7406 ns |  9.7406 ns | 11.2484 ns | 107663023.35 |   0.10 |     0.01 |      0 kB |
