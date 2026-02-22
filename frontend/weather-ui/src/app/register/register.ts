import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.html'
})
export class RegisterComponent {

  email = '';
  password = '';
  message = '';

  constructor(private auth: AuthService, private router: Router) {}

  onRegister() {
    this.auth.register({
      email: this.email,
      password: this.password
    }).subscribe({
      next: () => {
        this.message = 'Registration successful! Please login.';
        setTimeout(() => this.router.navigate(['/login']), 1500);
      },
      error: () => {
        this.message = 'Registration failed.';
      }
    });
  }
}
