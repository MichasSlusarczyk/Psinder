import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GeolocationService {

  latitude!: number
  longitude!: number
  cities: string[] = []
  private nominatimEndpoint = 'https://nominatim.openstreetmap.org/search';
  
  constructor(private http: HttpClient) {}

  getNearbyCities(lat: number, lng: number, radius: number): Observable<any> {
    const overpassQuery = `[out:json];
      (
        node(around:${radius * 1000},${lat},${lng})["place"="city"];
        way(around:${radius * 1000},${lat},${lng})["place"="city"];
        relation(around:${radius * 1000},${lat},${lng})["place"="city"];
      );
      out body; >; out skel qt;`;

    return this.http.get(`https://overpass-api.de/api/interpreter?data=${encodeURIComponent(overpassQuery)}`);
  }

  getGeolocation(): Observable<string[]> {
    return new Observable<string[]>(observer => {
      if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(
          position => {
            console.log("Lokalizacja:", position.coords.latitude, position.coords.longitude);
            const latitude = position.coords.latitude;
            const longitude = position.coords.longitude;
            this.getNearbyCities(latitude, longitude, 15).subscribe((data: any) => {
              const cities: string[] = data.elements.map((element: any) => element.tags.name);
              observer.next(cities);
              observer.complete();
            });
          },
          error => {
            console.error("Błąd lokalizacji:", error);
            observer.error(error);
          }
        );
      } else {
        console.error("Twoja przeglądarka nie obsługuje geolokalizacji.");
        observer.error("Brak obsługi geolokalizacji.");
      }
    });
  }

  geocode(city: string, address: string): Observable<any> {
    const params = {
      format: 'json',
      city: city,
      street: address
    };

    return this.http.get(this.nominatimEndpoint, { params });
  }
}
