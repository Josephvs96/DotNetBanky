# DotNetBanky

Bank managment system

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
- DotNetBanky.Tests : Xunit project to run tests with MOQ to moq the reposiroties 

## [Project Requierments](https://github.com/Josephvs96/DotNetBanky/blob/main/Krav%20st%C3%A4llningen.md)
