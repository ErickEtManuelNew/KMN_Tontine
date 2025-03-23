FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Installer openssl pour générer certificat
RUN apt-get update && apt-get install -y openssl

# Générer certificat auto-signé (valide 365 jours)
RUN mkdir /https && \
    openssl req -x509 -nodes -days 365 \
    -newkey rsa:2048 \
    -keyout /https/dev.key \
    -out /https/dev.crt \
    -subj "/CN=localhost"

# Convertir en PFX
RUN openssl pkcs12 -export \
    -out /https/devcert.pfx \
    -inkey /https/dev.key \
    -in /https/dev.crt \
    -passout pass:SuperSecret!

# Définir les variables utilisées par Kestrel pour charger le certificat
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/devcert.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=SuperSecret!

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
# Copie du certificat
#COPY ["KMN_Tontine.API/devcert.pfx", "https/"]
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

# Copier les fichiers publiés et le certificat dans le conteneur final
COPY --from=publish /app/publish .
COPY --from=base /https /https

ENTRYPOINT ["dotnet", "KMN_Tontine.API.dll"] 