$(".slider").labeledslider({ min: 1, max: 5, tickInterval: 1, value: 3, stop: sliderChanged })
$(".spinner").spinner({min:2, max: 20, value:5}).val(5);


function sliderChanged(event, ui)
{
    var id = this.id.replace("Slider", "Answer");
    $("#" + id).val(ui.value);
}

//------limiting checkbox selection-------------------
var numClicks = 0;//makes it so that the alert shows for 5 secs after click no matter what
var alert = $("#alert").first();

function limitChecked(ele, max) {
    var numChecked = 0;
    var checkboxes = $(ele).closest(".checkboxes").find("input[type=checkbox]").each(
        function (index, ele) {
            if (ele.checked)
                numChecked++;
        });

    if (max > 0 && numChecked > max) {
        numClicks++;
        $(ele).attr('checked', false);

        $(alert).html("Cannot excede " + max + " selections.");
        $(alert).show();

        setTimeout(function () {
            numClicks--;
            if (numClicks == 0)
                $(alert).hide();
        }, 5000);
    }
}

//Code for autoredirect after a certain time limit
var idleTime = 0;
$(document).ready(function () {
    var idleInterval = setInterval(timerIncrement, 60000);

    //checks for mouse input
    $(this).mousemove(function (e) {
        idleTime = 0;
    });
    //checks for keyboard input
    $(this).keypress(function (e) {
        idleTime = 0;
    });
});

//Increments the timer and if the value is greater than 3, submits the survey
function timerIncrement() {
    idleTime = idleTime + 1;

    if (idleTime > 2) {
        document.forms[0].submit();
    }
}


//-----------enable/disable carousel controls-------------------------------
var numSlides = 1;//init, but redclare value in the view

function leftCarouselClicked() {
    var slideID = $('div.item.active').attr("id");

    if (slideID == "slide_1") {
        $('a.left').hide();
        $('a.right.carousel-control').show();
    }
    else {
        $('a.carousel-control').show();
    }
}

function rightCarouselClicked() {
    var slideID = $('div.item.active').attr("id");

    if (slideID == "slide_" + (numSlides - 1)) {
        $('a.right').hide();
        $('a.left.carousel-control').show();
    }
    else {
        $('a.carousel-control').show();
    }
}

$('body').on('click', 'a.left.carousel-control', leftCarouselClicked);
$('body').on('click', 'a.right.carousel-control', rightCarouselClicked);