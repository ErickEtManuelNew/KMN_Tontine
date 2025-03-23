FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Copie du certificat
COPY devcert.pfx /https/devcert.pfx

# Définir les variables d'environnement attendues par Kestrel
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/devcert.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=SuperSecret!

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["KMN_Tontine.API/KMN_Tontine.API.csproj", "KMN_Tontine.API/"]
COPY ["KMN_Tontine.Application/KMN_Tontine.Application.csproj", "KMN_Tontine.Application/"]
COPY ["KMN_Tontine.Domain/KMN_Tontine.Domain.csproj", "KMN_Tontine.Domain/"]
COPY ["KMN_Tontine.Infrastructure/KMN_Tontine.Infrastructure.csproj", "KMN_Tontine.Infrastructure/"]
RUN dotnet restore "KMN_Tontine.API/KMN_Tontine.API.csproj"
COPY . .
WORKDIR "/src/KMN_Tontine.API"
RUN dotnet build "KMN_Tontine.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KMN_Tontine.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KMN_Tontine.API.dll"] 