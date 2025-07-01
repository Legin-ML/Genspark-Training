FROM alpine

WORKDIR /app

RUN apk add nodejs

COPY . .

EXPOSE 3000

CMD ["node", "server.js"]