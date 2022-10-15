FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BitwardenDuplicateRemover/BitwardenDuplicateRemover.csproj", "BitwardenDuplicateRemover/"]
RUN dotnet restore "BitwardenDuplicateRemover/BitwardenDuplicateRemover.csproj"
COPY . .
WORKDIR "/src/BitwardenDuplicateRemover"
RUN dotnet build "BitwardenDuplicateRemover.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BitwardenDuplicateRemover.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BitwardenDuplicateRemover.dll"]
