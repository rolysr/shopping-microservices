# Shopping Microservices

## Briefing
shopping-microservices is a backend ready-to-deploy project that is composed by two main microservices: Catalog and Inventory. These two microservices have the basic functionalities for managing items from a catalog of an e-commerce app and the inventory of a basic user that buys products shown on the catalog.

## Prerequisites 
- [.Net Core SDK v6.0.300](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- **(optional)** Some **UNIX** based OS (this projects was deploy on Ubuntu 22.04)

## How to run
  - Run the Catalog service 
    ```
    cd ./Shopping.Catalog/src/Shopping.Catalog.Service/
    dotnet run Program.cs
    ```
  - Run the Inventory service
    ```
    cd ./Shopping.Inventory/src/Shopping.Inventory.Service/
    dotnet run Program.cs
    ```
  - Run MongoDB database management system and RabbitMq message broker
    ```
    cd ./Shopping.Infra
    docker-compose up -d
    ```
