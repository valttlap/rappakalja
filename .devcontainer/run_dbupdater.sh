#!/bin/bash

# Path to the .NET project
PROJECT_PATH="/workspaces/sanasoppa/backend/Sanasoppa.DBUpdater/Sanasoppa.DBUpdater.csproj"

# Run the .NET project with arguments
dotnet run --project "$PROJECT_PATH" -- update "Server=sanasoppa.db.local;Port=5432;Database=sanasoppa;User Id=postgres;Password=postgres;"
