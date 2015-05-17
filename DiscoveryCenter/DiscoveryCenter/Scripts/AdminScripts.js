
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
    myUrl = "/Creation/BlankChoiceBox?allowDelete=" + allowDelete;

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

function NoQuestions() {
 try{
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
    catch(e){
        console.log("NoQuestions couldn't find a draggablePanelList.");
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
    var myUrl = '/Creation/RefreshChoices?typeIndex=' + index + '&choices=' + joinedChoices + '&images=' + joinedImages;


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

    
$(".indexSelectableList").selectable({
    stop: function (event, ui) {
        $(event.target).children('.ui-selected').not(':first').removeClass('ui-selected');
        refreshButtons(event.target);
    }
});

$(document).ready(refreshButtons(null));

function refreshButtons(target, callback) {
    if (target != null)
        var selected = $(target).children('.ui-selected').first();
    else
        var selected = null;

    if (selected != null) {
        var id = $(selected).find("input.id").first().val();
        $(".selection-buttons a, .selection-buttons button").each(function (index, ele) {
            $(ele).attr('href', function (i, a) {

                if (/id=[0-9]*&/.test(a))
                    return a.replace(/id=[0-9]*&/ig, "id=" + id);
                else {
                    a = a.replace(/\?.*$/, "");
                    return a + "?id=" + id;
                }
            });
            $(ele).removeAttr('disabled');
        });
        if(callback != undefined && callback != null)
            callback(id);
    }
    else {
        $(".selection-buttons a, .selection-buttons button").each(function (index, ele) {
            $(ele).attr("disabled", "disabled");
        });
    }

}

$(".confirmDelete").on("click", function () {
    var url = $(".confirmDelete").attr("href");
    $.ajax({
        url: url,
        method: 'GET'
    }).success(function (data) {
        $("#deleteDialog .modal-dialog .modal-content").html(data);
    })
});
