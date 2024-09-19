FROM node:alpine As build

WORKDIR /app

COPY package.json package-lock.json ./

RUN npm install

COPY . .

RUN npm run release

FROM nginx:alpine

COPY --from=build /app/dist/browser/ /usr/share/nginx/html
