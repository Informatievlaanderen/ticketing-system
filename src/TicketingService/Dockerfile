FROM mcr.microsoft.com/dotnet/runtime-deps:6.0.3

LABEL maintainer "Digitaal Vlaanderen <digitaal.vlaanderen@vlaanderen.be>"
LABEL microservice="ticketing-service"

COPY / /app
WORKDIR /app

RUN apt-get update && \
    apt-get install curl jq -y && \
    chmod +x ./init.sh

EXPOSE 9000/tcp
ENV ASPNETCORE_URLS http://*:9000

ENTRYPOINT ["./init.sh"]