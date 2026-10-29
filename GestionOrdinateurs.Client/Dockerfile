# =========================
# Build
# =========================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ../GestionOrdinateurs.Client/GestionOrdinateurs.Client.csproj ./GestionOrdinateurs.Client/
COPY ../GestionOrdinateurs.Shared/GestionOrdinateurs.Shared.csproj ./GestionOrdinateurs.Shared/
RUN dotnet restore ./GestionOrdinateurs.Client
RUN dotnet restore ./GestionOrdinateurs.Shared

# Copy the remaining source code
COPY ./GestionOrdinateurs.Client/ ./GestionOrdinateurs.Client/
COPY ./GestionOrdinateurs.Shared/ ./GestionOrdinateurs.Shared/
RUN dotnet publish ./GestionOrdinateurs.Client -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Set environment variables if needed (e.g., ASPNETCORE_ENVIRONMENT)
ENV ASPNETCORE_ENVIRONMENT=Development

# Expose port 5011
EXPOSE 5011

# Define the entry point
ENTRYPOINT ["dotnet", "GestionOrdinateurs.Client.dll"]
