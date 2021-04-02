FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /test

COPY *.sln .
COPY src/BalanceScaleSelenium/*.csproj ./src/BalanceScaleSelenium/
RUN dotnet restore

# Copy full solution over
COPY . .
WORKDIR /test/src/BalanceScaleSelenium
RUN dotnet build

# Run the selenium tests
FROM build AS final
WORKDIR /test/src/BalanceScaleSelenium
ENTRYPOINT ["dotnet", "test", "--logger:trx"]