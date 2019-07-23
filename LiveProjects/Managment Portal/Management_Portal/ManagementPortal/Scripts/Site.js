// Open and close the chat modal on click. Referenced in _Layout.cshtml
function openChat() {
    $("#chat-modal").modal()
};
function closeChat() {
    $("#chat-modal").fadeOut(170);
};

// Navigation link accordion effects
(function ($) {
    $(document).ready(function () {
        $('#menu > ul > li > a').click(function () {
            $('#menu li').removeClass('active');
            $(this).closest('li').addClass('active');
            var checkElement = $(this).next();
            if ((checkElement.is('ul')) && (checkElement.is(':visible'))) {
                $(this).closest('li').removeClass('active');
                checkElement.slideUp('normal');
            }
            if ((checkElement.is('ul')) && (!checkElement.is(':visible'))) {
                $('#menu ul ul:visible').slideUp('normal');
                checkElement.slideDown('normal');
            }
        });
    });
})(jQuery);

// Collapse and expand navigation
function openNav() {
    document.getElementById("navigation").style.width = "150px"; // Expands the nav to its full size
    document.getElementById("main").style.marginLeft = "150px";  // Add margin to the page body so the nav doesn't cover content  
};
function closeNav() {
    document.getElementById("navigation").style.width = "40px"; // Hides all but the nav icons
    document.getElementById("main").style.marginLeft = "40px";
};

// Hamburger menu icon
$(document).ready(function () {

    $('.second-button').on('click', function () {

        $('.animated-icon').toggleClass('open');
    });

});

//Datepicker Popup
$(document).ready(function () {
    $('.datepicker').datepicker({

    });

});

//Password Validation Box
$("input#Password").on("focus keyup", function () {
    var score = 0;
    console.log(score);
    var a = $(this).val();
    var desc = new Array();

    // strength desc
    desc[0] = "Too short";
    desc[1] = "Weak";
    desc[2] = "Good";
    desc[3] = "Strong";
    desc[4] = "Best";

    $("#pwd_strength_wrap").fadeIn(400);

    // password length
    if (a.length >= 6) {
        $("#length").removeClass("invalid").addClass("valid");
        score++;
    } else {
        $("#length").removeClass("valid").addClass("invalid");
    }

    // at least 1 digit in password
    if (a.match(/\d/)) {
        $("#pnum").removeClass("invalid").addClass("valid");
        score++;
    } else {
        $("#pnum").removeClass("valid").addClass("invalid");
    }

    // at least 1 capital & lower letter in password
    if (a.match(/[A-Z]/) && a.match(/[a-z]/)) {
        $("#capital").removeClass("invalid").addClass("valid");
        score++;
    } else {
        $("#capital").removeClass("valid").addClass("invalid");
    }

    // at least 1 special character in password {
    if (a.match(/.[!,@,#,$,%,^,&,*,?,_,~,-,(,)]/)) {
        $("#spchar").removeClass("invalid").addClass("valid");
        score++;
    } else {
        $("#spchar").removeClass("valid").addClass("invalid");
    }


    if (a.length > 0) {
        //show strength text
        $("#passwordDescription").text(desc[score]);
        // show indicator
        $("#passwordStrength").removeClass().addClass("strength" + score);
    } else {
        $("#passwordDescription").text("Password not entered");
        $("#passwordStrength").removeClass().addClass("strength" + score);
    }
});

$("input#Password").blur(function () {
    $("#pwd_strength_wrap").fadeOut(400);
});