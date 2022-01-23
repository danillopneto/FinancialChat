# Steps to run the application:

Unfortunately there wasn't time to run through docker, so we're going to run each application in a console in the order below:

*Obs.: Navigate the folder until you reach the solution folder.*

> **1.** docker run -d -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.9-management

> **2.** dotnet watch run --no-build --launch-profile "Jobsity.Challenge.FinancialChat.SignalR" --urls=https://localhost:5001/ --project ".\Jobsity.Challenge.FinancialChat.SignalR"

> **3.** dotnet watch run --no-build --launch-profile "Jobsity.Challenge.FinancialChat.Bot" --urls=https://localhost:7152 --project ".\Jobsity.Challenge.FinancialChat.Bot"

> **4.** dotnet watch run --no-build --launch-profile "Jobsity.Challenge.FinancialChat.Consumer" --project ".\Jobsity.Challenge.FinancialChat.Consumer"

> **5.** dotnet watch run --no-build --launch-profile "Jobsity.Challenge.FinancialChat.Web" --urls=https://localhost:7020 --project ".\Jobsity.Challenge.FinancialChat.Web"