import { TestBed } from '@angular/core/testing';

import { ChatapplicationServiceService } from './chatapplicationservice.service';

describe('ChatapplicationserviceService', () => {
  let service: ChatapplicationServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ChatapplicationServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
