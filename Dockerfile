FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
RUN apt-get update -yq \
    && apt-get install curl gnupg -yq \
    && curl -sL https://deb.nodesource.com/setup_14.x | bash \
    && apt-get install nodejs -yq
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
RUN apt-get update -yq \
    && apt-get install curl gnupg -yq \
    && curl -sL https://deb.nodesource.com/setup_14.x | bash \
    && apt-get install nodejs -yq
WORKDIR /src
COPY ["MoneyManager/MoneyManager.csproj", "MoneyManager/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Logging/Logging.csproj", "Logging/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Persistence/Persistence.csproj", "Persistence/"]
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