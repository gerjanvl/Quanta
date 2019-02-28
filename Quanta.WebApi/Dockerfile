FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Guacamole.WebSocket/Guacamole.WebSocket.csproj", "Guacamole.WebSocket/"]
RUN dotnet restore "Guacamole.WebSocket/Guacamole.WebSocket.csproj"
COPY . .
WORKDIR "/src/Guacamole.WebSocket"
RUN dotnet build "Guacamole.WebSocket.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Guacamole.WebSocket.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Guacamole.WebSocket.dll"]