FROM mcr.microsoft.com/dotnet/runtime-deps:8.0.2

# create work dir and set permissions as WORKDIR sets permissions as root
RUN mkdir /app && chown -R app:app /app
WORKDIR /app

LABEL maintainer "Digitaal Vlaanderen <digitaal.vlaanderen@vlaanderen.be>"
LABEL microservice="ticketing-monitoring"

COPY / /app
WORKDIR /app

RUN apt-get update && \
    apt-get install curl jq -y && \
    chmod +x ./init.sh

EXPOSE 9002/tcp
ENV ASPNETCORE_URLS http://*:9002

# switch to created user
USER app

ENTRYPOINT ["./init.sh"]
