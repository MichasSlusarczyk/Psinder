import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PetPageElementComponent } from './pet-page-element.component';

describe('PetPageElementComponent', () => {
  let component: PetPageElementComponent;
  let fixture: ComponentFixture<PetPageElementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PetPageElementComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PetPageElementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
