import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ReferencesRoutingModule} from './references-routing.module';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {MapLeafletComponent} from "./map-leaflet/map-leaflet.component";
import {PrimeNgModule} from "../../prime-ng.module";
import {HomeModule} from "../home.module";
import {LeafletModule} from "@asymmetrik/ngx-leaflet";
import {StickersComponent} from "./stickers/stickers.component";

@NgModule({
  declarations: [
    MapLeafletComponent,
    StickersComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    ReferencesRoutingModule,
    LeafletModule,
    PrimeNgModule,
    HomeModule
  ]
})
export class ReferencesModule {
}
