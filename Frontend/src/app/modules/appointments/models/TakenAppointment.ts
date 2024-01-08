export interface TakenAppointment {
    appointmentTime: string;
}

export interface TakenAppointmentsDto {
    petTaken: boolean
    takenAppointments: TakenAppointment[]
}

export enum AvailableTime {
    '08:00' = '8:00',
    '09:00' = '9:00',
    '10:00' = '10:00',
    '11:00' = '11:00',
    '12:00' = '12:00',
    '13:00' = '13:00',
    '14:00' = '14:00',
    '15:00' = '15:00',
}