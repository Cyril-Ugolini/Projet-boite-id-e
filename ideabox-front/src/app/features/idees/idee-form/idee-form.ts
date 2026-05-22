import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { SelectModule } from 'primeng/select';
import { DialogModule } from 'primeng/dialog';
import { IdeeService } from '../../../core/services/idee.service';
import { IdeeDetailModel, UpdateIdee } from '../../../models/idee.model';

/**
 * Composant formulaire d'edition d'une idee existante.
 * Affiche un dialogue modal pre-rempli avec les donnees de l'idee.
 * Emet un evenement 'ideeMiseAJour' apres modification reussie.
 * Utilise UpdateIdee comme payload vers PUT /api/idees/{id}.
 */
@Component({
  selector: 'app-idee-form',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ButtonModule,
    InputTextModule,
    TextareaModule,
    SelectModule,
    DialogModule
  ],
  templateUrl: './idee-form.html',
  styleUrls: ['./idee-form.css']
})
export class IdeeFormComponent implements OnInit {

  /** Idee a modifier, recue depuis le composant parent */
  @Input() idee!: IdeeDetailModel;

  /** Controle la visibilite du dialogue */
  @Input() visible = false;

  /** Emet true quand le dialogue doit se fermer */
  @Output() visibleChange = new EventEmitter<boolean>();

  /** Emet l'idee mise a jour apres sauvegarde */
  @Output() ideeMiseAJour = new EventEmitter<void>();

  /** Modele du formulaire d'edition */
  formData: UpdateIdee = {
    titre: '',
    contenu: '',
    priorite: 'moyenne',
    difficulte: 'moyenne'
  };

  /** Options pour les selects priorite et difficulte */
  niveaux = [
    { label: 'Basse', value: 'basse' },
    { label: 'Moyenne', value: 'moyenne' },
    { label: 'Haute', value: 'haute' }
  ];

  /**
   * @param ideeService - Service HTTP pour l'appel PUT /api/idees/{id}
   */
  constructor(private ideeService: IdeeService) {}

  /**
   * Initialise le formulaire avec les donnees de l'idee recue en Input.
   */
  ngOnInit(): void {
    this.formData = {
      titre: this.idee.titre,
      contenu: this.idee.contenu,
      priorite: this.idee.priorite,
      difficulte: this.idee.difficulte
    };
  }

  /**
   * Soumet le formulaire de modification.
   * Appelle PUT /api/idees/{id} et emet ideeMiseAJour si succes.
   */
  sauvegarder(): void {
    if (!this.formData.titre) return;

    this.ideeService.updateIdee(this.idee.idIdee, this.formData).subscribe({
      next: () => {
        this.ideeMiseAJour.emit();
        this.fermer();
      },
      error: (err) => console.error('Erreur mise a jour', err)
    });
  }

  /**
   * Ferme le dialogue sans sauvegarder.
   */
  fermer(): void {
    this.visibleChange.emit(false);
  }
}