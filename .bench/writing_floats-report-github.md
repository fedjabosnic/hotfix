``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3316018 Hz, Resolution=301.5665 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 |   Method |        Mean |     Error |    StdDev |    StdErr |         Min |          Q1 |      Median |          Q3 |         Max |        Op/s | Scaled | ScaledSD |  Gen 0 | Allocated |
 |--------- |------------:|----------:|----------:|----------:|------------:|------------:|------------:|------------:|------------:|------------:|-------:|---------:|-------:|----------:|
 | standard | 559.8382 ns | 6.1695 ns | 5.7710 ns | 1.4901 ns | 550.9681 ns | 556.1342 ns | 557.3432 ns | 564.8725 ns | 570.6534 ns |  1786230.25 |   1.00 |     0.00 | 0.0016 |   0.06 kB |
 |   hotfix |  54.6385 ns | 0.7168 ns | 0.6705 ns | 0.1731 ns |  53.8826 ns |  54.1943 ns |  54.5059 ns |  54.9796 ns |  56.0666 ns | 18302125.43 |   0.10 |     0.00 |      - |      0 kB |
