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
 | Standard | 126.6808 ns | 1.9971 ns | 1.1884 ns | 0.3961 ns | 125.5740 ns | 125.7249 ns | 126.1777 ns | 127.6870 ns | 128.8944 ns |  7893857.92 |   1.00 |     0.00 |      0 kB |
 |   Hotfix |  14.7375 ns | 1.0285 ns | 0.6120 ns | 0.2040 ns |  13.5636 ns |  14.4692 ns |  14.7710 ns |  15.2238 ns |  15.6766 ns | 67854113.66 |   0.12 |     0.00 |      0 kB |
