SingalR Example

AspNet Core 5.0, angular, SignalR, Jwt Authorization

## Start server: dotnet run --project ./server

## Start client: npm --prefix ./client start

Absolutely basic functionality
1. Server UserController
 - POST /user/authenticate to get jwt token need to make websoket connection
 - GET /user to check all login users, need authorization, not used in example

2. Server MessageController
 - POST /message/send endpoint used to send message to other login users, need authorization

3. Server MessageHub
 - I get information about login user from the token. There is a static list - connections - which keep information about all connected users. In OnConnectedAsync method, I'm adding a new user, and in OnDisconnectedAsync, I'm removing user, when they are disconnecting.

4. Server Startup
 - OnMessageReceived - config is needed to pass token to hub
 - all other config stuff is a minimum to proper work signalR with authorization

 5. Client generally
 - did not want to create a clean code, just check whether my solution works correctly
 - after click Login button, client sends user/authenticate request, in response I receive token and save it in UserService
 - after click Send button, client sends message/send request. Autherization token is added in AuthInterceptor
