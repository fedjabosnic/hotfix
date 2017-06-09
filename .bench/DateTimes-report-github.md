``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3312789 Hz, Resolution=301.8605 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  Job-LJQWSI : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0

InvocationCount=1000  LaunchCount=1  RunStrategy=Throughput  
TargetCount=10  UnrollFactor=1  WarmupCount=5  

```
 |   Method |        Mean |      Error |     StdDev |    StdErr |         Min |          Q1 |      Median |          Q3 |         Max |        Op/s | Scaled | ScaledSD | Allocated |
 |--------- |------------:|-----------:|-----------:|----------:|------------:|------------:|------------:|------------:|------------:|------------:|-------:|---------:|----------:|
 | Standard | 784.5353 ns | 36.0433 ns | 23.8404 ns | 7.5390 ns | 752.7494 ns | 768.4462 ns | 777.8038 ns | 795.6136 ns | 834.8555 ns |  1274639.86 |   1.00 |     0.00 |      0 kB |
 |   Hotfix |  56.6324 ns |  1.5311 ns |  0.9112 ns | 0.3037 ns |  55.6933 ns |  55.6933 ns |  57.2026 ns |  57.3535 ns |  57.8063 ns | 17657744.18 |   0.07 |     0.00 |      0 kB |
