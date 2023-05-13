FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Core/Sqs/SqsConsumer/SqsConsumer.csproj", "src/Core/Sqs/SqsConsumer/"]
RUN dotnet restore "src/Core/Sqs/SqsConsumer/SqsConsumer.csproj"
COPY . .
WORKDIR "/src/src/Core/Sqs/SqsConsumer"
RUN dotnet build "SqsConsumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SqsConsumer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SqsConsumer.dll"]
