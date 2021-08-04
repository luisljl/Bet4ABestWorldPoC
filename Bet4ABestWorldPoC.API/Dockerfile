#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Bet4ABestWorldPoC.API/Bet4ABestWorldPoC.API.csproj", "Bet4ABestWorldPoC.API/"]
RUN dotnet restore "Bet4ABestWorldPoC.API/Bet4ABestWorldPoC.API.csproj"
COPY . .
WORKDIR "/src/Bet4ABestWorldPoC.API"
RUN dotnet build "Bet4ABestWorldPoC.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bet4ABestWorldPoC.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bet4ABestWorldPoC.API.dll"]