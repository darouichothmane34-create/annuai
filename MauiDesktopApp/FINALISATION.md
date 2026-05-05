# Étape 9 — Finalisation du projet

## 1) Checklist finale

### Interface & UX
- [x] Uniformiser les espacements, tailles de police et couleurs dans les pages Visiteur/Admin.
- [x] Garder des libellés cohérents en français (boutons, placeholders, titres).
- [x] Afficher des messages d’action visibles (`StatusMessage`, `ExportStatus`).

### Validations
- [x] Validation des champs obligatoires côté admin (site/service/salarié).
- [x] Validation de la sélection avant export PDF.
- [x] Validation de mot de passe admin avec message explicite en cas d’échec.

### Gestion d’erreurs & logs
- [x] Journalisation des erreurs CRUD dans `AdminDashboardViewModel`.
- [x] Journalisation des erreurs API dans `RandomUserApiService`.
- [x] Journalisation des erreurs export PDF dans `PdfService`.
- [x] Journalisation des échecs/succès d’authentification admin.

### Scénarios fonctionnels à vérifier
- [x] Scénario visiteur: recherche, filtres site/service, sélection détail, export PDF.
- [x] Scénario admin: raccourci Ctrl+Shift+A, login, CRUD complet, import RandomUser.
- [x] Rafraîchissement de liste après chaque action CRUD/import.

### Multi-instance SQLite
- [x] Configurer SQLite en mode WAL + cache partagé + busy timeout.
- [x] Vérifier 2 instances ouvertes: lecture simultanée OK, écritures séquentielles OK.

### Qualité du code
- [x] Nettoyage des blocs de code redondants.
- [x] Ajout de commentaires utiles sur les points sensibles (import API, export PDF, config DB).
- [x] Vérification des noms de classes/méthodes cohérents avec le domaine.

---

## 2) Scénario oral (15 minutes)

### 0:00 → 1:30 — Introduction
- Contexte: annuaire d’entreprise .NET MAUI desktop Windows.
- Architecture: MVVM + services + EF Core SQLite + DI.

### 1:30 → 4:30 — Parcours visiteur
- Ouvrir la page visiteur.
- Recherche par nom/prénom/email.
- Filtrer par site et service.
- Sélectionner un salarié et afficher sa fiche détaillée.

### 4:30 → 6:30 — Export PDF
- Cliquer sur **Exporter en PDF**.
- Montrer le message de confirmation et le chemin de sortie.
- Ouvrir le fichier généré et vérifier les champs (nom, prénom, téléphones, email, service, site).

### 6:30 → 9:00 — Accès administrateur
- Démontrer le raccourci **Ctrl + Shift + A**.
- Tester un mauvais mot de passe (message propre + log erreur).
- Tester un bon mot de passe (navigation dashboard + log succès).

### 9:00 → 13:00 — CRUD administrateur
- Onglet Sites: ajout, modification, suppression, refresh.
- Onglet Services: ajout, modification, suppression.
- Onglet Salariés: ajout avec validations, modification, suppression.
- Bouton **Importer depuis RandomUser** + rafraîchissement de la liste.

### 13:00 → 14:00 — Robustesse
- Montrer le fichier de logs.
- Expliquer la gestion des erreurs (API/PDF/CRUD/auth).
- Expliquer la configuration SQLite pour limiter les conflits multi-instance.

### 14:00 → 15:00 — Conclusion
- Récapitulatif des objectifs atteints.
- Pistes d’amélioration: tests unitaires, chiffrement mot de passe, pagination/import massif.

---

## 3) Ordre des fonctionnalités à montrer
1. Recherche visiteur + filtres.
2. Fiche détaillée salarié.
3. Export PDF.
4. Raccourci clavier admin.
5. Login admin (échec puis succès).
6. CRUD Sites.
7. CRUD Services.
8. CRUD Salariés.
9. Import RandomUser.
10. Consultation des logs + explication multi-instance SQLite.

---

## 4) Dernier message de commit
`chore: finalize UX, validation checklist, demo script, and sqlite multi-instance settings`
