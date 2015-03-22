$(".slider").labeledslider({ min: 1, max: 5, tickInterval: 1, value: 3, stop: sliderChanged})


function sliderChanged(event, ui)
{
    var id = this.id.replace("Slider", "Answer");
    console.log($("#" + id));
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
    if (idleTime > 3) {
        document.getElementById("CompleteSurvey").click();

    }
}


function confirmExit() {
    if(confirm("Are you sure you wish to exit without submitting your answers?"))
        window.location.replace("@Url.Action("Index", "Home", new {id=Model.SurveyId},Request.Url.Scheme)");
}

var numSlides = @Model.QuestionModels.Count -1;

$('body').on('click', 'a.left.carousel-control', function (e) {
    var slideID = $('div.item.active').attr("id");
    console.log(slideID);

    if (slideID == "slide_1") {
        $('a.left').addClass("disabled");
    }

    $('a.right').removeClass("disabled");
});

$('body').on('click', 'a.right.carousel-control', function (e) {
    var slideID = $('div.item.active').attr("id");
    console.log(slideID);

    if (slideID == "slide_" + (numSlides - 1)) {
        $('a.right').addClass("disabled");
    }

    $('a.left').removeClass("disabled");
});

$('body').on('click', 'a.disabled', function (e) {
    e.preventDefault();
    return false;
});