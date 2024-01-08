import { KeyValue } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DateAdapter } from '@angular/material/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AttachmentService } from '../../../shared/components/attachment/services/attachment.service';
import { CustomErrorStateMatcher } from '../../../shared/utils/CustomErrorStateMatcher';
import { getLocale } from '../../../shared/utils/GetLocale';
import {
  Gender,
  PetSize,
  Pet,
  PetTraits,
} from '../models/Pet';
import { PetService } from '../services/pet.service';
import { FileWithImage } from '../../../shared/models/FileWithImage';
import { EnumsService } from 'src/app/shared/services/enums/enums.service';
import { LoggedInUserService } from '../../authentication/login/services/logged-in-user/logged-in-user.service';
import { ImageDto } from 'src/app/shared/components/attachment/models/Attachment';

@Component({
  selector: 'app-pet-form',
  templateUrl: './pet-form.component.html',
  styleUrls: ['./pet-form.component.scss'],
})
export class PetFormComponent implements OnInit {
  maxDate: Date = new Date();

  originalOrder = (a: KeyValue<any, any>, b: KeyValue<any, any>): number => {
    return 0;
  };

  constructor(
    private formBuilder: FormBuilder,
    private petService: PetService,
    private router: Router,
    private route: ActivatedRoute,
    private attachmentService: AttachmentService,
    private adapter: DateAdapter<any>,
    private enumsService: EnumsService,
    private loggedInUser: LoggedInUserService
  ) { }

  form!: FormGroup;
  matcher = new CustomErrorStateMatcher();
  filesWithImages: FileWithImage[] = [];
  selectedFiles: FileList | [] = [];

  imagesToDelete: number[] = [];

  pet!: Pet;
  id!: number;
  isEditing: boolean = false;
  isProcessing: boolean = false;
  petTraits: PetTraits[] = []

  selectedPetTraitsBeforeEditing: number[] = []
  petTraitsToDelete: number[] = []
  petTraitsToAdd: number[] = []

  getGender() {
    return this.enumsService.getGender();
  }

  getSize() {
    return this.enumsService.getPetSize();
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

  ngOnInit(): void {
    this.adapter.setLocale(getLocale());
    this.isProcessing = false;
    this.form = this.formBuilder.group({
      nameControl: ['', Validators.required],
      descriptionControl: ['', Validators.required],
      breedControl: ['', Validators.required],
      yearOfBirthControl: [2024, Validators.required],
      genderControl: [Gender.MALE, Validators.required],
      numberControl: ['', Validators.required],
      petSizeControl: [PetSize.MEDIUM, Validators.required],
      physicalActivityControl: [{}, Validators.required],
      attitudeDogsControl: [{}, Validators.required],
      attitudeChildrenControl: [{}, Validators.required],
      petTraitsControl: [[]],
      filesControl: [[]],
    });

    this.route.params.subscribe((params) => (this.id = params['id']));
    if (this.id) {
      this.isEditing = true;
      this.petService.getPetById(this.id).subscribe((petResponse) => {
        this.pet = petResponse.content;
        this.addPetImages(petResponse.attachments!);
        this.setFormDefaultValues(this.pet);
        this.selectedPetTraitsBeforeEditing = this.pet.petTraits
        this.form.get('petTraitsControl')?.valueChanges.subscribe((newValues) => {
          if (this.isEditing) {
            this.keepTrackOfEditedPetTraits(newValues)
          }
        });
      });
    } else {
      this.isEditing = false;
    }
  }

  enumMap: Record<number, PetTraits> = {
    1: PetTraits.DoesntBark,
    2: PetTraits.DefendsTheHouse,
    3: PetTraits.KnowsCommands,
    4: PetTraits.LikesToPlay,
    5: PetTraits.Shy,
    6: PetTraits.SuitableAsFirstDog,
    7: PetTraits.ShortHaired,
    8: PetTraits.LongHaired,
    9: PetTraits.DoesNotShedFur,
    10: PetTraits.Submissive,
    11: PetTraits.Dominant
  };

  setFormDefaultValues(pet: Pet) {
    this.form.get('nameControl')?.setValue(pet.name);
    this.form.get('descriptionControl')?.setValue(pet.description);
    this.form.get('breedControl')?.setValue(pet.breed);
    this.form.get('yearOfBirthControl')?.setValue(pet.yearOfBirth);
    this.form.get('genderControl')?.setValue(this.enumsService.getGenderFromOrdinal(pet.gender));
    this.form.get('numberControl')?.setValue(pet.number);
    this.form.get('petSizeControl')?.setValue(this.enumsService.getSizeFromOrdinal(pet.size));
    this.form.get('physicalActivityControl')?.setValue(this.enumsService.getPhysicalActivitiesFromOrdinal(pet.physicalActivity))
    this.form.get('attitudeDogsControl')?.setValue(this.enumsService.getAttitudesTowardsOtherDogsFromOrdinal(pet.attitudeTowardsOtherDogs))
    this.form.get('attitudeChildrenControl')?.setValue(this.enumsService.getAttitudesTowardsChildrenFromOrdinal(pet.attitudeTowardsChildren))
    this.form.get('petTraitsControl')?.setValue(this.getSelectedPetTraitsFromOrdinal(pet))
  }

  addPet() {
    console.log(this.form.get('petTraitsControl')?.value)
    this.isProcessing = true;

    let pet = this.getPetData();

    let formData = this.getFormDataWithPetData(pet);
    pet.petTraits.forEach((trait) => {
      formData.append('content.petTraits', trait.toString());
    });

    this.filesWithImages.forEach((file) =>
      formData.append('attachments', file.file!)
    );

    this.petService.addPet(formData).subscribe({
      next: (pet) => {
        this.router.navigate(['/our-shelter']);
      },
      error: (error) => { },
    });
  }

  editPet() {
    this.isProcessing = true;

    let petToUpdate: Pet = this.getPetData();
    petToUpdate.attachmentsToDelete = this.imagesToDelete;

    let formData = this.getFormDataWithPetData(petToUpdate);
    formData.append('content.id', this.id?.toString())
    this.petTraitsToAdd.forEach((trait) => {
      formData.append('content.petTraitsToAdd', trait.toString());
    });

    this.petTraitsToDelete.forEach((trait) => {
      formData.append('content.petTraitsToDelete', trait.toString());
    });

    petToUpdate.attachmentsToDelete?.forEach((imageToDelete) => {
      formData.append('attachmentsToDelete', imageToDelete.toString());
    });

    this.filesWithImages.forEach((file) => {
      if (!file.id) {
        formData.append('attachmentsToAdd', file.file!);
      }
    });

    this.petService.updatePet(formData).subscribe({
      next: (pet) => {
        this.router.navigate(['/our-shelter']);
      },
      error: (error) => {},
    });
  }

  keepTrackOfEditedPetTraits(newPetTraitsList: PetTraits[]) {
    let newList: number[] =  newPetTraitsList.map(pt => this.enumsService.getPetTraitsOrdinal(pt));
    this.petTraitsToAdd = newList.filter(item => !this.selectedPetTraitsBeforeEditing.includes(item));
    this.petTraitsToDelete = this.selectedPetTraitsBeforeEditing.filter(item => !newList.includes(item!));
  }


  getPetData(): Pet {
    return {
      name: this.form.get('nameControl')?.value,
      description: this.form.get('descriptionControl')?.value,
      breed: this.form.get('breedControl')?.value,
      yearOfBirth: this.form.get('yearOfBirthControl')?.value,
      gender: this.enumsService.getGenderOrdinal(this.form.get('genderControl')?.value),
      number: this.form.get('numberControl')?.value,
      size: this.enumsService.getSizeOrdinal(this.form.get('petSizeControl')?.value),
      physicalActivity: this.enumsService.getPhysicalActivitiesOrdinal(this.form.get('physicalActivityControl')?.value),
      attitudeTowardsOtherDogs: this.enumsService.getAttitudesTowardsOtherDogsOrdinal(this.form.get('attitudeDogsControl')?.value),
      attitudeTowardsChildren: this.enumsService.getAttitudesTowardsChildrenOrdinal(this.form.get('attitudeChildrenControl')?.value),
      petTraits: this.getSelectedPetTraits(),
      shelterId: this.getShelterId()!
    };
  }

  getSelectedPetTraits() {
    let petTraits: PetTraits[] = this.form.get('petTraitsControl')?.value;
    let petTraitsEnum: number[] = petTraits.map(pt => this.enumsService.getPetTraitsOrdinal(pt));

    return petTraitsEnum
  }

  getSelectedPetTraitsFromOrdinal(pet: Pet) {
    return pet.petTraits.map(pt => this.enumsService.getPetTraitsFromOrdinal(pt));
  }

  getFormDataWithPetData(pet: Pet): FormData {
    const formData = new FormData();

    formData.append('content.name', pet.name);
    formData.append('content.description', pet.description);
    formData.append('content.breed', pet.breed);
    formData.append('content.yearOfBirth', pet.yearOfBirth.toString());
    formData.append('content.gender', pet.gender.toString());
    formData.append('content.number', pet.number);
    formData.append('content.size', pet.size.toString());
    formData.append('content.physicalActivity', pet.physicalActivity.toString());
    formData.append('content.attitudeTowardsOtherDogs', pet.attitudeTowardsOtherDogs.toString());
    formData.append('content.attitudeTowardsChildren', pet.attitudeTowardsChildren.toString());
    formData.append('content.shelterId', pet.shelterId.toString())
    return formData;
  }

  addPetImages(attachments: ImageDto[]) {
    attachments.forEach((petImage) => {
      this.attachmentService.getImage(petImage.id!).subscribe((image) => {
        var reader = new FileReader();
        reader.readAsDataURL(image);

        reader.onload = (event) => {
          let imageToAdd: FileWithImage = {
            id: petImage.id,
            imageUrl: reader.result,
          };
          this.filesWithImages.push(imageToAdd);
        };
      });
    });
  }

  onFilesSelected(event: any): void {
    this.selectedFiles = event.target.files;

    for (let i = 0; i < this.selectedFiles.length; i++) {
      let fileWithImage: FileWithImage = {
        imageUrl: '',
        file: this.selectedFiles[i],
      };

      let reader = new FileReader();
      reader.readAsDataURL(fileWithImage.file!);

      reader.onload = (event) => {
        fileWithImage.imageUrl = reader.result;

        this.filesWithImages.push(fileWithImage);
      };
    }
  }

  removeFileWithImage(file: FileWithImage) {
    const index = this.filesWithImages.indexOf(file);

    if (index >= 0) {
      if (file.id) {
        this.imagesToDelete.push(file.id);
      }
      this.filesWithImages.splice(index, 1);
    }
  }

  getShelterId() {
    return this.loggedInUser.getLoggedInUser().shelterId;
  }
}
