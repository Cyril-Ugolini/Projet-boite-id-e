import { Component } from '@angular/core';

/**
 * Composant formulaire d'édition d'une idée existante.
 *
 * @todo Non implémenté par manque de temps.
 * Prévu pour gérer la modification (PUT) d'une idée existante
 * en recevant un objet IdeeDetailModel en entrée et en soumettant
 * un UpdateIdee vers l'API via IdeeService.updateIdee().
 *
 * Champs prévus : titre, contenu, priorité, difficulté.
 */
@Component({
  selector: 'app-idee-form',
  standalone: true,
  imports: [],
  templateUrl: './idee-form.html',
  styleUrls: ['./idee-form.css'],
})
export class IdeeForm {}