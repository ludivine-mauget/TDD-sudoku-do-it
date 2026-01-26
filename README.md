# 🎮 Sudoku - Application Complète

Application Sudoku complète avec générateur, solveur, API REST et interface web Blazor WebAssembly, développée en **Test-Driven Development (TDD)**.

---

## 📦 Installation

```bash
# Cloner le projet
git clone git@github.com:ludivine-mauget/TDD-sudoku-do-it.git
cd TDD-sudoku-do-it
```

---

## 🚀 Démarrage Rapide

### Option 1 : Application Web Complète (Recommandé)

```bash
# Script automatique (démarre API + Client)
./start-full-app.sh
```

**URLs disponibles :**
- 🌐 **Client Blazor** : `http://localhost:5xxx` (voir logs)
- 🔌 **API** : `http://localhost:5000`

**Logs :**
```bash
tail -f api.log     # Logs de l'API
tail -f client.log  # Logs du client
```

### Option 2 : Démarrage Manuel

```bash
# Terminal 1 - API
cd Sudoku.API
dotnet run --urls "http://localhost:5000"

# Terminal 2 - Client Blazor
cd Sudoku.Client
dotnet run
```

Ensuite, ouvrez votre navigateur à l'URL affichée.

### Option 3 : Application Console

```bash
# Lancer l'application console interactive
dotnet run --project Sudoku
```

### 🧪 Lancer les Tests

```bash
dotnet test
```


---

## ✨ Fonctionnalités

### 🌐 Application Web (Blazor WASM)

- 🎨 **Interface interactive** : Cliquez pour modifier les cellules
- 🎲 **4 niveaux de difficulté** : Facile, Moyen, Difficile, Expert
- 💡 **Résolution automatique** : Obtenez la solution instantanément
- ✅ **Validation** : Vérifiez votre solution en temps réel
- ↶↷ **Undo/Redo** : Annulez et refaites vos mouvements
- ⏱️ **Chronomètre** : Suivez votre temps de jeu
- 📱 **Responsive** : Jouez sur mobile, tablette ou desktop

### 🖥️ Application Console

- 🎲 **Générer un puzzle** : Créez une grille Sudoku avec solution unique
- 🧩 **Générer et résoudre** : Créez un puzzle puis résolvez-le automatiquement
- 🔄 **Comparaison** : Affichez le puzzle et sa solution côte à côte
- 🎯 **Difficulté personnalisable** : Choisissez le nombre de cellules vides (20-60+)

![Application Console](image-1.png)

### 🔌 API REST

- `POST /api/sudoku/generate` - Génère une nouvelle grille
- `POST /api/sudoku/solve` - Résout une grille
- `POST /api/sudoku/validate` - Valide une grille

---

## 🏗️ Architecture

```plaintext
Sudoku/
├── Sudoku/              # Bibliothèque de base
│   ├── Models/          # Structures de données (Cell, Grid)
│   ├── Solver/          # Algorithme de résolution (backtracking)
│   ├── Generator/       # Génération de grilles
│   └── Constants/       # Constantes de la grille
│
├── Sudoku.API/          # API REST (ASP.NET Core)
│   ├── Controllers/     # Endpoints API
│   ├── Services/        # Logique métier
│   ├── DTOs/            # Objets de transfert
│   └── Extensions/      # Extensions utilitaires
│
├── Sudoku.Client/       # Client Blazor WebAssembly
│   ├── Components/      # Composants Razor réutilisables
│   ├── Pages/           # Pages de l'application
│   ├── Services/        # API Client + Game State
│   └── wwwroot/         # Ressources statiques (CSS, images)
│
└── Sudoku.Tests/        # Tests unitaires
    ├── CellTests.cs
    ├── GridTests.cs
    ├── SolverTests.cs
    ├── GeneratorTests.cs
    ├── ValidationTests.cs
    ├── Helpers/
    └── TestData/
```

---

## 🎓 Développement TDD

La méthodologie **Test-Driven Development** a été appliquée pour le développement du **backend** (bibliothèque de base), notamment pour le **solveur** et le **générateur** de grilles :

1. **Red** 🔴 : Écriture des tests avant le code
2. **Green** 🟢 : Implémentation minimale pour passer les tests
3. **Refactor** 🔵 : Amélioration et optimisation du code

### Exemples de TDD appliqués

- Tests de validation (IsValid, IsComplete, IsSolved) écrits avant l'implémentation
- Tests d'exceptions avant la gestion des erreurs
- Tests de génération avec solution unique avant l'algorithme

---

## 🔧 Technologies

- **C# / .NET 9.0**
- **ASP.NET Core** pour l'API REST
- **Blazor WebAssembly** pour le client web
- **xUnit** pour les tests unitaires
- **Backtracking** pour la résolution
- **Algorithmes récursifs** pour la génération

---

## 🧪 Tests

Suite de tests complète avec **65 tests unitaires** couvrant :

- Manipulation des cellules et grilles
- Validation des règles du Sudoku
- Résolution de puzzles
- Génération de grilles

```bash
# Lancer tous les tests
dotnet test
```

---

## 📊 Projet

Ce projet a été réalisé dans le cadre de plusieurs **POK** et **MON** à Centrale Méditerranée :

### 📌 POK 1 - Test-Driven Development
- **Objectif** : Mettre en pratique le TDD
- **Réalisations** : Modélisation des données, solveur et générateur de grilles
- **Durée** : ~10 heures

### 📌 POK 2 - API & Blazor WebAssembly
- **Objectif** : Développer une API REST et découvrir Blazor WebAssembly
- **Réalisations** : API ASP.NET Core + Interface web interactive
- **Durée** : ~10 heures

### 📌 MON 2 - CI/CD avec Docker & GitHub Actions
- **Objectif** : Explorer la CI/CD
- **Réalisations** : Conteneurisation avec Docker, pipeline GitHub Actions
- **Durée** : ~10 heures

### 👩‍💻 Auteur

Ludivine Mauget - 3ème année à Centrale Méditerranée - Option Do_IT
