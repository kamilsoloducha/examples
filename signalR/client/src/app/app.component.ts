import { Component } from '@angular/core';
import { FormBuilder, FormControl } from '@angular/forms';
import { Listener, SingalRService } from './signalR.service';
import { MessageHttpService, UserHttpService } from './user-http.service';
import { UserService } from './user.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {

  loginControl: FormControl;
  messageControl: FormControl;
  isLogin = false;
  connectionId = '';

  messages: Message[];

  constructor(private readonly fb: FormBuilder,
              private readonly userService: UserService,
              private readonly userHttpService: UserHttpService,
              private readonly singalR: SingalRService,
              private readonly messageService: MessageHttpService) {
    this.loginControl = this.fb.control('');
    this.messageControl = this.fb.control('');
    this.messages = [];
  }

  login(): void {
    this.userHttpService.authenticate(this.loginControl.value).subscribe(x => {
      this.userService.setUser(x.token, x.userName, x.id);
      this.isLogin = true;
      this.singalR.start();
      this.singalR.connection.on('SendMessage', (message: MessageReceived) => {
        const newMessage = new Message(message.content, message.userName, '');
        this.messages.push(newMessage);
      });
    });
  }

  sendMessage(): void {
    const message = this.messageControl.value as string;
    if (message == null || message === ''){
      return;
    }
    const newMessage = new Message(message, this.userService.getUser().name, '');
    this.messages.push(newMessage);
    this.messageService.send(message, this.userService.getUser().name, this.userService.getUser().id).subscribe();
  }
}

export class Message{
  constructor(public readonly content: string, public readonly userName: string, public readonly userId: string){}
}

export interface MessageReceived{
  content: string;
  userName: string;
}
