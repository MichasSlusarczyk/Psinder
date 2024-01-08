import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Appointment } from '../models/Appointment';
import { PetAppointments, ShelterAppointments } from '../models/AllAppointments';

@Injectable({
  providedIn: 'root'
})
export class AppointmentService {

  constructor(private http: HttpClient) { }

  private url: string = `${environment.apiUrl}/appointment`;

  getUserAppointments(userId : number): Observable<PetAppointments> {
    return this.http.get<PetAppointments>(`${this.url}/user/${userId}`);
  }

  getPetAppointments(petId: number) {
    return this.http.get<PetAppointments>(`${this.url}/pet/${petId}`);
  }

  getShelterAppointments(shelterId: number) {
    return this.http.get<ShelterAppointments>(`${this.url}/shelter/${shelterId}`);
  }

  addAppointment(appointment: Appointment): Observable<Appointment> {
    return this.http.post<Appointment>(this.url, appointment);
  }

  deleteAppointment(id: number): Observable<null> {
    return this.http.delete<null>(`${this.url}/${id}/cancel`);
  }
}
