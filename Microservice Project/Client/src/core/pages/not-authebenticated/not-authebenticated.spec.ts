import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NotAuthebenticated } from './not-authebenticated';

describe('NotAuthebenticated', () => {
  let component: NotAuthebenticated;
  let fixture: ComponentFixture<NotAuthebenticated>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NotAuthebenticated]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NotAuthebenticated);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
