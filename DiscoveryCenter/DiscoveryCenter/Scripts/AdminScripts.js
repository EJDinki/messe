
$("#addItem").click(function () {
    $.ajax({
        url: this.href,
        cache: false,
        success: function (html) {
            html = FilterSomeeGarbage(html);
            $("#draggablePanelList").append(html);
            UpdatePanels();
            NoQuestions();
        }
    });
    return false;
});

$(document).on({
    mouseenter: function () {
        $(this).addClass("highlight");
    },
    mouseleave: function () {
        $(this).removeClass("highlight");
    }
}, ".panel-heading");



function AppendChoiceBox(ele, allowDelete) {
    myUrl = '@Url.Action("BlankChoiceBox", "Creation")?allowDelete=' + allowDelete;

    $.ajax({
        url: myUrl,
        cache: false,
        success: function (html) {
            html = FilterSomeeGarbage(html);
            $(ele).closest(".Choices").find(".AppendChoices").append(html);
        }
    });
    return false;
}

NoQuestions();
function NoQuestions() {
 
    if ($("#draggablePanelList").html().trim() == "") {
        $(tip).show();
        $(addItem).removeClass("btn-default");
        $(addItem).addClass("btn-success");

        $("#save").removeClass("btn-success");
        $("#save").addClass("btn-default");
    }
    else{
        $(tip).hide();
        $(addItem).removeClass("btn-success");
        $(addItem).addClass("btn-default");

        $("#save").removeClass("btn-default");
        $("#save").addClass("btn-success");
    }
}

function DeleteChoice(ele) {
    $(ele).closest(".Choice").remove();
}

function UpdateChoices(ele) {
    var choicesEle = $(ele).closest(".panel-body").find(".Choices").first();
    var choices = [];
    var images = [];
    var index = $(ele).val();


    $(choicesEle).find("input").each(function (index, elem) {
        choices.push($(elem).val());
    });

    $(choicesEle).find("select").each(function (index, elem) {

        if (elem.id.indexOf(".Img") > -1) {
            images.push($(elem).val());
        }
    });

    var joinedChoices = choices.join(";");
    var joinedImages = images.join(";");
    var myUrl = '@Url.Action("RefreshChoices", "Creation")?typeIndex=' + index + '&choices=' + joinedChoices + '&images=' + joinedImages;


    $.ajax({
        url: myUrl,
        cache: false,
        success: function (html) {
            html = FilterSomeeGarbage(html);
            $(choicesEle).html(html);
        }
    });
    return false;
}

function FilterSomeeGarbage(html) {
    return String(html).replace(/<!--[.\s\S\r\n]*/, "");
}

//---------------draggable panel---------------------
var panelList = $('#draggablePanelList');
panelList.sortable({
    // Only make the .panel-heading child elements support dragging.
    // Omit this to make then entire <li>...</li> draggable.
    handle: '.panel-heading',
    update: function (event, ui) { UpdatePanels(); },

});

function UpdatePanels() {
    $('.panel', panelList).each(function (index, elem) {
        var $listItem = $(elem),
            newIndex = $listItem.index();

        $(elem).find("Input.IndexInSurvey").each(function (index, elem) {
            $(elem).val(newIndex + 1);
        });

        $(elem).find("span.IndexInSurvey").each(function (index, elem) {
            $(elem).html(newIndex + 1);
        });

    });
}

function updateIconSrc(id, val) {
    document.getElementById(id).setAttribute('src', val.options[val.selectedIndex].value);
}

    
