import { Component, OnInit } from '@angular/core';
import { ShelterService } from '../services/shelter.service';
import { Shelter } from '../models/Shelter';
import { Router } from '@angular/router';

@Component({
  selector: 'app-shelter-page',
  templateUrl: './shelter-page.component.html',
  styleUrls: ['./shelter-page.component.scss']
})
export class ShelterPageComponent implements OnInit {

  constructor(private shelterService: ShelterService, private router: Router) {}

  shelters!: Shelter[];
  columnsToDisplay = ['name', 'city', 'address', 'action'];
  columnsToDisplayWithExpand = [...this.columnsToDisplay, 'expand'];
  shelter!: Shelter | null;

  ngOnInit(): void {
    this.shelterService.getShelters().subscribe((shelters) => {
      this.shelters = shelters.shelters;
    });
  }

  editShelter(id: number) {
    this.router.navigate([`/shelter-form/${id}`]);
  }
}
