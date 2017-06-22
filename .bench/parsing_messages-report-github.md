``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3316018 Hz, Resolution=301.5665 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0


```
 | Method |          Mean |     Error |    StdDev |    StdErr |           Min |            Q1 |        Median |            Q3 |           Max |      Op/s | Allocated |
 |------- |--------------:|----------:|----------:|----------:|--------------:|--------------:|--------------:|--------------:|--------------:|----------:|----------:|
 |  small |   412.6110 ns | 2.3425 ns | 2.1911 ns | 0.5657 ns |   408.5712 ns |   410.8804 ns |   412.5392 ns |   413.4265 ns |   416.6059 ns | 2423590.3 |      0 kB |
 | medium | 1,266.9544 ns | 8.5979 ns | 8.0424 ns | 2.0765 ns | 1,256.2294 ns | 1,261.4729 ns | 1,264.8757 ns | 1,274.4239 ns | 1,279.7387 ns | 789294.37 |      0 kB |
 |  large | 3,268.6147 ns | 9.9372 ns | 8.8091 ns | 2.3543 ns | 3,250.9425 ns | 3,263.1447 ns | 3,270.3927 ns | 3,276.8400 ns | 3,281.1516 ns | 305940.01 |      0 kB |
