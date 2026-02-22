import { Component, AfterViewInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import * as L from 'leaflet';
import { ChangeDetectorRef } from '@angular/core';
import { Weather } from '../services/weather';

@Component({
  selector: 'app-weather',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './weather.html'
})
export class WeatherComponent implements AfterViewInit {

  city = '';
  weather: any;
  error = '';
  map: any;
  marker: any;

  constructor(private weatherService: Weather,private cd: ChangeDetectorRef) {}

  ngAfterViewInit(): void {
    this.initMap();
  }

  initMap() {
    this.map = L.map('map').setView([20, 77], 5); 

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '© OpenStreetMap contributors'
    }).addTo(this.map);
  }

  searchWeather() {
    this.weatherService.getWeather(this.city).subscribe({
      next: (data) => {
        this.weather = data;
        this.error = '';
        this.updateMap(data);
        this.cd.detectChanges();
      },
      error: () => {
        this.weather = null;
        this.error = 'Unauthorized or error occurred';
      }
    });
  }

  updateMap(data: any) {
    const lat = data.lat;
    const lon = data.lon;

    this.map.setView([lat, lon], 10);

    if (this.marker) {
      this.map.removeLayer(this.marker);
    }

    this.marker = L.marker([lat, lon]).addTo(this.map)
      .bindPopup(`<b>${data.city}</b>`)
      .openPopup();
  }
}
