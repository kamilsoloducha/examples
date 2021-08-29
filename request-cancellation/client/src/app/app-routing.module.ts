import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  {
    path: 'request/cancellation',
    loadChildren:
      () => import('./request-cancellation/request-cancellation.module')
      .then(m => m.RequestCancellationModule),
  },
  {
    path: 'request/cancellation/store',
    loadChildren:
      () => import('./request-cancellation-store/request-cancellation-store.module')
      .then(m => m.RequestCancellationStoreModule),
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { scrollPositionRestoration: 'enabled' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
