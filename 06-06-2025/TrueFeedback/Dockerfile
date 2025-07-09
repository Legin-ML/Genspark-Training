FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#USER $APP_UID
WORKDIR /app

# Install openssl to generate certificates
RUN apt-get update && apt-get install -y openssl

# Generate self-signed certificate
RUN mkdir /https && \
    openssl req -x509 -nodes -days 365 \
      -newkey rsa:2048 \
      -keyout /https/aspnetcore.key \
      -out /https/aspnetcore.crt \
      -subj "/CN=localhost" && \
    openssl pkcs12 -export \
      -out /https/aspnetcore.pfx \
      -inkey /https/aspnetcore.key \
      -in /https/aspnetcore.crt \
      -passout pass:password

EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TrueFeedback.csproj", "./"]
RUN dotnet restore "TrueFeedback.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "./TrueFeedback.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TrueFeedback.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ARG BUILD_CONFIGURATION=Release
ENV ASPNETCORE_ENVIRONMENT=$BUILD_CONFIGURATION
ENV ASPNETCORE_URLS="http://+:8080;https://+:8081"
ENTRYPOINT ["dotnet", "TrueFeedback.dll"]
