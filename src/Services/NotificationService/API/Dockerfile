FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Services/NotificationService/Volvo.Service.Notification.API.csproj.API.csproj", "src/Services/NotificationService/"]
RUN dotnet restore "src/Services/NotificationService/Volvo.Service.Notification.API.csproj.API.csproj"

COPY src/Services/NotificationService/ ./src/Services/NotificationService/
WORKDIR "/src/Services/NotificationService"
RUN dotnet build "Volvo.Service.Notification.API.csproj.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Volvo.Service.Notification.API.csproj.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotificationService.dll"]