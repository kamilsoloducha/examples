import { Component } from '@angular/core';
import { finalize, Observable, scan, Subject, takeUntil } from 'rxjs';
import { JsonStreamDecoder, WeatherForecast } from './deckoder';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent{

  private stopSource = new Subject<boolean>();

  items$: Observable<WeatherForecast[]>;
  loading = true;
  
  constructor() { 
    this.items$ = this.fromFetchStream<WeatherForecast>('http://localhost:5000/weatherforecast', this.stopSource).pipe(
      takeUntil(this.stopSource),
      scan((all, item) => [...all, item,], [] as WeatherForecast[]),
      finalize(() => this.loading = false)
    );
  }

  public stop():void{
    this.stopSource.next(false);
  }

  fromFetchStream<T>(
    input: RequestInfo,
    subject: Subject<boolean>,
    init?: RequestInit,
  ):Observable<T>{
    return  new Observable<T>(observer =>{
      const controller = new AbortController();
      let shouldContinue = true;
      const sub = subject.subscribe(value => shouldContinue = value);
      fetch(input, {...init, signal: controller.signal})
      .then(async response =>{
        const reader = response.body?.getReader();
        while(shouldContinue){
          const stream = await reader?.read();

          const decoder = new JsonStreamDecoder();

          if(stream?.done) break;
          if(!stream?.value) continue;
          
          decoder.decodeChunk<T>(stream?.value, item => {
            console.log(item);
            observer.next(item);
          });
        }
        reader?.releaseLock();
        observer.complete();
      })
      .catch(err => observer.error(err))
      .finally(() => {observer.complete();
        sub.unsubscribe()});
    });
  }
}
