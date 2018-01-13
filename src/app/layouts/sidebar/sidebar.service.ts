import { Injectable } from '@angular/core';
import { Subject } from 'rxjs/Subject';

@Injectable()
export class SidebarService {

  private isSidebarOpen = true;

private foto = '';

  constructor() { }

  private observeSidebarStatus = new Subject<boolean>();
  private observeSidebarfoto = new Subject<string>();

  public sidebarObserver$ = this.observeSidebarStatus.asObservable();
  public fotorObserver$ = this.observeSidebarfoto.asObservable();


  public openSidebar() {
    this.isSidebarOpen = true; ///is autenticad?
    this.updateSidebarOpen();
}

public closeSidebar() {
    this.isSidebarOpen = false;
    this.updateSidebarOpen();
}

public toogleSidebar() {
   this.isSidebarOpen = !this.isSidebarOpen;
   this.updateSidebarOpen();
}

private updateSidebarOpen() {
  this.observeSidebarStatus.next(this.isSidebarOpen);
}

public setFoto(foto){
  this.observeSidebarfoto.next('data:'+foto.type+';base64,' + foto.data);
}

}
