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
 | Standard | 155.4581 ns | 1.9302 ns | 0.6884 ns | 0.2810 ns | 154.5526 ns | 154.8544 ns | 155.4581 ns | 156.0619 ns | 156.3637 ns |  6432599.94 |   1.00 |     0.00 |      0 kB |
 |   Hotfix |  22.3796 ns | 1.0593 ns | 0.6304 ns | 0.2101 ns |  21.5076 ns |  21.9604 ns |  22.1113 ns |  22.7150 ns |  23.6206 ns | 44683551.09 |   0.14 |     0.00 |      0 kB |
