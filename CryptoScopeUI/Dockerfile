FROM node:20 AS build
WORKDIR /app

COPY . ./
RUN npm install
RUN npm run build

FROM nginx:alpine
COPY --from=build /app/dist/spa /usr/share/nginx/html