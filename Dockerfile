FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base

WORKDIR /app
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY /ToX/ToX.csproj ToX/
RUN dotnet restore "ToX/ToX.csproj"

COPY . .

WORKDIR "/src/ToX"

RUN ls -l

RUN dotnet build "ToX.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ToX.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

WORKDIR /src
RUN ls -l
RUN apt-get update && DEBIAN_FRONTEND=noninteractive apt-get install -y ca-certificates curl
RUN curl -L -o vectors.bin -v 'https://dwqi5g.db.files.1drv.com/y4migN20BQnc2f-AKEJKebqTVQxpuOTU-sLP3maLnrkCNbZxn8a68konTe2PWnutfgYoMETHmY6jVKbO6KZ2ODVsn1V2EhzJyu_qAMYo-DmP7fD3gAhVR1IJbsq2n3Xz8i0JAvODZu9lR0TJWtQnARqcCmNvBjqABwID6P9HEexEay6VNcfai4i4eu5tbQ4jNaOnfRlVIAeOVOzZ_x8tyaxyA' 
RUN ls -l
ENV VECTOR_BIN=/src/vectors.bin

WORKDIR /app
RUN ls -l
ENTRYPOINT ["dotnet", "ToX.dll", "--urls", "http://0.0.0.0:5000", "--host", "0.0.0.0", "--port", "5000"]