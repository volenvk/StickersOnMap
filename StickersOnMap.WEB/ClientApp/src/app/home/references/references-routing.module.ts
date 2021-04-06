import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {StickersComponent} from "./stickers/stickers.component";

const routes: Routes = [
  {path: '', redirectTo: '/', pathMatch: 'full'},
  {path: 'stickers', component: StickersComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReferencesRoutingModule {
}
