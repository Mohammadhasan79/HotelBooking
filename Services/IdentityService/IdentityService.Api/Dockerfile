FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["Services/IdentityService/IdentityService.Api/IdentityService.Api.csproj", "Services/IdentityService/IdentityService.Api/"]
COPY ["Services/IdentityService/IdentityService.Application/IdentityService.Application.csproj", "Services/IdentityService/IdentityService.Application/"]
COPY ["Services/IdentityService/IdentityService.Domain/IdentityService.Domain.csproj", "Services/IdentityService/IdentityService.Domain/"]
COPY ["Services/IdentityService/IdentityService.Infrastructure/IdentityService.Infrastructure.csproj", "Services/IdentityService/IdentityService.Infrastructure/"]
COPY ["Shared.Contracts/Shared.Contracts.csproj", "Shared.Contracts/"]

RUN dotnet restore "Services/IdentityService/IdentityService.Api/IdentityService.Api.csproj"

COPY . .

WORKDIR "/src/Services/IdentityService/IdentityService.Api"
RUN dotnet publish "IdentityService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "IdentityService.Api.dll"]