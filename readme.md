# Financial Chat Challenge

## Assignment
The goal of this exercise is to create a simple browser-based chat application using .NET.  
This application should allow several users to talk in a chatroom and also to get stock quotes from an API using a specific command.

> ![Chat](/screenshots/chat_room.png?raw=true "Por hora")  

## Solution

* It was created 4 applications with the following roles:
    - `SignalR:` to provide a real-time chat functionality.
    - `Bot:` so the chat could be able to invoke commands to be executed and put into a queue to get processed messages later.
    - `Consumer:` so it watches/consumes the commands that were queued and calls a Chat API with the response.
    - `Web`: simple web application that consumes the SignalR app.

# Requirements
You must have *docker* installed on your operating system (Linux, Windows or Mac).  
Once you've installed, run the command bellow to start RabbitMQ:  

`docker run -d -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.9-management`

# Steps to run the application:

### **1º) Option**

Unfortunately there wasn't time to run through docker, so we're going to run each application in a console in the order below:

*Obs.: Navigate the folder until you reach the solution folder.  
You can use Visual Studio to open multiple consoles or use the Windows Terminal, for instance.*

> **1.** `dotnet watch run --no-build --launch-profile "Jobsity.Challenge.FinancialChat.SignalR" --urls=https://localhost:5001/ --project ".\Jobsity.Challenge.FinancialChat.SignalR"`  
> **2.** `dotnet watch run --no-build --launch-profile "Jobsity.Challenge.FinancialChat.Bot" --urls=https://localhost:7152 --project ".\Jobsity.Challenge.FinancialChat.Bot"`  
> **3.** `dotnet watch run --no-build --launch-profile "Jobsity.Challenge.FinancialChat.Consumer" --project ".\Jobsity.Challenge.FinancialChat.Consumer"`  
> **4.** `dotnet watch run --no-build --launch-profile "Jobsity.Challenge.FinancialChat.Web" --urls=https://localhost:7020 --project ".\Jobsity.Challenge.FinancialChat.Web"`  

### **2º) Option**

> 1. Open the solution with Visual Studio  
> 2. Right-click into the Solution -> `Properties`  
> 3. Check `Multiple startup projects` then set as bellow:  
> ![Startup](/screenshots/multiple_startup.png?raw=true "Por hora")  
> **4.** Click on `Start`  

After choosing the option, access https://localhost:7020 into a web-browser and start to using the **Financial Chat**.

## Features included

- Multiple rooms
- Messages ordered by their `Timestamps`
- Command to get stocks prices via `/stock=stock_code`
- Messages saved into `InMemoryDatabase` so a new user could read previous messages
- Handling errors/commands not allowed

## Features to be implemented

- Unit tests with `Mock` and `FluentAssertions`
- Authentication of users with .NET identity
- Installer or `docker.compose.yml`
- Limit of 50 messages per chat
- Saving/Gettings messages with encryption