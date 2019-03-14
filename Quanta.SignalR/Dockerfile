FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Quanta.SignalR/Quanta.SignalR.csproj", "Quanta.SignalR/"]
RUN dotnet restore "Quanta.SignalR/Quanta.SignalR.csproj"
COPY . .
WORKDIR "/src/Quanta.SignalR"
RUN dotnet build "Quanta.SignalR.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Quanta.SignalR.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Quanta.SignalR.dll"]