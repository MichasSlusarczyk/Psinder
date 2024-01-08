import { Injectable } from '@angular/core';
import { AttitudesTowardsChildren, AttitudesTowardsChildrenPL, AttitudesTowardsOtherDogs, AttitudesTowardsOtherDogsPL, Gender, GenderPL, PetSize, PetSizePL, PetTraits, PetTraitsPL, PhysicalActivities, PhysicalActivitiesPL } from 'src/app/modules/pets/models/Pet';
import { PersonGender, PersonGenderPL } from 'src/app/modules/users/models/PersonalData';
import { Role, RolePL } from 'src/app/modules/users/models/User';

@Injectable({
  providedIn: 'root'
})
export class EnumsService {

  constructor() { }

  getGender() {
    return localStorage.getItem('language') === 'en' ? Gender : GenderPL;
  }
  
  getPetSize() {
    return localStorage.getItem('language') === 'en' ? PetSize : PetSizePL;
  }
  
  getRole() {
    return localStorage.getItem('language') === 'en' ? Role : RolePL;
  }

  getPersonGender() {
    return localStorage.getItem('language') === 'en' ? PersonGender : PersonGenderPL;
  }

  getPhysicalActivities() {
    return localStorage.getItem('language') === 'en' ? PhysicalActivities : PhysicalActivitiesPL;
  }
  
  getAttitudesTowardsChildren() {
    return localStorage.getItem('language') === 'en' ? AttitudesTowardsChildren : AttitudesTowardsChildrenPL;
  }

  getAttitudesTowardsOtherDogs() {
    return localStorage.getItem('language') === 'en' ? AttitudesTowardsOtherDogs : AttitudesTowardsOtherDogsPL;
  }

  getPetTraits() {
    return localStorage.getItem('language') === 'en' ? PetTraits : PetTraitsPL;
  }

  getGenderFromEnum(gender: Gender) {
    if (localStorage.getItem('language') === 'en' || !!!localStorage.getItem('language')) {
      let index = Object.keys(Gender).indexOf(gender);
      return Object.values(Gender).at(index);
    } else {
      let index = Object.keys(GenderPL).indexOf(gender);
      return Object.values(GenderPL).at(index);
    }
  }
  
  getSizeFromEnum(size: PetSize) {
    if (localStorage.getItem('language') === 'en' || !!!localStorage.getItem('language')) {
      let index = Object.keys(PetSize).indexOf(size);
      return Object.values(PetSize).at(index);
    } else {
      let index = Object.keys(PetSizePL).indexOf(size);
      return Object.values(PetSizePL).at(index);
    }
  }

  getGenderOrdinal(gender: Gender) {
    if (localStorage.getItem('language') === 'en' || !!!localStorage.getItem('language')) {
      return Object.keys(Gender).indexOf(gender) + 1;
    } else {
      return Object.keys(GenderPL).indexOf(gender) + 1;
    }
  }

  getGenderFromOrdinal(gender: number) {
    if (localStorage.getItem('language') === 'en' || !!!localStorage.getItem('language')) {
      return Object.keys(Gender).at(gender - 1);
    } else {
      return Object.keys(GenderPL).at(gender - 1);
    }
  }

  getSizeOrdinal(size: PetSize) {
    if (localStorage.getItem('language') === 'en' || !!!localStorage.getItem('language')) {
      return Object.keys(PetSize).indexOf(size) + 1;
    } else {
      return Object.keys(PetSizePL).indexOf(size) + 1;
    }
  }

  getSizeFromOrdinal(size: number) {
    if (localStorage.getItem('language') === 'en' || !!!localStorage.getItem('language')) {
      return Object.keys(PetSize).at(size - 1);
    } else {
      return Object.keys(PetSizePL).at(size - 1);
    }
  }

  getPersonGenderOrdinal(gender: PersonGender) {
    if (localStorage.getItem('language') === 'en' || !!!localStorage.getItem('language')) {
      return Object.keys(PersonGender).indexOf(gender) + 1;
    } else {
      return Object.keys(PersonGenderPL).indexOf(gender) + 1;
    }
  }

  getPersonGenderFromOrdinal(gender: number) {
    if (localStorage.getItem('language') === 'en' || !!!localStorage.getItem('language')) {
      return Object.keys(PersonGender).at(gender - 1);
    } else {
      return Object.keys(PersonGenderPL).at(gender - 1);
    }
  }

  getRoleOrdinal(role: Role) {
    if (localStorage.getItem('language') === 'en' || !!!localStorage.getItem('language')) {
      return Object.keys(Role).indexOf(role) + 1;
    } else {
      return Object.keys(RolePL).indexOf(role) + 1;
    }
  }

  getRoleFromOrdinal(role: number) {
    if (localStorage.getItem('language') === 'en' || !!!localStorage.getItem('language')) {
      return Object.keys(Role).at(role - 1);
    } else {
      return Object.keys(RolePL).at(role - 1);
    }
  }

  getPhysicalActivitiesOrdinal(role: PhysicalActivities) {
    if (localStorage.getItem('language') === 'en' || !!!localStorage.getItem('language')) {
      return Object.keys(PhysicalActivities).indexOf(role) + 1;
    } else {
      return Object.keys(PhysicalActivitiesPL).indexOf(role) + 1;
    }
  }

  getPhysicalActivitiesFromOrdinal(role: number) {
    if (localStorage.getItem('language') === 'en' || !!!localStorage.getItem('language')) {
      return Object.keys(PhysicalActivities).at(role - 1);
    } else {
      return Object.keys(PhysicalActivitiesPL).at(role - 1);
    }
  }

  getPhysicalActivitiesFromOrdinalValue(role: number) {
    if (localStorage.getItem('language') === 'en' || !!!localStorage.getItem('language')) {
      return Object.values(PhysicalActivities).at(role - 1);
    } else {
      return Object.values(PhysicalActivitiesPL).at(role - 1);
    }
  }

  getAttitudesTowardsOtherDogsOrdinal(role: AttitudesTowardsOtherDogs) {
    if (localStorage.getItem('language') === 'en' || !!!localStorage.getItem('language')) {
      return Object.keys(AttitudesTowardsOtherDogs).indexOf(role) + 1;
    } else {
      return Object.keys(AttitudesTowardsOtherDogsPL).indexOf(role) + 1;
    }
  }

  getAttitudesTowardsOtherDogsFromOrdinal(role: number) {
    if (localStorage.getItem('language') === 'en' || !!!localStorage.getItem('language')) {
      return Object.keys(AttitudesTowardsOtherDogs).at(role - 1);
    } else {
      return Object.keys(AttitudesTowardsOtherDogsPL).at(role - 1);
    }
  }

  getAttitudesTowardsOtherDogsFromOrdinalValue(role: number) {
    if (localStorage.getItem('language') === 'en' || !!!localStorage.getItem('language')) {
      return Object.values(AttitudesTowardsOtherDogs).at(role - 1);
    } else {
      return Object.values(AttitudesTowardsOtherDogsPL).at(role - 1);
    }
  }

  getAttitudesTowardsChildrenOrdinal(role: AttitudesTowardsChildren) {
    if (localStorage.getItem('language') === 'en' || !!!localStorage.getItem('language')) {
      return Object.keys(AttitudesTowardsChildren).indexOf(role) + 1;
    } else {
      return Object.keys(AttitudesTowardsChildrenPL).indexOf(role) + 1;
    }
  }

  getAttitudesTowardsChildrenFromOrdinal(role: number) {
    if (localStorage.getItem('language') === 'en' || !!!localStorage.getItem('language')) {
      return Object.keys(AttitudesTowardsChildren).at(role - 1);
    } else {
      return Object.keys(AttitudesTowardsChildrenPL).at(role - 1);
    }
  }

  getAttitudesTowardsChildrenFromOrdinalValue(role: number) {
    if (localStorage.getItem('language') === 'en' || !!!localStorage.getItem('language')) {
      return Object.values(AttitudesTowardsChildren).at(role - 1);
    } else {
      return Object.values(AttitudesTowardsChildrenPL).at(role - 1);
    }
  }

  getPetTraitsOrdinal(role: PetTraits) {
    if (localStorage.getItem('language') === 'en' || !!!localStorage.getItem('language')) {
      return Object.keys(PetTraits).indexOf(role) + 1;
    } else {
      return Object.keys(PetTraitsPL).indexOf(role) + 1;
    }
  }

  getPetTraitsFromOrdinal(role: number) {
    if (localStorage.getItem('language') === 'en' || !!!localStorage.getItem('language')) {
      return Object.keys(PetTraits).at(role - 1);
    } else {
      return Object.keys(PetTraitsPL).at(role - 1);
    }
  }

  getPetTraitsFromOrdinalValue(role: number) {
    if (localStorage.getItem('language') === 'en' || !!!localStorage.getItem('language')) {
      return Object.values(PetTraits).at(role - 1);
    } else {
      return Object.values(PetTraitsPL).at(role - 1);
    }
  }
}
