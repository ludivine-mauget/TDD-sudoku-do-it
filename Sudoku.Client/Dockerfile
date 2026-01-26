# Dockerfile pour Sudoku.Client (Blazor WebAssembly)
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copier les fichiers de projet pour la restauration des dépendances
COPY ["Sudoku.Client/Sudoku.Client.csproj", "Sudoku.Client/"]
COPY ["Sudoku/Sudoku.csproj", "Sudoku/"]

# Restaurer les dépendances
RUN dotnet restore "Sudoku.Client/Sudoku.Client.csproj"

# Copier le reste du code source
COPY Sudoku.Client/ Sudoku.Client/
COPY Sudoku/ Sudoku/

# Build de l'application
WORKDIR "/src/Sudoku.Client"
RUN dotnet build "Sudoku.Client.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Sudoku.Client.csproj" -c Release -o /app/publish

# Runtime stage - Utiliser nginx pour servir les fichiers statiques Blazor WASM
FROM nginx:alpine AS final

# Copier la configuration nginx personnalisée
COPY Sudoku.Client/nginx.conf /etc/nginx/nginx.conf

# Copier les fichiers publiés dans le répertoire nginx
COPY --from=publish /app/publish/wwwroot /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]

