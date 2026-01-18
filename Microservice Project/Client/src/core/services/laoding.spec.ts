import { TestBed } from '@angular/core/testing';

import { Laoding } from './laoding';

describe('Laoding', () => {
  let service: Laoding;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Laoding);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
