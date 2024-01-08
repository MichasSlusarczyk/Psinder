import { TestBed } from '@angular/core/testing';

import { ChangeLanguageService } from './change-language.service';

describe('ChangeLanguageService', () => {
  let service: ChangeLanguageService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ChangeLanguageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
