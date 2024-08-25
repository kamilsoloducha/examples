import { Routes } from '@angular/router';
import { SwitchMapComponent } from './pages/switch-map/switch-map.component';
import { MergeMapComponent } from './pages/merge-map/merge-map.component';

export const routes: Routes = [
  { path: 'switch-map', component: SwitchMapComponent },
  { path: 'merge-map', component: MergeMapComponent },
];
