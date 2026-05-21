import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { BadgeModule } from 'primeng/badge';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { SelectModule } from 'primeng/select';
import { FormsModule } from '@angular/forms';
import { IdeeService } from '../../../core/services/idee.service';
import { Idee, CreateIdee } from '../../../models/idee.model';

/**
 * Composant de liste des idées.
 * Affiche toutes les idées sous forme de cards avec leurs métadonnées.
 * Permet de créer une nouvelle idée via un dialogue modal.
 * Route : /idees
 */
@Component({
  selector: 'app-idee-list',
  standalone: true,
  imports: [
    CommonModule,
    CardModule,
    ButtonModule,
    TagModule,
    BadgeModule,
    DialogModule,
    InputTextModule,
    TextareaModule,
    SelectModule,
    FormsModule
  ],
  templateUrl: './idee-list.html',
  styleUrl: './idee-list.css'
})
export class IdeeListComponent implements OnInit {

  /** Liste des idées chargées depuis l'API */
  idees: Idee[] = [];

  /** Contrôle la visibilité du dialogue de création */
  showDialog = false;

  /** Modèle du formulaire de création d'une nouvelle idée */
  newIdee: CreateIdee = {
    titre: '',
    contenu: '',
    auteur: '',
    priorite: 'moyenne',
    difficulte: 'moyenne'
  };

  /** Options disponibles pour les selects priorité et difficulté */
  niveaux = [
    { label: 'Basse', value: 'basse' },
    { label: 'Moyenne', value: 'moyenne' },
    { label: 'Haute', value: 'haute' }
  ];

  /**
   * @param ideeService - Service HTTP pour les appels API idées
   * @param router      - Service Angular pour la navigation entre les vues
   */
  constructor(
    private ideeService: IdeeService,
    private router: Router
  ) {}

  /**
   * Initialisation du composant.
   * Charge la liste des idées au démarrage.
   */
  ngOnInit(): void {
    this.loadIdees();
  }

  /**
   * Charge toutes les idées depuis l'API.
   * Les idées sont triées par date de création décroissante (géré côté API).
   */
  loadIdees(): void {
    this.ideeService.getIdees().subscribe({
      next: (data) => this.idees = data,
      error: (err) => console.error('Erreur chargement idées', err)
    });
  }

  /**
   * Navigue vers la page de détail d'une idée.
   * @param id - Identifiant de l'idée à afficher
   */
  ouvrirDetail(id: number): void {
    this.router.navigate(['/idees', id]);
  }

  /**
   * Ouvre le dialogue de création d'une nouvelle idée.
   */
  ouvrirDialog(): void {
    this.showDialog = true;
  }

  /**
   * Soumet le formulaire de création d'une idée.
   * Vérifie que le titre et l'auteur sont renseignés avant l'envoi.
   * Ferme le dialogue et recharge la liste après création.
   */
  creerIdee(): void {
    if (!this.newIdee.titre || !this.newIdee.auteur) return;

    this.ideeService.createIdee(this.newIdee).subscribe({
      next: () => {
        this.showDialog = false;
        this.newIdee = { titre: '', contenu: '', auteur: '', priorite: 'moyenne', difficulte: 'moyenne' };
        this.loadIdees();
      },
      error: (err) => console.error('Erreur création idée', err)
    });
  }

  /**
   * Retourne la sévérité PrimeNG correspondant au niveau donné.
   * Utilisé pour coloriser les tags priorité et difficulté.
   * @param niveau - Valeur du niveau : 'basse' | 'moyenne' | 'haute'
   * @returns Sévérité PrimeNG : 'success' | 'warn' | 'danger' | 'info'
   */
  getSeverity(niveau: string): 'success' | 'info' | 'warn' | 'danger' {
    switch (niveau) {
      case 'haute': return 'danger';
      case 'moyenne': return 'warn';
      case 'basse': return 'success';
      default: return 'info';
    }
  }
}