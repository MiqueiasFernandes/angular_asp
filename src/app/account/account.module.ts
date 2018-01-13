import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SettingsComponent } from './settings/settings.component';
import { RegisterComponent } from './register/register.component';

import { Routes, RouterModule } from '@angular/router';
import { SharedModule } from '../shared/shared.module';




const ACCOUNT_ROUTES = [
  {
    path: 'settings',
    component: SettingsComponent
  },
  {
    path: 'register',
    component: RegisterComponent
  },
];

export const accountRoutes: Routes = [{
   path: '',
   children: ACCOUNT_ROUTES
}];



@NgModule({
  imports: [
    CommonModule,
    RouterModule.forRoot(accountRoutes, { useHash: true }),
    SharedModule
  ],
  declarations: [SettingsComponent, RegisterComponent]
})
export class AccountModule { }
