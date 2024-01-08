import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { AppointmentSchedulerComponent } from '../../appointments/appointment-scheduler/appointment-scheduler.component';
import { AttachmentService } from '../../../shared/components/attachment/services/attachment.service';
import { CarouselImage } from '../../../shared/components/carousel/carousel.component';
import { DialogBodyComponent } from '../../../shared/components/dialog-body/dialog-body.component';
import { LoggedInUserService } from '../../authentication/login/services/logged-in-user/logged-in-user.service';
import { Gender, Pet, PhysicalActivities } from '../models/Pet';
import { PetService } from '../services/pet.service';
import { EnumsService } from 'src/app/shared/services/enums/enums.service';
import { ImageDto } from 'src/app/shared/components/attachment/models/Attachment';
import { ShelterService } from '../../shelter/services/shelter.service';

@Component({
  selector: 'app-pet-page-element',
  templateUrl: './pet-page-element.component.html',
  styleUrls: ['./pet-page-element.component.scss'],
})
export class PetPageElementComponent implements OnInit {
  constructor(
    private petService: PetService,
    private route: ActivatedRoute,
    private attachmentService: AttachmentService,
    private snackbar: MatSnackBar,
    private dialog: MatDialog,
    private router: Router,
    private loggedInUserService: LoggedInUserService,
    private translate: TranslateService,
    private enumsService: EnumsService,
    private shelterService: ShelterService,
    private cdr: ChangeDetectorRef
  ) { }

  pet!: Pet;
  id!: number;
  images: CarouselImage[] = [];
  date: Date = new Date();
  previousUrl!: string
  Gender = Gender;

  shelterAddress!: string
  shelterCity!: string

  ngOnInit(): void {
    Object.keys;
    if (!this.pet) {
      this.route.params.subscribe((params) => (this.id = params['id']));
      if (this.id) {
        this.petService.getPetById(this.id).subscribe((pet) => {
          this.pet = pet.content;
          this.addPetImages(pet.attachments!);
          this.setMap(this.pet.shelterId)
        });
      }
    }

    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.previousUrl = this.router.url;
      }
    });
  }

  getGender() {
    return this.enumsService.getGenderFromOrdinal(this.pet.gender)
  }

  getSize() {
    return this.enumsService.getSizeFromOrdinal(this.pet.size)
  }

  getPA() {
    return this.enumsService.getPhysicalActivitiesFromOrdinalValue(this.pet.physicalActivity)
  }

  getATC() {
    return this.enumsService.getAttitudesTowardsChildrenFromOrdinalValue(this.pet.attitudeTowardsChildren)
  }

  getATD() {
    return this.enumsService.getAttitudesTowardsOtherDogsFromOrdinalValue(this.pet.attitudeTowardsOtherDogs)
  }

  getPT() {
    return this.pet.petTraits.map(pt => this.enumsService.getPetTraitsFromOrdinalValue(pt));
  }


  isFromOurShelter() {
    return this.loggedInUserService.getLoggedInUser().shelterId === this.pet.shelterId
  }

  addPetImages(attachments: ImageDto[]) {
    attachments.forEach((petImage) => {
      this.attachmentService.getImage(petImage.id!).subscribe((image) => {
        var reader = new FileReader();
        reader.readAsDataURL(image);

        reader.onload = (event) => {
          let imageToAdd: CarouselImage = {
            imageSource: reader.result,
            imageAlt: image.type,
          };
          this.images.push(imageToAdd);
        };
      });
    });
  }

  deletePost() {
    this.translate.get('Dialog.DeletePost').subscribe((dialog: string) => {
      const dialogRef = this.dialog.open(DialogBodyComponent, {
        width: '250px',
        data: {
          result: 0,
          message: dialog,
        },
      });
      dialogRef.afterClosed().subscribe((result) => {
        if (result.data == 1) {
          this.petService.deletePet(this.id!).subscribe((deleted) => {
            this.router.navigate(['/our-shelter']);
            this.translate
              .get('Snackbar.PostDeleted')
              .subscribe((message: string) =>
                this.snackbar.open(message, 'X', { duration: 2000 })
              );
          });
        }
      });
    });
  }

  makeAnAppointment() {
    const dialogRef = this.dialog.open(AppointmentSchedulerComponent, {
      width: '60%',
      data: this.id,
    });
    dialogRef.afterClosed().subscribe((result) => {
      if (result.data == 1) {
        this.translate
          .get('Snackbar.AppointmentArranged')
          .subscribe((message: string) =>
            this.snackbar.open(message, 'X', { duration: 2000 })
          );
      }
    });
  }

  areUserDetailsFilledIn() {
    return !!this.loggedInUserService.getLoggedInUser() && !!this.loggedInUserService.getLoggedInUser().userDetails
  }

  isAdminLoggedIn() {
    return this.loggedInUserService.isAdminLoggedIn();
  }

  isUserLoggedIn() {
    return this.loggedInUserService.isUserLoggedIn();
  }

  isWorkerLoggedIn(): boolean {
    return this.loggedInUserService.isOnlyWorkerLoggedIn()
  }

  setMap(id: number) {
    this.shelterService.getShelterById(id).subscribe((shelter) => {
      this.shelterCity = shelter.city
      this.shelterAddress = shelter.address
      this.cdr.detectChanges();
    })
  }

  isMapVisible() {
    return this.shelterAddress !== undefined && this.shelterCity !== undefined
  }

  getAddress() {
    return this.shelterAddress
  }

  getCity() {
    return this.shelterCity
  }
}
