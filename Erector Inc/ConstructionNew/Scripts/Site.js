$(document).ready(function () {

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

// Takes a given string and sanitizes it. Ex: '>' characters render as "&gt;"
// This is extremely important in preventing script injection attacks.
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}

// Sets up the Manage Chat view's Javascript. Only runs on that page.
function InitiateChatManagementPage() {
    // Function which opens the edit message dialog when an edit button is clicked.
    $('.EditButton').click(function () {
        // Get the chat's ID from the button
        let messageId = $(this).val();

        // Get the important display data
        let tableRow = $(this).parents("tr");
        let messageSender = $(tableRow).children(".messageSender").text().trim();
        let messageContent = $(tableRow).children(".messageContent").text().trim();

        // Fill that data into the edit form
        $('#EditForm-MessageID').val(messageId);
        $('#EditForm-SenderDisplay').val(messageSender);
        $('#EditForm-Message').val(messageContent);

        // Reveal the edit form.
        $('#EditMessageModal').modal();
    });

    // Function which enables the delete-messages button only when at least one message is
    // selected for delete. 
    $('.DeleteCheckbox').click(function () {
        let selected = $('input:checked');
        let disable = (selected.length == 0);
        $('.DeleteButton').prop('disabled', disable);
    });

    // Function which, when the delete button is clicked, 
    // uses the ChatHub server to send a delete request for the selected chat items.
    $('.DeleteButton').click(function () {
        let chat = $.connection.chatHub;

        // Gather a list of all IDs for selected messages as strings
        let messageIds = [];
        $('input:checked').each(function () {
            messageIds.push($(this).val());
        });

        // Open a confirmation modal?
        //   If so, the below stuff needs to be put in a DeleteConfirmButton.click function.

        // Issue the delete request
        chat.server.deleteMessages(messageIds);

        // Update the manage chats view with the items we've deleted.
        // This is technically a lie; the DB probably hasn't finished updating by this point.
        messageIds.forEach(function (id) {
            $('button[value="' + id + '"]').parents("tr").remove();
        });
    });

    // Function which focuses the edit form's message input box when shown.
    $('#EditMessageModal').on('shown.bs.modal', function () {
        $('#EditForm-Message').focus();
    });

    // Function which saves the message's new contents when the edit form's save button is clicked.
    $('#EditForm-SaveButton').on('click', function () {
        // Gather important details.
        let chat = $.connection.chatHub;
        let messageId = $('#EditForm-MessageID').val();
        let messageContent = $('#EditForm-Message').val();

        // Request the server to make the change.
        chat.server.editMessage(messageId, messageContent);

        // Update the view with our changes.
        // This finds the edit button with the right ID, then finds the adjacent table column for message text.
        // This is also technically a lie; the DB probably hasn't finished updating by this point.
        $('button[value="' + messageId + '"]').parents("tr").children(".messageContent").text(messageContent);

        // Hide the edit form.
        $('#EditMessageModal').modal('hide');
    });

    // Function which 'clicks' the edit form's save button when enter is pressed.
    $('#EditForm-Message').on('keypress', function (event) {
        const EnterKeyCode = 13;
        if (event.which == EnterKeyCode && !event.shiftKey)
            $('#EditForm-SaveButton').click();
    });

    // Function which hides the edit form modal when the cancel button is clicked.
    $('#EditForm-CancelButton').on('click', function () {
        $('#EditMessageModal').modal('hide');
    });
};


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
    $('.datepicker').datepicker; //Initialise any date pickers
});

//Uncomment to test ShiftTime checker method

//$(document).ready(function () {
//    $.post("/Jobs/HasNonDefaultShiftTime");
//});

