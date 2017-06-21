``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3316018 Hz, Resolution=301.5665 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 |   Method |        Mean |     Error |    StdDev |    StdErr |         Min |          Q1 |      Median |          Q3 |         Max |        Op/s | Scaled | ScaledSD |  Gen 0 | Allocated |
 |--------- |------------:|----------:|----------:|----------:|------------:|------------:|------------:|------------:|------------:|------------:|-------:|---------:|-------:|----------:|
 | standard | 113.1440 ns | 0.6884 ns | 0.6440 ns | 0.1663 ns | 111.8505 ns | 112.6959 ns | 113.2079 ns | 113.5501 ns | 114.3303 ns |  8838290.83 |   1.00 |     0.00 | 0.0098 |   0.05 kB |
 |   hotfix |  37.6636 ns | 0.2009 ns | 0.1678 ns | 0.0465 ns |  37.3860 ns |  37.5428 ns |  37.6754 ns |  37.8000 ns |  37.9572 ns | 26550838.82 |   0.33 |     0.00 |      - |      0 kB |
