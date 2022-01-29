#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Jobsity.Challenge.FinancialChat.Bot/Jobsity.Challenge.FinancialChat.Bot.csproj", "Jobsity.Challenge.FinancialChat.Bot/"]
COPY ["Jobsity.Challenge.FinancialChat.Core/Jobsity.Challenge.FinancialChat.Core.csproj", "Jobsity.Challenge.FinancialChat.Core/"]
RUN dotnet restore "Jobsity.Challenge.FinancialChat.Bot/Jobsity.Challenge.FinancialChat.Bot.csproj"
COPY . .
WORKDIR "/src/Jobsity.Challenge.FinancialChat.Bot"
RUN dotnet build "Jobsity.Challenge.FinancialChat.Bot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Jobsity.Challenge.FinancialChat.Bot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Jobsity.Challenge.FinancialChat.Bot.dll"]