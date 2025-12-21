#!/bin/bash
# Script de démarrage de l'API Sudoku

echo "🎮 Démarrage de l'API Sudoku..."
echo ""

cd "$(dirname "$0")/Sudoku.API"

echo "📦 Build du projet..."
dotnet build --verbosity quiet

if [ $? -eq 0 ]; then
    echo "✅ Build réussi !"
    echo ""
    echo "🚀 Lancement de l'API..."
    echo "📍 Swagger UI: http://localhost:5000"
    echo "📍 API Base URL: http://localhost:5000/api"
    echo ""
    dotnet run
else
    echo "❌ Erreur lors du build"
    exit 1
fi

