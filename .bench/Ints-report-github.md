``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3312788 Hz, Resolution=301.8605 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 |   Method |        Mean |     Error |    StdDev |    StdErr |         Min |          Q1 |      Median |          Q3 |         Max |         Op/s | Scaled | ScaledSD |  Gen 0 | Allocated |
 |--------- |------------:|----------:|----------:|----------:|------------:|------------:|------------:|------------:|------------:|-------------:|-------:|---------:|-------:|----------:|
 | Standard | 129.8995 ns | 1.4792 ns | 1.3836 ns | 0.3573 ns | 128.6132 ns | 128.9361 ns | 129.4471 ns | 130.7102 ns | 132.8646 ns |   7698259.74 |   1.00 |     0.00 | 0.0062 |   0.04 kB |
 |   Hotfix |   9.1639 ns | 0.1100 ns | 0.1029 ns | 0.0266 ns |   9.0762 ns |   9.0867 ns |   9.1243 ns |   9.2364 ns |   9.4178 ns | 109124357.92 |   0.07 |     0.00 |      - |      0 kB |
