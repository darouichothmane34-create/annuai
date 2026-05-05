# Projet .NET MAUI (Windows Desktop) — Étape 1

Cette étape prépare la base du projet avec une structure propre orientée MVVM.

## Structure

- `Models/`
- `ViewModels/`
- `Views/`
- `Services/`
- `Data/`
- `Helpers/`
- `Resources/`

## Création du projet MAUI (à exécuter localement)

```bash
dotnet new maui -n MauiDesktopApp
```

> Pour cibler spécifiquement Windows Desktop, éditez ensuite le `.csproj` pour inclure `net8.0-windows10.0.19041.0` (ou version Windows souhaitée) dans `TargetFrameworks`.

## Packages NuGet recommandés

- `Microsoft.EntityFrameworkCore.Sqlite` : fournisseur SQLite pour EF Core (stockage local).
- `Microsoft.EntityFrameworkCore.Design` : outils de conception EF Core (migrations, scaffolding).
- `CommunityToolkit.Mvvm` : génération de code MVVM (ObservableObject, RelayCommand, etc.).
- Bibliothèque PDF : **`UglyToad.PdfPig`** (lecture/extraction PDF côté .NET, multiplateforme).

## Commandes d'installation NuGet

```bash
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package CommunityToolkit.Mvvm
dotnet add package UglyToad.PdfPig
```

## Proposition de premier commit Git

```bash
git add .
git commit -m "chore: initialise structure MAUI Windows et dépendances recommandées"
```
