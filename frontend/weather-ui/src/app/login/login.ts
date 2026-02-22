import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.html'
})
export class LoginComponent {

  email = '';
  password = '';
  errorMessage = '';

  constructor(private auth: AuthService, private router: Router) {}

  onLogin() {
    this.errorMessage = '';

    this.auth.login({
      email: this.email,
      password: this.password
    }).subscribe({
      next: () => {
        this.router.navigate(['/weather']);
      },
      error: () => {
        this.errorMessage = 'User does not exist or invalid credentials';
      }
    });
  }

  onGoogleLogin() {
    const googleUrl = this.auth.getGoogleLoginUrl();
    console.log('Opening Google OAuth popup:', googleUrl);
    const popup = window.open(googleUrl, 'oauth', 'width=600,height=700');

    const handleMessage = (event: MessageEvent) => {
      console.log('Message received from popup:', event.data, 'Origin:', event.origin);
      if (!event.data || !event.data.token) {
        console.log('No token in message, ignoring');
        return;
      }
      console.log('Token received, storing and navigating...');
      this.auth.loginWithToken(event.data.token);
      window.removeEventListener('message', handleMessage);
      try { popup?.close(); } catch (e) { console.log('Error closing popup:', e); }
      this.router.navigate(['/weather']);
    };

    window.addEventListener('message', handleMessage);
    console.log('Message listener attached');
  }
}
