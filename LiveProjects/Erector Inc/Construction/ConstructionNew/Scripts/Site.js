$(document).ready(function () {

    console.log("Hi there, I'm JavaScript :)");

    //Changes opacity of navbar when scrolling
    $(window).scroll(function () {

        var elem = document.getElementsByClassName("navbar")[0]; //Get's navbar Id
        if (($(this)).scrollTop() >= 30) { //If distance from top is greater than 30px
            elem.style.transition = "opacity 0.2s linear 0s"; //Call transition function on opacity
            elem.style.opacity = 0.7; //Set target opacity
        }
        else if (($(this)).scrollTop() < 30) { //If distance from top is less than 30px
            elem.style.transition = "opacity 0.2s linear 0s";
            elem.style.opacity = 1;
        }
    });
});

// navbar slidedown from top on page load
$(window).on("load", function () {
    $(".navbar").delay(1000).slideDown(1000); 
});

// navbar drop down animation
$(document).ready(function () {
    $(".dropdown").hover(
        function () {
            $('.dropdown-menu', this).not('.in .dropdown-menu').stop(true, true).slideDown("fast");
            $(this).toggleClass('open');
        },
        function () {
            $('.dropdown-menu', this).not('.in .dropdown-menu').stop(true, true).slideUp("fast");
            $(this).toggleClass('open');
        }
    );
});

// Called by the chat layout when it is created.
// Assigns event methods which create/remove a cookie remembering the chat box's collapsed state.
// Also recreates the previous session's state based on present chat box cookies.
function keepChatState(nameOfDiv, nameOfHeader) {
    // When the chatbox body is shown, remove the cookie.
    $("#" + nameOfDiv).on('shown.bs.collapse', function () {
        $.removeCookie(nameOfDiv);
    });
    // When the chatbox body is collapsed, save a cookie whose presence auto-hides it next session.
    $("#" + nameOfDiv).on('hidden.bs.collapse', function () {
        $.cookie(nameOfDiv, "true"); // this is a cookie.
    });

    // Confirm the existence of session cookies
    var showDiv = $.cookie(nameOfDiv);
    var chatFormDiv = $.cookie("chatForm");

    // If the chatbox body cookie is not present, uncollapse the chat dialog instantly.
    if (showDiv == null) {
        $("#" + nameOfDiv).addClass("in");                     // The div to show - 'in' means 'display uncollapsed' to something
        $("#" + nameOfHeader).removeClass("collapsed");        // The header to stylize as expanded - 'collapsed' is a flag
    }
    // If chatform cookie is not present, auto-reveal the chat box instantly.
    if (chatFormDiv == null) {
        $('#chatForm').show();
    }
};


function closeChat() {
    $.cookie("chatForm", "true");   // Adds cookie to maintain state through separate sessions
    $("#chatForm").fadeOut(150);    // Makes chat invisible
};

function openChat() {
    $.removeCookie("chatForm");     // Removes cookie that is added when chat is opened - returns state to default behavior
    $("#chatForm").fadeIn(150);     // Makes chat visible
};

// JQuery for creating a ChatBox connection - this runs when this script (Site.js) is loaded
// Creates a connection to the chathub server, attaches some Javascript methods to it, and does some light setup.
$(function () {
    const EnterKeyCode = 13;
    
    // Reference the auto-generated proxy for the hub.
    let chat = $.connection.chatHub;

    // Function to clear a client's chat dialog of all messages
    chat.client.refreshChat = function () {
        $("#discussion > li").remove();
    };

    // Function to scroll the chatbox's view to the bottom of the message list.
    chat.client.scrollToBottom = function () {
        let element = document.getElementById("discussion");
        element.scrollTop = element.scrollHeight - element.clientHeight;
    }

    // Function returns true if the chatbox's view is currently at the bottom of the chat window (viewing the newest chats).
    chat.client.viewingNewestChats = function () {
        let element = document.getElementById("discussion");
        let value = element.scrollHeight - element.scrollTop - element.clientHeight;
        return Math.floor(value) === 0;  // floor() is important because Chrome has some weirdness with element.scrollTop
    }

    // Function to append one new message to the bottom of the chat box's message list.
    chat.client.addNewMessageToPage = function (time, name, message) {
        let trackNewChats = chat.client.viewingNewestChats();

        // Targets the unordered list of chats and appends a new one.
        $('#discussion').append('<li><strong>' + htmlEncode(name)       // htmlEncode() prevents script-injection attacks
            + '</strong>&nbsp;&nbsp;' + htmlEncode(time) + "</li><li>" + htmlEncode(message) + '</li>');

        // If the user is viewing the most recent chats, keep it that way.
        if (trackNewChats) {
            chat.client.scrollToBottom();
        }
    };

    // When the 'Send' button is clicked: submit new chat and clear the text input box.
    $('#sendmessage').click(function () {
        // Get the user's message and clear the input box.
        let name = $('#displayname').val();
        let message = $('#messageBox').val();
        $('#messageBox').val('');

        // If the submitted chat message is only whitespace, discard it - quit this function early.
        if (message.trim() === "")
            return;

        // Ask the chathub server to submit the message and refresh the view.
        chat.server.send(name, message);

        // Focus the text box; let the user immediately start typing a new message.
        $('#messageBox').focus();
    });

    // When Enter is pressed inside the chatbox's textbox, 'click' the 'Send' button.
    $('#messageBox').keypress(function (event) {
        if (event.which == EnterKeyCode && !event.shiftKey) {
            $('#sendmessage').click();
            event.preventDefault();     // Prevents the enter key from typing a character.
        }
    });

    // Start the connection. This is where anything that needs to happen on initialization goes.
    // Leave this at the bottom of this function; chat.server.getMessages() depends on the Javascript functions defined above.
    // Unless leaving it at the bottom isn't important.
    $.connection.hub.start().done(function () {
        // Get all discussion messages.
        chat.server.getMessages();
    });
});

// Takes a given string and sanitizes it. Ex: '>' characters render as "&gt;"
// This is extremely important in preventing script injection attacks.
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}


// Navbar Border after Scrolling

$(window).scroll(function () {
    if ($(window).width() < 600) {  //skips making the navbar transparent if on mobile
        return;
    }
    //when you scroll down about 35% of the screen the navbar transitions
    if ($(window).scrollTop() >= ($(window).height() * .35)) {
        $('.navbar').css('background', '#e3e3e3');
        $('.navbar li a').css('color', '#000');
        $('#nav-brand').css('color', '#000');
        
    } else {
        $('.navbar').css('background', 'transparent');
        $('.navbar').css('border-color', 'transparent');
        $('.navbar li a').css('color', '#FFF');
        $('.dropdown li a').css('color', '#000');
        $('#nav-brand').css('color', '#FFF');
    }
});

 //Function to call News Modal on Click
$(function () {
    $(".OpenDialog").click(function (e) {
        $("#myModal #id").val($(this).data("id"));
        $("#myModal").modal('show');
    });
});

//on load welcome modal
$(window).on('load', function () {
    //checks if the modal has been shown
        if (sessionStorage.getItem('popState') != 'shown') {
            $('#myModal1').modal('show');
            if (window.location.pathname == "/Dashboard") {
                //if the page is the dashboard index the modal is set to shown so that it will only show once
                sessionStorage.setItem('popState', 'shown');
            }
        }
});

//password checker
$(window).on('load', function () {
    $("#passwordInput").keyup(function () {
        // set password variable
        var pswd = $(this).val();
        var pattern = new RegExp(/[~`!#$%\^&*+=\-\[\]\\';,/{}|\\":<>\?@()]/);
        //validate the length
        if (pswd.length < 8) {
            $('#length').removeClass('valid').addClass('invalid');
        } else {
            $('#length').removeClass('invalid').addClass('valid');
        }
        //validate letter
        if (pswd.match(/[A-z]/)) {
            $('#letter').removeClass('invalid').addClass('valid');
        } else {
            $('#letter').removeClass('valid').addClass('invalid');
        }

        //validate capital letter
        if (pswd.match(/[A-Z]/)) {
            $('#capital').removeClass('invalid').addClass('valid');
        } else {
            $('#capital').removeClass('valid').addClass('invalid');
        }

        //validate number
        if (pswd.match(/\d/)) {
            $('#number').removeClass('invalid').addClass('valid');
        } else {
            $('#number').removeClass('valid').addClass('invalid');
        }

        //validate special character
        if (pattern.test(pswd)) {
            $('#specialCharacter').removeClass('invalid').addClass('valid');
        }
        else {
            $('#specialCharacter').removeClass('valid').addClass('invalid');
        }
    });
});

$(window).on('load', function () {
    $("#passwordInput").focus(function () {
        $('#pswd_info').show();
    });
});

$(window).on('load', function () {
    $("#passwordInput").blur(function () {
        $('#pswd_info').hide();
    });
});
//date picker script
$(function () {
    $('.datepicker').datepicker(); //Initialise any date pickers
});

//Uncomment to test ShiftTime checker method

//$(document).ready(function () {
//    $.post("/Jobs/HasNonDefaultShiftTime");
//});





  

