import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IdeeDetail } from './idee-detail';

describe('IdeeDetail', () => {
  let component: IdeeDetail;
  let fixture: ComponentFixture<IdeeDetail>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [IdeeDetail],
    }).compileComponents();

    fixture = TestBed.createComponent(IdeeDetail);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
