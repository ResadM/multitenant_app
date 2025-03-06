# multitenant_app

## Table of Contents

- [Description](#description)
- [Features](#features)
- [appsettings.json](#appsettingsjson)

## Medium Article
[Building a Multi-Tenant API with Multiple Databases in .NET Core](https://medium.com/@rashad_m/building-a-multi-tenant-api-with-multiple-databases-in-net-core-93449fb06c01)

## Description
This is a multi-tenant application that
- Uses multiple database for each tenant
- Uses single codebase for all tenants
- Uses JWT authentication
- Uses Microsoft Identity for user management
- Uses Entity Framework Core for database operations
- Uses Swagger for API documentation


## Features
- Net.Core 8
- Microsoft Identity
- JWT Authentication
- Multi-Tenant
- Entity Framework Core
- Swagger

## appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DataBaseContextAdmin": "Persist Security Info=False;User ID={user};Password={password};Database=MultiTenantApp;Server={db_server};TrustServerCertificate=True",
    "DataBaseContextUser": "Persist Security Info=False;User ID={user};Password={password};Database={DatabaseName};Server={db_server_};TrustServerCertificate=True",
    "DataBaseContextDefault": "Persist Security Info=False;User ID={user};Password={password};Database=MultiTenantAppUserDefault;Server={db_server};TrustServerCertificate=True"
  },
  "JWTSettings": {
    "TokenKey": "this is a secret key and needs to be at least 12 characters.But with new net 8 update it must be longer"
  },
  "AllowedHosts": "*"
}
```