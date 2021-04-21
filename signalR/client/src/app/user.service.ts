import { Injectable } from '@angular/core';

@Injectable()
export class UserService {

    static user: User = null as any;

    constructor() { }

    setUser(token: string, name: string, id: string): void {
        UserService.user = new User(id, name, token);
    }

    getUser(): User {
        return UserService.user;
    }

    getToken(): string {
        return UserService.user.token;
    }

    isLogin(): boolean {
        return UserService.user != null;
    }
}

export class User {
    constructor(public id: string, public name: string, public token: string) { }
}
