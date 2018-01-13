import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { DashboardComponent } from './dashboard.component';
import { DASHBOARD_ROUTE } from './dashboard.route';
import { MaterialModule } from '../material/material.module';

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forRoot([DASHBOARD_ROUTE ], { useHash: true }),
    MaterialModule
  ],
  declarations: [DashboardComponent]
})
export class DashboardModule { }
