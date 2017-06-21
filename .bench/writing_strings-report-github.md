``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3316018 Hz, Resolution=301.5665 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 |   Method |       Mean |     Error |    StdDev |    StdErr |        Min |         Q1 |     Median |         Q3 |        Max |        Op/s | Scaled | ScaledSD | Allocated |
 |--------- |-----------:|----------:|----------:|----------:|-----------:|-----------:|-----------:|-----------:|-----------:|------------:|-------:|---------:|----------:|
 | standard | 33.6827 ns | 0.2533 ns | 0.2369 ns | 0.0612 ns | 33.3066 ns | 33.4946 ns | 33.6720 ns | 33.8445 ns | 34.1795 ns | 29688831.09 |   1.00 |     0.00 |      0 kB |
 |   hotfix | 10.4795 ns | 0.1510 ns | 0.1412 ns | 0.0365 ns | 10.3109 ns | 10.3616 ns | 10.4491 ns | 10.6363 ns | 10.7842 ns | 95424763.89 |   0.31 |     0.00 |      0 kB |
