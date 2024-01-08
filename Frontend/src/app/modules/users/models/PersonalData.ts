export interface PersonalData {
    id?: number
    firstName: string
    lastName: string
    phoneNumber: string
    city: string
    postalCode?: string
    street?: string
    streetNumber?: string
    gender: number
    birthDate?: string
}

export enum PersonGender {
    WOMAN = 'Woman',
    MAN = 'Man',
    OTHER = 'Other',
    PREFER_NOT_TO_RESPOND = 'Prefer not to respond'
}



export enum PersonGenderPL {
    WOMAN = 'Kobieta',
    MAN = 'Mężczyzna',
    OTHER = 'Inne',
    PREFER_NOT_TO_RESPOND = 'Wolę nie odpowiadać'
}