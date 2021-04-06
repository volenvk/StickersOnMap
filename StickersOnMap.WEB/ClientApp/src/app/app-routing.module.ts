import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {PageNotFoundComponent} from "./infrastructure/page-not-found.component";
import {ErrorComponent} from "./infrastructure/error.component";
import {InitGuard} from "./infrastructure/guards/init.guard";
import {SwaggerUiComponent} from "./infrastructure/swagger-ui/swagger-ui-component";

const routes: Routes = [
  {
    path: '',
    loadChildren: () => import('./home/home.module').then(m => m.HomeModule),
    canActivate: [InitGuard]
  },
  {path: 'error', component: ErrorComponent},
  {path: 'page-not-found', component: PageNotFoundComponent},
  {path: 'swagger', component: SwaggerUiComponent},
  {path: '**', redirectTo: '/'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})

export class AppRoutingModule {
}
