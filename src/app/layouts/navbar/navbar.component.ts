import { Component, OnInit } from '@angular/core';
import { SidebarService } from '../sidebar/sidebar.service';
import { Http, Response, Headers, RequestOptions } from '@angular/http';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {

  constructor(
    private sidebar: SidebarService,
    private http: Http
  ) { }

  ngOnInit() {
  }

  toogleSidebar(){
    this.sidebar.toogleSidebar();
  }

  login(){
    this.sidebar.setFoto('res.text()');
    let token;

    let headers = new Headers();
    headers.append('Content-Type', 'application/json');

    const dt = {
      "UserName": "alda",
      "Password": "aldaalda"
    };

    this.http
    .post(
    'http://localhost:49176/api/auth/login',
    ///  'api/auth/login',
    JSON.stringify(dt),{ headers }
    )
    .subscribe(res =>{
      token = res.json().accessToken;
      console.log('token: ' + token);
      console.log(res.json());





      headers.append('Authorization', `Bearer ${token}`);
  
      alert('tentando login');
  
      return this.http
        .get(
        /// 'http://localhost:51204/api/dashboard/home'
        'http://localhost:49176/api/accounts/foto2'
  
        , { headers })
        .subscribe(res => {

          this.sidebar.setFoto(res.json());


      


        },
        error => console.log(error)
        );


    },
    (error) => {
      let errMsg: string;
      if (error instanceof Response) {
        const body = error.json() || '';
        
        const err =  body.error || JSON.stringify(body);
        errMsg = `${error.status} - ${error.statusText || ''} ${err}`;
      } else {
        errMsg = error.message ? error.message : error.toString();
      }
      console.error(errMsg);
    });


    



  }

}
