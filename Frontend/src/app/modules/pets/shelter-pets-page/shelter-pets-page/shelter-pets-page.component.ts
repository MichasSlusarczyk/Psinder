import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoggedInUserService } from 'src/app/modules/authentication/login/services/logged-in-user/logged-in-user.service';
import { AttachmentService } from 'src/app/shared/components/attachment/services/attachment.service';
import { EnumsService } from 'src/app/shared/services/enums/enums.service';
import { ImagesService } from 'src/app/shared/services/images/images.service';
import { Pet, Gender } from '../../models/Pet';
import { PetService } from '../../services/pet.service';

@Component({
  selector: 'app-shelter-pets-page',
  templateUrl: './shelter-pets-page.component.html',
  styleUrls: ['./shelter-pets-page.component.scss']
})
export class ShelterPetsPageComponent implements OnInit {

  constructor(
    private petService: PetService,
    private attachmentService: AttachmentService,
    private router: Router,
    private imagesService: ImagesService,
    private enumsService: EnumsService,
    private loggedInUser: LoggedInUserService
  ) { }

  pets!: Pet[];
  isDataLoaded: boolean = false;
  Gender = Gender;

  date: Date = new Date();
  shelterId!: number

  @Input() maxLength = 100;

  ngOnInit(): void {
    this.getPets();
    this.shelterId = this.getShelterId();
  }

  getGender(pet: Pet) {
    return this.enumsService.getGenderFromOrdinal(pet.gender);
  }

  getSize(pet: Pet) {
    return this.enumsService.getSizeFromOrdinal(pet.size)
  }

  displayPet(pet: Pet) {
    this.router.navigate([`/pets/${pet.id}`]);
  }

  getPets() {
    this.petService.getPetsFromShelter(this.getShelterId()).subscribe((page) => {
      this.pets = page.list;
      this.isDataLoaded = true;

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
    });
  }

  getShelterId() {
    return this.loggedInUser.getLoggedInUser().shelterId!;
  }
}
