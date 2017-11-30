import { BrowserModule } from '@angular/platform-browser';
import { NgModule, isDevMode } from '@angular/core';

import { RouterModule, Routes } from '@angular/router';
import { AppRoutingModule } from './app.route.model';
import { AngularFontAwesomeModule } from 'angular-font-awesome';
import {
  ModalModule,
  AccordionModule,
  AccordionConfig,
  BsDatepickerModule, 
  TypeaheadModule
} from 'ngx-bootstrap';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';

import { LoginComponent } from './login/login.component';

import { PagesComponentBarrel } from './pages';
import { SharedComponentBarrel } from './shared';
import { ServicesBarrel } from './service';

import { IAppState, appReducer, INITIAL_STATE } from './store/store';

import { HttpClient } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';

import { NgRedux, NgReduxModule, DevToolsExtension } from 'ng2-redux';
import { ExpensesInsertUpdateComponent } from './pages/expenses/expenses-insert-update/expenses-insert-update.component';


@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    PagesComponentBarrel,
    SharedComponentBarrel,
    ExpensesInsertUpdateComponent    
  ],
  imports: [
    BrowserModule,
    RouterModule,
    AppRoutingModule,
    AngularFontAwesomeModule,
    HttpClientModule,
    FormsModule,
    ModalModule.forRoot(),
    BsDatepickerModule.forRoot(),
    TypeaheadModule,
    AccordionModule,    
    NgReduxModule
  ],
  providers: [
    HttpClient,
    AccordionConfig,
    ServicesBarrel
],
  bootstrap: [AppComponent]
})
export class AppModule { 
  
  constructor(ngRedux: NgRedux<IAppState>, private devTools: DevToolsExtension){
  var enhancers = isDevMode() ? [devTools.enhancer()] : []; 
  ngRedux.configureStore(appReducer, INITIAL_STATE, [], enhancers);  
}

}
