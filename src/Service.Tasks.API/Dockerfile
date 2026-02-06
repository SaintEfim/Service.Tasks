FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/Service.Tasks.API/Service.Tasks.API.csproj src/Service.Tasks.API/
COPY src/Service.Tasks.API.Abstractions/Service.Tasks.API.Abstractions.csproj src/Service.Tasks.API.Abstractions/
COPY src/Service.Tasks.Domain/Service.Tasks.Domain.csproj src/Service.Tasks.Domain/
COPY src/Service.Tasks.Domain.Abstractions/Service.Tasks.Domain.Abstractions.csproj src/Service.Tasks.Domain.Abstractions/
COPY src/Service.Tasks.Data/Service.Tasks.Data.csproj src/Service.Tasks.Data/
COPY src/Service.Tasks.Data.Abstractions/Service.Tasks.Data.Abstractions.csproj src/Service.Tasks.Data.Abstractions/
COPY src/Service.Tasks.Shared/Service.Tasks.Shared.csproj src/Service.Tasks.Shared/
COPY src/Service.Tasks.UserJwtToken/Service.Tasks.UserJwtToken.csproj src/Service.Tasks.UserJwtToken/
COPY src/Service.Tasks.UserJwtToken.Abstractions/Service.Tasks.UserJwtToken.Abstractions.csproj src/Service.Tasks.UserJwtToken.Abstractions/
COPY src/Service.Tasks.UserPasswordHasher/Service.Tasks.UserPasswordHasher.csproj src/Service.Tasks.UserPasswordHasher/
COPY src/Service.Tasks.UserPasswordHasher.Abstractions/Service.Tasks.UserPasswordHasher.Abstractions.csproj src/Service.Tasks.UserPasswordHasher.Abstractions/

RUN dotnet restore src/Service.Tasks.API/Service.Tasks.API.csproj

COPY src/ .

FROM build AS publish
WORKDIR /src/Service.Tasks.API
RUN dotnet publish -c Release -o /app/publish --no-restore /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Service.Tasks.API.dll"]
