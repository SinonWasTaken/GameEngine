FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Shader_Test.csproj", "./"]
RUN dotnet restore "Shader_Test.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "Shader_Test.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Shader_Test.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Shader_Test.dll"]
