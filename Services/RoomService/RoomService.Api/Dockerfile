FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["Services/RoomService/RoomService.Api/RoomService.Api.csproj", "Services/RoomService/RoomService.Api/"]
COPY ["Services/RoomService/RoomService.Application/RoomService.Application.csproj", "Services/RoomService/RoomService.Application/"]
COPY ["Services/RoomService/RoomService.Domain/RoomService.Domain.csproj", "Services/RoomService/RoomService.Domain/"]
COPY ["Services/RoomService/RoomService.Infrastructure/RoomService.Infrastructure.csproj", "Services/RoomService/RoomService.Infrastructure/"]
COPY ["Shared.Contracts/Shared.Contracts.csproj", "Shared.Contracts/"]

RUN dotnet restore "Services/RoomService/RoomService.Api/RoomService.Api.csproj"

COPY . .

WORKDIR "/src/Services/RoomService/RoomService.Api"
RUN dotnet publish "RoomService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "RoomService.Api.dll"]