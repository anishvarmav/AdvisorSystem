# Advisor Management System

## Overview
The Advisor System is a simple CRUD application built using Angular, .NET Core, and Entity Framework Core. 
The system allows you to create, view, update, and delete advisor records with caching to improve performance.

## Features
- Create a new advisor
- Retrieve a list of advisors
- Update an existing advisor
- Delete an advisor
- Most Recently Used (MRU) caching

## Prerequisites
- .NET 8 SDK - https://dotnet.microsoft.com/download
- Node.js and npm - https://nodejs.org/
- Angular CLI- https://angular.io/cli

## Installation
1. **Clone the repository:**
    ```bash
    git clone https://github.com/your-repo/advisor-system.git
    cd advisor-system
    ```

2. **Set up the backend:**
    ```bash
    cd AdvisorSystem.Server
    dotnet restore
    dotnet build
    ```

3. **Set up the frontend:**
    ```bash
    cd ../advisorsystem.client
    npm install
    ```

## Running the Application
1. **Start the backend server:**
    ```bash
    cd AdvisorSystem.Server
    dotnet run
    ```
    The backend server will start at `http://localhost:5106`.

2. **Start the frontend server:**
    ```bash
    cd ../advisorsystem.client
    ng serve
    ```
    The frontend server will start at `http://localhost:4200`.

## Running Tests
1. **Navigate to the test project directory:**
    ```bash
    cd AdvisorSystem.Server.Tests
    ```

2. **Run the tests:**
    ```bash
    dotnet test
    ```

## Usage
- To view all advisors, navigate to the main page.
- To add an advisor, click the "+ Create" button and fill out the form.
- To edit an advisor, click the "Edit" icon in the advisor's row and edit the form.
- To delete an advisor, click the "Delete" icon in the advisor's row.
