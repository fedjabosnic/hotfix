``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3316018 Hz, Resolution=301.5665 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 |   Method |        Mean |     Error |    StdDev |    StdErr |         Min |          Q1 |      Median |          Q3 |         Max |       Op/s | Scaled | ScaledSD |  Gen 0 | Allocated |
 |--------- |------------:|----------:|----------:|----------:|------------:|------------:|------------:|------------:|------------:|-----------:|-------:|---------:|-------:|----------:|
 | standard | 761.5072 ns | 8.7510 ns | 8.1857 ns | 2.1135 ns | 752.8821 ns | 755.1369 ns | 759.4534 ns | 766.0244 ns | 779.7318 ns | 1313185.18 |   1.00 |     0.00 | 0.0424 |   0.23 kB |
 |   hotfix |  88.2885 ns | 0.5327 ns | 0.4722 ns | 0.1262 ns |  87.7754 ns |  87.9438 ns |  88.1784 ns |  88.4314 ns |  89.4376 ns | 11326509.5 |   0.12 |     0.00 |      - |      0 kB |
