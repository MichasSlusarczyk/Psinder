export interface Shelter {
    id?: number
    name: string
    city: string
    address: string
    description: string
    phoneNumber: string
    email: string
}

export interface ShelterResponse {
    shelters: Shelter[]
}