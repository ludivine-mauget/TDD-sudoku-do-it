#!/bin/bash

# Script de démarrage du client Blazor WebAssembly
# Ce script arrête les instances existantes et démarre le client sur un port disponible

echo "=========================================="
echo "Démarrage du client Sudoku Blazor"
echo "=========================================="
echo ""

# Tuer les processus existants du client
echo "1. Arrêt des instances existantes..."
pkill -f "Sudoku.Client" 2>/dev/null
sleep 2

# Se déplacer vers le répertoire du client
cd "$(dirname "$0")/Sudoku.Client" || exit 1

# Démarrer le client
echo "2. Démarrage du client Blazor..."
echo ""

dotnet run

echo ""
echo "Client arrêté."

