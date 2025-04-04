import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';

const ports = [5001, 5002];
export const port = ports[(Math.random() * 10) % 2];
console.log(port);

bootstrapApplication(AppComponent, appConfig).catch((err) =>
  console.error(err)
);
