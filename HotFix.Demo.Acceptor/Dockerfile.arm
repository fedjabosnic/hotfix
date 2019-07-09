FROM microsoft/dotnet:2.1-runtime-deps-stretch-slim-arm32v7  AS base

FROM microsoft/dotnet:2.1-sdk-alpine AS builder
ENV NUGET_XMLDOC_MODE skip
COPY . /src
RUN dotnet publish -c Release -r linux-arm -o /app /src/HotFix.Demo.Acceptor/HotFix.Demo.Acceptor.csproj

FROM base AS production
COPY --from=builder /app /app

ENTRYPOINT ["/app/HotFix.Demo.Acceptor"]
CMD ["0.0.0.0", "12345"]