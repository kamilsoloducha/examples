import { Component } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { RouterOutlet } from '@angular/router';
import { SingalRService } from './signalR.service';
import { MessageHttpService, UserHttpService } from './user-http.service';
import { UserService } from './user.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, ReactiveFormsModule, CommonModule],
  providers: [UserService, UserHttpService, SingalRService, MessageHttpService],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  title = 'client';
  loginControl: FormControl;
  messageControl: FormControl;
  isLogin = false;
  connectionId = '';

  messages: Message[];

  constructor(
    private userService: UserService,
    private readonly userHttpService: UserHttpService,
    private readonly singalR: SingalRService,
    private readonly messageService: MessageHttpService
  ) {
    this.loginControl = new FormControl('');
    this.messageControl = new FormControl('');
    this.messages = [];
  }

  login(): void {
    this.userHttpService
      .authenticate(this.loginControl.value)
      .subscribe((x: any) => {
        this.userService.setUser(x.token, x.userName, x.id);
        this.isLogin = true;
        this.singalR.start();
        this.singalR.connection.on(
          'SendMessage',
          (message: MessageReceived) => {
            const newMessage = new Message(
              message.content,
              message.userName,
              ''
            );
            this.messages.push(newMessage);
          }
        );
      });
  }

  sendMessage(): void {
    const message = this.messageControl.value as string;
    if (message == null || message === '') {
      return;
    }
    const newMessage = new Message(
      message,
      this.userService.getUser().name,
      ''
    );
    this.messages.push(newMessage);
    this.messageService
      .send(
        message,
        this.userService.getUser().name,
        this.userService.getUser().id
      )
      .subscribe();
  }
}

export class Message {
  constructor(
    public readonly content: string,
    public readonly userName: string,
    public readonly userId: string
  ) {}
}

export interface MessageReceived {
  content: string;
  userName: string;
}
