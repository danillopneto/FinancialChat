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

## Requirements
You must have *docker* installed on your operating system (Linux, Windows or Mac).  

# Steps to run the application

*Obs.: Navigate the folder until you reach the project' folder.*

### Run the command:
- ` docker-compose up --build` 

### After that , access http://localhost into a web-browser and start to using the **Financial Chat**.

### To stop the execution inside console ('detached' mode):
- ` docker-compose down` 

###  To stop the execution inside console ('attached' mode):
- <kbd>Crtl</kbd> + <kbd>C</kbd>
 
# Steps to debug the application

1. Run the following commands to start RabbitMQ and MySQL:  
    - `docker run -d -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.9-management`  
    - `docker run -d -it --rm --name mysql -p 3306:3306 -e MYSQL_RANDOM_ROOT_PASSWORD=1 -e MYSQL_DATABASE=financialchat -e MYSQL_USER=dbuser -e MYSQL_PASSWORD=dbuserpassword mysql:8.0.0`
1. Open the solution with Visual Studio  
2. Right-click into the Solution -> `Properties`  
3. Check `Multiple startup projects` then set as bellow:  
![Startup](/screenshots/multiple_startup.png?raw=true "Por hora")  
4. Click on `► Start`  

# Features included

- Multiple rooms
- Messages ordered by their `Timestamps`
- Command to get stocks prices via `/stock=stock_code`
- Messages saved into `MySql Database` so a new user could read previous messages
- Handling errors/commands not allowed
- Run the application via `docker.compose.yml`
- Limit of 50 messages per chat

# Features to be implemented

- Unit tests with `Mock` and `FluentAssertions`
- Authentication of users with .NET identity
- Saving/Gettings messages with encryption
