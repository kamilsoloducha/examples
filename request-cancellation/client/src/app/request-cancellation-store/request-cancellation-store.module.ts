import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageComponent } from './page/page.component';
import { RequestCancellationStoreRoutingModule } from './request-cancellation-store-routing.module';
import { StoreModule } from '@ngrx/store';
import { reducer } from './store/reducer';
import { EffectsModule } from '@ngrx/effects';
import { RequestCancellationEffects } from './store/effects';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [
    PageComponent
  ],
  imports: [
    CommonModule,
    RequestCancellationStoreRoutingModule,
    HttpClientModule,
    StoreModule.forFeature('requestCancellationStore', reducer),
    EffectsModule.forFeature([RequestCancellationEffects])
  ]
})
export class RequestCancellationStoreModule { }
