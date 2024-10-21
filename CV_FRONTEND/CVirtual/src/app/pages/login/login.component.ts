import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  username: string = '';
  password: string = '';
  errorMessage: string = '';

  private validUsername: string = 'admin';
  private validPassword: string = '12345';

  constructor(
    private router: Router
  )
  { }

  login() {
    if (this.username === 'admin' && this.password === '12345') {
      this.router.navigate(['/home']);
    } else {
      alert('Credenciales incorrectas');
    }
  }
}

