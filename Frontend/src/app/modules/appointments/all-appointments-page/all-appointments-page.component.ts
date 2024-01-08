import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AttachmentService } from '../../../shared/components/attachment/services/attachment.service';
import { ImagesService } from '../../../shared/services/images/images.service';
import { AppointmentService } from '../services/appointment.service';
import { AppointmentWithPet } from '../models/AllAppointments';
import { Appointment } from '../models/Appointment';
import { LoggedInUserService } from '../../authentication/login/services/logged-in-user/logged-in-user.service';
import { UserService } from '../../users/services/user.service';
import { PetService } from '../../pets/services/pet.service';

@Component({
  selector: 'app-all-appointments-page',
  templateUrl: './all-appointments-page.component.html',
  styleUrls: ['./all-appointments-page.component.scss'],
})
export class AllAppointmentsPageComponent implements OnInit {
  constructor(
    private appointmentService: AppointmentService,
    private attachmentService: AttachmentService,
    private router: Router,
    private imagesService: ImagesService,
    private loggedInUser: LoggedInUserService,
    private userService: UserService,
    private petService: PetService
  ) { }

  appointments: AppointmentWithPet[] = [];

  isLoading: boolean = true;

  ngOnInit(): void {
    this.getAppointments();
  }

  getAppointments() {
    this.appointmentService.getShelterAppointments(this.getShelterId()).subscribe((appointments) => {
      this.appointments = appointments.pets;
      this.isLoading = true;
      this.appointments = this.appointments.filter((app) => !!app.appointments && app.appointments.length > 0)
      if (this.appointments.length === 0) {
        this.isLoading = false
      }
      this.appointments.forEach((app) => {
        app.appointments = app.appointments.filter(ap => ap.appointmentStatus == 1)
        this.petService.getPetById(app.petId).subscribe((pet) => {
          app.pet = pet.content
          app.mainImageId = pet.attachments?.at(0)?.id
          this.attachmentService
          if (!!app.mainImageId) {
            this.attachmentService
              .getImage(app.mainImageId)
              .subscribe((mainImage: Blob) => {
                this.imagesService.createImageFromBlobInPost(mainImage, app.pet!);
              });
          }
        })
        app.appointments.forEach((appointment) => {
          this.userService.getUserById(appointment.userId).subscribe((user) => {
            appointment.user = user
          })
        })
        this.isLoading = false;
      })
    })
  }

  displayPet(appointment: AppointmentWithPet) {
    this.router.navigate([`/pets/${appointment.petId}`]);
  }

  deleteAppointment(dateAppointment: Appointment) {
    this.appointmentService.deleteAppointment(dateAppointment.id!).subscribe(() => this.getAppointments());
  }

  getShelterId() {
    return this.loggedInUser.getLoggedInUser().shelterId!;
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
