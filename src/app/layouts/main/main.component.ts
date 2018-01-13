import { Component, OnInit } from '@angular/core';
import { SidebarService } from '../sidebar/sidebar.service';

@Component({
  selector: 'app-root',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss']
})
export class MainComponent implements OnInit {

  isSidebarOpen = false;

  constructor(
    private sidebarService: SidebarService
  ) { }

  ngOnInit() {
    this.sidebarService.sidebarObserver$.subscribe(
     (status) => {
       this.isSidebarOpen = status;
     }
    );
    this.sidebarService.openSidebar();
  }

}
