import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllAppointmentsPageComponent } from './all-appointments-page.component';

describe('AllAppointmentsPageComponent', () => {
  let component: AllAppointmentsPageComponent;
  let fixture: ComponentFixture<AllAppointmentsPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AllAppointmentsPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AllAppointmentsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
