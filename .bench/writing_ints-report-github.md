``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3316018 Hz, Resolution=301.5665 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 |   Method |        Mean |     Error |    StdDev |    StdErr |         Min |          Q1 |      Median |          Q3 |         Max |        Op/s | Scaled | ScaledSD |  Gen 0 | Allocated |
 |--------- |------------:|----------:|----------:|----------:|------------:|------------:|------------:|------------:|------------:|------------:|-------:|---------:|-------:|----------:|
 | standard | 113.9619 ns | 0.2120 ns | 0.1771 ns | 0.0491 ns | 113.7139 ns | 113.8396 ns | 113.9197 ns | 114.0336 ns | 114.3110 ns |  8774861.63 |   1.00 |     0.00 | 0.0098 |   0.05 kB |
 |   hotfix |  37.0036 ns | 0.2250 ns | 0.1994 ns | 0.0533 ns |  36.6796 ns |  36.8875 ns |  36.9954 ns |  37.1601 ns |  37.3308 ns | 27024411.03 |   0.32 |     0.00 |      - |      0 kB |
