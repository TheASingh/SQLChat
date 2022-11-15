import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ForgotpassworduserentryComponent } from './forgotpassworduserentry.component';

describe('ForgotpassworduserentryComponent', () => {
  let component: ForgotpassworduserentryComponent;
  let fixture: ComponentFixture<ForgotpassworduserentryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ForgotpassworduserentryComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ForgotpassworduserentryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
