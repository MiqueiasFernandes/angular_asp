import { Component, OnInit } from '@angular/core';
import { SidebarService } from './sidebar.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss'],
  host: {
      class:'sidebar2'
  }
})
export class SidebarComponent implements OnInit {

foto = '';

constructor(private sidebar: SidebarService) { }

  ngOnInit() {
   this.sidebar.fotorObserver$.subscribe(f => this.foto = f);
  }

}
