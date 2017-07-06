``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3316018 Hz, Resolution=301.5665 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 |   Method |        Mean |     Error |    StdDev |    StdErr |         Min |          Q1 |      Median |          Q3 |         Max |        Op/s | Scaled | ScaledSD |  Gen 0 | Allocated |
 |--------- |------------:|----------:|----------:|----------:|------------:|------------:|------------:|------------:|------------:|------------:|-------:|---------:|-------:|----------:|
 | standard | 162.5383 ns | 0.4145 ns | 0.3675 ns | 0.0982 ns | 161.7875 ns | 162.2483 ns | 162.6282 ns | 162.7277 ns | 163.2863 ns |  6152396.43 |   1.00 |     0.00 | 0.0080 |   0.05 kB |
 |   hotfix |  13.3318 ns | 0.0755 ns | 0.0630 ns | 0.0175 ns |  13.2591 ns |  13.2806 ns |  13.3137 ns |  13.3818 ns |  13.4815 ns | 75008379.24 |   0.08 |     0.00 |      - |      0 kB |
