FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/SGM.WebApp/SGM.WebApp.csproj src/SGM.WebApp/
RUN dotnet restore src/SGM.WebApp/SGM.WebApp.csproj

COPY src/SGM.WebApp/ src/SGM.WebApp/
RUN dotnet publish src/SGM.WebApp/SGM.WebApp.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["./SGM.WebApp"]
