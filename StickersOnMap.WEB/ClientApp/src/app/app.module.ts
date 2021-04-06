import {BrowserModule} from '@angular/platform-browser';
import {LOCALE_ID, NgModule} from '@angular/core';
import {HttpClientModule} from '@angular/common/http';
import {AppComponent} from './app.component';
import {HomeModule} from "./home/home.module";
import {RouterModule} from "@angular/router";
import {AppRoutingModule} from "./app-routing.module";
import {ErrorComponent} from "./infrastructure/error.component";
import {PageNotFoundComponent} from "./infrastructure/page-not-found.component";
import {MessagePageComponent} from "./infrastructure/message-page/message-page.component";
import {PrimeNgModule} from "./prime-ng.module";
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import {registerLocaleData} from '@angular/common';
import localeRu from '@angular/common/locales/ru';
import {SwaggerUiComponent} from "./infrastructure/swagger-ui/swagger-ui-component";

registerLocaleData(localeRu);

@NgModule({
  declarations: [
    AppComponent,
    MessagePageComponent,
    PageNotFoundComponent,
    ErrorComponent,
    SwaggerUiComponent
  ],
  imports: [
    BrowserModule,
    RouterModule,
    HttpClientModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HomeModule,
    PrimeNgModule
  ],
  exports: [
    RouterModule,
    MessagePageComponent,
  ],
  providers: [
    {provide: LOCALE_ID, useValue: 'ru'},
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
