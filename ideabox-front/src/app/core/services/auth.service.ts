import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { Router } from '@angular/router';

export interface LoginDto {
  login: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  role: string;
  login: string;
}

/**
 * Service d'authentification.
 * Gère la connexion, la déconnexion et le stockage du token JWT.
 */
@Injectable({ providedIn: 'root' })
export class AuthService {

  private apiUrl = 'http://localhost:5014/api/auth';
  private tokenKey = 'ideabox_token';
  private roleKey  = 'ideabox_role';
  private loginKey = 'ideabox_login';

  constructor(private http: HttpClient, private router: Router) {}

  /**
   * Envoie les identifiants à l'API et stocke le token en sessionStorage.
   */
  login(dto: LoginDto): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, dto).pipe(
      tap(res => {
        sessionStorage.setItem(this.tokenKey, res.token);
        sessionStorage.setItem(this.roleKey,  res.role);
        sessionStorage.setItem(this.loginKey, res.login);
      })
    );
  }

  /**
   * Supprime le token et redirige vers la page de connexion.
   */
  logout(): void {
    sessionStorage.removeItem(this.tokenKey);
    sessionStorage.removeItem(this.roleKey);
    sessionStorage.removeItem(this.loginKey);
    this.router.navigate(['/login']);
  }

  /** Retourne true si un token est présent en session. */
  isLoggedIn(): boolean {
    return !!sessionStorage.getItem(this.tokenKey);
  }

  /** Retourne le token JWT. */
  getToken(): string | null {
    return sessionStorage.getItem(this.tokenKey);
  }

  /** Retourne le rôle de l'utilisateur connecté. */
  getRole(): string | null {
    return sessionStorage.getItem(this.roleKey);
  }

  /** Retourne le login de l'utilisateur connecté. */
  getLogin(): string | null {
    return sessionStorage.getItem(this.loginKey);
  }

  /** Retourne true si l'utilisateur est dev. */
  isDev(): boolean {
    return this.getRole() === 'dev';
  }
}