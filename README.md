# Money Manager

## How it works

#### Deployment
We have 2 deploy pipelines:
1. WebApi + SPA:
- executed if we detec changes in WebApp folder
- create docker image (it needed because WebApi and SPA works together and need Node.js + NET as execution stack)
- push to docker
- each time when Azure App Service detected new version of the image, it pulls this image to Azure App Service
2. Azure Function:
- executed if changes in WorkerFunctions folder detected
- build and push to azure .NET app using publish profile (can be downloaded on Azure Portal)

#### Tests
We have 2 test pipelines:
- executed on pull request to main branch
1. WebApi + SPA:
- executed if we detec changes in WebApp folder
- build .NET app and run tests for .NET
2. Azure Function:
- executed if changes in WorkerFunctions folder detected
- build .NET app and run tests