import { Component, Input, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { Router } from '@angular/router';
import { AttachmentService } from '../../../shared/components/attachment/services/attachment.service';
import { ImagesService } from '../../../shared/services/images/images.service';
import { Gender, Pet } from '../models/Pet';
import { PetFilter } from '../models/PetFilter';
import { PetService } from '../services/pet.service';
import { EnumsService } from 'src/app/shared/services/enums/enums.service';
import { GeolocationService } from '../../map/services/geolocation/geolocation.service';
import { Shelter } from '../../shelter/models/Shelter';
import { ShelterService } from '../../shelter/services/shelter.service';
import { LoggedInUserService } from '../../authentication/login/services/logged-in-user/logged-in-user.service';
import { FormBuilder, FormGroup } from '@angular/forms';
import { PetFilterService } from '../pet-filters/services/pet-filter.service';

@Component({
  selector: 'app-pet-page',
  templateUrl: './pet-page.component.html',
  styleUrls: ['./pet-page.component.scss'],
})
export class PetPageComponent implements OnInit {
  constructor(
    private petService: PetService,
    private attachmentService: AttachmentService,
    private router: Router,
    private imagesService: ImagesService,
    private enumsService: EnumsService,
    private geolocationService: GeolocationService,
    private loggedInUserService: LoggedInUserService,
    private formBuilder: FormBuilder,
    private filterService: PetFilterService
  ) { }

  pets!: Pet[];

  pageIndex: number = 0;
  pageSize: number = 10;
  length: number = 10;
  pageEvent!: PageEvent;

  Gender = Gender;

  date: Date = new Date();

  form!: FormGroup;

  isDataLoaded: boolean = false;

  filters!: PetFilter;

  selectedShelter?: Shelter
  cities: string[] = []

  @Input() maxLength = 100;

  ngOnInit(): void {
    this.isDataLoaded = false;

    this.form = this.formBuilder.group({
      shelterControl: [null]
    });

    this.filters = this.filterService.getFilters()
    this.pageSize = this.filterService.getPageSize()
    this.pageIndex = this.filterService.getPage()

    this.form.get('shelterControl')?.setValue(!!this.filters.shelterId ? this.filters.shelterId : null)

    this.getPets(this.filters);
  }

  getGender(pet: Pet) {
    return this.enumsService.getGenderFromOrdinal(pet.gender);
  }

  getSize(pet: Pet) {
    return this.enumsService.getSizeFromOrdinal(pet.size)
  }

  getPA(pet: Pet) {
    return this.enumsService.getPhysicalActivitiesFromOrdinalValue(pet.physicalActivity)
  }

  getATC(pet: Pet) {
    return this.enumsService.getAttitudesTowardsChildrenFromOrdinalValue(pet.attitudeTowardsChildren)
  }

  getATD(pet: Pet) {
    return this.enumsService.getAttitudesTowardsOtherDogsFromOrdinalValue(pet.attitudeTowardsOtherDogs)
  }

  getPT(pet: Pet) {
    return pet.petTraits.map(pt => this.enumsService.getPetTraitsFromOrdinalValue(pt));
  }

  pageChanged(pageEvent: PageEvent) {
    if(this.pageIndex !== pageEvent.pageIndex) {
      this.pageIndex = pageEvent.pageIndex;
    }
    this.isDataLoaded = false;
    this.pageSize = pageEvent.pageSize;
    this.filterService.setFilters(this.filters, this.pageIndex, this.pageSize)

    this.getPets(this.filters);

    return pageEvent;
  }

  displayPet(pet: Pet) {
    this.router.navigate([`/pets/${pet.id}`]);
  }

  filterPets(petFilter: PetFilter) {
    this.isDataLoaded = false;
    petFilter.cities = this.filters.cities
    petFilter.shelterId = this.filters.shelterId
    this.filters = petFilter
    this.filterService.setFilters(this.filters, this.pageIndex, this.pageSize)
    this.getPets(petFilter);
  }

  clearFilters() {
    this.isDataLoaded = false;

    this.filters = {}
    this.filterService.clearFilters()
    this.getPets(this.filters);
  }

  getPets(petFilter: PetFilter) {
    this.isDataLoaded = false;
    this.petService.getPets(petFilter, this.pageIndex, this.pageSize).subscribe((page) => {
      this.pets = page.list;
      this.isDataLoaded = true;
      this.length = page.totalSize;

      this.pets.forEach((pet) => {
        pet.mainImageId = pet.attachments?.at(0)?.id
        if (pet.mainImageId != null) {
          this.attachmentService
            .getImage(pet.mainImageId)
            .subscribe((mainImage: Blob) => {
              this.imagesService.createImageFromBlobInPost(mainImage, pet);
            });
        }
      });
      this.isDataLoaded = true
    });
  }

  getSheltersFromLocation() {
    this.isDataLoaded = false
    this.cities = this.filterService.getCities()

    if (!!this.cities && this.cities.length > 0) {
      this.getPetsFromCities(this.cities)
    } else {
      this.geolocationService.getGeolocation().subscribe(cities => {
        this.getPetsFromCities(cities)
      });
    }
  }

  getPetsFromCities(cities: string[]) {
    this.cities = cities;
    this.filters.shelterId = undefined
    this.filters.cities = cities;
    this.filterService.setCities(cities)
    this.form.get('shelterControl')?.setValue(null)
    this.filterService.setFilters(this.filters, this.pageIndex, this.pageSize)
    this.getPets(this.filters);
  }

  isWorkerSignedIn(): boolean {
    return this.loggedInUserService.isOnlyWorkerLoggedIn()
  }

  isShelterChosen(): boolean {
    return this.cities.length !== 0 || !!this.selectedShelter;
  }

  onShelterChange(shelterId: any) {
    if (!!!shelterId || shelterId === '') {
      this.filters.shelterId = undefined
      this.filterService.setFilters(this.filters, this.pageIndex, this.pageSize)
      this.getPets(this.filters)
    }
    if (shelterId !== undefined) {
      this.filters.shelterId = shelterId
      this.filters.cities = undefined
      this.filterService.setFilters(this.filters, this.pageIndex, this.pageSize)
      this.getPets(this.filters)
    }
  }
}
