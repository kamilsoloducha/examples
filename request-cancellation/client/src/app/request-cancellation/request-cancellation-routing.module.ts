import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RequestCancellationComponent } from './request-cancellation/request-cancellation.component';

const routes: Routes = [
  {
    path: '',
    component: RequestCancellationComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RequestCancellationRoutingModule { }
