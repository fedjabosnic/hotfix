FROM microsoft/dotnet:2.1-runtime-bionic AS base
WORKDIR /vsdbg
RUN apt-get update \
    && apt-get install -y --no-install-recommends unzip libunwind-dev \
    && rm -rf /var/lib/apt/lists/* \
    && curl -sSL https://aka.ms/getvsdbgsh | bash /dev/stdin -v latest -l /vsdbg

FROM microsoft/dotnet:2.1-sdk-bionic AS builder
ENV NUGET_XMLDOC_MODE skip
COPY . /src
RUN dotnet publish -c Release -r ubuntu.18.04-x64 -o /app /src/HotFix.Demo.Acceptor/HotFix.Demo.Acceptor.csproj

FROM base AS production
COPY --from=builder /app /app

ENTRYPOINT ["/app/HotFix.Demo.Acceptor"]
CMD ["0.0.0.0", "12345"]