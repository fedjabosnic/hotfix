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
 | Standard | 129.4529 ns | 4.9263 ns | 2.5766 ns | 0.9110 ns | 125.3777 ns | 127.6417 ns | 129.9057 ns | 130.6603 ns | 133.8298 ns |   7724819.9 |   1.00 |     0.00 |      0 kB |
 |   Hotfix |  14.9421 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns |  14.9421 ns |  14.9421 ns |  14.9421 ns |  14.9421 ns |  14.9421 ns | 66925041.95 |   0.12 |     0.00 |      0 kB |
