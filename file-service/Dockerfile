FROM node:14

WORKDIR /app/src
COPY package.json ./
RUN npm install
RUN npm build

COPY . .
EXPOSE 3000
CMD ["node", "/app/src/dist/index.js"]
