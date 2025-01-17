# ASP.Net Core Web API with a Vue front-end hosted as an SPA (Single-Page Application)

## Setup

This project was created with the back-end first, using the following command:

```cli
dotnet new webapi
```

After running the command, the default `WeatherForecast` part was removed along with the `Swagger` parts that are generated automatically.

To generate the front-end, this command was run in the project folder of the Web API:

```cli
npm init vue@latest
```

This command requires some settings and for this project the following settings were used:

- Name: frontend
- Typescript: Yes
- JSX Support: No
- Vue Router: Yes
- Pinia: No
- Vitest: No
- End-to-end testing: No
- ESLint: Yes
- Prettier: Yes

## Configuration

In terms of configuration, there are some things required by the back-end in order to be able to serve the front-end inside the same application. These can be found inside `Program.cs`, `ActionExtensions.csproj` and `vite.config.ts`.

Inside `Program.cs` you will also find the configuration for Visma Connect. To authenticate towards Visma Connect, client credentials from [Visma Developer Portal](https://oauth.developers.visma.com/) need to be used. (make sure to use the credentials from [Developer Portal Stage](https://oauth.developers.stagaws.visma.com/) if you are running the application in a development/staging environment). Of course, the secret of the credentials should be stored and retrieved from somewhere **safe** and not directly in the code repository.

In the `constants.ts`-file the base URL for calls to the backend is being set. This needs to be set to **your** specific localhost backend URL in order to be able to run the frontend locally.

## Debugging

When running the application locally for debugging, do the following

1. Using `cd` in a terminal, navigate to the `frontend`-folder then run `npm run build`
2. Debug the project using standard .Net-procedures.

Whenever changes are made to the frontend, make sure to run npm run build again. The backend can keep on running during this time.

## Using the Extension Framework

The setup for the calls using the Visma Net Extension Framework can be found in the following files:

- `WindowRequestsView.vue`: Contains the requests available to send through the window communication
- `App.vue`: Contains the response handling for the aforementioned requests that have been sent to Visma Net ERP
- `constants.ts`, `requestTypes.ts`, `responseTypes.ts`: Contains some base values and types that are referenced in the other two files

## Integrations

This example contains code that shows how to authenticate to Visma Connect when running a Web Application (as configured in Developer Portal) as well as how to send requests to an integration that has been added in Developer Portal. In this case the Visma Net Interactive API is being used and the requests are being sent to the following endpoints:

- **GET** to /API/controller/api/v1/supplier/supplierClass
- **POST** to /API/controller/api/v1/supplierInvoice

## Deployment note

When publishing and deploying, this application runs with **both** the front-end and back-end on the **same URL** meaning that it's **very** important to separate the routing on the two. For the back-end, a global path like `/api` can be added to the controllers of course. The root URL ("https://example.com/") and any un-mapped routes will always present the front-end.
