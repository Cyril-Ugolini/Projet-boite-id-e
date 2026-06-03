import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { CardModule } from 'primeng/card';
import { MessageModule } from 'primeng/message';
import { AuthService } from '../../core/services/auth.service';

/**
 * Composant de connexion.
 * Affiche un formulaire login/password et redirige vers /idees après connexion.
 * Deux comptes disponibles : user/user123 et dev/dev123.
 */
@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ButtonModule,
    InputTextModule,
    CardModule,
    MessageModule
  ],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class LoginComponent {

  /** Identifiants saisis dans le formulaire */
  credentials = { login: '', password: '' };

  /** Message d'erreur affiché si la connexion échoue */
  erreur = '';

  /** Indique si une requête est en cours */
  chargement = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  /**
   * Soumet le formulaire de connexion.
   * Redirige vers /idees si succès, affiche une erreur sinon.
   */
  seConnecter(): void {
    if (!this.credentials.login || !this.credentials.password) {
      this.erreur = 'Veuillez remplir tous les champs.';
      return;
    }

    this.chargement = true;
    this.erreur = '';

    this.authService.login(this.credentials).subscribe({
      next: () => {
        this.chargement = false;
        this.router.navigate(['/idees']);
      },
      error: () => {
        this.chargement = false;
        this.erreur = 'Identifiants invalides. Essayez user/user123 ou dev/dev123.';
      }
    });
  }
}