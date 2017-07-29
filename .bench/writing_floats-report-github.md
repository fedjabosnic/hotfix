``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3312788 Hz, Resolution=301.8605 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 |   Method |        Mean |     Error |    StdDev |    StdErr |         Min |          Q1 |      Median |          Q3 |         Max |        Op/s | Scaled | ScaledSD |  Gen 0 | Allocated |
 |--------- |------------:|----------:|----------:|----------:|------------:|------------:|------------:|------------:|------------:|------------:|-------:|---------:|-------:|----------:|
 | standard | 549.2370 ns | 9.5689 ns | 8.4826 ns | 2.2671 ns | 540.8282 ns | 542.2569 ns | 545.7795 ns | 555.1501 ns | 568.1713 ns |  1820707.57 |   1.00 |     0.00 | 0.0013 |   0.06 kB |
 |   hotfix |  62.0243 ns | 1.1076 ns | 1.0361 ns | 0.2675 ns |  60.8746 ns |  61.0131 ns |  61.6912 ns |  62.9505 ns |  64.1909 ns | 16122701.14 |   0.11 |     0.00 |      - |      0 kB |
