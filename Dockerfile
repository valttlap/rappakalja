# Stage 1: Build Angular Project
ARG AUTH0_DOMAIN
ARG AUTH0_CLIENT_ID
ARG AUTH0_AUDIENCE
ARG BUILD_ENV
ENV AUTH0_DOMAIN=$AUTH0_DOMAIN AUTH0_CLIENT_ID=$AUTH0_CLIENT_ID AUTH0_AUDIENCE=$AUTH0_AUDIENCE BUILD_ENV=$BUILD_ENV
FROM node:latest as build-node
WORKDIR /app
COPY client/Sanasoppa.UI/package.json client/Sanasoppa.UI/package-lock.json ./
RUN npm install
COPY client/Sanasoppa.UI/ .
RUN npm run build -- --output-path=./dist/out

# Stage 2: Build .NET Projects
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-net
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ["backend/Sanasoppa.Model/Sanasoppa.Model.csproj", "./backend/Sanasoppa.Model/"]
COPY ["backend/Sanasoppa.Core/Sanasoppa.Core.csproj", "./backend/Sanasoppa.Core/"]
COPY ["backend/Sanasoppa.API/Sanasoppa.API.csproj", "./backend/Sanasoppa.API/"]
RUN dotnet restore "backend/Sanasoppa.API/Sanasoppa.API.csproj"

# Copy everything else and build
COPY backend/ ./backend
COPY --from=build-node /app/dist/out ./backend/Sanasoppa.API/wwwroot
RUN dotnet publish backend/Sanasoppa.API/Sanasoppa.API.csproj -c Release -o out

# Stage 3: Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-net /app/out .
EXPOSE 80
ENTRYPOINT ["dotnet", "Sanasoppa.API.dll"]
