import { Component, OnInit } from '@angular/core';
import { fromEvent, interval, Observable, Subscription, switchMap } from 'rxjs';

@Component({
  selector: 'app-switch-map',
  standalone: true,
  imports: [],
  templateUrl: './switch-map.component.html',
})
export class SwitchMapComponent implements OnInit {
  brokenButton: Element | null = null;
  brokenClick$?: Observable<any>;
  brokenInterval$ = interval(1000);

  workingButton: Element | null = null;
  workingClick$?: Observable<any>;
  workingInterval$ = interval(1000);

  ngOnInit(): void {
    this.brokenButton = document.querySelector('#broken');
    this.brokenClick$ = fromEvent(this.brokenButton as any, 'click');
    this.brokenInterval$ = interval(1000);

    // after each click there is created a new observable
    this.brokenClick$.subscribe((_) => {
      this.brokenInterval$.subscribe((value) => console.log(value));
    });

    this.workingButton = document.querySelector('#correct');
    this.workingClick$ = fromEvent(this.workingButton as any, 'click');
    this.workingInterval$ = interval(1000);

    // swtichMap cancel previous observable and create a new
    this.workingClick$
      .pipe(switchMap((_) => this.workingInterval$))
      .subscribe((value) => console.log(`Working: ${value}`));
  }
}
