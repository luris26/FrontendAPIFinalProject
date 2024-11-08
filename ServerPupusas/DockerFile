FROM mcr.microsoft.com/dotnet/sdk:8.0 as build

WORKDIR /app
EXPOSE 8080

COPY *.csproj .

RUN dotnet restore
COPY . .


# RUN dotnet build -c Release -o /app/build

RUN dotnet publish -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:8.0 as runtime
WORKDIR /app

COPY --from=build /app/publish .
CMD [ "dotnet", "ServerPupusas.dll" ]