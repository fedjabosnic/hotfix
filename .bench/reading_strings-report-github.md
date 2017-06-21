``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3316018 Hz, Resolution=301.5665 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 |   Method |       Mean |     Error |    StdDev |    StdErr |        Min |         Q1 |     Median |         Q3 |        Max |        Op/s | Scaled | ScaledSD |  Gen 0 | Allocated |
 |--------- |-----------:|----------:|----------:|----------:|-----------:|-----------:|-----------:|-----------:|-----------:|------------:|-------:|---------:|-------:|----------:|
 | standard | 26.3191 ns | 0.4089 ns | 0.3825 ns | 0.0988 ns | 25.9428 ns | 26.0664 ns | 26.1917 ns | 26.6423 ns | 27.3540 ns | 37995217.75 |   1.00 |     0.00 | 0.0067 |      0 GB |
 |   hotfix | 18.9089 ns | 0.3420 ns | 0.3032 ns | 0.0810 ns | 18.5915 ns | 18.6313 ns | 18.8296 ns | 19.1813 ns | 19.5827 ns | 52885164.17 |   0.72 |     0.01 | 0.0072 |      0 GB |
