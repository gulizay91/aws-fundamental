FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Core/S3/SimpleS3/SimpleS3.csproj", "src/Core/S3/SimpleS3/"]
RUN dotnet restore "src/Core/S3/SimpleS3/SimpleS3.csproj"
COPY . .
WORKDIR "/src/src/Core/S3/SimpleS3"
RUN dotnet build "SimpleS3.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SimpleS3.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimpleS3.dll"]
