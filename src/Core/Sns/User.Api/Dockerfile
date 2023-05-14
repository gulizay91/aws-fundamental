FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Core/Sns/User.Api/User.Api.csproj", "src/Core/Sns/User.Api/"]
COPY ["src/Shared/Infrastructure/Infrastructure.csproj", "src/Shared/Infrastructure/"]
COPY ["src/Shared/Contracts/Contracts.csproj", "src/Shared/Contracts/"]
RUN dotnet restore "src/Core/Sns/User.Api/User.Api.csproj"
COPY . .
WORKDIR "/src/src/Core/Sns/User.Api"
RUN dotnet build "User.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "User.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "User.Api.dll"]
