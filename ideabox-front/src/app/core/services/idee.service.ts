import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Idee, IdeeDetailModel, CreateIdee, UpdateIdee, Commentaire, CreateCommentaire, CreateVote } from '../../models/idee.model';

/**
 * Service HTTP pour la communication avec l'API IdeaBox.
 * Centralise tous les appels vers les endpoints de l'API .NET.
 * Injecté dans les composants via l'injection de dépendances Angular.
 */
@Injectable({
  providedIn: 'root'
})
export class IdeeService {

  /** URL de base de l'API .NET */
  private apiUrl = 'http://localhost:5014/api';

  /**
   * @param http - Client HTTP Angular pour les requêtes REST
   */
  constructor(private http: HttpClient) {}

  // ── Idées ──────────────────────────────────────────────

  /**
   * Récupère la liste de toutes les idées.
   * Retourne une version allégée (sans contenu HTML) triée par date décroissante.
   * @returns Observable<Idee[]> - Liste des idées
   */
  getIdees(): Observable<Idee[]> {
    return this.http.get<Idee[]>(`${this.apiUrl}/idees`);
  }

  /**
   * Récupère le détail complet d'une idée avec ses commentaires et votes.
   * @param id - Identifiant de l'idée
   * @returns Observable<IdeeDetailModel> - Détail complet de l'idée
   */
  getIdee(id: number): Observable<IdeeDetailModel> {
    return this.http.get<IdeeDetailModel>(`${this.apiUrl}/idees/${id}`);
  }

  /**
   * Crée une nouvelle idée.
   * @param idee - Données de la nouvelle idée
   * @returns Observable<IdeeDetailModel> - Idée créée
   */
  createIdee(idee: CreateIdee): Observable<IdeeDetailModel> {
    return this.http.post<IdeeDetailModel>(`${this.apiUrl}/idees`, idee);
  }

  /**
   * Met à jour une idée existante.
   * Seuls le titre, contenu, priorité et difficulté sont modifiables.
   * @param id   - Identifiant de l'idée à modifier
   * @param idee - Nouvelles données de l'idée
   * @returns Observable<void> - 204 No Content
   */
  updateIdee(id: number, idee: UpdateIdee): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/idees/${id}`, idee);
  }

  /**
   * Supprime une idée et en cascade ses commentaires et votes.
   * @param id - Identifiant de l'idée à supprimer
   * @returns Observable<void> - 204 No Content
   */
  deleteIdee(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/idees/${id}`);
  }

  // ── Commentaires ───────────────────────────────────────

  /**
   * Ajoute un commentaire sur une idée.
   * @param ideeId      - Identifiant de l'idée concernée
   * @param commentaire - Données du commentaire (contenu + auteur)
   * @returns Observable<Commentaire> - Commentaire créé
   */
  addCommentaire(ideeId: number, commentaire: CreateCommentaire): Observable<Commentaire> {
    return this.http.post<Commentaire>(`${this.apiUrl}/idees/${ideeId}/commentaires`, commentaire);
  }

  /**
   * Supprime un commentaire d'une idée.
   * @param ideeId        - Identifiant de l'idée parente
   * @param commentaireId - Identifiant du commentaire à supprimer
   * @returns Observable<void> - 204 No Content
   */
  deleteCommentaire(ideeId: number, commentaireId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/idees/${ideeId}/commentaires/${commentaireId}`);
  }

  // ── Votes ──────────────────────────────────────────────

  /**
   * Enregistre un vote pour une idée.
   * Retourne une erreur 409 si l'auteur a déjà voté pour cette idée.
   * @param ideeId - Identifiant de l'idée à voter
   * @param vote   - Données du vote (auteur)
   * @returns Observable<{ message: string, nbVotes: number }>
   */
  voter(ideeId: number, vote: CreateVote): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/idees/${ideeId}/votes`, vote);
  }
}