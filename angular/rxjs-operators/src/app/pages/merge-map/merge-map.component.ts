import {
  AfterViewInit,
  Component,
  ElementRef,
  signal,
  ViewChild,
} from '@angular/core';
import { fromEvent, map, mergeMap } from 'rxjs';

@Component({
  standalone: true,
  imports: [],
  templateUrl: './merge-map.component.html',
})
export class MergeMapComponent implements AfterViewInit {
  @ViewChild('brokenInput1') brokenInput1?: ElementRef;
  @ViewChild('brokenInput2') brokenInput2?: ElementRef;
  brokenResult = signal('');

  @ViewChild('workingInput1') workingInput1?: ElementRef;
  @ViewChild('workingInput2') workingInput2?: ElementRef;
  workingResult = signal('');

  ngAfterViewInit(): void {
    // broken - it's updating result after each input changed, but it's ignoring other value
    fromEvent(this.brokenInput1?.nativeElement, 'input').subscribe(
      (value: any) => this.brokenResult.set(value.target.value as any)
    );
    fromEvent(this.brokenInput2?.nativeElement, 'input').subscribe(
      (value: any) => this.brokenResult.set(value.target.value as any)
    );

    // working - it's taking into account both values0
    const ob1$ = fromEvent<InputEvent>(
      this.workingInput1?.nativeElement,
      'input'
    );
    const ob2$ = fromEvent<InputEvent>(
      this.workingInput2?.nativeElement,
      'input'
    );

    ob1$
      .pipe(
        mergeMap((event1: any) => {
          return ob2$.pipe(
            map((event2: any) => {
              return event1.target.value + ' ' + event2.target.value;
            })
          );
        })
      )
      .subscribe((mergedValue) => this.workingResult.set(mergedValue));
  }
}
