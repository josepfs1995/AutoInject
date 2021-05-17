# AutoInject
AutoInject in .NET Core

# AutoInject
![License](https://img.shields.io/github/license/josepfs1995/AutoInject)

## Introduction

 This project is used to automatically generate your depencendy injections for your Web NET Core.

 Esto proyecto se utiliza para inyectar automaticamente para tu pagina web en NET CORE

## Installation
Install Package:
```
Install-Package JF.AutoInject -Version 1.0.2
 ```
First step: You need write this code in the section Configure in your startup:

Necesitas escribir este codigo en la sección Configure de tu startup:
```
services.AddAutoDI(GetType().Assembly, ServiceLifetime.Transient);
```
Second step: You need add the interface IAutoDI in your class that will be injected

Necesitas agregar la interfa IAutoDI a tus clases que seran inyectadas
```
public class PeopleRepository:IPeopleRepository, IAutoDI
{
  ....
}
```
## Important
 Your class and interface must have same name
 
 Example
 
 ```
public class PeopleRepository:IPeopleRepository, IAutoDI
public class CarRepository:ICarRepository, IAutoDI
```
