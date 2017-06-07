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
 | Standard | 97.7139 ns | 2.7564 ns | 1.6403 ns | 0.5468 ns | 96.0369 ns | 96.6406 ns | 96.9425 ns | 99.2064 ns | 100.8667 ns |  10233957.59 |   1.00 |     0.00 |      0 kB |
 |   Hotfix |  9.6897 ns | 0.3757 ns | 0.2485 ns | 0.0786 ns |  9.4784 ns |  9.4784 ns |  9.6294 ns |  9.7803 ns |  10.0821 ns | 103202135.21 |   0.10 |     0.00 |      0 kB |
