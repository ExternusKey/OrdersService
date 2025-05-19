FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["OrderService.Daemon/OrderService.Daemon.csproj", "OrderService.Daemon/"]
COPY ["Common/Common.csproj", "Common/"]
RUN dotnet restore "OrderService.Daemon/OrderService.Daemon.csproj"
COPY . .
WORKDIR "/src/OrderService.Daemon"
RUN dotnet build "OrderService.Daemon.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderService.Daemon.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderService.Daemon.dll"]
