import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VendortripComponent } from './vendortrip.component';

describe('VendortripComponent', () => {
  let component: VendortripComponent;
  let fixture: ComponentFixture<VendortripComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VendortripComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VendortripComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
