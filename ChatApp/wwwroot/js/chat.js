﻿class Message {
    constructor(username, text, userid, touserid, when) {
        this.userName = username;
        this.text = text;
        
        this.userID = userid;
        this.toUserId = touserid;

        this.when = when;
    }
}

// userName is declared in razor page.
const username = userName;
const touserid = toUserId;
const userid = userId;
const textInput = document.getElementById('messageText');
const whenInput = document.getElementById('when');
const chat = document.getElementById('chat');
const messagesQueue = [];

document.getElementById('submitButton').addEventListener('click', () => {
    var currentdate = new Date();
    //when.innerHTML =
    //    (currentdate.getMonth() + 1) + "/"
    //    + currentdate.getDate() + "/"
    //    + currentdate.getFullYear() + " "
    //    + currentdate.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true })
});

function clearInputField() {
    messagesQueue.push(textInput.value);
    textInput.value = "";
}

function sendMessage() {
    let text = messagesQueue.shift() || "";
    
    if (text.trim() === "") return;
    
    let when = new Date();
    let message = new Message(username, text, userid, touserid);
    console.log(message)
    sendMessageToHub(message);
}

function addMessageToChat(message) {
    if (message.userID == userid || (message.toUserId == userid && message.userID == touserid)) {
        let isCurrentUserMessage = message.username === username;

        let container = document.createElement('div');
        container.className = isCurrentUserMessage ? "container darker" : "container";

        let sender = document.createElement('p');
        sender.className = "sender";
        sender.innerHTML = message.username;
        let text = document.createElement('p');
        text.innerHTML = message.text;

        let when = document.createElement('span');
        when.className = isCurrentUserMessage ? "time-left" : "time-right";
        var currentdate = new Date();
        when.innerHTML =
            (currentdate.getMonth() + 1) + "/"
            + currentdate.getDate() + "/"
            + currentdate.getFullYear() + " "
            + currentdate.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true })

        container.appendChild(sender);
        container.appendChild(text);
        container.appendChild(when);
        chat.appendChild(container);
    }
}
