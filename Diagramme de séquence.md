```mermaid

sequenceDiagram

    participant U as Utilisateur

    participant F as Frontend

    participant A as API

    participant B as Base de données

  

    %% Création d'une idée

    U->>F: Saisit titre + contenu + auteur

    F->>A: POST /idees

    A->>B: INSERT INTO idee

    B-->>A: OK

    A-->>F: 201 Created

    F-->>U: Idée créée

  

    %% Ajout d'un commentaire

    U->>F: Ajoute commentaire

    F->>A: POST /commentaires

    A->>B: INSERT INTO commentaire

    B-->>A: OK

    A-->>F: 201 Created

    F-->>U: Commentaire ajouté