export interface Idee {
  idIdee: number;
  titre: string;
  auteur: string;
  priorite: 'basse' | 'moyenne' | 'haute';
  difficulte: 'basse' | 'moyenne' | 'haute';
  nbCommentaires: number;
  nbVotes: number;
  dateCreation: string;
}

export interface IdeeDetailModel extends Idee {
  contenu: string;
  dateModification: string;
  commentaires: Commentaire[];
}

export interface Commentaire {
  idCommentaire: number;
  contenu: string;
  auteur: string;
  dateCreation: string;
}

export interface CreateIdee {
  titre: string;
  contenu: string;
  auteur: string;
  priorite: string;
  difficulte: string;
}

export interface UpdateIdee {
  titre: string;
  contenu: string;
  priorite: string;
  difficulte: string;
}

export interface CreateCommentaire {
  contenu: string;
  auteur: string;
}

export interface CreateVote {
  auteur: string;
}