FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY /ToX/ToX.csproj ToX/
RUN dotnet restore "ToX/ToX.csproj"

COPY . .

WORKDIR "/src/ToX"

RUN apt-get update && DEBIAN_FRONTEND=noninteractive apt-get install -y ca-certificates curl
RUN  curl -O 'https://dwqi5g.db.files.1drv.com/y4mJZ0MBu23SvZupYnJ0alRGwyhIObwcv_x_5XsCSAFH8JcVegG3I88y3WOOmUM7JvDluzYF22x7FJVWZk4pU04lB7oHhHBKXVSeoX4BSfC3NnI_YN30EleaTYf5mOrYYBQ-Kqfh8mev67HVKMQFZqwvU8FQzFA5u2g_TobOdyhQZ1rtjl8AMEyiAPHW5_pfbN7/GoogleNews-vectors-negative300.bin?download&psid=1' \
               -H 'authority: dwqi5g.db.files.1drv.com' \
               -H 'accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7' \
               -H 'accept-language: en-US,en;q=0.9' \
               -H 'dnt: 1' \
               -H 'sec-ch-ua: "Microsoft Edge";v="117", "Not;A=Brand";v="8", "Chromium";v="117"' \
               -H 'sec-ch-ua-mobile: ?0' \
               -H 'sec-ch-ua-platform: "Windows"' \
               -H 'sec-fetch-dest: document' \
               -H 'sec-fetch-mode: navigate' \
               -H 'sec-fetch-site: none' \
               -H 'sec-fetch-user: ?1' \
               -H 'upgrade-insecure-requests: 1' \
               -H 'user-agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/117.0.0.0 Safari/537.36 Edg/117.0.2045.60' \
               --compressed

RUN dotnet build "ToX.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ToX.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "ToX.dll", "--urls", "http://0.0.0.0:5000", "--host", "0.0.0.0", "--port", "5000"]