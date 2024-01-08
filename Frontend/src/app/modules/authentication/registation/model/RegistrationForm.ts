import { PersonalData } from "src/app/modules/users/models/PersonalData"
import { Role } from "src/app/modules/users/models/User"

export interface RegistrationForm {
    password?: string
    repeatPassword?: string
    role?: number
    email?: string
    userDetails: PersonalData
    blocked?: boolean
    shelterId? :number
}