import { Pet } from "../../pets/models/Pet"
import { User } from "../../users/models/User"
import { Appointment } from "./Appointment"

export interface ShelterAppointments {
    pets: AppointmentWithPet[]
}

export interface PetAppointments {
    appointments: Appointment[]
}

export interface AppointmentWithPet {
    petId: number 
    appointments: Appointment[]
    pet?: Pet
    mainImageId?: number
}