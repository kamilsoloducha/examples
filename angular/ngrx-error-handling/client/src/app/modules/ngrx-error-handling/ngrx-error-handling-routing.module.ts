import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NgrxErrorHandlingPageComponent } from './pages/ngrx-error-handling-page/ngrx-error-handling-page.component';

const routes: Routes = [
  {
    component: NgrxErrorHandlingPageComponent,
    path: 'error-handling',
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class NgrxErrorHandlingRoutingModule {}
