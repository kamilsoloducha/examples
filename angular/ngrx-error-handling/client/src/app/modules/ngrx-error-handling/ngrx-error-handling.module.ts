import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { provideHttpClient } from '@angular/common/http';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { NgrxErrorHandlingRoutingModule } from './ngrx-error-handling-routing.module';
import { errorHandlingFeature, LocalStoreEffects } from './store';

@NgModule({
  declarations: [],
  imports: [
    NgrxErrorHandlingRoutingModule,
    CommonModule,
    StoreModule.forFeature(errorHandlingFeature),
    EffectsModule.forFeature([LocalStoreEffects]),
  ],
  exports: [NgrxErrorHandlingRoutingModule],
  providers: [provideHttpClient()],
})
export class NgrxErrorHandlingModule {}
