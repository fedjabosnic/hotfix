``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3312788 Hz, Resolution=301.8605 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 |   Method |        Mean |     Error |    StdDev |    StdErr |         Min |          Q1 |      Median |          Q3 |         Max |       Op/s | Scaled | ScaledSD |  Gen 0 | Allocated |
 |--------- |------------:|----------:|----------:|----------:|------------:|------------:|------------:|------------:|------------:|-----------:|-------:|---------:|-------:|----------:|
 | Standard | 806.8647 ns | 7.8552 ns | 7.3478 ns | 1.8972 ns | 798.9380 ns | 800.6865 ns | 804.5970 ns | 812.9705 ns | 824.3249 ns | 1239365.18 |   1.00 |     0.00 | 0.0045 |   0.07 kB |
 |   Hotfix |  57.8101 ns | 0.3910 ns | 0.3466 ns | 0.0926 ns |  57.3533 ns |  57.5079 ns |  57.8991 ns |  58.1409 ns |  58.3507 ns | 17298016.1 |   0.07 |     0.00 |      - |      0 kB |
