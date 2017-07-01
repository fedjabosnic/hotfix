``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3312788 Hz, Resolution=301.8605 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 | Method |          Mean |      Error |     StdDev |     StdErr |           Min |            Q1 |        Median |            Q3 |           Max |       Op/s | Allocated |
 |------- |--------------:|-----------:|-----------:|-----------:|--------------:|--------------:|--------------:|--------------:|--------------:|-----------:|----------:|
 |  small |   935.6403 ns |  6.8230 ns |  6.3822 ns |  1.6479 ns |   926.5019 ns |   929.5773 ns |   935.8132 ns |   939.8671 ns |   951.7083 ns | 1068786.84 |      0 MB |
 | medium | 2,581.2081 ns | 14.4145 ns | 13.4833 ns |  3.4814 ns | 2,563.1369 ns | 2,570.0621 ns | 2,577.7899 ns | 2,586.7025 ns | 2,610.2243 ns |  387415.48 |      0 MB |
 |  large | 6,127.1105 ns | 93.8376 ns | 87.7757 ns | 22.6636 ns | 6,040.8924 ns | 6,053.5636 ns | 6,107.5693 ns | 6,207.8356 ns | 6,310.2575 ns |  163209.07 |      0 MB |
