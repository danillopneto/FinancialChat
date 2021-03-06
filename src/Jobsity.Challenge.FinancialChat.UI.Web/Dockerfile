#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Jobsity.Challenge.FinancialChat.UI.Web/Jobsity.Challenge.FinancialChat.UI.Web.csproj", "Jobsity.Challenge.FinancialChat.UI.Web/"]
COPY ["Jobsity.Challenge.FinancialChat.CrossCutting/Jobsity.Challenge.FinancialChat.CrossCutting.csproj", "Jobsity.Challenge.FinancialChat.CrossCutting/"]
COPY ["Jobsity.Challenge.FinancialChat.UseCases/Jobsity.Challenge.FinancialChat.UseCases.csproj", "Jobsity.Challenge.FinancialChat.UseCases/"]
COPY ["Jobsity.Challenge.FinancialChat.Infra/Jobsity.Challenge.FinancialChat.Infra.csproj", "Jobsity.Challenge.FinancialChat.Infra/"]
COPY ["Jobsity.Challenge.FinancialChat.Core/Jobsity.Challenge.FinancialChat.Core.csproj", "Jobsity.Challenge.FinancialChat.Core/"]
COPY ["Jobsity.Challenge.FinancialChat.Domain/Jobsity.Challenge.FinancialChat.Domain.csproj", "Jobsity.Challenge.FinancialChat.Domain/"]
COPY ["Jobsity.Challenge.FinancialChat.Interfaces/Jobsity.Challenge.FinancialChat.Interfaces.csproj", "Jobsity.Challenge.FinancialChat.Interfaces/"]
RUN dotnet restore "Jobsity.Challenge.FinancialChat.UI.Web/Jobsity.Challenge.FinancialChat.UI.Web.csproj"
COPY . .
WORKDIR "/src/Jobsity.Challenge.FinancialChat.UI.Web"
RUN dotnet build "Jobsity.Challenge.FinancialChat.UI.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Jobsity.Challenge.FinancialChat.UI.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Jobsity.Challenge.FinancialChat.UI.Web.dll"]