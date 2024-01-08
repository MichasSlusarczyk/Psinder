import { Component, OnInit } from '@angular/core';
import { ShelterService } from '../services/shelter.service';
import { Shelter } from '../models/Shelter';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { CustomErrorStateMatcher } from 'src/app/shared/utils/CustomErrorStateMatcher';

@Component({
  selector: 'app-shelter-form',
  templateUrl: './shelter-form.component.html',
  styleUrls: ['./shelter-form.component.scss']
})
export class ShelterFormComponent implements OnInit {

  constructor(
    private formBuilder: FormBuilder,
    private shelterService: ShelterService,
    private router: Router,
    private route: ActivatedRoute,
  ) {}

  form!: FormGroup;
  matcher = new CustomErrorStateMatcher();

  shelter!: Shelter;
  id!: number;
  isEditing: boolean = false;
  isProcessing: boolean = false;

  ngOnInit(): void {
    this.isProcessing = false;
    this.form = this.formBuilder.group({
      nameControl: ['', Validators.required],
      descriptionControl: ['', Validators.required],
      phoneNumberControl: ['', [Validators.required, Validators.pattern(/^\d{9}$/)]],
      emailControl: ['',  [Validators.required, Validators.email]],
      addressControl: ['', Validators.required],
      cityControl: ['', Validators.required],
    });

    this.route.params.subscribe((params) => (this.id = params['id']));
    if (this.id) {
      this.isEditing = true;
      this.shelterService.getShelterById(this.id).subscribe((shelter) => {
        this.shelter = shelter;
        this.setFormDefaultValues(shelter);
      });
    } else {
      this.isEditing = false;
    }
  }

  setFormDefaultValues(shelter: Shelter) {
    this.form.get('nameControl')?.setValue(shelter.name);
    this.form.get('descriptionControl')?.setValue(shelter.description);
    this.form.get('phoneNumberControl')?.setValue(shelter.phoneNumber);
    this.form.get('emailControl')?.setValue(shelter.email);
    this.form.get('addressControl')?.setValue(shelter.address);
    this.form.get('cityControl')?.setValue(shelter.city);
  }

  addShelter() {
    this.isProcessing = true;

    let shelter = this.getShelterData();

    this.shelterService.addShelter(shelter).subscribe({
      next: () => {
        this.router.navigate(['/shelters']);
      },
      error: (error) => {},
    });
  }

  editShelter() {
    this.isProcessing = true;

    let shelter = this.getShelterData();
    shelter.id = this.shelter.id
    this.shelterService.updateShelter(shelter).subscribe({
      next: () => {
        this.router.navigate(['/shelters']);
      },
      error: (error) => {},
    });
  }

  getShelterData(): Shelter {
    return {
      name: this.form.get('nameControl')?.value,
      description: this.form.get('descriptionControl')?.value,
      phoneNumber: this.form.get('phoneNumberControl')?.value,
      email: this.form.get('emailControl')?.value,
      address: this.form.get('addressControl')?.value,
      city: this.form.get('cityControl')?.value,
    };
  }
}
