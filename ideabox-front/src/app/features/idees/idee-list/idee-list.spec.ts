import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IdeeList } from './idee-list';

describe('IdeeList', () => {
  let component: IdeeList;
  let fixture: ComponentFixture<IdeeList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [IdeeList],
    }).compileComponents();

    fixture = TestBed.createComponent(IdeeList);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
