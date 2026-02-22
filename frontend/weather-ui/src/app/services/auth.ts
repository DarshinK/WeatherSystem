import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseUrl = 'https://localhost:7207/api/auth'; 

  constructor(private http: HttpClient) {}

  register(data: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/register`, data);
  }

  login(data: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/login`, data)
      .pipe(
        tap(response => {
          console.log('Login success, storing token:', response.token?.substring(0, 20) + '...');
          localStorage.setItem('token', response.token);
          console.log('Token stored. Verify in localStorage:', localStorage.getItem('token')?.substring(0, 20) + '...');
        })
      );
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  logout() {
    localStorage.removeItem('token');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  // helper to accept a token received from OAuth popup
  loginWithToken(token: string) {
    localStorage.setItem('token', token);
  }

  // returns the backend OAuth login endpoint for Google
  getGoogleLoginUrl(): string {
    return `${this.baseUrl}/google-login`;
  }
}
