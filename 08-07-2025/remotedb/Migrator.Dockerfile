FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Development

RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

WORKDIR /src
COPY ["remotedb.csproj", "./"]
RUN dotnet restore "remotedb.csproj"
COPY . .

RUN dotnet build "remotedb.csproj" -c $BUILD_CONFIGURATION -o /app/build

WORKDIR /src
ENTRYPOINT ["dotnet", "ef", "database", "update", "--project", "remotedb.csproj"]
