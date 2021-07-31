FROM rabbitmq:management-alpine

ADD ./definitions.json /etc/rabbitmq/

RUN chown rabbitmq:rabbitmq /etc/rabbitmq/definitions.json
RUN chown -R rabbitmq:rabbitmq /var/lib/rabbitmq
RUN chmod -R 777 /var/lib/rabbitmq

USER rabbitmq

CMD ["rabbitmq-server"]