import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-request-cancellation',
  templateUrl: './request-cancellation.component.html',
  styleUrls: ['./request-cancellation.component.css']
})
export class RequestCancellationComponent implements OnInit {

  private requestSubscription: Subscription;
  private readonly host = 'http://localhost:5000/date/ct';

  loadedDate: string;

  constructor(private readonly httpClient: HttpClient) { }

  ngOnInit(): void {
  }

  sendRequest(): void{
    console.log('Request sent');
    this.requestSubscription = this.httpClient.get<string>(this.host).subscribe(x => {
      console.log('Request done', x);
      this.loadedDate = x;
    });
  }

  cancelRequest(): void{
    if (this.requestSubscription != null){
      console.log('Request cancelled');
      this.requestSubscription.unsubscribe();
    }
  }

}
