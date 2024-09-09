import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpOperatorsComponent } from './http-operators.component';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { HttpOperatorsEffects, HttpOperatorsFetaure } from './store';
import { provideHttpClient } from '@angular/common/http';
import { HttpOperatorsRoutingModule } from './http-operators-routing.module';

@NgModule({
  declarations: [HttpOperatorsComponent],
  imports: [
    HttpOperatorsRoutingModule,
    CommonModule,
    StoreModule.forFeature(HttpOperatorsFetaure),
    EffectsModule.forFeature([HttpOperatorsEffects]),
  ],
  providers: [provideHttpClient()],
})
export class HttpOperatorsModule {}
