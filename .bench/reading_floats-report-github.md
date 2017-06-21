``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3316018 Hz, Resolution=301.5665 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 |   Method |        Mean |     Error |    StdDev |    StdErr |         Min |          Q1 |      Median |          Q3 |         Max |        Op/s | Scaled | ScaledSD |  Gen 0 | Allocated |
 |--------- |------------:|----------:|----------:|----------:|------------:|------------:|------------:|------------:|------------:|------------:|-------:|---------:|-------:|----------:|
 | standard | 191.0454 ns | 0.7476 ns | 0.6993 ns | 0.1806 ns | 189.4368 ns | 190.6766 ns | 191.2620 ns | 191.5261 ns | 192.1203 ns |  5234357.38 |   1.00 |     0.00 | 0.0083 |   0.05 kB |
 |   hotfix |  19.7948 ns | 0.1919 ns | 0.1795 ns | 0.0464 ns |  19.5616 ns |  19.6412 ns |  19.7780 ns |  20.0066 ns |  20.0661 ns | 50518426.53 |   0.10 |     0.00 |      - |      0 kB |
