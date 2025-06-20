FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Services/ApprovalService/Volvo.Service.Approval.API.csproj", "src/Services/ApprovalService/"]
RUN dotnet restore "src/Services/ApprovalService/Volvo.Service.Approval.API.csproj"

COPY src/Services/ApprovalService/ ./src/Services/ApprovalService/
WORKDIR "/src/Services/ApprovalService"
RUN dotnet build "Volvo.Service.Approval.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Volvo.Service.Approval.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApprovalService.dll"]