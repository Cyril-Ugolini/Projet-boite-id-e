CREATE TABLE idee(
   Id_idee SERIAL,
   titre VARCHAR(50) NOT NULL,
   contenu TEXT NOT NULL,
   auteur VARCHAR(100) NOT NULL,
   priorite VARCHAR(10) NOT NULL CHECK (priorite IN ('basse', 'moyenne', 'haute')),
   difficulte VARCHAR(10) NOT NULL CHECK (difficulte IN ('basse', 'moyenne', 'haute')),
   date_creation TIMESTAMP NOT NULL DEFAULT NOW(),
   date_modification TIMESTAMP NOT NULL DEFAULT NOW(),
   PRIMARY KEY(Id_idee)
);

CREATE TABLE commentaire(
   Id_commentaire SERIAL,
   contenu TEXT NOT NULL,
   auteur VARCHAR(100) NOT NULL,
   date_creation TIMESTAMP NOT NULL DEFAULT NOW(),
   Id_idee INTEGER NOT NULL,
   PRIMARY KEY(Id_commentaire),
   FOREIGN KEY(Id_idee) REFERENCES idee(Id_idee) ON DELETE CASCADE
);

CREATE TABLE vote(
   Id_vote SERIAL,
   auteur VARCHAR(100) NOT NULL,
   date_creation TIMESTAMP NOT NULL DEFAULT NOW(),
   Id_idee INTEGER NOT NULL,
   PRIMARY KEY(Id_vote),
   FOREIGN KEY(Id_idee) REFERENCES idee(Id_idee) ON DELETE CASCADE,
UNIQUE(Id_idee, auteur);
