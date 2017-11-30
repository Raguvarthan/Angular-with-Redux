import { Component, Input, Output, EventEmitter } from '@angular/core';
import { VehicleViewService } from "../../../service/vehicleview.service";
import { NgRedux } from 'ng2-redux';
import { IAppState } from "../../../store/store";
import * as Const from '../../../store/actions';

@Component({
  selector: 'app-vehicle-view-update',
  templateUrl: './vehicle-view-update.component.html',
  styleUrls: ['./vehicle-view-update.component.css']
})
export class VehicleViewUpdateComponent {
  
  @Input() public vehicleData: any;
  @Output() cancelClicked = new EventEmitter<boolean>();

  constructor(private vehicleViewService: VehicleViewService, private ngRedux: NgRedux<IAppState>, ) {
    
   }

  closeItem(event: any) {
      this.cancelClicked.emit(true);
  }
  updateVehicle(vehicle: any) {
   this.vehicleViewService.updateVechile(vehicle);
   this.ngRedux.subscribe(() => {
    if( this.ngRedux.getState().modal == Const.UPDATED_CLOSE_MODAL) {
      this.cancelClicked.emit(true);
    }
    })
  }
}
