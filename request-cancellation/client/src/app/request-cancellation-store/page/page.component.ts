import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { CancelGetDate, GetDate } from '../store/actions';
import { RequestCancellationState } from '../store/state';

@Component({
  selector: 'app-page',
  templateUrl: './page.component.html',
  styleUrls: ['./page.component.css']
})
export class PageComponent implements OnInit {

  constructor(private readonly store: Store<RequestCancellationState>) { }

  ngOnInit(): void {
  }

  sendRequest(): void{
    this.store.dispatch(new GetDate());
  }

  cancelRequest(): void{
    this.store.dispatch(new CancelGetDate());
  }

}
