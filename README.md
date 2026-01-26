# 🎮 Sudoku - Application Complète

Application Sudoku complète avec générateur, solveur, API REST et interface web Blazor WebAssembly, développée en **Test-Driven Development (TDD)**.

## 🚀 Démarrage Rapide

### Lancer l'application web complète

```bash
# Script automatique (démarre API + Client)
./start-full-app.sh
```

Ou manuellement :

```bash
# Terminal 1 - API
cd Sudoku.API
dotnet run

# Terminal 2 - Client Blazor
cd Sudoku.Client
dotnet run
```

Ensuite, ouvrez votre navigateur à l'URL affichée (généralement `http://localhost:5xxx`)

### Lancer l'application console

```bash
# Lancer l'application console interactive
dotnet run --project Sudoku

# Lancer les tests
dotnet test
```

📖 **Pour plus de détails, consultez [`QUICK_START.md`](QUICK_START.md)**

## ✨ Fonctionnalités

### 🌐 Application Web (Blazor WASM) - **NOUVEAU !**

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

## 🏗️ Architecture

```plaintext
Sudoku/
├── Sudoku/              # Bibliothèque de base
│   ├── Models/          # Structures de données (Cell, Grid)
│   ├── Solver/          # Algorithme de résolution (backtracking)
│   ├── Generator/       # Génération de grilles
│   └── Display/         # Affichage console
│
├── Sudoku.API/          # API REST (ASP.NET Core)
│   ├── Controllers/     # Endpoints API
│   ├── Services/        # Logique métier
│   └── DTOs/            # Objets de transfert
│
├── Sudoku.Client/       # Client Blazor WebAssembly ✨
│   ├── Components/      # Composants Razor réutilisables
│   ├── Pages/           # Pages de l'application
│   ├── Services/        # API Client + Game State
│   └── wwwroot/         # Ressources statiques (CSS, images)
│
└── Sudoku.Tests/        # Tests unitaires
    ├── GridTests.cs
    ├── SolverTests.cs
    └── GeneratorTests.cs
```
└── Program.cs       → Application console avec menu

Sudoku.Tests/
├── CellTests.cs
├── GridTests.cs
├── SolverTests.cs
├── GeneratorTests.cs
├── Helpers/
└── TestData/
```

## 🎓 Développement TDD

Ce projet a été développé en suivant la méthodologie **Test-Driven Development** :

1. **Red** 🔴 : Écriture des tests avant le code
2. **Green** 🟢 : Implémentation minimale pour passer les tests
3. **Refactor** 🔵 : Amélioration et optimisation du code

### Exemples de TDD appliqués

- Tests de validation (IsValid, IsComplete, IsSolved) écrits avant l'implémentation
- Tests d'exceptions avant la gestion des erreurs
- Tests de génération avec solution unique avant l'algorithme
- Tests d'affichage avant la création de GridDisplay

## 🔧 Technologies

- **C# / .NET 9.0**
- **xUnit** pour les tests unitaires
- **Backtracking** pour la résolution
- **Algorithmes récursifs** pour la génération

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

## 📊 Projet

Ce projet a été réalisé dans le cadre d'un **POK** pour mettre en pratique le **Test-Driven Development**.

- **Durée** : ~20 heures
- **MVP Initial** : Modélisation + Solver
- **Objectif Final** : Générateur avec solution unique
- **Bonus** : Interface console interactive

### 👩‍💻 Auteur

Ludivine Mauget - 3ème année à Centrale Méditerranée - Option Do_IT
