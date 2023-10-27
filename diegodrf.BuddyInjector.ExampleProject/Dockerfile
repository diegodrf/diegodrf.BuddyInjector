FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["diegodrf.BuddyInjector.ExampleProject/diegodrf.BuddyInjector.ExampleProject.csproj", "diegodrf.BuddyInjector.ExampleProject/"]
RUN dotnet restore "diegodrf.BuddyInjector.ExampleProject/diegodrf.BuddyInjector.ExampleProject.csproj"
COPY . .
WORKDIR "/src/diegodrf.BuddyInjector.ExampleProject"
RUN dotnet build "diegodrf.BuddyInjector.ExampleProject.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "diegodrf.BuddyInjector.ExampleProject.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "diegodrf.BuddyInjector.ExampleProject.dll"]
