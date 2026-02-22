import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class Weather {
  private apiUrl = "https://localhost:7077/api/weather";

  constructor(private http: HttpClient){}

  getWeather(city: string): Observable<any>{
    console.log("hi");
    return this.http.get(`${this.apiUrl}/${city}`);
  }
}
