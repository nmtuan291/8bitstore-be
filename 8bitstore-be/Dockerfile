# Use the official .NET 8 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["8bitstore-be.csproj", "./"]
RUN dotnet restore "8bitstore-be.csproj"

# Copy the rest of the source code
COPY . .

# Build the application
RUN dotnet build "8bitstore-be.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "8bitstore-be.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Install necessary packages for production
RUN apt-get update && apt-get install -y \
    curl \
    && rm -rf /var/lib/apt/lists/*

# Copy the published application
COPY --from=publish /app/publish .

# Expose port 80 and 443
EXPOSE 80
EXPOSE 443

# Set the entry point
ENTRYPOINT ["dotnet", "8bitstore-be.dll"] 