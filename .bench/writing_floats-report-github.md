``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3316018 Hz, Resolution=301.5665 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 |   Method |        Mean |     Error |    StdDev |    StdErr |         Min |          Q1 |      Median |          Q3 |         Max |        Op/s | Scaled | ScaledSD |  Gen 0 | Allocated |
 |--------- |------------:|----------:|----------:|----------:|------------:|------------:|------------:|------------:|------------:|------------:|-------:|---------:|-------:|----------:|
 | standard | 558.8236 ns | 3.7538 ns | 3.5113 ns | 0.9066 ns | 554.8705 ns | 555.8089 ns | 557.4214 ns | 561.0083 ns | 567.6972 ns |  1789473.58 |   1.00 |     0.00 | 0.0029 |   0.06 kB |
 |   hotfix |  54.9049 ns | 0.2283 ns | 0.2024 ns | 0.0541 ns |  54.5932 ns |  54.7917 ns |  54.8571 ns |  54.9887 ns |  55.3110 ns | 18213316.29 |   0.10 |     0.00 |      - |      0 kB |
