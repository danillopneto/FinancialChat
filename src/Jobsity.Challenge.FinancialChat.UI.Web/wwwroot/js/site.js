window.financialchat = {};

window.financialchat = function ($el, model) {
    this.$el = $el;
    this.$el.data('FinancialChat', this);
    this.model = model;

    this.$elAddNewGroup = this.$el.find('#addNewGroup');

    this._initialize();
}

window.financialchat.prototype = {
    _initialize: function () {
        this._addEvents();

        this.chat = this._createChatController();
        this.chat.loadUser();
    },

    _addEvents: function () {
        var _this = this;

        $(document).on('keypress', '.message_input', function (event) {
            var keycode = (event.keyCode ? event.keyCode : event.which);
            if (keycode == '13') {
                $(`[data-chat="${$(event.currentTarget).data('chat')}"] + .send_button`).click();
            }
        });

        $(document).on('click', '.room-selection', function (e) {
            _this._openChat(e.currentTarget);
        });

        this.$elAddNewGroup.on('click', function () {
            _this._createChatRoom();
        })
    },

    _createChatController: function () {
        var _this = this;

        var user = {
            name: null,
            dtConnection: null
        }

        return {
            state: user,
            connection: null,
            loadUser: function () {
                this.state.name = _this.model;
                this.state.dtConnection = _this._getCurrentDateWithoutTimezone();
                this.connectUserToChat();
            },
            connectUserToChat: function () {
                _this._startConnection(this);
            },
            sendMessage: function (to) {
                var chatMessage = {
                    sender: this.state,
                    message: to.message,
                    destination: to.destination,
                    timestamp: _this._getCurrentDateWithoutTimezone()
                };

                this.connection.invoke("SendMessage", (chatMessage))
                    .catch(err => console.log(x = err));

                to.field.val('').focus();
            },

            onReceiveMessage: function () {
                this.connection.on("Receive", (chatData) => {
                    _this._insertMessage(chatData);
                });

                this.connection.on("CommandReceived", (chatData) => {
                    _this._insertMessage(chatData);
                });
            }
        };
    },

    _startConnection: async function (chat) {
        var _this = this;

        try {
            var signalRUrl = $('#SignalRUrl').val();
            _this.chat.connection = new signalR.HubConnectionBuilder().withUrl(`${signalRUrl}chat?user=` + JSON.stringify(_this.chat.state)).build();
            await _this.chat.connection.start();

            $('.info_chat').show();

            _this.chat.connection.on('userData', (user) => {
                _this.chat.state = user;
            });

            _this._loadRooms(_this.chat.connection);

            _this.chat.connection.onclose(async () => {
                await _this._startConnection(chat);
            });

            _this.chat.onReceiveMessage();
        } catch (err) {
            console.log(err);
            setTimeout(() => _this._startConnection(_this.chat), 5000);
        }
    },

    _createChatRoom: async function () {
        let chatRoomName = '';
        while (chatRoomName == '') {
            chatRoomName = prompt('Type the name of the chatroom', '');
        }

        this._addToChatRoom(chatRoomName);
    },

    _addToChatRoom: async function (chatRoomName) {
        this.chat.connection.invoke("AddToChatRoom", (chatRoomName))
            .catch(err => console.log(x = err));
    },

    _loadRooms: async function (connection) {
        var _this = this;

        connection.on('chat', (rooms, user) => {
            rooms.forEach((room) => {
                if (!_this._checkIfElementExist(room.id, 'id')) {
                    let sectionRoom = `
                        <section class="room-selection room box_shadow_0" data-id="${room.id}" data-name="${room.name}">
                        <p class="room_name"> ${room.name}</p>
                        <p class="room_count">(${room.users.length} users)</p>
                        </section>
                    `;

                    $('.chats > .rooms').append(sectionRoom);

                    let containerChats = $('.chats_wrapper');
                    if (!_this._checkIfElementExist(room.id, 'chat')) {
                        const chat =
                            `
                        <section class="chat" data-chat="${room.id}" data-chat-name="${room.name}">
                        <header>
                            ${room.name}
                        </header>
                        <main>
                        </main>
                        <footer>
                            <input type="text" placeholder="Type here your message" class="message_input" data-chat="${room.id}" >
                            <a class="send_button" data-chat="${room.id}">Send</a>
                        </footer>
                        </section>
                        `

                        containerChats.append(`<div data-id="${room.id}" class="roomChat" style="display:none;">${chat}</div>`);
                        room.messages.forEach((m) => {
                            _this._insertMessage(m);
                        });
                        containerChats.find(`.roomChat[data-id="${room.id}"] .send_button`).on('click', function (e) {
                            _this._sendMessage(e.currentTarget);
                        });
                    }
                } else {
                    $(`section.room[data-id="${room.id}"] .room_count`).html(`(${room.users.length} users)`);
                }
            });
        });
    },

    _openChat: function (e) {
        var room = {
            id: $(e).data('id'),
            name: $(e).data('name')
        }

        if (room.name == null || room.name == '') {
            return;
        }

        let chatsContainer = $('.chats_wrapper');
        chatsContainer.find('> div').hide();
        this._addToChatRoom(room.name);
        chatsContainer.find(`[data-id="${room.id}"]`).show();
        $(`.rooms .room[data-id=${room.id}]`).removeClass('new_notification');
        this._goToLastMessage(room.id);
    },

    _insertMessage: function (chatData) {
        var sender = chatData.sender != null ? chatData.sender.name == this.chat.state.name ? 'me' : 'their' : 'room';
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
        } else {
            this._goToLastMessage(chatData.destination);
        }
    },

    _goToLastMessage: function (chatId) {
        var lastMessage = $(`[data-id="${chatId}"] .chat main .message`).last();
        if (lastMessage.length) {
            lastMessage[0].scrollIntoView();
        }
    },

    _sendMessage: function (e) {
        var input = {
            destination: $(e).data('chat'),
            field: $(`input[data-chat="${$(e).data('chat')}"]`),
            message: $(`input[data-chat="${$(e).data('chat')}"]`).val()
        }

        this.chat.sendMessage(input);
    },

    _checkIfElementExist: (id, data) => $('section[data-' + data + '="' + id + '"]') && $('section[data-' + data + '="' + id + '"]').length > 0,

    _getCurrentDateWithoutTimezone: () => new Date().toISOString().slice(0, -1)
};


