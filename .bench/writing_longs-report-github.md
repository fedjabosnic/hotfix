``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3316018 Hz, Resolution=301.5665 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 |   Method |        Mean |     Error |    StdDev |    StdErr |         Min |          Q1 |      Median |          Q3 |         Max |        Op/s | Scaled | ScaledSD |  Gen 0 | Allocated |
 |--------- |------------:|----------:|----------:|----------:|------------:|------------:|------------:|------------:|------------:|------------:|-------:|---------:|-------:|----------:|
 | standard | 143.8393 ns | 0.4215 ns | 0.3943 ns | 0.1018 ns | 143.2382 ns | 143.5289 ns | 143.7791 ns | 144.0418 ns | 144.6543 ns |  6952201.74 |   1.00 |     0.00 | 0.0120 |   0.06 kB |
 |   hotfix |  64.1697 ns | 0.2030 ns | 0.1799 ns | 0.0481 ns |  63.8922 ns |  64.0294 ns |  64.1257 ns |  64.3110 ns |  64.5121 ns | 15583682.52 |   0.45 |     0.00 |      - |      0 kB |
