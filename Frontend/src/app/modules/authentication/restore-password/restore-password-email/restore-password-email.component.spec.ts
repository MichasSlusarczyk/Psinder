import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RestorePasswordEmailComponent } from './restore-password-email.component';

describe('RestorePasswordEmailComponent', () => {
  let component: RestorePasswordEmailComponent;
  let fixture: ComponentFixture<RestorePasswordEmailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RestorePasswordEmailComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RestorePasswordEmailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
