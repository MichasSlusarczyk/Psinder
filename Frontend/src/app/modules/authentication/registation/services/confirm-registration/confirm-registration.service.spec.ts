import { TestBed } from '@angular/core/testing';

import { ConfirmRegistrationService } from './confirm-registration.service';

describe('ConfirmRegistrationService', () => {
  let service: ConfirmRegistrationService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ConfirmRegistrationService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
