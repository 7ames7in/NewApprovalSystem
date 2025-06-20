# 이 Dockerfile은 .NET 8.0 ASP.NET Core 웹 애플리케이션을 컨테이너화하기 위한 것입니다.
# 
# 1. base 스테이지:
#    - mcr.microsoft.com/dotnet/aspnet:8.0 이미지를 기반으로 하며, 실제 앱이 실행될 환경을 제공합니다.
#    - WORKDIR /app: 작업 디렉터리를 /app으로 설정합니다.
#    - EXPOSE 8080: 컨테이너가 8080 포트를 리스닝하도록 지정합니다. (외부 접근을 위해 필요)
#
# 2. build 스테이지:
#    - mcr.microsoft.com/dotnet/sdk:8.0 이미지를 사용하여 빌드 도구를 제공합니다.
#    - WORKDIR /src: 소스 코드 작업 디렉터리 설정.
#    - COPY ["src/UI/ApprovalWeb/Volvo.Web.Approval.csproj", "."]: 프로젝트 파일만 복사하여 종속성 캐싱을 최적화합니다.
#    - RUN dotnet restore "Volvo.Web.Approval.csproj": NuGet 패키지 복원. (빌드 속도 향상 및 계층 캐싱)
#    - COPY src/UI/ApprovalWeb/ ./: 나머지 소스 코드 복사.
#    - RUN dotnet build ...: 프로젝트를 Release 모드로 빌드합니다.
#
# 3. publish 스테이지:
#    - build 스테이지에서 파생.
#    - RUN dotnet publish ...: 최적화된 배포용 파일을 생성합니다. (UseAppHost=false로 리눅스 호환성 향상)
#
# 4. final 스테이지:
#    - base 스테이지에서 파생.
#    - WORKDIR /app: 실행 디렉터리 설정.
#    - COPY --from=publish ...: 빌드 결과물만 복사하여 이미지 크기를 최소화합니다.
#    - ENTRYPOINT ...: 컨테이너 시작 시 애플리케이션을 실행합니다.
#
# 이렇게 멀티스테이지 빌드를 사용하면, 최종 이미지는 실행에 필요한 파일만 포함하여 보안성과 효율성을 높일 수 있습니다.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Volvo.Web.Approval.csproj", "."]
RUN dotnet restore "Volvo.Web.Approval.csproj"

COPY . .
RUN dotnet build "Volvo.Web.Approval.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Volvo.Web.Approval.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Volvo.Web.Approval.dll"]