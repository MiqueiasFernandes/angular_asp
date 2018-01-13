import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';


import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { Ng2Webstorage } from 'ng2-webstorage';
import { MainComponent } from './layouts/main/main.component';
import { LayoutsModule } from './layouts/layouts.module';
import { NavbarComponent } from './layouts/navbar/navbar.component';
import { SidebarComponent } from './layouts/sidebar/sidebar.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { DashboardModule } from './dashboard/dashboard.module';
import { MaterialModule } from './material/material.module';
import { AccountModule } from './account/account.module';
import { UsuariosComponent } from './usuarios/usuarios.component';
import { usuariosRoute } from './usuarios/usuarios.route';
import { RouterModule } from '@angular/router';

import { HttpModule } from '@angular/http';
import { FormsModule } from '@angular/forms';

const APP_ROUTES = [
  usuariosRoute
];



@NgModule({
  declarations: [
    MainComponent,
    UsuariosComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    Ng2Webstorage.forRoot({ prefix: 'backendhoteldrevolution', separator: '-'}),
    FormsModule,
    HttpModule,
    LayoutsModule,
    DashboardModule,
    MaterialModule,
    AccountModule,
    RouterModule.forRoot(APP_ROUTES, { useHash: true }),
  ],
  providers: [],
  bootstrap: [MainComponent],
  exports: [
  ]
})
export class AppModule { }
