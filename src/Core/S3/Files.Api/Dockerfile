FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Core/S3/Files.Api/Files.Api.csproj", "src/Core/S3/Files.Api/"]
RUN dotnet restore "src/Core/S3/Files.Api/Files.Api.csproj"
COPY . .
WORKDIR "/src/src/Core/S3/Files.Api"
RUN dotnet build "Files.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Files.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Files.Api.dll"]
