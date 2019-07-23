console.log("JS is working");

var curVal = 1; //Keeps track of old position of slider

var totalItems = $('.news-item').length; //Get number of carousel items
var slider = document.getElementById("news-range");
$("#news-range").attr("max", totalItems - 1); //Set the max attribute of the html input tag

$('#news-range').on('change', function() { //This will execute when slider is moved
    var movedVal = 0;
    movedVal = $("#news-range").val();
        
    if(movedVal > curVal){
        for(var i = 0; i < movedVal - curVal; i++){ //If slider is moved to the right
            $("#next-button").click(); //Click next button
        }
    }
    else if(movedVal < curVal){ //If slider is moved to the right
        for(var i = 0; i < curVal - movedVal; i++){
            $("#prev-button").click(); //Click previous button
        }
    }
    curVal = movedVal; //Reset position of slider
  });

//Repopulates carousel when end is reached
$('#news-container').on('slide.bs.carousel', function (e) {

    var $e = $(e.relatedTarget); //Get element associated with click event
    var idx = $e.index(); //Get the number representing which photo news heading number we're at
    var itemsPerSlide = 3;
    
    if (idx >= totalItems-(itemsPerSlide-1)) { //Starts adding elements to slide only if there are already 4 present
        var it = itemsPerSlide - (totalItems - idx);
        for (var i=0; i <it; i++) {
            // append slides to end by constructing new element (eq)
            if (e.direction=="left") {
                $('.news-item').eq(i).appendTo('.carousel-inner');
            }
            else {
                $('.news-item').eq(0).appendTo('.carousel-inner');
            }
        }
    }
});

//Function to get csrf token
function getCookie(name) {
    var cookieValue = null;
    if (document.cookie && document.cookie != '') {
        var cookies = document.cookie.split(';');
        for (var i = 0; i < cookies.length; i++) {
            var cookie = jQuery.trim(cookies[i]);
            // Does this cookie string begin with the name we want?
            if (cookie.substring(0, name.length + 1) == (name + '=')) {
                cookieValue = decodeURIComponent(cookie.substring(name.length + 1));
                break;
            }
        }
    }
    return cookieValue;
}

//Post request for preference changes
function change_preferences() {
    
    //Check if slider is on or off
    var result;
    if($('#geo-button').prop("checked") == true) result = 1
    else result = 0
    
    var csrftoken = getCookie('csrftoken');

    //Make ajax post with the result of the button
    $.ajax({
        url: "create_post/", //Target python function is called "create_post"
        type: "POST",
        data:  {csrfmiddlewaretoken: csrftoken, text: result}, //Result of button
        error : function() { //Error message
            alert("Something went wrong with that AJAX call.");
        },
    });
};


$('.reply').click(function () {
    var id = $(this).attr("id");
    $("#response-"+id).toggle();
});

$('.recipe-display').click(function (){
    var id = $(this).attr("id");
    id = "#form-" + id;
    $(id).submit();
});
//Display details for movie that user clicks on
$(".movie-deets").click(function (){
    var id = $(this).attr("id");
    $(this).text($(this).text() == "See Details" ? "Hide Details" : "See Details"); //Toggle button text logic
    id = id.replace("button", "#details") //Get div id
    $(id).toggle(); //Toggle display of this div
});

//Updates page when user selects different number of movies
$(".display-item").on('click', function (){
    var option = $(this).text(); //Get value of selected option
    $("#display").val(option); //Store this value in input tag
    $("#display-num").submit(); //Submit value of input
});

//Updates page when user selects a genre to sort by
$(".sort-item").on('click', function (){
    var option = $(this).attr('id');
    $("#sort").val(option);
    $("#sort-form").submit();
});

//Updates page when user chooses to sort by year
$("#year-sort").on('click', function (){
    if( parseInt($("#sort-input").val()) == 1){
        $("#sort-input").val(0);
    }
    else {
        $("#sort-input").val(1);
    }
    $("#sort-year").submit();
});

//Stops playing youtube video when modal is closed
var src = $('.modal').find('iframe').attr('src'); //Get url for youtube video (needed later)
var close =  $('.modal').find("button"); //Get close button object

close.on('click', function(){ //Function to execute when modal closed
    $('.modal').find('iframe').attr('src', ''); //Removes url source for video, thus closing it
    $('.modal').find('iframe').attr('src', src); //Puts it back right away to be used for next time the modal is opened
});