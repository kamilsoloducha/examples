import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {HttpClientModule} from '@angular/common/http';
import { RequestCancellationComponent } from './request-cancellation/request-cancellation.component';
import { RequestCancellationRoutingModule } from './request-cancellation-routing.module';

@NgModule({
  declarations: [
    RequestCancellationComponent
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    RequestCancellationRoutingModule
  ]
})
export class RequestCancellationModule { }
