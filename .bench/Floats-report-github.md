``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3312788 Hz, Resolution=301.8605 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 |   Method |        Mean |     Error |    StdDev |    StdErr |         Min |          Q1 |      Median |          Q3 |         Max |        Op/s | Scaled | ScaledSD |  Gen 0 | Allocated |
 |--------- |------------:|----------:|----------:|----------:|------------:|------------:|------------:|------------:|------------:|------------:|-------:|---------:|-------:|----------:|
 | Standard | 191.8979 ns | 1.6560 ns | 1.3828 ns | 0.3835 ns | 189.6330 ns | 191.1547 ns | 191.8258 ns | 192.5910 ns | 195.1238 ns |   5211105.2 |   1.00 |     0.00 | 0.0083 |   0.05 kB |
 |   Hotfix |  19.6762 ns | 0.1140 ns | 0.0890 ns | 0.0257 ns |  19.5600 ns |  19.6042 ns |  19.6557 ns |  19.7700 ns |  19.8015 ns | 50822721.56 |   0.10 |     0.00 |      - |      0 kB |
