import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';

export class SingalRService {

    connection!: HubConnection;

    constructor() {
    }

    start(): void {
        this.connection = new HubConnectionBuilder()
            .configureLogging(LogLevel.Information)
            .withUrl('http://localhost:5000/notify')
            .withAutomaticReconnect([1000, 5000, 10000, 60000])
            .build();

        this.connection.start().then(() => {
            console.log('Start listening...');
        }).catch(error => {
            console.log('Error has appeard', error);
        });

        this.connection.on('BroadcastMessage', (message) => {
            console.log('Message received:', message);
        });
    }
}
