import { Component, OnInit, computed, signal, effect } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent implements OnInit {
  showCount = signal(false);
  count = signal(0);
  conditionalCount = computed(() => {
    if (this.showCount()) {
      return `The count is ${this.count()}.`;
    } else {
      return 'Nothing to see here!';
    }
  });

  constructor() {
    effect(() => {
      console.log(`The count is: ${this.count()}`);
    });
  }

  ngOnInit(): void {}

  click(): void {
    this.count.update((previous) => previous + 1);
    console.log(this.conditionalCount());
  }
}
