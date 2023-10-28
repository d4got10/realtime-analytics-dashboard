<h1 align="center">Real-time Analytics Dashboard</h1>

## Features

* Events (**In progress**)
* Funnels (**TBD**)
* Install tracking (**TBD**)
* Real-time data visualisation (**TBD**)

## Task tracker - [Trello](https://trello.com/b/zaOhJgSq/all)

## Integration

Android SDK - **TBD**

## Deployment

**Prerequisites**

* [Docker](https://www.docker.com/)
* [Docker Compose](https://docs.docker.com/compose/)

```
git clone https://github.com/d4got10/realtime-analytics-dashboard.git
cd realtime-analytics-dashboard
docker-compose up -d
```

## Usage

* ```http://*your_ip*:4000/``` - data collection api
* ```http://*your_ip*:5000/``` - aggregated data access api
* ```https://*your_ip*/``` - data visualisation web app

For each API there is a swagger documentation that can be found at ```*host:port*/swagger/index.html```<br>
**Example**: ```http://*your_ip*:4000/swagger/index.html``` for data collection api documentation

## Architecture

![Diagram of an application architecture](/assets/analytics-dashboard-v1.png)

## Used technologies

* [.NET 7](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-7)
* [Fast Endpoints](https://fast-endpoints.com/) wrapper around minimal apis
* [Angular JS](https://angular.io/) for data visualisation client web app
* [Kafka](https://kafka.apache.org/) for microservice messaging
* [PostgreSQL 16.0](https://www.postgresql.org/) for persistent data storage
* [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/) for database object mapping and migration maganement
