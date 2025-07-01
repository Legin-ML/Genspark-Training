FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 7226

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
ENTRYPOINT ["dotnet", "TrueFeedback.dll"]
