# Hotfix

Hotfix is a **FIX engine** written in pure .net geared towards **low latency** and **high throughput**.

### Performance

Performance will depend on _hardware_ and _operating system_ characteristics therefore it is recommended you _run the benchmarks on your own hardware_ - however the below should give a decent **indication** of the performance that should be expected.

Message processing from `network stream` to `user code` (including _message validation_ and _session management_) on a _development desktop machine_:

```
BenchmarkDotNet=v0.10.4, OS=Windows 10.0.10586
Processor=Intel Core i7-3770 CPU 3.40GHz (Ivy Bridge), ProcessorCount=8
Frequency=3312788 Hz, Resolution=301.8605 ns, Timer=TSC
  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1086.0
```

 | Message size |      Min |   Median |      Max |     Mean |     StdDev |         **Op/s** | Allocated |
 |------------- |---------:|---------:|---------:|---------:|-----------:|-----------------:|----------:|
 | Small        | 0.911 us | 0.921 us | 0.939 us | 0.922 us |  8.4168 ns | **1,083,924.04** |      0 kB |
 | Medium       | 2.499 us | 2.516 us | 2.545 us | 2.519 us | 13.5180 ns |   **396,908.59** |      0 kB |
 | Large        | 5.996 us | 6.030 us | 6.136 us | 6.046 us | 43.6667 ns |   **165,376.55** |      0 kB |

- `Small message`: `0.10 KB` message with `10 fields` (*eg. heartbeat*)
- `Med'm message`: `0.35 KB` message with `30 fields` (*eg. execution report*)
- `Large message`: `1.00 KB` message with `90 fields` (*eg. market data incremental refresh*)

> For more detailed benchmarks, see the `.bench` folder in the repository root

### Usage

``` csharp
var engine = new Engine();
var configuration = new Configuration
{
    Role = Role.Initiator,
    Version = "FIX.4.2",
    Host = "127.0.0.1",
    Port = 1234,
    Sender = "Client",
    Target = "Server",
    InboundSeqNum = 1,
    OutboundSeqNum = 1,
    HeartbeatInterval = 5
};

using (var session = engine.Open(configuration))
{
    session.Logon();

    while (session.Active)
    {
        if (session.Receive())
        {
            Console.WriteLine($"Received message from {session.Inbound[49].AsString}");
        }
    }
}
```

> For a more in depth example, see `HotFix\Program.cs`