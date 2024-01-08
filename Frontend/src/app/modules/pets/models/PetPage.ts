import { Pet } from "./Pet"

export interface PetPage {
    pageNumber: number
    pageSize: number 
    totalSize: number 
    list: Pet[]
}