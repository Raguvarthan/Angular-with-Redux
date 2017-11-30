import { tassign } from 'tassign';
import * as Const from './actions';

export interface IAppState {
 vehicleInfo: Array<any>; 
 vendorInfo: Array<any>;
 officeInfo: Array<any>;
 tripInfo: Array<any>;
 driverInfo: Array<any>
 modal: string;
}

export const INITIAL_STATE: IAppState = {
 vehicleInfo: [],
 vendorInfo: [],
 officeInfo: [],
 tripInfo: [],
 driverInfo: [],
 modal: 'CLOSE'
}

function getVehicles(state, action) {
    return tassign(state, { vehicleInfo: action.vehicleInfo});
}
function getVendors(state, action) {
    return tassign(state, { vendorInfo: action.vendorInfo});
}
function getOffice(state, action) {
    return tassign(state, { officeInfo: action.officeInfo});
}
function getTrips(state, action) {
    return tassign(state, { tripInfo: action.tripInfo});
}
function getDrivers(state, action) {
    return tassign(state, { driverInfo: action.driverInfo});
}

function updateVehicle(state, action) {
   var updatedItem = state.vehicleInfo.find(i => i.id === action.vehicleInfo.id);
    var index = state.vehicleInfo.indexOf(updatedItem);
    var beforeItems = state.vehicleInfo.slice(0, index);
    var afterItems = state.vehicleInfo.slice(index + 1);    
    var newarray = [...beforeItems, action.vehicleInfo, ...afterItems];
    return tassign(state, { vehicleInfo: newarray, modal: Const.UPDATED_CLOSE_MODAL});
}

function updateVendor(state, action) {
    var updatedItem = state.vendorInfo.find(i => i.id === action.vendorInfo.id);
    var index = state.vendorInfo.indexOf(updatedItem);
    var beforeItems = state.vendorInfo.slice(0, index);
    var afterItems = state.vendorInfo.slice(index + 1);    
    var newarray = [...beforeItems, action.vendorInfo, ...afterItems];
    return tassign(state, { vendorInfo: newarray, modal: Const.UPDATED_CLOSE_MODAL});
}

function updateOffice(state, action) {
    var updatedItem = state.officeInfo.find(i => i.id === action.officeInfo.id);
    var index = state.officeInfo.indexOf(updatedItem);
    var beforeItems = state.officeInfo.slice(0, index);
    var afterItems = state.officeInfo.slice(index + 1);    
    var newarray = [...beforeItems, action.officeInfo, ...afterItems];
    return tassign(state, { officeInfo: newarray, modal: Const.UPDATED_CLOSE_MODAL});
}

function updateTrip(state, action) {
    var updatedItem = state.tripInfo.find(i => i.id === action.tripInfo.id);
    var index = state.tripInfo.indexOf(updatedItem);
    var beforeItems = state.tripInfo.slice(0, index);
    var afterItems = state.tripInfo.slice(index + 1);    
    var newarray = [...beforeItems, action.tripInfo, ...afterItems];
    return tassign(state, { tripInfo: newarray, modal: Const.UPDATED_CLOSE_MODAL});
}

function updateDriver(state, action) {
    var updatedItem = state.driverInfo.find(i => i.id === action.driverInfo.id);
    var index = state.driverInfo.indexOf(updatedItem);
    var beforeItems = state.driverInfo.slice(0, index);
    var afterItems = state.driverInfo.slice(index + 1);    
    var newarray = [...beforeItems, action.driverInfo, ...afterItems];
    return tassign(state, { driverInfo: newarray, modal: Const.UPDATED_CLOSE_MODAL});
}

export function appReducer(state: IAppState, action): IAppState {

switch(action.type) {
    case Const.FETCH_ALL_VECHICLES_SUCCESS: return getVehicles(state, action);
    case Const.ADD_VECHICLE_SUCCESS:
    var addedToList = state.vehicleInfo.concat(action.vehicleInfo);
    return tassign(state, { vehicleInfo: addedToList, modal: Const.UPDATED_CLOSE_MODAL});
    case Const.UPDATE_VECHICLE_SUCCESS: return updateVehicle(state, action);
 
    case Const.FETCH_ALL_VENDORS_SUCCESS: return getVendors(state, action);
    case Const.ADD_VENDOR_SUCCESS:
    var addedToList = state.vendorInfo.concat(action.vendorInfo);
    return tassign(state, { vendorInfo: addedToList, modal: Const.UPDATED_CLOSE_MODAL});
    case Const.UPDATE_VENDOR_SUCCESS: return updateVendor(state, action);

    case Const.FETCH_ALL_OFFICE_SUCCESS: return getOffice(state, action);
    case Const.ADD_OFFICE_SUCCESS:
    var addedToList = state.officeInfo.concat(action.officeInfo);
    return tassign(state, { officeInfo: addedToList, modal: Const.UPDATED_CLOSE_MODAL});
    case Const.UPDATE_OFFICE_SUCCESS: return updateOffice(state, action);

    case Const.FETCH_ALL_TRIP_SUCCESS: return getTrips(state, action);
    case Const.UPDATE_TRIP_SUCCESS: return updateTrip(state, action);
    case Const.ADD_TRIP_SUCCESS:
    var addedToList = state.tripInfo.concat(action.tripInfo);
    return tassign(state, { tripInfo: addedToList, modal: Const.UPDATED_CLOSE_MODAL});

    case Const.FETCH_ALL_DRIVER_SUCCESS: return getDrivers(state, action);
    case Const.UPDATE_DRIVER_SUCCESS: return updateDriver(state, action);
    case Const.ADD_DRIVER_SUCCESS:
    var addedToList = state.driverInfo.concat(action.driverInfo);
    return tassign(state, { driverInfo: addedToList, modal: Const.UPDATED_CLOSE_MODAL});

}

return state;

}
