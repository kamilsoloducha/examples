import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { NgrxErrorHandlingModule } from './modules/ngrx-error-handling/ngrx-error-handling.module';
import { HttpOperatorsModule } from './modules/http-operators/http-operators.module';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgrxErrorHandlingModule,
    HttpOperatorsModule,
    EffectsModule.forRoot([]),
    StoreModule.forRoot({}, {}),
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
