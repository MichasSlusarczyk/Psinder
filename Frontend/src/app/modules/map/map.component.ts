import { Component, Input, OnInit } from '@angular/core';
import * as L from 'leaflet';
import { MapService } from './services/map/map.service';
import { GeolocationService } from './services/geolocation/geolocation.service';

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.scss']
})
export class MapComponent implements OnInit {
  latitude: number = 51.505;
  longitude: number = -0.09;
  @Input() city!: string
  @Input() address!: string
  map!: L.Map

  constructor(private geolocationService: GeolocationService) { }

  ngOnInit() {
    this.geocode();
  }

  geocode() {
    if (this.city === undefined || this.address === undefined) {
      return;
    }

    this.geolocationService.geocode(this.city, this.address)
      .subscribe((data: any[]) => {
        if (data && data.length > 0) {
          this.latitude = data[0].lat;
          this.longitude = data[0].lon;
          this.map = L.map('map').setView([this.latitude, this.longitude], 17);

          L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
            maxZoom: 19,
            attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
          }).addTo(this.map);

          const marker = L.marker([this.latitude, this.longitude]).addTo(this.map);
        } else {
          console.log('Geocoding failed');
        }
      });
  }
}
