``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3316018 Hz, Resolution=301.5665 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 |   Method |        Mean |     Error |    StdDev |    StdErr |         Min |          Q1 |      Median |          Q3 |         Max |         Op/s | Scaled | ScaledSD |  Gen 0 | Allocated |
 |--------- |------------:|----------:|----------:|----------:|------------:|------------:|------------:|------------:|------------:|-------------:|-------:|---------:|-------:|----------:|
 | standard | 127.4408 ns | 0.6049 ns | 0.5362 ns | 0.1433 ns | 126.5796 ns | 127.0165 ns | 127.5085 ns | 128.0116 ns | 128.3190 ns |   7846777.94 |   1.00 |     0.00 | 0.0062 |   0.04 kB |
 |   hotfix |   8.9566 ns | 0.0675 ns | 0.0631 ns | 0.0163 ns |   8.8519 ns |   8.9140 ns |   8.9584 ns |   9.0117 ns |   9.0460 ns | 111649847.33 |   0.07 |     0.00 |      - |      0 kB |
