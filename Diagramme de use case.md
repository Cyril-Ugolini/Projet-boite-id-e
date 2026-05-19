```mermaid

flowchart LR

    Utilisateur -->|Créer| Idee

    Utilisateur -->|Modifier| Idee

    Utilisateur -->|Supprimer| Idee

    Utilisateur -->|Consulter| Idee

    Utilisateur -->|Commenter| Commentaire

    Utilisateur -->|Voter| Vote

  

    Idee --> Commentaire

    Idee --> Vote