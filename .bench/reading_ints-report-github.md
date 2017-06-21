``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3316018 Hz, Resolution=301.5665 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 |   Method |        Mean |     Error |    StdDev |    StdErr |         Min |          Q1 |      Median |          Q3 |         Max |         Op/s | Scaled | ScaledSD |  Gen 0 | Allocated |
 |--------- |------------:|----------:|----------:|----------:|------------:|------------:|------------:|------------:|------------:|-------------:|-------:|---------:|-------:|----------:|
 | standard | 130.2358 ns | 1.1223 ns | 0.9372 ns | 0.2599 ns | 128.9637 ns | 129.1061 ns | 130.5493 ns | 130.8909 ns | 131.9953 ns |   7678381.06 |   1.00 |     0.00 | 0.0063 |   0.04 kB |
 |   hotfix |   9.1901 ns | 0.0659 ns | 0.0550 ns | 0.0153 ns |   9.0908 ns |   9.1668 ns |   9.1910 ns |   9.2217 ns |   9.2822 ns | 108812548.87 |   0.07 |     0.00 |      - |      0 kB |
