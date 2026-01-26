#!/bin/bash

# Script pour démarrer l'API et le client Blazor simultanément

echo "🎮 Démarrage de l'application Sudoku complète..."
echo ""

# Fonction pour arrêter les processus au Ctrl+C
cleanup() {
    echo ""
    echo "🛑 Arrêt de l'application..."
    kill $API_PID $CLIENT_PID 2>/dev/null
    exit 0
}

trap cleanup SIGINT SIGTERM

# Démarrer l'API en arrière-plan
echo "🚀 Démarrage de l'API Sudoku sur http://localhost:5000..."
cd Sudoku.API
dotnet run --urls "http://localhost:5000" > ../api.log 2>&1 &
API_PID=$!
cd ..

# Attendre que l'API soit prête
echo "⏳ Attente du démarrage de l'API..."
sleep 5

# Démarrer le client Blazor en arrière-plan
echo "🚀 Démarrage du client Blazor..."
cd Sudoku.Client
dotnet run > ../client.log 2>&1 &
CLIENT_PID=$!
cd ..

echo ""
echo "✅ Application démarrée avec succès!"
echo ""
echo "📝 Informations:"
echo "   - API:    http://localhost:5050"
echo "   - Client: Vérifiez client.log pour l'URL"
echo ""
echo "📋 Logs:"
echo "   - API:    tail -f api.log"
echo "   - Client: tail -f client.log"
echo ""
echo "⚠️  Appuyez sur Ctrl+C pour arrêter l'application"
echo ""

# Afficher les logs en temps réel
tail -f api.log client.log &
TAIL_PID=$!

# Attendre indéfiniment
wait $API_PID $CLIENT_PID

