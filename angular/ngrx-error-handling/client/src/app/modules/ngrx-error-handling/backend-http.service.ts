import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class BackendHttpService {
  private readonly httpClient = inject(HttpClient);

  throwException(value: number): Observable<number> {
    return this.httpClient.get<number>(
      `http://localhost:5000/exception/${value}`
    );
  }
}
