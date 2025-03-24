# Base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

# Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["KMN_Tontine.Blazor.UI/KMN_Tontine.Blazor.UI.csproj", "KMN_Tontine.Blazor.UI/"]
RUN dotnet restore "KMN_Tontine.Blazor.UI/KMN_Tontine.Blazor.UI.csproj"
COPY . .
WORKDIR "/src/KMN_Tontine.Blazor.UI"
RUN dotnet publish "KMN_Tontine.Blazor.UI.csproj" -c Release -o /app/publish

# Runtime final
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "KMN_Tontine.Blazor.UI.dll"]
