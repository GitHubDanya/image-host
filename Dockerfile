# Stage 1: Base runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Stage 2: Build environment
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY ["image-host.csproj", "./"]
RUN dotnet restore "image-host.csproj"

# Copy full source code and build release binaries
COPY . .
RUN dotnet build "image-host.csproj" -c Release -o /app/build

# Stage 3: Publish binaries
FROM build AS publish
RUN dotnet publish "image-host.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 4: Final image creation
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "image-host.dll"]
