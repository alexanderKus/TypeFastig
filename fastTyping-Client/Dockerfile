FROM node:18.15.0 as build

WORKDIR /app
COPY . ./
RUN npm ci
RUN npm run build

FROM nginx:alpine

COPY --from=build /app/dist/fast-typing-client /usr/share/nginx/html
EXPOSE 4200:80
#ENTRYPOINT [ "nginx", "-g", "daemon off;" ]