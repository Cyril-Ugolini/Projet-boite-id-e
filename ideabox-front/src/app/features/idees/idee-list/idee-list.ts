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

  idees: Idee[] = [];
  showDialog = false;

  newIdee: CreateIdee = {
    titre: '',
    contenu: '',
    auteur: '',
    priorite: 'moyenne',
    difficulte: 'moyenne'
  };

  niveaux = [
    { label: 'Basse', value: 'basse' },
    { label: 'Moyenne', value: 'moyenne' },
    { label: 'Haute', value: 'haute' }
  ];

  constructor(
    private ideeService: IdeeService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadIdees();
  }

  loadIdees(): void {
    this.ideeService.getIdees().subscribe({
      next: (data) => this.idees = data,
      error: (err) => console.error('Erreur chargement idées', err)
    });
  }

  ouvrirDetail(id: number): void {
    this.router.navigate(['/idees', id]);
  }

  ouvrirDialog(): void {
    this.showDialog = true;
  }

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

  getSeverity(niveau: string): 'success' | 'info' | 'warn' | 'danger' {
    switch (niveau) {
      case 'haute': return 'danger';
      case 'moyenne': return 'warn';
      case 'basse': return 'success';
      default: return 'info';
    }
  }
}