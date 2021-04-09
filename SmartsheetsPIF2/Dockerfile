#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["SmartsheetsPIF2/SmartsheetsPIF.csproj", "SmartsheetsPIF2/"]
RUN dotnet restore "SmartsheetsPIF2/SmartsheetsPIF.csproj"
COPY . .
WORKDIR "/src/SmartsheetsPIF2"
RUN dotnet build "SmartsheetsPIF.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SmartsheetsPIF.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartsheetsPIF.dll"]
