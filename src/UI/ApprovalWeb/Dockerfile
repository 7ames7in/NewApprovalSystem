FROM mcr.microsoft.com/dotnet/aspnet:9.0-nanoserver-1809 AS base
WORKDIR /app
EXPOSE 5229

ENV ASPNETCORE_URLS=http://+:5229

FROM mcr.microsoft.com/dotnet/sdk:9.0-nanoserver-1809 AS build
ARG configuration=Release
WORKDIR /src
COPY ["NewApprovalSystem/src/UI/ApprovalWeb/ApprovalWeb.csproj", "NewApprovalSystem/src/UI/ApprovalWeb/"]
RUN dotnet restore "NewApprovalSystem\src\UI\ApprovalWeb\ApprovalWeb.csproj"
COPY . .
WORKDIR "/src/NewApprovalSystem/src/UI/ApprovalWeb"
RUN dotnet build "ApprovalWeb.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "ApprovalWeb.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApprovalWeb.dll"]
