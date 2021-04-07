import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {StickersComponent} from "./stickers/stickers.component";
import {SwaggerUiComponent} from "../../infrastructure/swagger-ui/swagger-ui-component";

const routes: Routes = [
  {path: '', redirectTo: '/', pathMatch: 'full'},
  {path: 'stickers', component: StickersComponent},
  {path: 'API', component: SwaggerUiComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReferencesRoutingModule {
}
