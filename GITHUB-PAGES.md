# 🚀 Déploiement sur GitHub Pages

Ce document explique en détail le déploiement de l'application Blazor WebAssembly Sudoku sur GitHub Pages.

## 📋 Table des matières

- [Prérequis](#prérequis)
- [Architecture du déploiement](#architecture-du-déploiement)
- [Configuration GitHub Pages](#configuration-github-pages)
- [Explication du workflow](#explication-du-workflow)
- [Fichiers importants](#fichiers-importants)
- [Dépannage](#dépannage)

---

## Prérequis

- Un repository GitHub (public ou privé avec GitHub Pro)
- L'application Blazor WebAssembly fonctionnelle
- GitHub Actions activé sur le repository

---

## Architecture du déploiement

```
┌─────────────────┐     ┌──────────────────┐     ┌─────────────────┐
│   Push sur      │────▶│  GitHub Actions  │────▶│  GitHub Pages   │
│   main/master   │     │  (Build & Deploy)│     │  (Hébergement)  │
└─────────────────┘     └──────────────────┘     └─────────────────┘
```

**Blazor WebAssembly** est une application **100% côté client**. Une fois compilée, elle génère des fichiers statiques (HTML, CSS, JS, DLL) qui peuvent être hébergés sur n'importe quel serveur de fichiers statiques, comme GitHub Pages.

---

## Configuration GitHub Pages

### Étape 1 : Activer GitHub Pages

1. Allez sur votre repository GitHub
2. Cliquez sur **Settings** (⚙️)
3. Dans le menu latéral, cliquez sur **Pages**
4. Dans **"Build and deployment"** :
   - **Source** : Sélectionnez `GitHub Actions`

![Configuration GitHub Pages](image.png)

### Étape 2 : Pousser le code

```bash
git add .
git commit -m "Add GitHub Pages deployment"
git push origin main
```

### Étape 3 : Vérifier le déploiement

1. Allez dans l'onglet **Actions** de votre repository
2. Vérifiez que le workflow "Deploy to GitHub Pages" s'exécute
3. Une fois terminé, votre site est accessible à :

```
https://<votre-username>.github.io/<nom-du-repo>/
```

---

## Explication du workflow

Le fichier `.github/workflows/deploy-gh-pages.yml` contient toute la logique de déploiement.

### 🔧 Déclencheurs (`on`)

```yaml
on:
  push:
    branches: [ main, master ]  # Déclenché à chaque push sur main ou master
  workflow_dispatch:            # Permet le déclenchement manuel depuis GitHub
```

| Déclencheur | Description |
|-------------|-------------|
| `push` | Exécute le workflow automatiquement à chaque push |
| `workflow_dispatch` | Bouton "Run workflow" dans l'onglet Actions |

### 🔐 Permissions

```yaml
permissions:
  contents: read      # Lire le code source
  pages: write        # Écrire sur GitHub Pages
  id-token: write     # Authentification OIDC pour le déploiement
```

Ces permissions sont **obligatoires** pour utiliser `actions/deploy-pages`.

### 🚦 Concurrence

```yaml
concurrency:
  group: "pages"
  cancel-in-progress: false
```

Empêche plusieurs déploiements simultanés. Si un déploiement est en cours, les suivants attendent.

### 🏗️ Job `build`

#### 1. Checkout du code

```yaml
- name: Checkout
  uses: actions/checkout@v4
```

Clone le repository dans l'environnement de build.

#### 2. Installation de .NET

```yaml
- name: Setup .NET
  uses: actions/setup-dotnet@v4
  with:
    dotnet-version: '9.0.x'
```

Installe le SDK .NET 9.0 nécessaire pour compiler Blazor.

#### 3. Cache NuGet

```yaml
- name: Cache NuGet packages
  uses: actions/cache@v4
  with:
    path: ~/.nuget/packages
    key: ${{ runner.os }}-nuget-${{ hashFiles('**/*.csproj') }}
```

**Optimisation** : Met en cache les packages NuGet pour accélérer les builds suivants.

#### 4. Restauration des dépendances

```yaml
- name: Restore dependencies
  run: dotnet restore Sudoku.Client/Sudoku.Client.csproj
```

Télécharge tous les packages NuGet nécessaires.

#### 5. Publication de l'application

```yaml
- name: Publish Blazor WASM
  run: dotnet publish Sudoku.Client/Sudoku.Client.csproj -c Release -o release
```

| Option | Description |
|--------|-------------|
| `-c Release` | Mode Release (optimisé, minifié) |
| `-o release` | Dossier de sortie |

Génère les fichiers dans `release/wwwroot/`.

#### 6. Modification du `base href`

```yaml
- name: Change base href in index.html
  run: |
    REPO_NAME=$(echo "${{ github.repository }}" | cut -d'/' -f2)
    sed -i 's|<base href="/" />|<base href="/'"$REPO_NAME"'/" />|g' release/wwwroot/index.html
```

**Critique !** GitHub Pages héberge votre site à `/<nom-repo>/`, pas à la racine `/`.

| Avant | Après |
|-------|-------|
| `<base href="/" />` | `<base href="/Sudoku/" />` |

Sans cette modification, les ressources (CSS, JS, DLL) ne seraient pas trouvées.

#### 7. Fichier `.nojekyll`

```yaml
- name: Add .nojekyll file
  run: touch release/wwwroot/.nojekyll
```

GitHub Pages utilise Jekyll par défaut, qui **ignore les fichiers commençant par `_`**. Or, Blazor génère un dossier `_framework/` contenant les DLL.

Le fichier `.nojekyll` **désactive Jekyll** et permet de servir tous les fichiers.

#### 8. Fichier `404.html`

```yaml
- name: Create 404.html for SPA routing
  run: cp release/wwwroot/index.html release/wwwroot/404.html
```

Blazor utilise le **routing côté client**. Si l'utilisateur accède directement à `/game`, GitHub Pages renvoie une erreur 404.

En copiant `index.html` vers `404.html`, GitHub Pages charge l'application Blazor qui gère ensuite la route.

#### 9. Upload de l'artefact

```yaml
- name: Upload artifact for GitHub Pages
  uses: actions/upload-pages-artifact@v3
  with:
    path: release/wwwroot
```

Prépare les fichiers pour le déploiement.

### 🚀 Job `deploy`

```yaml
deploy:
  name: Deploy to GitHub Pages
  needs: build  # Attend que le build soit terminé
  runs-on: ubuntu-latest
  environment:
    name: github-pages
    url: ${{ steps.deployment.outputs.page_url }}
  steps:
    - name: Deploy to GitHub Pages
      id: deployment
      uses: actions/deploy-pages@v4
```

| Élément | Description |
|---------|-------------|
| `needs: build` | Dépendance : attend le job `build` |
| `environment` | Crée un environnement "github-pages" visible dans Settings |
| `outputs.page_url` | URL du site déployé |

---

## Fichiers importants

### Structure après publication

```
release/wwwroot/
├── _framework/           # Runtime Blazor et DLL
│   ├── blazor.boot.json
│   ├── blazor.webassembly.js
│   └── *.dll
├── css/
├── js/
├── index.html            # Point d'entrée
├── 404.html              # Copie pour SPA routing
└── .nojekyll             # Désactive Jekyll
```

### Fichier `index.html`

Le fichier HTML principal contient :

```html
<base href="/Sudoku/" />  <!-- Chemin de base modifié -->
<script src="_framework/blazor.webassembly.js"></script>
```

---

## Dépannage

### ❌ Erreur 404 sur les ressources

**Symptôme** : Page blanche, erreurs dans la console pour `_framework/blazor.webassembly.js`

**Cause** : Le `base href` n'est pas correct

**Solution** : Vérifier que la modification du `base href` s'applique bien

### ❌ Page blanche après déploiement

**Symptôme** : La page se charge mais reste blanche

**Causes possibles** :
1. Fichiers `_framework/` non servis → Vérifier `.nojekyll`
2. Erreur JavaScript → Ouvrir la console du navigateur (F12)

### ❌ Le routing ne fonctionne pas

**Symptôme** : Erreur 404 quand on rafraîchit une page

**Cause** : `404.html` manquant

**Solution** : Vérifier que l'étape de copie du 404.html est présente

### ❌ Le workflow échoue

**Symptôme** : Erreur de permissions

**Solution** : 
1. Vérifier que GitHub Pages est configuré sur "GitHub Actions"
2. Vérifier les permissions dans le workflow

---

## 📊 Résumé du flux

```
1. Push sur main/master
        ↓
2. GitHub Actions démarre
        ↓
3. Build de l'application Blazor
        ↓
4. Modification du base href
        ↓
5. Ajout de .nojekyll et 404.html
        ↓
6. Upload de l'artefact
        ↓
7. Déploiement sur GitHub Pages
        ↓
8. Site accessible à https://<user>.github.io/<repo>/
```

---

## 🔗 Liens utiles

- [Documentation GitHub Pages](https://docs.github.com/en/pages)
- [Documentation Blazor WebAssembly](https://learn.microsoft.com/en-us/aspnet/core/blazor/host-and-deploy/webassembly)
- [GitHub Actions - deploy-pages](https://github.com/actions/deploy-pages)

