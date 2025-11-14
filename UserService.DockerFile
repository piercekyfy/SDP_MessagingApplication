FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /Source

COPY UserService/UserService.csproj UserService/
COPY Shared/Shared.csproj Shared/

RUN dotnet restore UserService/UserService.csproj

# Copy sources
COPY UserService/ UserService/
COPY Shared/ Shared/

#  Build/Publish
WORKDIR /Source/UserService
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app .

ENTRYPOINT ["dotnet", "UserService.dll"]
