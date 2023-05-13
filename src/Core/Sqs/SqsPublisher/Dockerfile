FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Core/Sqs/SqsPublisher/SqsPublisher.csproj", "src/Core/Sqs/SqsPublisher/"]
COPY ["src/Shared/Contracts/Contracts.csproj", "src/Shared/Contracts/"]
COPY ["src/Shared/Infrastructure/Infrastructure.csproj", "src/Shared/Infrastructure/"]
RUN dotnet restore "src/Core/Sqs/SqsPublisher/SqsPublisher.csproj"
COPY . .
WORKDIR "/src/src/Core/Sqs/SqsPublisher"
RUN dotnet build "SqsPublisher.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SqsPublisher.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SqsPublisher.dll"]
