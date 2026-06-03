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
import { AuthService } from '../../../core/services/auth.service';
import { IdeeDetailModel, CreateCommentaire } from '../../../models/idee.model';
import { IdeeFormComponent } from '../idee-form/idee-form';

/**
 * Composant de detail d'une idee.
 * Affiche le contenu complet, les commentaires et les votes.
 * Permet d'ajouter/supprimer des commentaires, voter et modifier l'idee.
 * Les actions de modification/suppression sont reservees au role dev.
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
    ToastModule,
    IdeeFormComponent
  ],
  providers: [ConfirmationService, MessageService],
  templateUrl: './idee-detail.html',
  styleUrl: './idee-detail.css'
})
export class IdeeDetailComponent implements OnInit {

  /** Idee courante chargee depuis l'API, null pendant le chargement */
  idee: IdeeDetailModel | null = null;

  /** Modele du formulaire de creation d'un nouveau commentaire */
  newCommentaire: CreateCommentaire = {
    contenu: '',
    auteur: ''
  };

  /** Prénom de l'auteur saisi pour voter */
  auteurVote = '';

  /** Prénom saisi pour retirer son vote */
  auteurSupprimerVote = '';

  /** Id du commentaire en cours d'edition, null si aucun */
  commentaireEnEdition: number | null = null;

  /** Contenu temporaire pendant l'edition */
  contenuEdition = '';

  /** Controle la visibilite du dialogue d'edition */
  showEditDialog = false;

  /**
   * @param route               - Service Angular pour acceder aux parametres de la route (:id)
   * @param router              - Service Angular pour la navigation entre les vues
   * @param ideeService         - Service HTTP pour les appels API idees/commentaires/votes
   * @param confirmationService - Service PrimeNG pour les dialogues de confirmation
   * @param messageService      - Service PrimeNG pour les notifications toast
   * @param authService         - Service d'authentification pour verifier le role (public pour le template)
   */
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private ideeService: IdeeService,
    private confirmationService: ConfirmationService,
    private messageService: MessageService,
    public authService: AuthService
  ) {}

  /**
   * Initialisation du composant.
   * Recupere l'id depuis l'URL et charge l'idee correspondante.
   */
  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.loadIdee(id);
  }

  /**
   * Charge une idee depuis l'API avec ses commentaires et votes.
   */
  loadIdee(id: number): void {
    this.ideeService.getIdee(id).subscribe({
      next: (data) => this.idee = data,
      error: (err) => console.error('Erreur chargement idee', err)
    });
  }

  /**
   * Ouvre le dialogue d'edition de l'idee.
   */
  ouvrirEdition(): void {
    this.showEditDialog = true;
  }

  /**
   * Callback apres modification reussie de l'idee.
   */
  onIdeeMiseAJour(): void {
    this.loadIdee(this.idee!.idIdee);
    this.messageService.add({ severity: 'success', summary: 'Idee modifiee avec succes !' });
  }

  /**
   * Soumet le formulaire d'ajout de commentaire.
   */
  ajouterCommentaire(): void {
    if (!this.newCommentaire.contenu || !this.newCommentaire.auteur || !this.idee) return;

    this.ideeService.addCommentaire(this.idee.idIdee, this.newCommentaire).subscribe({
      next: () => {
        this.newCommentaire = { contenu: '', auteur: '' };
        this.loadIdee(this.idee!.idIdee);
        this.messageService.add({ severity: 'success', summary: 'Commentaire ajoute' });
      },
      error: () => this.messageService.add({ severity: 'error', summary: 'Erreur' })
    });
  }

  /**
   * Supprime un commentaire apres confirmation.
   */
  supprimerCommentaire(commentaireId: number): void {
    this.confirmationService.confirm({
      message: 'Supprimer ce commentaire ?',
      accept: () => {
        this.ideeService.deleteCommentaire(this.idee!.idIdee, commentaireId).subscribe({
          next: () => {
            this.loadIdee(this.idee!.idIdee);
            this.messageService.add({ severity: 'success', summary: 'Commentaire supprime' });
          }
        });
      }
    });
  }

  /**
   * Enregistre un vote pour l'idee courante.
   */
  voter(): void {
    if (!this.auteurVote || !this.idee) return;

    this.ideeService.voter(this.idee.idIdee, { auteur: this.auteurVote }).subscribe({
      next: (res) => {
        this.auteurVote = '';
        this.loadIdee(this.idee!.idIdee);
        this.messageService.add({ severity: 'success', summary: `Vote enregistre ! Total : ${res.nbVotes}` });
      },
      error: (err) => {
        if (err.status === 409)
          this.messageService.add({ severity: 'warn', summary: 'Vous avez deja vote !' });
        else
          this.messageService.add({ severity: 'error', summary: 'Erreur lors du vote' });
      }
    });
  }

  /**
   * Active le mode edition sur un commentaire.
   */
  activerEdition(commentaireId: number, contenuActuel: string): void {
    this.commentaireEnEdition = commentaireId;
    this.contenuEdition = contenuActuel;
  }

  /**
   * Annule l'edition en cours sans sauvegarder.
   */
  annulerEdition(): void {
    this.commentaireEnEdition = null;
    this.contenuEdition = '';
  }

  /**
   * Sauvegarde la modification d'un commentaire.
   */
  sauvegarderCommentaire(commentaireId: number): void {
    if (!this.contenuEdition || !this.idee) return;

    this.ideeService.updateCommentaire(this.idee.idIdee, commentaireId, this.contenuEdition).subscribe({
      next: () => {
        this.commentaireEnEdition = null;
        this.contenuEdition = '';
        this.loadIdee(this.idee!.idIdee);
        this.messageService.add({ severity: 'success', summary: 'Commentaire modifie !' });
      },
      error: () => this.messageService.add({ severity: 'error', summary: 'Erreur modification' })
    });
  }

  /**
   * Supprime le vote d'un auteur sur l'idee courante.
   */
  retirerVote(): void {
    if (!this.auteurSupprimerVote || !this.idee) return;

    this.ideeService.supprimerVote(this.idee.idIdee, this.auteurSupprimerVote).subscribe({
      next: () => {
        this.auteurSupprimerVote = '';
        this.loadIdee(this.idee!.idIdee);
        this.messageService.add({ severity: 'success', summary: 'Vote retiré !' });
      },
      error: (err) => {
        if (err.status === 404)
          this.messageService.add({ severity: 'warn', summary: 'Aucun vote trouvé pour cet auteur.' });
        else
          this.messageService.add({ severity: 'error', summary: 'Erreur lors de la suppression du vote.' });
      }
    });
  }

  /**
   * Navigue vers la liste des idees.
   */
  retour(): void {
    this.router.navigate(['/idees']);
  }

  /**
   * Retourne la severite PrimeNG correspondant au niveau donne.
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