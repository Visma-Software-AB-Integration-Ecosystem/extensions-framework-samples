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
- Vue Router: No
- Pinia: No
- Vitest: No
- End-to-end testing: No
- ESLint: Yes
- Prettier: Yes

## Configuration

In terms of configuration, there are some things required by the back-end in order to be able to serve the front-end inside the same application. These can be found inside `Program.cs`, `ActionExtensions.csproj` and `vite.config.ts`.

Inside `Program.cs` you will also find the configuration for Visma Connect. To authenticate towards Visma Connect, client credentials from [Visma Developer Portal](https://oauth.developers.visma.com/) need to be used. (make sure to use the credentials from [Developer Portal Stage](https://oauth.developers.stagaws.visma.com/) if you are running the application in a development/staging environment). Of course, the secret of the credentials should be stored and retrieved from somewhere **safe** and not directly in the code repository.

## Deployment note

When publishing and deploying, this application runs with **both** the front-end and back-end on the **same URL** meaning that it's **very** important to separate the routing on the two. For the back-end, a global path like `/api` can be added to the controllers of course. The root URL ("https://example.com/") and any un-mapped routes will always present the front-end.
