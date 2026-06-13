FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["Services/PaymentService/PaymentService.Api/PaymentService.Api.csproj", "Services/PaymentService/PaymentService.Api/"]
COPY ["Services/PaymentService/PaymentService.Application/PaymentService.Application.csproj", "Services/PaymentService/PaymentService.Application/"]
COPY ["Services/PaymentService/PaymentService.Domain/PaymentService.Domain.csproj", "Services/PaymentService/PaymentService.Domain/"]
COPY ["Services/PaymentService/PaymentService.Infrastructure/PaymentService.Infrastructure.csproj", "Services/PaymentService/PaymentService.Infrastructure/"]
COPY ["Shared.Contracts/Shared.Contracts.csproj", "Shared.Contracts/"]

RUN dotnet restore "Services/PaymentService/PaymentService.Api/PaymentService.Api.csproj"

COPY . .

WORKDIR "/src/Services/PaymentService/PaymentService.Api"
RUN dotnet publish "PaymentService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PaymentService.Api.dll"]