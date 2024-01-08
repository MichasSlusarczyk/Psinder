import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ShelterFilters } from '../models/ShelterFilters';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Page } from 'src/app/core/models/Page';
import { ImageDto } from 'src/app/shared/components/attachment/models/Attachment';
import { Pet } from '../../pets/models/Pet';
import { Shelter, ShelterResponse } from '../models/Shelter';

@Injectable({
  providedIn: 'root'
})
export class ShelterService {

  private url: string = `${environment.apiUrl}/shelter`;

  constructor(private http: HttpClient) {}

  getShelters(): Observable<ShelterResponse> {
    return this.http.get<ShelterResponse>(`${this.url}/all`);
  }


  getShelterById(id: number): Observable<Shelter> {
    return this.http.get<Shelter>(`${this.url}/${id}`);
  }

  updateShelter(shelter: Shelter): Observable<Pet> {
    return this.http.put<Pet>(this.url, shelter);
  }

  addShelter(shelter: Shelter): Observable<Shelter> {
    return this.http.post<Shelter>(this.url, shelter);
  }
}
