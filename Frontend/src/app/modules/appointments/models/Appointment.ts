import { Pet } from "../../pets/models/Pet";
import { User } from "../../users/models/User";

export interface Appointment {
    id?: number
    appointmentTimeStart: string;
    appointmentTimeEnd: string;
    petId: number
    userId: number
    user?: User
    pet?: Pet
    appointmentStatus?: number
}