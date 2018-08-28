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
COPY HotFix.Demo.Initiator/bin/scd /bin/hotfix/
CMD ["./bin/hotfix/HotFix.Demo.Initiator", "127.0.0.1", "12345", "100000"]