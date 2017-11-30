import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OfficeInfoComponent } from './office.component';

describe('officeinfoComponent', () => {
  let component: OfficeInfoComponent;
  let fixture: ComponentFixture<OfficeInfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OfficeInfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OfficeInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
