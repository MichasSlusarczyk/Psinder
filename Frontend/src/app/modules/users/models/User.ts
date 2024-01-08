import {PersonalData} from './PersonalData'

export interface User {
    id?: number
    password?: string
    role?: number
    email?: string
    userDetails: PersonalData
    blocked?: boolean
    shelterId?: number
    accountStatus?: number
}

export enum Role {
    ADMIN = 'ADMIN',
    USER = 'USER',
    WORKER = 'WORKER'
}

export enum RolePL {
    ADMIN = 'ADMIN',
    USER = 'UŻYTKOWNIK',
    WORKER = 'PRACOWNIK'
}

export interface ChangePassword {
    oldPassword: string
    newPassword: string
    repeatNewPassword: string
}

export interface UserResponse {
    users: User[]
}