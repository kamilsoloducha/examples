import { Injectable } from '@angular/core';
import { HttpTransportType, HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { UserService } from './user.service';

@Injectable()
export class SingalRService {

    connections: string[];
    connection!: HubConnection;

    constructor(private readonly userService: UserService) {
        this.connections = [];
    }

    start(): void {
        this.connection = new HubConnectionBuilder()
            .configureLogging(LogLevel.Information)
            .withUrl('http://localhost:5000/notify', {
                skipNegotiation: false,
                transport: HttpTransportType.WebSockets,
                accessTokenFactory: () => this.userService.getToken(),
            })
            .withAutomaticReconnect([1000, 5000, 10000, 60000])
            .build();

        this.connection.start()
            .then((data: any) => {
                console.log('Start listening...', data);
            })
            .catch(error => {
                console.log('Error has appeard', error);
            });

        this.connection.on('NewConnection', (connectionId) => {
            console.log('new connection: ', connectionId);
            this.connections.push(connectionId);
        });

        this.connection.on('ConnectionLost', (connectionId) => {
            console.log('connection lost: ', connectionId);
            const index = this.connections.findIndex(connectionId);
            this.connections = this.connections.splice(index, 1);
        });
    }

    stop(): void {
        this.connection.stop()
            .then(() => {
                console.log('Stop listening...');
            })
            .catch(error => {
                console.log('Error has appeard', error);
            });
    }

    addListener(listener: Listener<any>): void {
        this.connection.on(listener.name, (arg) => listener.callback(arg));
    }

    addListeners(listeners: Listener<any>[]): void {
        listeners.forEach(value => {
            this.connection.on(value.name, (arg) => value.callback(arg));
        });
    }

    removeListener(listenerName: string): void {
        this.connection.off(listenerName);
    }

    removeListeners(listenerNames: string[]): void {
        listenerNames.forEach(value => {
            this.connection.off(value);
        });
    }
}

export class Listener<T> {
    constructor(public readonly name: string,
                public readonly callback: (arg: T) => void) { }
}
