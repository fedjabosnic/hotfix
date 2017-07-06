``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3316018 Hz, Resolution=301.5665 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 |   Method |        Mean |     Error |    StdDev |    StdErr |         Min |          Q1 |      Median |          Q3 |         Max |        Op/s | Scaled | ScaledSD |  Gen 0 | Allocated |
 |--------- |------------:|----------:|----------:|----------:|------------:|------------:|------------:|------------:|------------:|------------:|-------:|---------:|-------:|----------:|
 | standard | 143.1342 ns | 1.1867 ns | 1.1100 ns | 0.2866 ns | 141.6912 ns | 142.3200 ns | 142.7282 ns | 144.3025 ns | 145.1383 ns |  6986448.58 |   1.00 |     0.00 | 0.0120 |   0.06 kB |
 |   hotfix |  69.0580 ns | 0.5683 ns | 0.5037 ns | 0.1346 ns |  68.4384 ns |  68.6617 ns |  68.9773 ns |  69.1991 ns |  70.0921 ns | 14480577.48 |   0.48 |     0.00 |      - |      0 kB |
