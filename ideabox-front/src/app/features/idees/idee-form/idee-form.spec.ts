import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IdeeForm } from './idee-form';

describe('IdeeForm', () => {
  let component: IdeeForm;
  let fixture: ComponentFixture<IdeeForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [IdeeForm],
    }).compileComponents();

    fixture = TestBed.createComponent(IdeeForm);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
