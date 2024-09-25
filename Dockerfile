FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app 
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CommonDrawing/CommonDrawing.csproj", "CommonDrawing/"]
RUN dotnet restore "CommonDrawing/CommonDrawing.csproj"
COPY . . 
WORKDIR "/src/CommonDrawing"
RUN dotnet build "./CommonDrawing.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CommonDrawing.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CommonDrawing.dll"]