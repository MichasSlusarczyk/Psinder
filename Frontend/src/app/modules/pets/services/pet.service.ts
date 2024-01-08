import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ImageDto } from '../../../shared/components/attachment/models/Attachment';
import { Page } from '../../../core/models/Page';
import { Pet, PetResponse } from '../models/Pet';
import { PetRequest } from '../models/PetRequest';
import { PetFilter } from '../models/PetFilter';
import { PetPage } from '../models/PetPage';

@Injectable({
  providedIn: 'root'
})
export class PetService {

  private url: string = `${environment.apiUrl}/pet`;

  constructor(private http: HttpClient) {}

  getPets(filter: PetFilter, page: number, pageSize: number): Observable<PetPage> { 
    let params = new HttpParams().set('page', page + 1).set('pageSize', pageSize).set('orderBy', 'Score DESC')
    return this.http.post<PetPage>(`${this.url}/list`, filter, {params: params});
  }

  getPetsFromShelter(shelterId: number): Observable<PetPage> {
    let filter = {
      shelterId: shelterId
    }
    return this.http.post<PetPage>(`${this.url}/list`, filter);
  }

  getPetImages(id: number): Observable<ImageDto[]> {
    return this.http.get<ImageDto[]>(`${this.url}/${id}/images`);
  }

  getPetById(id: number): Observable<PetResponse> {
    return this.http.get<PetResponse>(`${this.url}/${id}`);
  }

  updatePet(formData: FormData): Observable<Pet> {
    return this.http.put<Pet>(this.url, formData);
  }

  addPet(formData: FormData): Observable<Pet> {
    return this.http.post<Pet>(this.url, formData);
  }

  deletePet(id: number): Observable<null> {
    return this.http.delete<null>(`${this.url}/${id}`);
  }
}
