# DotNetBanky

Bank managment system

[Project Requierments](https://github.com/Josephvs96/DotNetBanky/blob/main/Krav%20st%C3%A4llningen.md)

## Demo site on Azure
[DotNetBanky](https://dotnetbanky.azurewebsites.net/)

### Login: 

- Email: demo@dotnetbanky.se
- Password: HejDemo!123

## Areas
- Admin Dashboard made with Asp.net Razor Pages that is used to manage the customers, accounts and make transactions between accounts.
- API made with Asp.net Web API for customers to use when using the mobile app (Not included in this project).
- Console App that is used to create a search index in azure cognitive search.
- Console App that is used to check for suspicious transactions in a fictional scenario.    

## Featuers
- Microsoft Identity for managing the users and their roles:
  - Uses Cookie Authentication for authenticating users in the web based Razor pages project.
  - Uses JWT tokens for authenticating users in the web API project.
  - Three diffrent roles with diffrent level of access all managed by microsoft identity.
-  Search Service:
    - Uses Azure Cognitive Search to create a search engine for the users for effective searching.
    - Index is created once by the console application.
    - Continuous updates for the index when adding new customers or editing customers.
- Database structure:
  - Entity Framework Core to manage the database relations between entities 
  - The database was created by a database first approach but was converted into a code first approach to use migrations with the database
  - The database is an Azure SQL database hosted in an Azure SQL Server 
  

## Uses the N-Tier architecture as folowes:
### Diagram
<p align="center">
<img width="600"  src="https://github.com/Josephvs96/DotNetBanky/blob/main/Git%20Assets/Architect.png?raw=true" />
</p>

### Layers
- DotNetBanky.DAL : The data access layer of the application with repositories 
- DotNetBanky.BLL : The business layer of the application with all the services for exchaning data between the data layer and the presentation layers
- DotNetBanky.ADMIN : First presentation layer, A Razor-Pages application for the admins and cashiers to manage the application
- DotNetBanky.API : The second presentation layer, A Web API that will be used to comunicate with the Mobile app for Banky (Not included in this projcet)
- DotNetBanky.Core : The data entites and the transfare objects that will be used to trasfare data between the presntation layer and business layer
- DotNetBanky.Common : The dependency injection layer that will configure the DI container to be used in diffrent applications
- DotNetBanky.Tests : Xunit project to run tests with MOQ to mock the reposiroties 

## Deployment & Development
### Hosting
The Demo Application is hosted in Azure in a windows app container, uses Azure Configuration to provied the sensitve on start app settings like the database connection string and the Azure search API keys.

### Deployment
The app is auto deployed by github actions to Azure on pullrequest merge or push to Main branch.

### Devemopment
The dev branch is where the development will be done and this branch will alwyas be more updated and has more features than main.

#### Dev workflow
When starting new feature branch start always from Dev branch and never merge directly into main, make a pull request to merge with dev instead. 

