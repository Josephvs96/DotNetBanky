# DotNetBanky

Bank managment system

## Uses the N-Tier architecture as folowes:
### Layers
- DotNetBanky.DAL : The data access layer of the application with repositories 
- DotNetBanky.BLL : The business layer of the application with all the services for exchaning data between the data layer and the presentation layers
- DotNetBanky.ADMIN.MVC : First presentation layer, An MVC application for the admins and cashiers to manage the application
- DotNetBanky.API : The second presentation layer, A Web API that will be used to comunicate with the Mobile app for Banky (Not included in this projcet)
- DotNetBanky.Entity : All the database entities.
- DotNetBanky.DTO : The data transfare objects that will be used to trasfare data between the presntation layer and business layer
- DotNetBanky.IoC : The dependency injection layer that will configure the DI container to be used in diffrent applications
- DotNetBanky.Utility : The helper classes, methodes and constatnts of the application, as well as the AutoMapper profiles 
- DotNetBanky.Tests : Xunit project to run tests with MOQ to moq the reposiroties 
