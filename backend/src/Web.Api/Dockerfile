# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files and restore dependencies
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["src/Web.Api/Web.Api.csproj", "src/Web.Api/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/SharedKernel/SharedKernel.csproj", "src/SharedKernel/"]
RUN dotnet restore "src/Web.Api/Web.Api.csproj"

# Copy the remaining source code and build
COPY . .
WORKDIR "/src/src/Web.Api"
RUN dotnet publish "Web.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final Production Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
ENV DOTNET_USE_POLLING_FILE_WATCHER=true \
    DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false
RUN mkdir -p /app/Data
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Web.Api.dll"]
