FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
COPY ["Blog.PublicApi/Blog.PublicApi.csproj", "Blog.PublicApi/"]
RUN dotnet restore "Blog.PublicApi/Blog.PublicApi.csproj"
COPY . .
WORKDIR "Blog.PublicApi"
RUN dotnet build "Blog.PublicApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Blog.PublicApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Blog.PublicApi.dll"]
