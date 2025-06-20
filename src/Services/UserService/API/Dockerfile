# 현재 Dockerfile 위치: /Users/7ames7in/Documents/GitHub/New-Projects/NewApprovalSystem/src/Services/UserService/API
# 
# 이 Dockerfile은 .NET 8.0 기반의 UserService.API 애플리케이션을 컨테이너로 빌드하고 실행하기 위한 설정입니다.
#
# 1. base 스테이지:
#    - mcr.microsoft.com/dotnet/aspnet:8.0 이미지를 기반으로 런타임 환경을 구성합니다.
#    - 작업 디렉터리를 /app으로 설정하고, 8080 포트를 노출합니다.
#
# 2. build 스테이지:
#    - mcr.microsoft.com/dotnet/sdk:8.0 이미지를 사용하여 빌드 환경을 구성합니다.
#    - 현재 Dockerfile이 위치한 폴더: src/Services/UserService/API
#    - 프로젝트 파일(UserService.API.csproj)을 복사한 후, 의존성을 복원합니다.
#    - 전체 소스를 복사하고, Release 모드로 빌드하여 /app/build에 출력합니다.
#
# 3. publish 스테이지:
#    - build 스테이지에서 빌드된 결과물을 기반으로, dotnet publish 명령어를 통해 최종 실행 파일을 /app/publish에 생성합니다.
#
# 4. final 스테이지:
#    - base 런타임 이미지를 기반으로, publish 스테이지에서 생성된 파일을 /app에 복사합니다.
#    - 컨테이너 시작 시 'dotnet UserService.API.dll' 명령어로 애플리케이션을 실행합니다.

# FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
# # 전체 경로: /Users/7ames7in/Documents/GitHub/New-Projects/NewApprovalSystem/src/Services/UserService/API/Dockerfile
# # /app 디렉터리를 컨테이너 내에서 작업 디렉터리(working directory)로 설정합니다.
# # 이후의 명령(COPY, RUN 등)은 이 디렉터리 기준으로 실행됩니다.
# WORKDIR /app
# # WORKDIR /app이 실행되면, Docker 컨테이너 안에서 작업 디렉터리가 /app으로 설정됩니다.
# # 구체적으로 어떤 일이 일어나는가?
# # 이후의 모든 명령(COPY, RUN, CMD 등)은 /app 디렉터리 기준으로 실행됩니다.
# # 만약 /app 디렉터리가 없다면, Docker가 자동으로 생성합니다.
# # 예시
# # 위 예시에서 COPY . .와 RUN ls는 모두 /app 디렉터리에서 실행됩니다.

# # 요약
# # 컨테이너 내부의 현재 작업 디렉터리가 /app으로 바뀐다.
# # 이후 명령들은 모두 /app 기준으로 동작한다.
# # /app 폴더가 없으면 자동 생성된다.
# # 이 설정은 프로젝트 파일을 한 곳에 모으고, 명령 실행 위치를 명확히 하기 위해 자주 사용됩니다.
# EXPOSE 8080

# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# WORKDIR /src
# # 현재 위치: src/Services/UserService/API
# COPY ["src/Services/UserService/API/UserService.API.csproj", "."]
# RUN dotnet restore "UserService.API.csproj"

# COPY . .
# WORKDIR "/src"
# RUN dotnet build "UserService.API.csproj" -c Release -o /app/build

# FROM build AS publish
# RUN dotnet publish "UserService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "UserService.API.dll"]



# base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# csproj 복사 + restore
COPY src/Services/UserService/API/UserService.API.csproj .
RUN dotnet restore "UserService.API.csproj"

# 전체 소스 복사
#COPY src/Services/UserService/API/. .
# 이 명령어는 빌드 컨텍스트(일반적으로 Dockerfile이 있는 디렉터리)에서 src/Services/UserService/API/ 폴더 안의 모든 파일과 폴더를 이미지의 작업 디렉터리(WORKDIR로 지정된 위치, 지정하지 않았다면 기본적으로 /)로 복사합니다.

# 첫 번째 . : 복사할 소스 디렉터리 (src/Services/UserService/API/의 모든 내용)
# 두 번째 . : 컨테이너 내부의 현재 작업 디렉터리(예: /app 등)
# 즉,
# src/Services/UserService/API/ 폴더 안의 모든 파일과 폴더가 컨테이너의 현재 디렉터리로 복사됩니다.

# 주의할 점:

# 컨테이너의 작업 디렉터리가 어디인지(예: WORKDIR /app 등) 확인하는 것이 중요합니다.
# COPY 명령은 숨김 파일(.env 등)도 포함합니다.
# 궁금한 점 있으면 추가로 질문해 주세요!
COPY src/Services/UserService/API/. .

# build
RUN dotnet build "UserService.API.csproj" -c Release -o /app/build

# publish
FROM build AS publish
RUN dotnet publish "UserService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserService.API.dll"]