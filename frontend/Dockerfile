# Imagem das telas (arquitetura.md, seção 12.1). Contexto de construção: a pasta frontend.
# Etapa 1: compila as telas com o Node.js. Etapa 2: servidor web leve, que também repassa /api (ADR-21).
FROM node:24-slim AS compilacao
WORKDIR /origem
COPY package.json package-lock.json ./
RUN npm ci
COPY . .
RUN npm run build

FROM nginx:stable-alpine
COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=compilacao /origem/dist/timeline-web/browser /usr/share/nginx/html
EXPOSE 80
