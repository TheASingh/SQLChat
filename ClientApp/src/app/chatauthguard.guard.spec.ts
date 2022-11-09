import { TestBed } from '@angular/core/testing';

import { ChatauthguardGuard } from './chatauthguard.guard';

describe('ChatauthguardGuard', () => {
  let guard: ChatauthguardGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(ChatauthguardGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
