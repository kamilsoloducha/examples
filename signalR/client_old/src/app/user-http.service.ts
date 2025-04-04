import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpEvent, HttpHandler, HttpHeaders, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { UserService } from './user.service';

@Injectable()
export class UserHttpService {

    constructor(private readonly httpClient: HttpClient) { }

    authenticate(name: string): Observable<AuthenticateResponse> {
        const body = {
            name
        };
        return this.httpClient.post<AuthenticateResponse>('http://localhost:5000/user/authenticate', body);
    }
}

@Injectable()
export class MessageHttpService{

    constructor(private readonly httpClient: HttpClient) { }

    send(message: string, userName: string, userId: string): Observable<AuthenticateResponse> {
        const body = {
            message, userName, userId
        };
        return this.httpClient.post<any>('http://localhost:5000/message/send', body);
    }
}

export interface AuthenticateResponse {
    id: string;
    userName: string;
    token: string;
}


@Injectable()
export class AuthInterceptor implements HttpInterceptor {

    constructor(private readonly userService: UserService){}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
      if (this.userService.isLogin()){
        req = req.clone({
            setHeaders: {
                'Content-Type' : 'application/json; charset=utf-8',
                Authorization: `Bearer ${this.userService.getToken()}`,
            },
          });
      }
      return next.handle(req);
  }
}
