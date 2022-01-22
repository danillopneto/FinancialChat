$(document).ready(function () {
    window.chat = createChatController();
    window.chat.loadUser();
});

function createChatController() {
    var user = {
        name: null,
        dtConnection: null
    }

    return {
        state: user,
        connection: null,
        loadUser: function () {
            this.state.name = prompt('Type your name to enter the chat', 'Username');
            this.state.dtConnection = new Date();
            this.connectUserToChat();
        },
        connectUserToChat: function () {
            startConnection(this);
        },
        sendMessage: function (to) {
            var chatMessage = {
                sender: this.state,
                message: to.message,
                destination: to.destination,
                timestamp: new Date()
            };

            this.connection.invoke("SendMessage", (chatMessage))
                .catch(err => console.log(x = err));

            to.field.val('').focus();
        },

        onReceiveMessage: function () {
            this.connection.on("Receive", (chatData) => {
                insertMessage(chatData);
            });
        }
    };
}

async function startConnection(chat) {
    try {

        chat.connection = new signalR.HubConnectionBuilder().withUrl("https://localhost:5001/chat?user=" + JSON.stringify(window.chat.state)).build();
        await chat.connection.start();

        $('.info_chat').show();

        chat.connection.on('userData', (user) => {
            window.chat.state = user;
        });
    
        loadRooms(chat.connection);

        chat.connection.onclose(async () => {
            await startConnection(chat);
        });

        window.chat.onReceiveMessage();

        $('.userAvatar').html(`
            <section class="user box_shadow_0">
            <span class="user_icon">${chat.state.name.charAt(0)}</span>
            <p class="user_name"> ${chat.state.name} </p>
            </section>
        `);

    } catch (err) {
        console.log(err);
        setTimeout(() => startConnection(chat), 5000);
    }
};

async function createChatRoom() {
    let chatRoomName = '';
    while (chatRoomName == '') {
        chatRoomName = prompt('Type the name of the chatroom', '');
    }

    addToChatRoom(chatRoomName);
}

async function addToChatRoom(chatRoomName) {
    chat.connection.invoke("AddToChatRoom", (chatRoomName))
        .catch(err => console.log(x = err));
}

async function loadRooms(connection) {
    connection.on('chat', (rooms, user) => {
        rooms.forEach((room) => {
            if (!checkIfElementExist(room.id, 'id')) {
                let sectionRoom = `
                    <section class="room box_shadow_0" onclick="openChat(this)" data-id="${room.id}" data-name="${room.name}">
                    <p class="room_name"> ${room.name}</p>
                    <p class="room_count">(${room.users.length} active users)</p>
                    </section>
                `;

                $('.chats > .rooms').append(sectionRoom);

                let containerChats = $('.chats_wrapper');
                containerChats.find('.roomChat').hide();
                if (!checkIfElementExist(room.id, 'chat')) {
                    const chat =
                        `
                    <section class="chat" data-chat="${room.id}" data-chat-name="${room.name}">
                    <header>
                        ${room.name}
                    </header>
                    <main>
                    </main>
                    <footer>
                        <input type="text" placeholder="Type here your message" data-chat="${room.id}">
                        <a onclick="sendMessage(this)" data-chat="${room.id}">Send</a>
                    </footer>
                    </section>
                    `
        
                    containerChats.append(`<div data-id="${room.id}" class="roomChat">${chat}</div>`);
                    room.messages.forEach((m) => {
                        insertMessage(m);
                    });
                }
            } else {
                $(`section.room[data-id="${room.id}"] .room_count`).html(`(${room.users.length} active users)`);
            }
        });        
    });
}

function openChat(e) {
    var room = {
        id:  $(e).data('id'),
        name: $(e).data('name')
    }

    let chatsContainer = $('.chats_wrapper');
    chatsContainer.find('> div').hide();
    addToChatRoom(room.name);
    chatsContainer.find(`[data-id="${room.id}"]`).show();
    $('.rooms .room').removeClass('new_notification');
}

function insertMessage(chatData) {
    var sender = chatData.sender != null ? chatData.sender.name == window.chat.state.name ? 'me' : 'their' : 'room';
    const chatMessage = `
    <div class="message ${sender}">
        ${sender != 'room' ? `<b>${chatData.sender.name}</b>` : ''}
        <label>${chatData.message}<label>
        <span>${new Date(chatData.timestamp).toLocaleTimeString()}</span>
    </div>
    `;

    var chatContainer = $(`section[data-chat="${chatData.destination}"]`);
    chatContainer.find('main').append(chatMessage);
    if (!chatContainer.is(':visible')) {
        $(`.rooms .room[data-id="${chatData.destination}"]`).addClass('new_notification');
    }
}

function sendMessage(e) {
    var input = {
        destination: $(e).data('chat'),
        field: $(`input[data-chat="${$(e).data('chat')}"]`),
        message: $(`input[data-chat="${$(e).data('chat')}"]`).val()
    }

    window.chat.sendMessage(input);
}

function checkIfElementExist(id, data) {
    return $('section[data-' + data + '="' + id + '"]') && $('section[data-' + data + '="' + id + '"]').length > 0;
}