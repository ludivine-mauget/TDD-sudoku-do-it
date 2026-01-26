# Dockerfile pour Sudoku.API
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copier les fichiers de projet pour la restauration des dépendances
COPY ["Sudoku.API/Sudoku.API.csproj", "Sudoku.API/"]
COPY ["Sudoku/Sudoku.csproj", "Sudoku/"]

# Restaurer les dépendances
RUN dotnet restore "Sudoku.API/Sudoku.API.csproj"

# Copier le reste du code source
COPY Sudoku.API/ Sudoku.API/
COPY Sudoku/ Sudoku/

# Build de l'application
WORKDIR "/src/Sudoku.API"
RUN dotnet build "Sudoku.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Sudoku.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Install curl for Docker health checks
RUN apt-get update && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*

EXPOSE 8080
EXPOSE 8081

# Copier les fichiers publiés
COPY --from=publish /app/publish .

# Définir le point d'entrée
ENTRYPOINT ["dotnet", "Sudoku.API.dll"]
