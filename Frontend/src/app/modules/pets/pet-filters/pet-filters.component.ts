import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { PetFilter } from '../models/PetFilter';
import { EnumsService } from 'src/app/shared/services/enums/enums.service';
import { PetTraits } from '../models/Pet';
import { PetFilterService } from './services/pet-filter.service';
import { Shelter } from '../../shelter/models/Shelter';
import { ShelterService } from '../../shelter/services/shelter.service';

@Component({
  selector: 'app-pet-filters',
  templateUrl: './pet-filters.component.html',
  styleUrls: ['./pet-filters.component.scss'],
})
export class PetFiltersComponent implements OnInit {
  constructor(private formBuilder: FormBuilder, private enumsService: EnumsService, private filterService: PetFilterService,
    private shelterService: ShelterService) { }

  form!: FormGroup;

  @Output() filterPetsEvent: EventEmitter<PetFilter> =
    new EventEmitter<PetFilter>();

  @Output() clearPetsFiltersEvent: EventEmitter<null> =
    new EventEmitter<null>();

  @Output() petsFromLocationFiltersEvent: EventEmitter<null> =
    new EventEmitter<null>();

  @Output() choosenShelterFiltersEvent: EventEmitter<number> =
    new EventEmitter<number>();

  shelters: Shelter[] = []

  shelterId!: number | undefined

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      breedControl: [null],
      yearOfBirthControl: [null],
      numberControl: [null],
      nameControl: [null],
      genderControl: [null],
      petSizeControl: [null],
      petPAControl: [null],
      petATDControl: [null],
      petATCControl: [null],
      petTraitsControl: [null],
      shelterControl: [null]
    });

    this.setFilterValues(this.filterService.getFilters())
    this.shelterId = this.filterService.getFilters().shelterId
    this.getShelters()
  }

  getSheltersFromLocation() {
    this.petsFromLocationFiltersEvent.emit()
  }

  onShelterChange() {
    this.choosenShelterFiltersEvent.emit(this.form.get('shelterControl')?.value)
    this.shelterId = this.form.get('shelterControl')?.value
  }

  clearFilters() {
    this.clearPetsFiltersEvent.emit();
    this.form.get('nameControl')?.setValue(null)
    this.form.get('breedControl')?.setValue(null)
    this.form.get('yearOfBirthControl')?.setValue(null)
    this.form.get('numberControl')?.setValue(null)
    this.form.get('genderControl')?.setValue(null)
    this.form.get('petSizeControl')?.setValue(null)
    this.form.get('petPAControl')?.setValue(null)
    this.form.get('petATDControl')?.setValue(null)
    this.form.get('petATCControl')?.setValue(null)
    this.form.get('petTraitsControl')?.setValue(null)
    this.form.get('shelterControl')?.setValue(null)
    this.shelterId = undefined
  }

  filter() {
    let petFilter: PetFilter = {
      name: this.form.get('nameControl')?.value ? this.form.get('nameControl')?.value : undefined,
      breed: this.form.get('breedControl')?.value ? this.form.get('breedControl')?.value : undefined,
      yearOfBirth: this.form.get('yearOfBirthControl')?.value ? (new Date().getFullYear() - this.form.get('yearOfBirthControl')?.value).toString() : undefined,
      number: this.form.get('numberControl')?.value ? this.form.get('numberControl')?.value : undefined,
      gender: this.form.get('genderControl')?.value ? this.enumsService.getGenderOrdinal(this.form.get('genderControl')?.value) : undefined,
      size: this.form.get('petSizeControl')?.value ? this.enumsService.getSizeOrdinal(this.form.get('petSizeControl')?.value) : undefined,
      physicalActivity: this.form.get('petPAControl')?.value ? this.enumsService.getPhysicalActivitiesOrdinal(this.form.get('petPAControl')?.value) : undefined,
      attitudeTowardsOtherDogs: this.form.get('petATDControl')?.value ? this.enumsService.getAttitudesTowardsOtherDogsOrdinal(this.form.get('petATDControl')?.value) : undefined,
      attitudeTowardsChildren: this.form.get('petATCControl')?.value ? this.enumsService.getAttitudesTowardsChildrenOrdinal(this.form.get('petATCControl')?.value) : undefined,
      petTraits: this.getSelectedPetTraits()
    };

    this.filterPetsEvent.emit(petFilter);
  }

  setFilterValues(filters: PetFilter) {
    this.form.get('nameControl')?.setValue(filters.name)
    this.form.get('breedControl')?.setValue(filters.breed)
    this.form.get('yearOfBirthControl')?.setValue(filters.yearOfBirth)
    this.form.get('numberControl')?.setValue(filters.number)
    this.form.get('genderControl')?.setValue(!!filters.gender ? this.enumsService.getGenderFromOrdinal(filters.gender) : null)
    this.form.get('petSizeControl')?.setValue(!!filters.size ? this.enumsService.getSizeFromOrdinal(filters.size) : null)
    this.form.get('petPAControl')?.setValue(!!filters.physicalActivity ? this.enumsService.getPhysicalActivitiesFromOrdinal(filters.physicalActivity) : null)
    this.form.get('petATDControl')?.setValue(!!filters.attitudeTowardsOtherDogs ? this.enumsService.getAttitudesTowardsOtherDogsFromOrdinal(filters.attitudeTowardsOtherDogs) : null)
    this.form.get('petATCControl')?.setValue(!!filters.attitudeTowardsChildren ? this.enumsService.getAttitudesTowardsChildrenFromOrdinal(filters.attitudeTowardsChildren) : null)
    this.form.get('petTraitsControl')?.setValue(!!filters.petTraits ? this.getSelectedPetTraitsFromOrdinal(filters) : null)
    this.shelterId = filters.shelterId
  }

  getSelectedPetTraits() {
    let petTraits: PetTraits[] = this.form.get('petTraitsControl')?.value;
    let petTraitsEnum: number[] = petTraits?.map(pt => this.enumsService.getPetTraitsOrdinal(pt));

    return petTraitsEnum
  }

  getSelectedPetTraitsFromOrdinal(filter: PetFilter) {
    return filter.petTraits?.map(pt => this.enumsService.getPetTraitsFromOrdinal(pt));
  }

  getPetSize() {
    return this.enumsService.getPetSize();
  }

  getPetGender() {
    return this.enumsService.getGender();
  }

  getPA() {
    return this.enumsService.getPhysicalActivities()
  }

  getATD() {
    return this.enumsService.getAttitudesTowardsOtherDogs()
  }

  getATC() {
    return this.enumsService.getAttitudesTowardsChildren()
  }

  getPT() {
    return this.enumsService.getPetTraits()
  }

  getShelters() {
    this.shelterService.getShelters().subscribe({
      next: (shelters) => {
        this.shelters = shelters.shelters
      },
      error: (error) => {
        console.log(error)
        this.shelters = []
      }
    })
  }
}
