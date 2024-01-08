import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShelterPetsPageComponent } from './shelter-pets-page.component';

describe('ShelterPetsPageComponent', () => {
  let component: ShelterPetsPageComponent;
  let fixture: ComponentFixture<ShelterPetsPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShelterPetsPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ShelterPetsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
