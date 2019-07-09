FROM microsoft/dotnet:2.1-runtime-deps-bionic AS base

FROM microsoft/dotnet:2.1-sdk-alpine AS builder
ENV NUGET_XMLDOC_MODE skip
COPY . /src
RUN dotnet publish -c Release -r ubuntu.18.04-x64 -o /app /src/HotFix.Demo.Initiator/HotFix.Demo.Initiator.csproj

FROM base AS production
COPY --from=builder /app /app

ENTRYPOINT ["/app/HotFix.Demo.Initiator"]
CMD ["127.0.0.1", "12345", "10000"]