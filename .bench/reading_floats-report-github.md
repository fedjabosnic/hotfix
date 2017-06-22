``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3316018 Hz, Resolution=301.5665 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 |   Method |        Mean |     Error |    StdDev |    StdErr |         Min |          Q1 |      Median |          Q3 |         Max |        Op/s | Scaled | ScaledSD |  Gen 0 | Allocated |
 |--------- |------------:|----------:|----------:|----------:|------------:|------------:|------------:|------------:|------------:|------------:|-------:|---------:|-------:|----------:|
 | standard | 188.8231 ns | 1.8592 ns | 1.7391 ns | 0.4490 ns | 186.4982 ns | 187.4883 ns | 188.2730 ns | 190.4131 ns | 192.0371 ns |  5295963.52 |   1.00 |     0.00 | 0.0082 |   0.05 kB |
 |   hotfix |  19.3937 ns | 0.0755 ns | 0.0669 ns | 0.0179 ns |  19.2694 ns |  19.3479 ns |  19.3886 ns |  19.4585 ns |  19.4798 ns | 51563179.04 |   0.10 |     0.00 |      - |      0 kB |
