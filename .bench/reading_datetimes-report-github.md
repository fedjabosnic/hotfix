``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3316018 Hz, Resolution=301.5665 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 |   Method |        Mean |     Error |    StdDev |    StdErr |         Min |          Q1 |      Median |          Q3 |         Max |        Op/s | Scaled | ScaledSD |  Gen 0 | Allocated |
 |--------- |------------:|----------:|----------:|----------:|------------:|------------:|------------:|------------:|------------:|------------:|-------:|---------:|-------:|----------:|
 | standard | 799.0100 ns | 3.1482 ns | 2.6289 ns | 0.7291 ns | 794.2697 ns | 797.6974 ns | 798.9932 ns | 799.9730 ns | 805.3916 ns |  1251548.75 |   1.00 |     0.00 | 0.0035 |   0.07 kB |
 |   hotfix |  57.6819 ns | 0.3102 ns | 0.2750 ns | 0.0735 ns |  57.2945 ns |  57.4883 ns |  57.6666 ns |  57.7790 ns |  58.3383 ns | 17336456.13 |   0.07 |     0.00 |      - |      0 kB |
