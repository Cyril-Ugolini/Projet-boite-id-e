import { Routes } from '@angular/router';
import { IdeeListComponent } from './features/idees/idee-list/idee-list';
import { IdeeDetailComponent } from './features/idees/idee-detail/idee-detail';
import { LoginComponent } from './features/login/login';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'idees', component: IdeeListComponent, canActivate: [authGuard] },
  { path: 'idees/:id', component: IdeeDetailComponent, canActivate: [authGuard] }
];