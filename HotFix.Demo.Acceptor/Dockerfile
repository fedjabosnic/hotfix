FROM ubuntu:bionic
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        ca-certificates \
        \
# .NET Core dependencies
        libc6 \
        libgcc1 \
        libgssapi-krb5-2 \
        libicu60 \
        liblttng-ust0 \
        libssl1.0.0 \
        libstdc++6 \
        libunwind-dev \
        zlib1g \
    && rm -rf /var/lib/apt/lists/*
COPY HotFix.Demo.Acceptor/bin/scd /bin/hotfix/
CMD ["./bin/hotfix/HotFix.Demo.Acceptor", "0.0.0.0", "12345"]