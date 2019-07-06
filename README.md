# Hotfix

[![NuGet](https://img.shields.io/nuget/v/HotFix.svg?style=flat-square)](https://www.nuget.org/packages/Hotfix/) [![AppVeyor](https://img.shields.io/appveyor/ci/fedjabosnic/hotfix.svg?style=flat-square)](https://ci.appveyor.com/project/fedjabosnic/hotfix/history) [![Maintainability](https://api.codeclimate.com/v1/badges/d96a4242f70e61da0b84/maintainability)](https://codeclimate.com/github/fedjabosnic/hotfix/maintainability)

Hotfix is a **FIX engine** written in pure .net geared towards **low latency** and **high throughput**.

The engine is _highly optimized_ and _garbage free_, exhibiting predictably low latency even to the higher percentiles. Messages can be encoded/decoded in **< 1 μs** and over loopback the engine can achieve a median **12 μs** round trip time...

HotFix supports both `dotnet core 2.0` and `.net framework 4.7.2`...

## Install

The library is available via Nuget as package [HotFix](https://www.nuget.org/packages/Hotfix/)

#### Frameworks
- **Version 0.5.0** and higher:
    - `dotnet standard 2.0`
    - `dotnet framework 4.7.2`
- **Version 0.4.0** and lower:
    - `dotnet framework 4.6.1`

## Usage

For detailed examples of usage, see [HotFix.Demo.Acceptor/Program.cs](https://github.com/fedjabosnic/hotfix/tree/master/HotFix.Demo.Acceptor/Program.cs) and [HotFix.Demo.Initiator/Program.cs](https://github.com/fedjabosnic/hotfix/tree/master/HotFix.Demo.Initiator/Program.cs).

#### Usage example

``` csharp
var engine = new Engine();
var configuration = new Configuration
{
    Role = Role.Initiator,
    Version = "FIX.4.2",
    Host = "127.0.0.1",
    Port = 12345,
    Sender = "Client",
    Target = "Server",
    InboundSeqNum = 1,
    OutboundSeqNum = 1,
    HeartbeatInterval = 5
};

engine.Run
(
    configuration,
    logon: (session) => Console.WriteLine("Logged on"),
    logout: (session) => Console.WriteLine("Logged out"),
    inbound: (session, message) => Console.WriteLine($"Inbound message of type {session.Inbound[35].AsString}"),
    outbound: (session, message) => Console.WriteLine($"Outbound message of type {session.Inbound[35].AsString}")
);


```

For persistent sessions, the inbound and outbound sequence numbers can be stored and reused.

#### Engine settings

There are some additional engine settings that can be customized to further tweak performance:

```csharp
var engine = new Engine
{
    BufferSize = 8192,       // Data transmission buffer size - default 65536
    MaxMessageLength = 8192, // Maximum expected length of messages - default 4096
    MaxMessageFields = 2048, // Maximum expected number of fields in messages - default 1024
};
```

#### Logging

HotFix comes with a highly optimized built-in logging engine, which can be easily enabled by specifying a log file name:

```csharp
var configuration = new Configuration
{
    // ...
    LogFile = "messages.log"
};
```

#### Session schedules

It is possible to setup simple session schedules using the built-in configuration:

```csharp
var configuration = new Configuration
{
    // ...
    Schedules = new List<Schedule>
    {
        new Schedule
        {
            Name = "MON",
            OpenDay = DayOfWeek.Monday, OpenTime = TimeSpan.Parse("09:00:00"),
            CloseDay = DayOfWeek.Monday, CloseTime = TimeSpan.Parse("19:00:00")
        },
        new Schedule
        {
            Name = "TUE",
            OpenDay = DayOfWeek.Tuesday, OpenTime = TimeSpan.Parse("09:00:00"),
            CloseDay = DayOfWeek.Tuesday, CloseTime = TimeSpan.Parse("19:00:00")
        },
        // ...
    }
};
```

*It should be noted that all times are in UTC - session management in custom timezones is currently not provided out of the box.*

## Performance

Performance depends on _hardware_ and _operating system_ characteristics so we recommended you run the [benchmark suite](https://github.com/fedjabosnic/hotfix/tree/master/HotFix.Benchmark) on your own hardware or even better run the benchmark [acceptor](https://github.com/fedjabosnic/hotfix/tree/master/HotFix.Demo.Acceptor) and [initiator](https://github.com/fedjabosnic/hotfix/tree/master/HotFix.Demo.Initiator) applications...

### Round trip benchmark

This benchmark shows the round trip time for trading by running the [acceptor](https://github.com/fedjabosnic/hotfix/tree/master/HotFix.Demo.Acceptor/Program.cs) and [initiator](https://github.com/fedjabosnic/hotfix/tree/master/HotFix.Demo.Initiator/Program.cs) benchmark apps over loopback on a Windows development desktop...

![image](https://user-images.githubusercontent.com/1388990/28753994-8829666e-7534-11e7-8a45-85c892d84c4f.png)

Below is a more detailed analysis of the same run:

![image](https://user-images.githubusercontent.com/1388990/28753622-1c451a24-752f-11e7-8f1d-a008a6566af1.png)

### Message encoding/decoding benchmarks

These benchmarks show the latency for encoding and decoding FIX messages of varying sizes.

- `Small message`: `0.10 KB` message with `10 fields` (*eg. heartbeat*)
- `Med'm message`: `0.35 KB` message with `30 fields` (*eg. execution report*)
- `Large message`: `1.00 KB` message with `90 fields` (*eg. market data incremental refresh*)

#### Encode

 | Message size |      Min |   **Median** |      Max |     Mean |     StdDev |         **Op/s** | Allocated |
 |------------- |---------:|-------------:|---------:|---------:|-----------:|-----------------:|----------:|
 | Small        | 0.270 μs | **0.273 μs** | 0.275 μs | 0.273 μs |  1.4298 ns | **3,661,402.84** |      0 kB |
 | Medium       | 1.277 μs | **1.283 μs** | 1.293 μs | 1.283 μs |  4.5345 ns |   **779,363.04** |      0 kB |
 | Large        | 2.831 μs | **2.849 μs** | 2.856 μs | 2.847 μs |  8.4264 ns |   **351,238.64** |      0 kB |

[/.bench/writing_messages-report-github.md](https://github.com/fedjabosnic/hotfix/blob/master/.bench/writing_messages-report-github.md)

#### Decode

 | Message size |      Min |   **Median** |      Max |     Mean |     StdDev |         **Op/s** | Allocated |
 |------------- |---------:|-------------:|---------:|---------:|-----------:|-----------------:|----------:|
 | Small        | 0.408 μs | **0.413 μs** | 0.417 μs | 0.413 μs |  2.1911 ns | **2,423,590.30** |      0 kB |
 | Medium       | 1.256 μs | **1.264 μs** | 1.280 μs | 1.267 μs |  8.0424 ns |   **789,294.37** |      0 kB |
 | Large        | 3.250 μs | **3.270 μs** | 3.281 μs | 3.269 μs |  8.8091 ns |   **305,940.01** |      0 kB |

[/.bench/parsing_messages-report-github.md](https://github.com/fedjabosnic/hotfix/blob/master/.bench/parsing_messages-report-github.md)

### Other benchmarks

For more detailed benchmarks, see the `.bench` folder in the repository root

## License

Copyright (C) 2017 Fedja Bosnic and contributors

Released under the [GNU Lesser General Public License V3](http://www.gnu.org/licenses/lgpl.html)

## Contributors

* [Fedja Bosnic](https://github.com/fedjabosnic)
* [Giorgos Flourentzos](https://github.com/GeorgeF0)
