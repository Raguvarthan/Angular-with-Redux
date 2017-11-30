import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TripInsertUpdateComponent } from './trip-insert-update.component';

describe('TripInsertUpdateComponent', () => {
  let component: TripInsertUpdateComponent;
  let fixture: ComponentFixture<TripInsertUpdateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TripInsertUpdateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TripInsertUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
