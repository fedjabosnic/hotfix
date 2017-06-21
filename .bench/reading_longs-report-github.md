``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3316018 Hz, Resolution=301.5665 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 |   Method |        Mean |     Error |    StdDev |    StdErr |         Min |          Q1 |      Median |          Q3 |         Max |        Op/s | Scaled | ScaledSD |  Gen 0 | Allocated |
 |--------- |------------:|----------:|----------:|----------:|------------:|------------:|------------:|------------:|------------:|------------:|-------:|---------:|-------:|----------:|
 | standard | 163.9217 ns | 1.1690 ns | 1.0363 ns | 0.2770 ns | 162.2905 ns | 163.2500 ns | 163.9897 ns | 164.2455 ns | 166.1290 ns |  6100474.03 |   1.00 |     0.00 | 0.0082 |   0.05 kB |
 |   hotfix |  13.6955 ns | 0.1845 ns | 0.1726 ns | 0.0446 ns |  13.4414 ns |  13.5638 ns |  13.6645 ns |  13.8179 ns |  14.1367 ns | 73016769.35 |   0.08 |     0.00 |      - |      0 kB |
