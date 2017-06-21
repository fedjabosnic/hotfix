``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3316018 Hz, Resolution=301.5665 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 |   Method |       Mean |     Error |    StdDev |    StdErr |        Min |         Q1 |     Median |         Q3 |        Max |        Op/s | Scaled | ScaledSD | Allocated |
 |--------- |-----------:|----------:|----------:|----------:|-----------:|-----------:|-----------:|-----------:|-----------:|------------:|-------:|---------:|----------:|
 | standard | 34.0371 ns | 0.2280 ns | 0.1904 ns | 0.0528 ns | 33.6702 ns | 33.8994 ns | 34.0109 ns | 34.2019 ns | 34.3421 ns | 29379716.69 |   1.00 |     0.00 |      0 kB |
 |   hotfix | 10.4361 ns | 0.0666 ns | 0.0591 ns | 0.0158 ns | 10.3540 ns | 10.3993 ns | 10.4185 ns | 10.4655 ns | 10.5715 ns | 95821039.57 |   0.31 |     0.00 |      0 kB |
