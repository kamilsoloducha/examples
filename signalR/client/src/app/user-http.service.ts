import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpEvent, HttpHandler, HttpHandlerFn, HttpHeaders, HttpInterceptor, HttpRequest } from '@angular/common/http';
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
export class MessageHttpService {

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

export function loggingInterceptor(req: HttpRequest<unknown>, next: HttpHandlerFn): Observable<HttpEvent<unknown>> {

    const userService = inject(UserService);
    if (userService.isLogin()) {
        req = req.clone({
            setHeaders: {
                'Content-Type': 'application/json; charset=utf-8',
                Authorization: `Bearer ${userService.getToken()}`,
            },
        });
    }
    return next(req);
}