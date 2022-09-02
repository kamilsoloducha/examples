import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  private url = 'http://localhost:5000/';

  constructor(private readonly http: HttpClient) { }

  send(): void {
    this.http.get(this.url + "get/test").subscribe(
      {
        complete: () => console.log('get - correct'),
        error: (error) => console.error('get - error', error)
      }
    )
  }

  delete(): void {
    this.http.delete(this.url + "delete/test",{
      // headers:{
      //   "TestHeader": "TestHeaderValue"
      // }
    }).subscribe(
      {
        complete: () => console.log('delete - correct'),
        error: (error) => console.error('delete - error', error)
      }
    )
  }

  post(): void {
    this.http.post(this.url + "post", {value:"test"}, {
      headers: {
        "content-type": "application/json",
        "TestHeader": "TestHeaderValue"
      }
    }).subscribe(
      {
        complete: () => console.log('post - correct'),
        error: (error) => console.error('post - error', error)
      }
    )
  }
}
