``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3312788 Hz, Resolution=301.8605 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 |   Method |       Mean |     Error |    StdDev |    StdErr |        Min |         Q1 |     Median |         Q3 |        Max |        Op/s | Scaled | ScaledSD |  Gen 0 | Allocated |
 |--------- |-----------:|----------:|----------:|----------:|-----------:|-----------:|-----------:|-----------:|-----------:|------------:|-------:|---------:|-------:|----------:|
 | Standard | 26.5307 ns | 0.1906 ns | 0.1592 ns | 0.0441 ns | 26.3598 ns | 26.3825 ns | 26.4795 ns | 26.6694 ns | 26.8031 ns | 37692117.06 |   1.00 |     0.00 | 0.0068 |      0 GB |
 |   Hotfix | 19.1744 ns | 0.3250 ns | 0.3040 ns | 0.0785 ns | 18.7622 ns | 18.9578 ns | 19.1217 ns | 19.3635 ns | 19.7573 ns | 52152996.62 |   0.72 |     0.01 | 0.0072 |      0 GB |
