import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {HomeComponent} from './home.component';
import {MapLeafletComponent} from "./references/map-leaflet/map-leaflet.component";

const homeRoutes: Routes = [
  {
    path: '', component: HomeComponent, children: [
      {path: '', redirectTo: 'map', pathMatch: 'full'},
      {path: 'map', component: MapLeafletComponent},
      {path: 'references', loadChildren: () => import('./references/references.module').then(m => m.ReferencesModule)},
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(homeRoutes)],
  exports: [RouterModule]
})
export class HomeRoutingModule {}
