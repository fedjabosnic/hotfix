``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3312788 Hz, Resolution=301.8605 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 |   Method |        Mean |     Error |    StdDev |    StdErr |         Min |          Q1 |      Median |          Q3 |         Max |       Op/s | Scaled | ScaledSD |  Gen 0 | Allocated |
 |--------- |------------:|----------:|----------:|----------:|------------:|------------:|------------:|------------:|------------:|-----------:|-------:|---------:|-------:|----------:|
 | Standard | 165.0419 ns | 1.3968 ns | 1.2383 ns | 0.3309 ns | 163.8146 ns | 164.0463 ns | 164.5945 ns | 165.5470 ns | 168.0424 ns | 6059065.86 |   1.00 |     0.00 | 0.0081 |   0.05 kB |
 |   Hotfix |  13.4353 ns | 0.1412 ns | 0.1320 ns | 0.0341 ns |  13.2967 ns |  13.3323 ns |  13.3714 ns |  13.5422 ns |  13.7153 ns | 74431000.5 |   0.08 |     0.00 |      - |      0 kB |
