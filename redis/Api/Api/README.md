- redis

``docker-copmose -f docker-compose.redis.yml up``

- build

``docker build -t api-redis:latest .``

- run service

```
docker service create --name api-redis \
       --publish target=8080,mode=host \
       --replicas 2 \
       --env RedisConfiguration__Host=redis \
api-redis:latest
```

