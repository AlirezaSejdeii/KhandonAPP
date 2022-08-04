#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Src/Khandon.Presentation/Khandon.Persentation.API/Khandon.Persentation.API.csproj", "Src/Khandon.Presentation/Khandon.Persentation.API/"]
RUN dotnet restore "Src/Khandon.Presentation/Khandon.Persentation.API/Khandon.Persentation.API.csproj"
COPY . .
WORKDIR "/src/Src/Khandon.Presentation/Khandon.Persentation.API"
RUN dotnet build "Khandon.Persentation.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Khandon.Persentation.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Khandon.Persentation.API.dll"]