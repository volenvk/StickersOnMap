import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {HomeComponent} from './home.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {HomeRoutingModule} from "./home-routing.module";
import {PrimeNgModule} from "../prime-ng.module";
import {ConfirmationService, MessageService} from "primeng/api";
import {InputTimeComponent} from "../infrastructure/controls/input-time/input-time.component";
import {InputDateComponent} from "../infrastructure/controls/input-date/input-date.component";
import {ActiveFilterButtonComponent} from "../infrastructure/active-filter-button/active-filter-button.component";
import {AddFloatingButtonComponent} from "../infrastructure/add-floating-button/add-floating-button.component";


@NgModule({
  declarations: [
    HomeComponent,
    ActiveFilterButtonComponent,
    InputDateComponent,
    InputTimeComponent,
    AddFloatingButtonComponent
  ],
  exports: [
    ActiveFilterButtonComponent,
    InputDateComponent,
    InputTimeComponent,
    AddFloatingButtonComponent
  ],
  imports: [
    CommonModule,
    HomeRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    PrimeNgModule,
  ],
  providers: [
    MessageService,
    ConfirmationService
  ]
})

export class HomeModule {
}
