FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY /ToX/ToX.csproj ToX/
RUN dotnet restore "ToX/ToX.csproj"

COPY . .

WORKDIR "/src/ToX"
RUN dotnet build "ToX.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ToX.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

WORKDIR /
RUN ls -alh
COPY .env /
WORKDIR /app

ENTRYPOINT ["dotnet", "ToX.dll", "--urls", "http://0.0.0.0:5000", "--host", "0.0.0.0", "--port", "5000"]