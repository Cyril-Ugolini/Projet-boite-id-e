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

  idee: IdeeDetailModel | null = null;

  newCommentaire: CreateCommentaire = {
    contenu: '',
    auteur: ''
  };

  auteurVote = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private ideeService: IdeeService,
    private confirmationService: ConfirmationService,
    private messageService: MessageService
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.loadIdee(id);
  }

  loadIdee(id: number): void {
    this.ideeService.getIdee(id).subscribe({
      next: (data) => this.idee = data,
      error: (err) => console.error('Erreur chargement idée', err)
    });
  }

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

  retour(): void {
    this.router.navigate(['/idees']);
  }

  getSeverity(niveau: string): 'success' | 'info' | 'warn' | 'danger' {
    switch (niveau) {
      case 'haute': return 'danger';
      case 'moyenne': return 'warn';
      case 'basse': return 'success';
      default: return 'info';
    }
  }
}