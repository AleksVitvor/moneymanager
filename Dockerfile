FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

RUN curl -sL https://deb.nodesource.com/setup_14.x |  bash -
RUN apt-get install -y nodejs

COPY ["MoneyManager/MoneyManager.csproj", "MoneyManager/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Logging/Logging.csproj", "Logging/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "MoneyManager/MoneyManager.csproj"
COPY . .
WORKDIR "/src/MoneyManager"
RUN dotnet build "MoneyManager.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MoneyManager.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MoneyManager.dll"]