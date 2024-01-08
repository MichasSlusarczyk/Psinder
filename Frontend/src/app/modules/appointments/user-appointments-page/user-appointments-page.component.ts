import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AttachmentService } from '../../../shared/components/attachment/services/attachment.service';
import { LoggedInUserService } from '../../authentication/login/services/logged-in-user/logged-in-user.service';
import { ImagesService } from '../../../shared/services/images/images.service';
import { AppointmentService } from '../services/appointment.service';
import { AppointmentWithPet, ShelterAppointments } from '../models/AllAppointments';
import { Appointment } from '../models/Appointment';
import { PetService } from '../../pets/services/pet.service';
import { Pet } from '../../pets/models/Pet';

@Component({
  selector: 'app-user-appointments-page',
  templateUrl: './user-appointments-page.component.html',
  styleUrls: ['./user-appointments-page.component.scss']
})
export class UserAppointmentsPageComponent implements OnInit {

  constructor(private loggedInUserService: LoggedInUserService,
    private appointmentService: AppointmentService,
    private attachmentService: AttachmentService,
    private router: Router,
    private imagesService: ImagesService,
    private petService: PetService
  ) { }


  userAppointments: Appointment[] = [];

  userId!: number;
  isProcessing: boolean = false;

  ngOnInit(): void {
    this.isProcessing = true;
    this.userId = this.loggedInUserService.getLoggedInUser().id!;

    this.getUserAppointments()
  }

  getUserAppointments() {
    this.appointmentService.getUserAppointments(this.userId).subscribe(appointments => {
      this.userAppointments = appointments.appointments;
      this.userAppointments = this.userAppointments.filter((app) => app.appointmentStatus !== 2)
      this.isProcessing = false;
      this.userAppointments.forEach((app) => {
        this.petService.getPetById(app.petId).subscribe((pet) => {
          app.pet = pet.content
          app.pet.mainImageId = pet.attachments?.at(0)?.id
          this.attachmentService
          if (!!app.pet.mainImageId) {
            this.attachmentService
              .getImage(app.pet.mainImageId)
              .subscribe((mainImage: Blob) => {
                this.imagesService.createImageFromBlobInPost(mainImage, app.pet!);
              });
          }
        })
        this.isProcessing = false;
      })
    })
  }

  displayPet(appointment: Appointment) {
    if(!!appointment) {
      this.router.navigate([`/pets/${appointment.petId}`])
    }
  }

  deleteAppointment(appointment: Appointment) {
    this.appointmentService.deleteAppointment(appointment.id!).subscribe(() => this.getUserAppointments());
  }

  getFormattedDate(date: string) {
    const originalDate = new Date(date);

    const day = originalDate.getDate();
    const month = originalDate.getMonth() + 1;
    const year = originalDate.getFullYear();
    const hours = originalDate.getHours();
    const minutes = originalDate.getMinutes();

    return `${day.toString().padStart(2, '0')}/${month.toString().padStart(2, '0')}/${year} ${hours.toString().padStart(2, '0')}:${minutes.toString().padStart(2, '0')}`;
  }
}
