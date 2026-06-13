FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["Services/BookingService/BookingService.Api/BookingService.Api.csproj", "Services/BookingService/BookingService.Api/"]
COPY ["Services/BookingService/BookingService.Application/BookingService.Application.csproj", "Services/BookingService/BookingService.Application/"]
COPY ["Services/BookingService/BookingService.Domain/BookingService.Domain.csproj", "Services/BookingService/BookingService.Domain/"]
COPY ["Services/BookingService/BookingService.Infrastructure/BookingService.Infrastructure.csproj", "Services/BookingService/BookingService.Infrastructure/"]
COPY ["Shared.Contracts/Shared.Contracts.csproj", "Shared.Contracts/"]

RUN dotnet restore "Services/BookingService/BookingService.Api/BookingService.Api.csproj"

COPY . .

WORKDIR "/src/Services/BookingService/BookingService.Api"
RUN dotnet publish "BookingService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BookingService.Api.dll"]