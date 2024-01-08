import { TestBed } from '@angular/core/testing';

import { PetFilterService } from './pet-filter.service';

describe('PetFilterService', () => {
  let service: PetFilterService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PetFilterService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
