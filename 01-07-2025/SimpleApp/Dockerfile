FROM debian:bullseye

WORKDIR /app

RUN apt-get update &&\
    apt-get install curl -y &&\
    curl -fsSL https://deb.nodesource.com/setup_23.x -o nodesource_setup.sh &&\
    bash nodesource_setup.sh &&\
    apt-get install nodejs -y

COPY . .

EXPOSE 3000

CMD ["node", "server.js"]