import { KeyValue } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { CustomErrorStateMatcher } from '../../../shared/utils/CustomErrorStateMatcher';
import { Appointment } from '../models/Appointment';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AppointmentService } from '../services/appointment.service';
import { LoggedInUserService } from '../../authentication/login/services/logged-in-user/logged-in-user.service';
import { DateAdapter } from '@angular/material/core';
import { AvailableTime } from '../models/TakenAppointment';
import { getLocale } from '../../../shared/utils/GetLocale';
import { getFormattedDate } from 'src/app/shared/utils/GetFormattedDate';

@Component({
  selector: 'app-appointment-scheduler',
  templateUrl: './appointment-scheduler.component.html',
  styleUrls: ['./appointment-scheduler.component.scss'],
})
export class AppointmentSchedulerComponent implements OnInit {
  constructor(
    public appointmentSchedulerDialog: MatDialogRef<AppointmentSchedulerComponent>,
    public formBuilder: FormBuilder,
    private appointmentService: AppointmentService,
    private loggedInUserService: LoggedInUserService,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private adapter: DateAdapter<any>
  ) {
    appointmentSchedulerDialog.disableClose = true;
  }

  form!: FormGroup;
  matcher = new CustomErrorStateMatcher();
  minDate!: Date;
  maxDate!: Date;

  petId!: number;

  AvailableTime = AvailableTime;

  availableTimes: string[] = [];

  isPetTaken: boolean = false;

  originalOrder = (
    a: KeyValue<AvailableTime, AvailableTime>,
    b: KeyValue<AvailableTime, AvailableTime>
  ): number => {
    return 0;
  };

  weekendsFilter = (d: Date | null): boolean => {
    const day = (d || new Date()).getDay();
    return day !== 0 && day !== 6;
  };

  ngOnInit(): void {
    this.adapter.setLocale(getLocale());

    this.petId = this.data;

    Object.keys(AvailableTime).forEach((time) => {
      this.availableTimes.push(time);
    });

    this.form = this.formBuilder.group({
      dateControl: ['', Validators.required],
      timeControl: ['', Validators.required],
    });
    this.minDate = new Date();
    this.maxDate = new Date();
    this.maxDate.setDate(this.minDate.getDate() + 20);
  }

  dateChanged() {
    this.isPetTaken = false;
    this.availableTimes = [];
    const localDate: Date = this.form.get('dateControl')?.value;
    const correctedDate = new Date(localDate);
    correctedDate.setDate(correctedDate.getDate() + 1);
    const targetDate = correctedDate.toISOString().split('T')[0];
    this.getAvailableTimes(targetDate)
  }

  getAvailableTimes(targetDate: string) {
    this.appointmentService
      .getPetAppointments(this.petId)
      .subscribe((petAppointments) => {

        const appointmentHours = this.getTakenHours(petAppointments.appointments, targetDate)

        Object.keys(AvailableTime).forEach((time) => {
          this.availableTimes.push(time);

          appointmentHours.forEach((taken) => {
            const index = this.availableTimes.indexOf(taken);

            if (index >= 0) {
              this.availableTimes.splice(index, 1);
              if (this.availableTimes.length === 0) {
                this.isPetTaken = true;
              }
            }
          });
        })
      });
  }

  getTakenHours(appointments: Appointment[], targetDate: string) {
    const filteredAppointments: Appointment[] = appointments.filter(appointment => {
      const startDate = new Date(appointment.appointmentTimeStart).toISOString().split('T')[0];
      return startDate === targetDate;
    });

    const appointmentHours: string[] = filteredAppointments.map(appointment => {
      const startTime = new Date(appointment.appointmentTimeStart);
      const formattedTime = startTime.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
      return formattedTime;
    });

    return appointmentHours;
  }

  onSubmitClick(): void {
    const formattedDate = getFormattedDate(this.form.get('dateControl')?.value);
    const appointmentTime = this.form.get('timeControl')?.value
    const combinedDateTimeString = `${formattedDate}T${appointmentTime}:00.000Z`;

    const dateTime = new Date(combinedDateTimeString);
    const formattedDateTimeString = dateTime.toISOString();
    dateTime.setHours(dateTime.getHours() + 1);
    const formattedDateTimeStringWithOneHourAdded = dateTime.toISOString();

    let appointment: Appointment = {
      appointmentTimeStart: formattedDateTimeString,
      appointmentTimeEnd: formattedDateTimeStringWithOneHourAdded,
      petId: this.petId,
      userId: this.loggedInUserService.getLoggedInUser().id!,
    };

    this.appointmentService
      .addAppointment(appointment)
      .subscribe((appointment) =>
        this.appointmentSchedulerDialog.close({ data: 1 })
      );
  }

  onCancelClick(): void {
    this.appointmentSchedulerDialog.close({ data: 0 });
  }
}
