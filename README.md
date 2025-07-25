# ProjectTrackingApi
Asp.net core api project 
## Instruction to run the project

1. Clone the repository

2. Open the solution file from Visual studio.

3. Update the connection string in appsettings.json with valid server name and db name.

4. In Visual Studio, go to Tools > NuGet Package Manager > Package Manager Console.
Ensure the default project is set to ProjectTrackerApi in the dropdown at the top of the console. Go to the folder and run the following command
``
Update-Database
``

**Alternatively, Open the terminal, navigate to the project folder**

``
cd ProjectTrackerApi
``
And run

``
dotnet ef database update
``

5. Run the application

- Swagger page with the endpoints should load.

### To allow frontend to call this api endpoint

- Update the correct frontend url(Cors:AllowedOrigins) in appsettings.json file

## Built with

1. Asp.net core web api
2. Sql Server EfCore 

## Features

- Implemented ProjectsController with endpoints
  - GET /api/Projects to fetch all projects
  - GET /api/Projects/{id} to get a specific Project by ID
  - POST /api/Projects to create/add a project
  - PUT /api/Projects/{id} to update the project for the ID
  - DELETE /api/Projects/{id} to delete the project by ID

- Implemented swagger to test api endpoints.

- Data Model
  - Defined Project entity with **Name** **Description** **Status** **Owner** **StartDate** and **EndDate**
  - Defined ProjectDTO to be used in api/repository classes