import { Component } from '@angular/core';
import { SingalRService } from './signalR-service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  constructor(private readonly signalRService: SingalRService) { }

  start(): void {
    this.signalRService.start();
  }
}
