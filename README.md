# 💡 IdeaBox — Boîte à idées

Outil interne permettant aux membres de l'équipe de déposer des idées, de les commenter et de voter pour elles.

> Projet de stage — Cyril Ugolini

---

## 🛠️ Stack technique

| Couche | Technologie |
|---|---|
| Base de données | PostgreSQL |
| API | .NET 10 Web API |
| Front-end | Angular 21 + PrimeNG (thème Aura) |
| CSS utilitaires | PrimeFlex |

---

## 📁 Structure du projet

```
Projet boite à idée/
├── IdeaBox.Api/          ← API REST .NET 10
│   ├── Controllers/      ← IdeesController, CommentairesController, VotesController
│   ├── Services/         ← IIdeeService, IdeeService, ICommentaireService...
│   ├── Models/           ← Entités EF Core (Idee, Commentaire, Vote)
│   ├── DTOs/             ← Objets d'échange API
│   ├── Data/             ← AppDbContext
│   └── Program.cs        ← Configuration et démarrage
└── ideabox-front/        ← Front Angular 21
    └── src/app/
        ├── core/services/    ← IdeeService (appels HTTP)
        ├── models/           ← Interfaces TypeScript
        └── features/idees/   ← Composants (list, detail, form)
```

---

## 🚀 Lancer le projet

### Prérequis

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org)
- [PostgreSQL 15+](https://www.postgresql.org/download/)

### 1. Base de données

Créer la base et exécuter le script SQL :

```sql
CREATE DATABASE boite_a_idee;
```

Puis exécuter le script de création des tables (disponible dans `IdeaBox.Api/database/init.sql`) dans pgAdmin ou DBeaver.

### 2. API .NET

```bash
cd IdeaBox.Api

# Modifier la connexion PostgreSQL dans appsettings.json
# "DefaultConnection": "Host=localhost;Port=5432;Database=boite_a_idee;Username=postgres;Password=VOTRE_MOT_DE_PASSE"

dotnet restore
dotnet run
```

L'API démarre sur `http://localhost:5014`
Swagger disponible sur `http://localhost:5014/swagger`

### 3. Front Angular

```bash
cd ideabox-front

npm install
ng serve
```

Le front démarre sur `http://localhost:4200`

---

## 📌 Endpoints API

### Idées

| Méthode | Route | Description |
|---|---|---|
| GET | `/api/idees` | Liste toutes les idées |
| GET | `/api/idees/{id}` | Détail d'une idée |
| POST | `/api/idees` | Créer une idée |
| PUT | `/api/idees/{id}` | Modifier une idée |
| DELETE | `/api/idees/{id}` | Supprimer une idée |

### Commentaires

| Méthode | Route | Description |
|---|---|---|
| POST | `/api/idees/{id}/commentaires` | Ajouter un commentaire |
| PUT | `/api/idees/{id}/commentaires/{cid}` | Modifier un commentaire |
| DELETE | `/api/idees/{id}/commentaires/{cid}` | Supprimer un commentaire |

### Votes

| Méthode | Route | Description |
|---|---|---|
| GET | `/api/idees/{id}/votes` | Nombre de votes |
| POST | `/api/idees/{id}/votes` | Voter pour une idée |
| DELETE | `/api/idees/{id}/votes/{auteur}` | Retirer son vote |

---

## 🏗️ Architecture

```
Angular (front)
    ↕ HTTP / JSON
.NET Controllers  →  Interfaces  →  Services  →  AppDbContext (EF Core)
                                                        ↕ SQL
                                                   PostgreSQL
```

### Couche Services

Chaque service est défini par une interface pour faciliter l'injection de dépendances et le mocking en tests :

```
IIdeeService         →  IdeeService
ICommentaireService  →  CommentaireService
IVoteService         →  VoteService
```

---

## ✅ Fonctionnalités implémentées

- [x] CRUD complet sur les idées (titre, contenu riche, auteur, priorité, difficulté)
- [x] CRUD complet sur les commentaires
- [x] Système de votes avec protection doublon
- [x] Suppression de vote
- [x] Tags colorés priorité / difficulté
- [x] Notifications toast (succès / erreur)
- [x] Confirmation avant suppression
- [x] Logs avec ILogger dans les controllers
- [x] Gestion globale des exceptions
- [x] Documentation Swagger

## ❌ Non implémenté (manque de temps)

- [ ] Authentification / Autorisation (JWT)
- [ ] Tests unitaires (xUnit + Moq côté .NET, Jasmine côté Angular)
- [ ] Sanitisation XSS du contenu HTML
- [ ] Docker / docker-compose
- [ ] CI/CD GitLab

---

## 🔐 Sécurité

Cette application est un outil interne de démonstration. Elle n'est **pas sécurisée pour un usage en production** :

- Pas d'authentification — l'auteur est saisi en texte libre
- Contenu HTML affiché sans sanitisation XSS
- Pas de rate limiting sur l'API

---

## 🌿 Git

Branches :
- `main` → code stable
- `dev` → développement en cours

Convention de commits : [Conventional Commits](https://www.conventionalcommits.org/)

```
feat(scope): description
fix(scope): description
chore(scope): description
```

---

## 👤 Auteur

**Cyril Ugolini** — Stage entreprise 2026