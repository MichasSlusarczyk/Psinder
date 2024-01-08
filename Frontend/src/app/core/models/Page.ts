export interface Page<Type> {
    totalNumberOfPages: number
    totalNumberOfElements: number
    elements: Type[];
}