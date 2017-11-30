import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VehiclepaymentComponent } from './vehiclepayment.component';

describe('VehiclepaymentComponent', () => {
  let component: VehiclepaymentComponent;
  let fixture: ComponentFixture<VehiclepaymentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VehiclepaymentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VehiclepaymentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
