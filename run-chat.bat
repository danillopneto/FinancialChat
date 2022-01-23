@ECHO OFF 
:: This batch file will put the chat up.

TITLE Financial Chat
ECHO Please wait... Starting RabbitMq.
docker run -d -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.9-management
ECHO Starting SignalR Chat
dotnet watch run --launch-profile "Jobsity.Challenge.FinancialChat.SignalR" --urls=https://localhost:5001/ --project ".\Jobsity.Challenge.FinancialChat.SignalR"
ECHO Starting Bot to respond to commands
dotnet watch run --launch-profile "Jobsity.Challenge.FinancialChat.Bot" --urls=https://localhost:7152 --project ".\Jobsity.Challenge.FinancialChat.Bot"
ECHO Starting Consumer to publish to stocks prices
dotnet watch run --launch-profile "Jobsity.Challenge.FinancialChat.Consumer" --project ".\Jobsity.Challenge.FinancialChat.Consumer"