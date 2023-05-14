FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Core/Sns/SnsPublisher/SnsPublisher.csproj", "src/Core/Sns/SnsPublisher/"]
RUN dotnet restore "src/Core/Sns/SnsPublisher/SnsPublisher.csproj"
COPY . .
WORKDIR "/src/src/Core/Sns/SnsPublisher"
RUN dotnet build "SnsPublisher.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SnsPublisher.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SnsPublisher.dll"]
