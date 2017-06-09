``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3312789 Hz, Resolution=301.8605 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  Job-LJQWSI : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0

InvocationCount=1000  LaunchCount=1  RunStrategy=Throughput  
TargetCount=10  UnrollFactor=1  WarmupCount=5  

```
 |   Method |       Mean |     Error |    StdDev |    StdErr |        Min |         Q1 |     Median |         Q3 |         Max |         Op/s | Scaled | ScaledSD | Allocated |
 |--------- |-----------:|----------:|----------:|----------:|-----------:|-----------:|-----------:|-----------:|------------:|-------------:|-------:|---------:|----------:|
 | Standard | 98.1768 ns | 1.9719 ns | 1.1734 ns | 0.3911 ns | 96.7010 ns | 97.1538 ns | 97.9084 ns | 99.2668 ns | 100.0215 ns |  10185709.94 |   1.00 |     0.00 |      0 kB |
 |   Hotfix |  9.5841 ns | 0.6735 ns | 0.4455 ns | 0.1409 ns |  8.7389 ns |  9.3426 ns |  9.4935 ns |  9.9463 ns |  10.2482 ns | 104339827.31 |   0.10 |     0.00 |      0 kB |
