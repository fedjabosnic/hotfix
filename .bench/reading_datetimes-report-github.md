``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3316018 Hz, Resolution=301.5665 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 |   Method |        Mean |     Error |    StdDev |    StdErr |         Min |          Q1 |      Median |          Q3 |         Max |       Op/s | Scaled | ScaledSD |  Gen 0 | Allocated |
 |--------- |------------:|----------:|----------:|----------:|------------:|------------:|------------:|------------:|------------:|-----------:|-------:|---------:|-------:|----------:|
 | standard | 792.0946 ns | 3.9530 ns | 3.6976 ns | 0.9547 ns | 787.3959 ns | 789.3481 ns | 790.5893 ns | 795.3948 ns | 799.1174 ns |  1262475.5 |   1.00 |     0.00 | 0.0045 |   0.07 kB |
 |   hotfix |  57.2437 ns | 0.1881 ns | 0.1571 ns | 0.0436 ns |  57.0385 ns |  57.0657 ns |  57.2757 ns |  57.3561 ns |  57.5006 ns | 17469179.4 |   0.07 |     0.00 |      - |      0 kB |
