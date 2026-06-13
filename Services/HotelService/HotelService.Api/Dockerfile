FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["Services/HotelService/HotelService.Api/HotelService.Api.csproj", "Services/HotelService/HotelService.Api/"]
COPY ["Services/HotelService/HotelService.Application/HotelService.Application.csproj", "Services/HotelService/HotelService.Application/"]
COPY ["Services/HotelService/HotelService.Domain/HotelService.Domain.csproj", "Services/HotelService/HotelService.Domain/"]
COPY ["Services/HotelService/HotelService.Infrastructure/HotelService.Infrastructure.csproj", "Services/HotelService/HotelService.Infrastructure/"]
COPY ["Shared.Contracts/Shared.Contracts.csproj", "Shared.Contracts/"]

RUN dotnet restore "Services/HotelService/HotelService.Api/HotelService.Api.csproj"

COPY . .

WORKDIR "/src/Services/HotelService/HotelService.Api"
RUN dotnet publish "HotelService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "HotelService.Api.dll"]