import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Idee, IdeeDetailModel, CreateIdee, UpdateIdee, Commentaire, CreateCommentaire, CreateVote } from '../../models/idee.model';

@Injectable({
  providedIn: 'root'
})
export class IdeeService {

  private apiUrl = 'http://localhost:5014/api';

  constructor(private http: HttpClient) {}

  // ── Idées ──────────────────────────────────────────────

  getIdees(): Observable<Idee[]> {
    return this.http.get<Idee[]>(`${this.apiUrl}/idees`);
  }

  getIdee(id: number): Observable<IdeeDetailModel> {
    return this.http.get<IdeeDetailModel>(`${this.apiUrl}/idees/${id}`);
  }

  createIdee(idee: CreateIdee): Observable<IdeeDetailModel> {
    return this.http.post<IdeeDetailModel>(`${this.apiUrl}/idees`, idee);
  }

  updateIdee(id: number, idee: UpdateIdee): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/idees/${id}`, idee);
  }

  deleteIdee(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/idees/${id}`);
  }

  // ── Commentaires ───────────────────────────────────────

  addCommentaire(ideeId: number, commentaire: CreateCommentaire): Observable<Commentaire> {
    return this.http.post<Commentaire>(`${this.apiUrl}/idees/${ideeId}/commentaires`, commentaire);
  }

  deleteCommentaire(ideeId: number, commentaireId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/idees/${ideeId}/commentaires/${commentaireId}`);
  }

  // ── Votes ──────────────────────────────────────────────

  voter(ideeId: number, vote: CreateVote): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/idees/${ideeId}/votes`, vote);
  }
}