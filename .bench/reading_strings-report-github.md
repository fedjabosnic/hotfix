``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3316018 Hz, Resolution=301.5665 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 |   Method |       Mean |     Error |    StdDev |    StdErr |        Min |         Q1 |     Median |         Q3 |        Max |        Op/s | Scaled | ScaledSD |  Gen 0 | Allocated |
 |--------- |-----------:|----------:|----------:|----------:|-----------:|-----------:|-----------:|-----------:|-----------:|------------:|-------:|---------:|-------:|----------:|
 | standard | 26.8143 ns | 0.5716 ns | 0.5347 ns | 0.1380 ns | 26.1788 ns | 26.4118 ns | 26.5014 ns | 27.2970 ns | 27.7584 ns | 37293479.67 |   1.00 |     0.00 | 0.0068 |      0 GB |
 |   hotfix | 18.9426 ns | 0.0383 ns | 0.0299 ns | 0.0086 ns | 18.9042 ns | 18.9262 ns | 18.9380 ns | 18.9514 ns | 19.0188 ns | 52791053.79 |   0.71 |     0.01 | 0.0072 |      0 GB |
