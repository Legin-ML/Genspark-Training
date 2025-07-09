FROM node:22-alpine AS builder

WORKDIR /app

COPY package*.json ./

RUN npm install

COPY . .

RUN npm run prodbuild


FROM node:22-alpine

WORKDIR /app

COPY --from=builder /app/dist/true-feedback-fe ./dist/true-feedback-fe

COPY server.js ./

RUN npm install express

EXPOSE 3000

CMD ["node", "server.js"]
