import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VehiclePendingPaymentComponent } from './vehicle-pending-payment.component';

describe('VehiclePendingPaymentComponent', () => {
  let component: VehiclePendingPaymentComponent;
  let fixture: ComponentFixture<VehiclePendingPaymentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VehiclePendingPaymentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VehiclePendingPaymentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
