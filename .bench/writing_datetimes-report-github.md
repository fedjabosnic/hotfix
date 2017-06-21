``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3316018 Hz, Resolution=301.5665 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 |   Method |        Mean |     Error |    StdDev |    StdErr |         Min |          Q1 |      Median |          Q3 |         Max |        Op/s | Scaled | ScaledSD |  Gen 0 | Allocated |
 |--------- |------------:|----------:|----------:|----------:|------------:|------------:|------------:|------------:|------------:|------------:|-------:|---------:|-------:|----------:|
 | standard | 760.5068 ns | 3.9871 ns | 3.5345 ns | 0.9446 ns | 754.8828 ns | 758.9345 ns | 760.7468 ns | 761.5298 ns | 767.0200 ns |  1314912.58 |   1.00 |     0.00 | 0.0432 |   0.23 kB |
 |   hotfix |  89.0584 ns | 0.3511 ns | 0.3112 ns | 0.0832 ns |  88.5818 ns |  88.7140 ns |  89.1176 ns |  89.2358 ns |  89.6488 ns | 11228584.52 |   0.12 |     0.00 |      - |      0 kB |
