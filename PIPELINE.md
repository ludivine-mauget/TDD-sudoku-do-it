# 🔄 Pipeline CI/CD - Sudoku

Ce document décrit en détail la pipeline d'intégration et de déploiement continus (CI/CD) du projet Sudoku.

## 📋 Vue d'ensemble

La pipeline est configurée via GitHub Actions et se déclenche automatiquement lors de :
- **Push** sur les branches `main`, `master` ou `develop`
- **Pull Requests** vers ces mêmes branches
- **Déclenchement manuel** via `workflow_dispatch`

### Architecture de la Pipeline

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                              DÉCLENCHEURS                                    │
│         push (main/master/develop) │ pull_request │ workflow_dispatch       │
└─────────────────────────────────────────────────────────────────────────────┘
                                      │
                                      ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                            JOBS PARALLÈLES                                   │
├─────────────────────────────────┬───────────────────────────────────────────┤
│         build-and-test          │            code-quality                   │
│     (Build, Tests, Coverage)    │      (Formatage, Analyseurs)              │
└─────────────────────────────────┴───────────────────────────────────────────┘
                │                                    
                ▼ (succès)
┌─────────────────────────────────────────────────────────────────────────────┐
│                          JOBS SÉQUENTIELS                                    │
├─────────────────────────────────┬───────────────────────────────────────────┤
│       build-docker-images       │           security-scan                   │
│   (API + Client en parallèle)   │    (Vulnérabilités packages)              │
│  (push sur main/master uniq.)   │   (push + pull_request + commentaire PR)  │
│        + Scan Trivy             │                                           │
└─────────────────────────────────┴───────────────────────────────────────────┘
                │
                ▼ (succès + push sur main)
┌─────────────────────────────────────────────────────────────────────────────┐
│                              DEPLOY                                          │
│                    (Notification + Déploiement)                              │
└─────────────────────────────────────────────────────────────────────────────┘
```

---

## 🔧 Configuration Globale

### Contrôle de Concurrence
```yaml
concurrency:
  group: ${{ github.workflow }}-${{ github.ref }}
  cancel-in-progress: true
```
- **Objectif** : Éviter les exécutions multiples sur la même branche
- **Comportement** : Si un nouveau commit arrive pendant l'exécution, l'ancien job est annulé
- **Avantage** : Économie de ressources et résultats plus rapides

### Variables d'Environnement
| Variable | Valeur | Description |
|----------|--------|-------------|
| `DOTNET_VERSION` | `9.0.x` | Version du SDK .NET utilisée |
| `REGISTRY` | `ghcr.io` | GitHub Container Registry |
| `API_IMAGE_NAME` | `<repo>/sudoku-api` | Nom de l'image Docker API |
| `CLIENT_IMAGE_NAME` | `<repo>/sudoku-client` | Nom de l'image Docker Client |

---

## 📦 Job 1 : Build & Test

**Durée maximale** : 15 minutes

### Étapes détaillées

#### 1. Checkout du code
```yaml
uses: actions/checkout@v4
with:
  fetch-depth: 0
```
- `fetch-depth: 0` récupère tout l'historique Git
- Nécessaire pour l'analyse de couverture et les tags sémantiques

#### 2. Setup .NET
- Installe le SDK .NET 9.0.x sur le runner

#### 3. Cache NuGet
```yaml
key: ${{ runner.os }}-nuget-${{ hashFiles('**/*.csproj') }}
```
- **Clé de cache** : basée sur le hash des fichiers `.csproj`
- **Avantage** : Réutilise les packages NuGet entre les builds (gain de temps ~30-60s)

#### 4. Build de la solution
```bash
dotnet build Sudoku.sln --configuration Release --no-restore
```
- Build en mode Release pour optimiser les performances
- `--no-restore` car le restore a déjà été fait

#### 5. Exécution des tests
```bash
dotnet test Sudoku.Tests/Sudoku.Tests.csproj \
  --configuration Release \
  --no-build \
  --collect:"XPlat Code Coverage" \
  --results-directory ./TestResults \
  --logger "trx;LogFileName=test-results.trx"
```
- Collecte la couverture de code (format Cobertura)
- Génère un fichier `.trx` pour le reporting visuel

#### 6. Upload des artefacts
- **test-results** : Résultats des tests (rétention 30 jours)

#### 7. Publication des résultats de tests
```yaml
uses: dorny/test-reporter@v1.9.1
```
- Affiche les résultats directement dans l'interface GitHub
- Facilite le debugging en cas d'échec
- ⚠️ **Note** : Ce projet est archivé et ne recevra plus de mises à jour. La version v1.9.1 est la dernière version stable recommandée. À terme, envisager une alternative comme `phoenix-actions/test-reporting` ou les fonctionnalités natives de GitHub Actions.

#### 8. Codecov
- Upload automatique des rapports de couverture vers Codecov
- Nécessite le secret `CODECOV_TOKEN`

#### 9. Code Coverage Summary
```yaml
thresholds: '60 80'
```
- **60%** : Seuil d'avertissement (jaune)
- **80%** : Seuil de succès (vert)
- Génère un badge de couverture

---

## 🎨 Job 2 : Code Quality

**Durée maximale** : 10 minutes  
**Exécution** : En parallèle avec Build & Test

### Vérifications effectuées

#### 1. Formatage du code
```bash
dotnet format Sudoku.sln --verify-no-changes --verbosity diagnostic
```
- Vérifie que le code respecte les conventions de formatage
- **Échoue** si des fichiers ne sont pas correctement formatés
- **Solution** : Exécuter `dotnet format Sudoku.sln` localement avant de commit

#### 2. Analyseurs de code
```bash
dotnet build Sudoku.sln --configuration Release /p:RunAnalyzers=true
```
- Exécute les analyseurs Roslyn configurés dans le projet
- Détecte les problèmes de qualité de code (nullable, etc.)

---

## 🐳 Job 3 : Build Docker Images

**Durée maximale** : 20 minutes  
**Condition** : Uniquement sur `push` vers `main` ou `master`  
**Dépendance** : Succès de `build-and-test`

### Stratégie Matrix

La pipeline utilise une stratégie matrix pour builder les images en parallèle :

| Image | Dockerfile | Nom de l'image |
|-------|------------|----------------|
| API | `./Sudoku.API/Dockerfile` | `sudoku-api` |
| Client | `./Sudoku.Client/Dockerfile` | `sudoku-client` |

### Tags générés

| Type | Exemple | Description |
|------|---------|-------------|
| `ref` | `main` | Nom de la branche |
| `sha` | `abc1234` | SHA court du commit |
| `latest` | `latest` | Sur la branche par défaut uniquement |
| `semver` | `1.2.3` | Version complète (si tag) |
| `semver` | `1.2` | Version majeure.mineure (si tag) |

### Plateforme par défaut
```yaml
# Par défaut : linux/amd64 uniquement
# Pour activer ARM64, décommentez dans le workflow :
# platforms: linux/amd64,linux/arm64
```
- **amd64** : Serveurs x86_64 classiques (configuration par défaut)
- **arm64** (optionnel) : Apple Silicon, AWS Graviton, Raspberry Pi

> ⚠️ **Note** : La construction multi-plateforme (amd64 + arm64) augmente significativement le temps de build et l'utilisation des ressources. Activez ARM64 uniquement si nécessaire pour vos cibles de déploiement.

### Cache Docker
```yaml
cache-from: type=gha
cache-to: type=gha,mode=max
```
- Utilise le cache GitHub Actions
- `mode=max` : Cache toutes les couches intermédiaires

### Scan de sécurité Trivy
```yaml
uses: aquasecurity/trivy-action@master
image-ref: <registry>/<image>@<digest>
severity: 'CRITICAL,HIGH'
```
- Scan automatique des vulnérabilités dans l'image Docker
- Utilise le **digest** de l'image (et non un tag) pour garantir de scanner exactement l'image qui vient d'être construite
- Focus sur les vulnérabilités critiques et élevées
- Résultats uploadés vers l'onglet Security de GitHub (format SARIF)

---

## 🔒 Job 4 : Security Scan

**Durée maximale** : 10 minutes  
**Condition** : Sur `push` et `pull_request`  
**Dépendance** : Succès de `build-and-test`

### Analyses effectuées

#### 1. Packages vulnérables
```bash
dotnet list package --vulnerable --include-transitive
```
- Détecte les CVE connues dans les dépendances
- Inclut les dépendances transitives

#### 2. Packages dépréciés
```bash
dotnet list package --deprecated
```
- Identifie les packages qui ne sont plus maintenus

#### 3. Packages obsolètes
```bash
dotnet list package --outdated
```
- Liste les packages avec des versions plus récentes disponibles

### Rapport
- Généré au format Markdown
- Uploadé comme artefact (rétention 30 jours)
- Ajouté automatiquement en commentaire sur les Pull Requests (permet de voir les problèmes de sécurité avant le merge)

---

## 🚀 Job 5 : Deploy

**Condition** : Uniquement sur `push` vers `main`  
**Environnement** : `production`  
**Dépendance** : Succès de `build-docker-images`

### Fonctionnement actuel

Le job affiche les informations de déploiement :
- SHA du commit déployé
- Images Docker disponibles
- Commandes pour déployer manuellement

### Configuration du déploiement automatique

Pour activer le déploiement automatique vers un serveur, décommentez et configurez :

```yaml
- name: Deploy to server
  uses: appleboy/ssh-action@master
  with:
    host: ${{ secrets.DEPLOY_HOST }}
    username: ${{ secrets.DEPLOY_USER }}
    key: ${{ secrets.DEPLOY_KEY }}
    script: |
      cd /path/to/app
      docker compose pull
      docker compose up -d
```

#### Secrets à configurer
| Secret | Description |
|--------|-------------|
| `DEPLOY_HOST` | Adresse IP ou hostname du serveur |
| `DEPLOY_USER` | Nom d'utilisateur SSH |
| `DEPLOY_KEY` | Clé privée SSH (format PEM) |

---

## 🔑 Secrets requis

| Secret | Obligatoire | Description |
|--------|-------------|-------------|
| `GITHUB_TOKEN` | ✅ Auto | Token automatique pour GHCR |
| `CODECOV_TOKEN` | ❌ Optionnel | Token pour les rapports Codecov |
| `DEPLOY_HOST` | ❌ Optionnel | Pour le déploiement SSH |
| `DEPLOY_USER` | ❌ Optionnel | Pour le déploiement SSH |
| `DEPLOY_KEY` | ❌ Optionnel | Pour le déploiement SSH |

---

## 📊 Permissions

Le job `build-docker-images` requiert des permissions spécifiques :

```yaml
permissions:
  contents: read          # Lire le code source
  packages: write         # Pousser vers GHCR
  security-events: write  # Uploader les rapports SARIF
```

---

## 🛠️ Commandes locales utiles

### Vérifier le formatage avant de commit
```bash
dotnet format Sudoku.sln --verify-no-changes
```

### Corriger le formatage automatiquement
```bash
dotnet format Sudoku.sln
```

### Exécuter les tests avec couverture
```bash
dotnet test Sudoku.Tests/Sudoku.Tests.csproj --collect:"XPlat Code Coverage"
```

### Vérifier les packages vulnérables
```bash
dotnet list package --vulnerable --include-transitive
```

### Builder les images Docker localement
```bash
docker compose build
```

---

## 📈 Métriques et monitoring

### Temps d'exécution typiques

| Job | Durée moyenne | Timeout |
|-----|---------------|---------|
| Build & Test | 2-4 min | 15 min |
| Code Quality | 1-2 min | 10 min |
| Build Docker | 5-10 min | 20 min |
| Security Scan | 1-2 min | 10 min |
| Deploy | < 1 min | - |

### Indicateurs de santé
- ✅ Badge de build dans le README
- ✅ Badge de couverture Codecov
- ✅ Rapports de tests visuels
- ✅ Alertes de sécurité GitHub

---

## 🔄 Workflow de développement recommandé

1. **Créer une branche** depuis `develop`
2. **Développer** et commit régulièrement
3. **Ouvrir une Pull Request** vers `develop`
4. **Vérifier** que tous les checks passent
5. **Merger** la PR
6. **Merger** `develop` vers `main` pour déployer

---

## 📚 Ressources

- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [Docker Build Push Action](https://github.com/docker/build-push-action)
- [Trivy Security Scanner](https://github.com/aquasecurity/trivy)
- [Codecov](https://codecov.io/)

