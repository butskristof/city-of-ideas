# City of Ideas

## Introduction
This .NET Core solution contains the main part of the City of Ideas project we made as an assignment for the 'Integratieproject 1' course. This is a part of the bachelor in applied computer science at the Karel de Grote University College.

## Technology
Following technologies are used:
* .NET Core 2.2 SDK with EF Core and ASP.NET Identity
* JetBrains Rider (tested in Visual Studio 2019)
* NodeJS

## Configuration
To run the project, complete the following steps:
* build the whole solution (`dotnet build`)
* navigate to the UI-MVC project
	* create a `coi_env.json` file to configure runtime parameters. You can find the required layout below.
	* run `npm install`, this will install the necessary front-end dependencies
	* run `npm run build`, this will run a webpack configuration
	* run the UI-MVC project. This will start the API and front-end web site. 
	* The web site and API are now accessible at `https://localhost:5001`
	* If you want adjustments to the front-end dependencies to be updated while you are coding, run `npm run watch`. This will continuously run webpack when changes are detected, so they should be reflected immediately in your browser. It may be necessary to refresh without cache to apply them.
	
## Tips
* If you edit your run configuration in Rider, it's possible to add the npm build script as a build step. 
	* `Run` menu option
	* `Edit configurations` 
	* select `MVC` under `.NET Launch Settings Profile` on the left-hand side 
	* On the bottom there will be a `Before launch:` panel. Here you can add an extra step: `Run npm script`
	* Select the `package.json` file in the UI-MVC project
	* Command: `run`
	* Scripts: `build`
	* Confirm
* Mind that even with the configuration above, you'll still have to run `npm install` the first time, and when you've modified the dependencies in the `package.json` file. 

* For testing, you can browse to `https://localhost:5001/Database/Seed`, which will provide some test data.

## `coi_env.json` layout
The `coi_env.json` file should be placed in the UI-MVC folder. It should contain the following key-value pairs:
```
{
	"Jwt": {
		"Key": "YOUR_SECRET_KEY",
		"Audience": "CityOfIdeas",
		"Issuer": "CityOfIdeas"
	},
	"Sqlite": {
		"ConnectionString": "Data Source=../db/CityOfIdeas.db"
	}, 
	"AdminUser": {
		"UserEmail": "ADMIN_EMAIL",
		"UserPassword": "ADMIN_PASSWORD",
		"FirstName": "Admin",
		"LastName": "COI"
	},
	"SendGrid": {
		"User": "SENDGRID_USER",
		"Key": "SENDGRID_KEY"
	},
	"Facebook":{
		"AppId": "FB_APP_ID",
		"AppSecret": "FB_APP_SECRET"
	}, 
	"Google": {
		"ClientId": "GOOGLE_CLIENT_ID",
		"ClientSecret": "GOOGLE_CLIENT_SECRET"
	}
}
``` 

Since it contains sensitive data (e.g. signing key for JWT tokens), it should **not** be committed to source control.