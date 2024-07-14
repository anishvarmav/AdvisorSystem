#!/bin/bash

# Variables
SERVER_DIR="AdvisorSystem.Server"
CLIENT_DIR="advisorsystem.client"
TEST_DIR="AdvisorSystem.Server.Tests"
BACKEND_PORT=5106
FRONTEND_PORT=4200

# Step 1: Set up the backend
echo "Setting up the backend"
cd $SERVER_DIR
dotnet restore
dotnet build

# Step 2: Set up the frontend
echo "Setting up the frontend"
cd ../$CLIENT_DIR
npm install

# Step 3: Run the backend
echo "Starting the backend server"
cd ../$SERVER_DIR
dotnet run &
BACKEND_PID=$!
echo "Backend server started at http://localhost:$BACKEND_PORT with PID $BACKEND_PID"

# Step 4: Run the frontend
echo "Starting the frontend server"
cd ../$CLIENT_DIR
ng serve --port $FRONTEND_PORT &
FRONTEND_PID=$!
echo "Frontend server started at http://localhost:$FRONTEND_PORT with PID $FRONTEND_PID"

# Wait for user input to terminate
read -p "Press ENTER to terminate the servers..."

# Terminate the servers
kill $BACKEND_PID
kill $FRONTEND_PID
echo "Servers terminated"
