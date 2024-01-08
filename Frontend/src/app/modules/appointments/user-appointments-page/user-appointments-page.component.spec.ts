import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserAppointmentsPageComponent } from './user-appointments-page.component';

describe('UserAppointmentsPageComponent', () => {
  let component: UserAppointmentsPageComponent;
  let fixture: ComponentFixture<UserAppointmentsPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserAppointmentsPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserAppointmentsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
