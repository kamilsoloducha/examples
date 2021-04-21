import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SingalRService } from './signalR.service';
import { ReactiveFormsModule } from '@angular/forms';
import { UserService } from './user.service';
import { AuthInterceptor, MessageHttpService, UserHttpService } from './user-http.service';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  providers: [SingalRService, UserService, UserHttpService, MessageHttpService,
    {
      provide : HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi   : true,
    }, ],
  bootstrap: [AppComponent]
})
export class AppModule { }
