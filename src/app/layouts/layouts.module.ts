import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { navbarRoute } from './navbar/navbar.route';
import {sidebarRoute} from './sidebar/sidebar.route';

const LAYOUT_ROUTES = [
  navbarRoute,
  sidebarRoute
];


import { SidebarComponent } from './sidebar/sidebar.component';
import { NavbarComponent } from './navbar/navbar.component';
import { MaterialModule } from '../material/material.module';
import { SidebarService } from './sidebar/sidebar.service';


@NgModule({
  imports: [
    CommonModule,
    RouterModule.forRoot(LAYOUT_ROUTES, { useHash: true }),
    MaterialModule
  ],
  exports: [
      RouterModule,
  ],
  declarations: [
    SidebarComponent,
    NavbarComponent
  ],
  providers: [
    SidebarService
  ]
})
export class LayoutsModule { }
