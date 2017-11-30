import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExpensesInsertUpdateComponent } from './expenses-insert-update.component';

describe('ExpensesInsertUpdateComponent', () => {
  let component: ExpensesInsertUpdateComponent;
  let fixture: ComponentFixture<ExpensesInsertUpdateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExpensesInsertUpdateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExpensesInsertUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
