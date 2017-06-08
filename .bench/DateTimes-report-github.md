``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3316022 Hz, Resolution=301.5662 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  Job-LJQWSI : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0

InvocationCount=1000  LaunchCount=1  RunStrategy=Throughput  
TargetCount=10  UnrollFactor=1  WarmupCount=5  

```
 |   Method |        Mean |      Error |     StdDev |    StdErr |         Min |          Q1 |      Median |          Q3 |         Max |        Op/s | Scaled | ScaledSD | Allocated |
 |--------- |------------:|-----------:|-----------:|----------:|------------:|------------:|------------:|------------:|------------:|------------:|-------:|---------:|----------:|
 | Standard | 782.1369 ns | 32.9629 ns | 19.6157 ns | 6.5386 ns | 758.8158 ns | 768.9183 ns | 773.2910 ns | 795.3053 ns | 822.4463 ns |  1278548.47 |   1.00 |     0.00 |      0 kB |
 |   Hotfix |  58.1467 ns |  1.5386 ns |  1.0177 ns | 0.3218 ns |  57.0912 ns |  57.0912 ns |  58.2975 ns |  58.9006 ns |  59.5038 ns | 17197876.39 |   0.07 |     0.00 |      0 kB |
