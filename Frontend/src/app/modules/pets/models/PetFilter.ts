
export interface PetFilter {
  name?: string 
  breed?: string
  yearOfBirth?: string
  gender?: number
  number?: string
  size?: number
  shelterId?: number
  physicalActivity?: number
  attitudeTowardsChildren?: number
  attitudeTowardsOtherDogs?: number
  petTraits?: number[]
  cities?: string[]
  page?: number
  pageSize?: number
  orderBy?: string
}
	