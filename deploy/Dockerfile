FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY VTS.sln ./
COPY src/VTS.API/VTS.API.csproj ./src/VTS.API/
COPY src/VTS.BLL/VTS.BLL.csproj ./src/VTS.BLL/
COPY src/VTS.Common/VTS.Common.csproj ./src/VTS.Common/
COPY src/VTS.DAL/VTS.DAL.csproj ./src/VTS.DAL/

RUN dotnet restore "VTS.sln"

COPY . .
RUN dotnet publish "src/VTS.API/VTS.API.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ARG PORT=80
ENV ASPNETCORE_URLS=http://+:${PORT}
EXPOSE ${PORT}

ENTRYPOINT ["dotnet", "VTS.API.dll"]
