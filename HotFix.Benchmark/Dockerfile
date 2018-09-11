FROM microsoft/dotnet:2.1-sdk-bionic AS builder
WORKDIR /vsdbg
RUN apt-get update \
    && apt-get install -y --no-install-recommends unzip libunwind-dev \
    && rm -rf /var/lib/apt/lists/* \
    && curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg
    
ENV NUGET_XMLDOC_MODE skip
COPY . /src
RUN dotnet build -c Release /src/HotFix.Benchmark/HotFix.Benchmark.csproj

ENTRYPOINT ["dotnet", "run", "-c", "Release", "/src/HotFix.Benchmark/HotFix.Benchmark.csproj"]