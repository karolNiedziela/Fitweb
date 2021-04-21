import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountExercisesComponent } from './account-exercises.component';

describe('AccountExercisesComponent', () => {
  let component: AccountExercisesComponent;
  let fixture: ComponentFixture<AccountExercisesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AccountExercisesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountExercisesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
