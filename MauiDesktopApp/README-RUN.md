# Exécution locale

Si `dotnet run` affiche **"Il n'existe aucun projet à exécuter"**, c'est qu'aucun fichier `.csproj` n'était présent dans le dossier courant.

## Commandes

```powershell
cd .\MauiDesktopApp
dotnet restore
dotnet build
dotnet run
```

Option explicite (recommandée):

```powershell
dotnet run --project .\MauiDesktopApp.csproj
```
