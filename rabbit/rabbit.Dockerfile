FROM rabbitmq:management-alpine
USER 0

ADD ./definitions.json /etc/rabbitmq/

RUN chown rabbitmq:rabbitmq /etc/rabbitmq/definitions.json
RUN chown -R rabbitmq:rabbitmq /var/lib/rabbitmq
RUN chmod -R 777 /var/lib/rabbitmq

RUN wget -P $RABBITMQ_HOME/plugins https://github.com/rabbitmq/rabbitmq-delayed-message-exchange/releases/download/v4.1.0/rabbitmq_delayed_message_exchange-4.1.0.ez
RUN chown rabbitmq:rabbitmq $RABBITMQ_HOME/plugins/rabbitmq_delayed_message_exchange-4.1.0.ez

RUN rabbitmq-plugins enable --offline rabbitmq_delayed_message_exchange

USER rabbitmq
CMD ["rabbitmq-server"]