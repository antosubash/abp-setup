# abp-setup

set up script to bootstrap my abp microservice application.

`init.ps1` file has some basic script for creating a microservice application using ABP. This will not create function application instead this will create folders and files which are required.

The project structure is inspired by : <https://github.com/abpframework/eShopOnAbp>

## Instructions

To create a new app run

```bash
.\init.ps1 YourProjectName
```

## Roadmap

- [] Create a cli to use this script
- [] Fix the build errors after setup
- [] Add correct reference for the projects
- [] Add the dependency to the projects
- [] Update the connection string
- [] Update the Urls in appsettings.json
- [] Prepare ef core in all the services
- [] Prepare shared project
- [] Prepare Gateway
- [] Prepare data seeding
- [] Prepare Identity server
- [] Prepare RabbitMQ
- [] Handle Tenant Events
- [] Init tye
- [] Init git
