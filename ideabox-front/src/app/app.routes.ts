import { Routes } from '@angular/router';
import { IdeeListComponent } from './features/idees/idee-list/idee-list';
import { IdeeDetailComponent } from './features/idees/idee-detail/idee-detail';

export const routes: Routes = [
  { path: '', redirectTo: 'idees', pathMatch: 'full' },
  { path: 'idees', component: IdeeListComponent },
  { path: 'idees/:id', component: IdeeDetailComponent }
];