# Use the official .NET runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["SimpleGateway.csproj", "."]
RUN dotnet restore "./SimpleGateway.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "SimpleGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SimpleGateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimpleGateway.dll"]
