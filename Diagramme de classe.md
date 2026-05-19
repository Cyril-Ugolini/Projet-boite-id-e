```mermaid

classDiagram

  

class Idee {

  id_idee : int

  titre : string

  contenu : string

  auteur : string

  priorite : string

  difficulte : string

  date_creation : DateTime

  date_modification : DateTime

  +creer()

  +modifier()

  +supprimer()

}

  

class Commentaire {

  id_commentaire : int

  contenu : string

  auteur : string

  date_creation : DateTime

  +ajouter()

  +supprimer()

}

  

class Vote {

  id_vote : int

  auteur : string

  date_creation : DateTime

  +voter()

}

  

Idee --> Commentaire

Idee --> Vote