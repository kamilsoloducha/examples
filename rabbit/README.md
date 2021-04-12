Communication between 3 services by rabbitMq queue. Service1 contains 3 endpoints to send 3 type of events: 
 - Service2Event is a single event sending to Service2 only,
 - Service2And3 is a event sending to Service2 and Service3,
 - Service3th2 is a event sending to Service3 through Service2 (Service2 changes Service3th2 into Service3from2 in Service3th2EventConsumer)

In testing purpose use swagger to send a request to Service1.

MassTransit Seq RabbitMq DotNetCore AspNetCore Dockerfile docker compose Swagger

run in docker containers:
docker-compose up --build

to run rabbit only in docker container:
docker-compose -f docker-compose-rabbit.yml

to run seq only in docker container:
docker-compose -f docker-compose-seq.yml

rabbit admin panel:
http://localhost:15672
username: guest
password: guest

seq dashboard:
http://localhost:5341

service1 swagger panel:
http://localhost:5000/swagger/index.html
