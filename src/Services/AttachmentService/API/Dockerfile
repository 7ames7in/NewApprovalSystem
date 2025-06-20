FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/Services/AttachmentService/Volvo.Service.Attachment.API.csproj", "src/Services/AttachmentService/"]
RUN dotnet restore "src/Services/AttachmentService/Volvo.Service.Attachment.API.csproj"

COPY src/Services/AttachmentService/ ./src/Services/AttachmentService/
WORKDIR "/src/Services/AttachmentService"
RUN dotnet build "Volvo.Service.Attachment.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Volvo.Service.Attachment.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AttachmentService.dll"]