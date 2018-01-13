import { Component, OnInit, Output, EventEmitter, ViewChild, ElementRef, Input } from '@angular/core';
import { User } from '../../usuarios/user.model';


import { MatDialogRef, MatSnackBar } from '@angular/material';
import { Http, Response, Headers, RequestOptions } from '@angular/http';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  _error: any = {};
  user: User;
  password;
  prepare = false;






  constructor(
    private http: Http,
    private snackBar: MatSnackBar,
  ) {
    this.user = new User();
    this.user.data = {
      image: undefined,
      error: this._error,
      funcao: this.enviar,
      http: this.http,
      snackBar: this.snackBar,
      user: this.user
    };

  }

  ngOnInit() {
  }



  onFileSelect($event) {
    this.user.data.image = $event[0];
    this.user.enviar();
  }



  enviar(data: {
    image,
    funcao,
    http,
    snackBar,
    error,
    user: User
  }) {
    console.log(data.user);

    let body = JSON.stringify({
      FirstName: data.user.FirstName,
      LastName: data.user.LastName,
      UserName: data.user.UserName,
      Email: data.user.Email,
      Password: data.user.Password,
      UserPhoto: data.user.UserPhoto,
      UserPhotoExtensao: data.image.type
    });
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let options = new RequestOptions({ headers: headers });
    console.log('enviar');
console.log({
  FirstName: data.user.FirstName,
  LastName: data.user.LastName,
  UserName: data.user.UserName,
  Email: data.user.Email,
  Password: data.user.Password,
  UserPhoto: data.user.UserPhoto,
  UserPhotoExtensao: data.image.type
});


    data.http.post(
      "http://localhost:49176/api/accounts"
      //'api/accounts'

      , body, options)
      .toPromise().then(res => {

        if (!res.ok) {

          if (res.status === 400) {
            data.error = res.json();
          } else {
            alert('Houve um erro inesperado');
          }

          /////insucessso
          return;
        }


        data.snackBar.open('Usuario registrado com sucesso!', data.user.UserName, { duration: 3000 });





       





      }

      ).catch((error) => {
        let errMsg: string;
        if (error instanceof Response) {
          const body = error.json() || '';
          data.error = body;
          const err = body.error || JSON.stringify(body);
          errMsg = `${error.status} - ${error.statusText || ''} ${err}`;
        } else {
          errMsg = error.message ? error.message : error.toString();
        }
        console.error(errMsg);
      });
  }

}
