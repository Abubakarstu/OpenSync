FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/OpenSync.Api/OpenSync.Api.csproj", "src/OpenSync.Api/"]
COPY ["src/OpenSync.Application/OpenSync.Application.csproj", "src/OpenSync.Application/"]
COPY ["src/OpenSync.Core/OpenSync.Core.csproj", "src/OpenSync.Core/"]
COPY ["src/OpenSync.Infrastructure/OpenSync.Infrastructure.csproj", "src/OpenSync.Infrastructure/"]
RUN dotnet restore "src/OpenSync.Api/OpenSync.Api.csproj"
COPY . .
WORKDIR "/src/src/OpenSync.Api"
RUN dotnet build "OpenSync.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OpenSync.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OpenSync.Api.dll"]
