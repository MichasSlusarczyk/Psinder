import { Injectable } from '@angular/core';
import { PetFilter } from '../../models/PetFilter';

@Injectable({
  providedIn: 'root'
})
export class PetFilterService {

  filters: PetFilter = {}
  page: number = 0
  pageSize: number = 10
  cities: string[] = []
  constructor() { }

  setFilters(petFilter: PetFilter, page: number, pageSize: number) {
    this.filters = petFilter
    this.page = page
    this.pageSize = pageSize
  }

  clearFilters() {
    this.filters = {}
  }

  getFilters() {
    return this.filters
  }

  getPage() {
    return this.page
  }

  getPageSize() {
    return this.pageSize
  }

  setCities(cities: string[]) {
    this.cities = cities
  } 

  getCities() {
    return this.cities
  }
}
