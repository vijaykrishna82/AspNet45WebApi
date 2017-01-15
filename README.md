# AspNetWebApiStarter
Starter project of Asp.net WebApi

## Prerequisites
+ IIS with ASP.net 4.5 HTTP Activation enabled (in windows features)
+ SQL Server 2016 Express

## Database Setup
+ Go to package manager console 
	`Tools -> NuGet Package Manager -> Package Manager Console`
+ Enable migrations `enable-migrations`
+ Add an initial migration: `add-migration initial`
+ Update Database: `update-database`

## Postman
+ Open Postman (windows)
+ Import collection
+ select one of the collections
+ click on the hamburger menu and access the collection 