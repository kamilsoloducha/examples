import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HttpOperatorsComponent } from './http-operators.component';

const routes: Routes = [
  {
    component: HttpOperatorsComponent,
    path: 'http-operators',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class HttpOperatorsRoutingModule {}
