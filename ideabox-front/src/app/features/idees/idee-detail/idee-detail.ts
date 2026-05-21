import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CardModule } from 'primeng/card';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { DividerModule } from 'primeng/divider';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmationService, MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';
import { IdeeService } from '../../../core/services/idee.service';
import { IdeeDetailModel, CreateCommentaire } from '../../../models/idee.model';

/**
 * Composant de détail d'une idée.
 * Affiche le contenu complet d'une idée ainsi que ses commentaires et votes.
 * Permet d'ajouter/supprimer des commentaires et de voter pour une idée.
 * Route : /idees/:id
 */
@Component({
  selector: 'app-idee-detail',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    CardModule,
    ButtonModule,
    TagModule,
    InputTextModule,
    TextareaModule,
    DividerModule,
    ConfirmDialogModule,
    ToastModule
  ],
  providers: [ConfirmationService, MessageService],
  templateUrl: './idee-detail.html',
  styleUrl: './idee-detail.css'
})
export class IdeeDetailComponent implements OnInit {

  /** Idée courante chargée depuis l'API, null pendant le chargement */
  idee: IdeeDetailModel | null = null;

  /** Modèle du formulaire de création d'un nouveau commentaire */
  newCommentaire: CreateCommentaire = {
    contenu: '',
    auteur: ''
  };

  /** Prénom de l'auteur saisi pour voter */
  auteurVote = '';

  /**
   * @param route    - Service Angular pour accéder aux paramètres de la route (:id)
   * @param router   - Service Angular pour la navigation entre les vues
   * @param ideeService       - Service HTTP pour les appels API idées/commentaires/votes
   * @param confirmationService - Service PrimeNG pour les dialogues de confirmation
   * @param messageService    - Service PrimeNG pour les notifications toast
   */
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private ideeService: IdeeService,
    private confirmationService: ConfirmationService,
    private messageService: MessageService
  ) {}

  /**
   * Initialisation du composant.
   * Récupère l'id depuis l'URL et charge l'idée correspondante.
   */
  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.loadIdee(id);
  }

  /**
   * Charge une idée depuis l'API avec ses commentaires et votes.
   * @param id - Identifiant de l'idée à charger
   */
  loadIdee(id: number): void {
    this.ideeService.getIdee(id).subscribe({
      next: (data) => this.idee = data,
      error: (err) => console.error('Erreur chargement idée', err)
    });
  }

  /**
   * Soumet le formulaire d'ajout de commentaire.
   * Vérifie que le contenu et l'auteur sont renseignés avant l'envoi.
   * Recharge l'idée après création pour afficher le nouveau commentaire.
   */
  ajouterCommentaire(): void {
    if (!this.newCommentaire.contenu || !this.newCommentaire.auteur || !this.idee) return;

    this.ideeService.addCommentaire(this.idee.idIdee, this.newCommentaire).subscribe({
      next: () => {
        this.newCommentaire = { contenu: '', auteur: '' };
        this.loadIdee(this.idee!.idIdee);
        this.messageService.add({ severity: 'success', summary: 'Commentaire ajouté' });
      },
      error: () => this.messageService.add({ severity: 'error', summary: 'Erreur' })
    });
  }

  /**
   * Supprime un commentaire après confirmation de l'utilisateur.
   * Utilise le ConfirmationService PrimeNG pour afficher un dialogue de confirmation.
   * @param commentaireId - Identifiant du commentaire à supprimer
   */
  supprimerCommentaire(commentaireId: number): void {
    this.confirmationService.confirm({
      message: 'Supprimer ce commentaire ?',
      accept: () => {
        this.ideeService.deleteCommentaire(this.idee!.idIdee, commentaireId).subscribe({
          next: () => {
            this.loadIdee(this.idee!.idIdee);
            this.messageService.add({ severity: 'success', summary: 'Commentaire supprimé' });
          }
        });
      }
    });
  }

  /**
   * Enregistre un vote pour l'idée courante.
   * Gère le cas où l'auteur a déjà voté (409 Conflict).
   * Recharge l'idée après vote pour mettre à jour le compteur.
   */
  voter(): void {
    if (!this.auteurVote || !this.idee) return;

    this.ideeService.voter(this.idee.idIdee, { auteur: this.auteurVote }).subscribe({
      next: (res) => {
        this.auteurVote = '';
        this.loadIdee(this.idee!.idIdee);
        this.messageService.add({ severity: 'success', summary: `Vote enregistré ! Total : ${res.nbVotes}` });
      },
      error: (err) => {
        if (err.status === 409)
          this.messageService.add({ severity: 'warn', summary: 'Vous avez déjà voté !' });
        else
          this.messageService.add({ severity: 'error', summary: 'Erreur lors du vote' });
      }
    });
  }

  /**
   * Navigue vers la liste des idées.
   */
  retour(): void {
    this.router.navigate(['/idees']);
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