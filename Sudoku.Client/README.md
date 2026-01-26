# Sudoku Blazor WebAssembly Client

Application web de jeu de Sudoku développée avec Blazor WebAssembly.

## Fonctionnalités

- ✅ Génération de grilles avec 4 niveaux de difficulté (Facile, Moyen, Difficile, Expert)
- ✅ Interface interactive pour remplir les cellules
- ✅ Résolution automatique de la grille
- ✅ Validation de la solution
- ✅ Réinitialisation de la grille
- ✅ Undo/Redo des mouvements
- ✅ Chronomètre de partie
- ✅ Design responsive

## Prérequis

- .NET 7.0 SDK ou supérieur
- Un navigateur web moderne

## Structure du projet

```
Sudoku.Client/
├── Components/           # Composants Razor réutilisables
│   ├── CellComponent.razor
│   ├── DifficultySelector.razor
│   ├── GameControls.razor
│   ├── SudokuGrid.razor
│   └── Timer.razor
├── Pages/               # Pages de l'application
│   └── Home.razor       # Page principale du jeu
├── Services/            # Services métier
│   ├── ISudokuApiClient.cs
│   ├── SudokuApiClient.cs
│   └── GameStateService.cs
└── wwwroot/
    └── css/
        └── sudoku.css   # Styles du jeu
```

## Lancement de l'application

### 1. Démarrer l'API backend

D'abord, assurez-vous que l'API Sudoku est en cours d'exécution :

```bash
# Depuis la racine du projet
cd Sudoku.API
dotnet run
```

L'API devrait être accessible sur `http://localhost:5050`

### 2. Démarrer le client Blazor

Dans un nouveau terminal :

**Option 1 : Avec le script (recommandé)**
```bash
# Depuis la racine du projet
./start-client.sh
```

**Option 2 : Manuellement**
```bash
# Depuis la racine du projet
cd Sudoku.Client
dotnet run
```

**Note** : Si vous êtes déjà dans le répertoire `Sudoku.Client`, utilisez simplement :
```bash
dotnet run
```

L'application sera accessible sur `http://localhost:5046` (ou un autre port indiqué dans la console)

### 3. Utiliser l'application

1. Ouvrez votre navigateur et accédez à l'URL affichée
2. Sélectionnez un niveau de difficulté
3. Cliquez sur une cellule vide pour la modifier
4. Utilisez les boutons pour :
   - **Résoudre** : Obtenir la solution complète
   - **Valider** : Vérifier votre solution
   - **Réinitialiser** : Revenir à la grille initiale
   - **Annuler/Refaire** : Naviguer dans l'historique des mouvements
   - **Nouveau Jeu** : Sélectionner une nouvelle difficulté

## Configuration

L'URL de l'API est configurée dans `Program.cs` :

```csharp
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5050") });
```

Si votre API tourne sur un port différent, modifiez cette ligne.

## Architecture

### Composants

- **CellComponent** : Représente une cellule individuelle de la grille
- **SudokuGrid** : Affiche la grille 9x9 complète
- **DifficultySelector** : Permet de choisir la difficulté
- **GameControls** : Boutons d'actions du jeu
- **Timer** : Affiche le temps écoulé

### Services

- **ISudokuApiClient / SudokuApiClient** : Communication avec l'API backend
- **GameStateService** : Gestion de l'état du jeu (grille, historique, cellules fixes)

### Gestion d'état

Le `GameStateService` maintient :
- La grille actuelle
- La grille initiale
- Les cellules fixes (non modifiables)
- L'historique des mouvements pour undo/redo
- Le statut du jeu (actif ou terminé)

## Styles

Les styles sont définis dans `wwwroot/css/sudoku.css` avec :
- Layout CSS Grid pour l'affichage de la grille
- Styles pour les différents états des cellules
- Design responsive pour mobile
- Animations et transitions

## Tests

Pour exécuter les tests :

```bash
cd Sudoku.Tests
dotnet test
```

## Prochaines améliorations possibles

- [ ] Sauvegarde locale des parties en cours
- [ ] Système d'indices intelligents
- [ ] Statistiques de jeu
- [ ] Mode multijoueur
- [ ] Personnalisation des thèmes
- [ ] Validation en temps réel avec indication des erreurs

## 🐳 Docker

### Lancement avec Docker Compose

Depuis la racine du projet :

```bash
# Construire et démarrer tous les services
docker compose up --build

# Ou en arrière-plan
docker compose up -d --build
```

L'application sera accessible sur :
- **Client** : http://localhost:8080
- **API** : http://localhost:5050

### Construction manuelle de l'image

```bash
# Depuis la racine du projet
docker build -t sudoku-client -f Sudoku.Client/Dockerfile .
docker run -p 8080:80 sudoku-client
```

