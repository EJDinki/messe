$(document).ready(function () {
    clicked = false;
    $(".draggable").draggable(
                {
                    appendTo: 'body',
                    helper: 'clone',
                    axis: 'x',
                    scroll: false,
                    revert: 'invalid',
                    snap: 'dropable',
                    snapMode: 'inner'
                    /*, drag: function (event, ui) {
                        $(ui).scrollParent().scroll(10, 0);
                        console.log(event.originalEvent);
                    }*/
                });

    $(".draggable").on({
        'mousemove': function (e) {
            clicked && updateScrollPos(e, this);
        },
        'mousedown': function (e) {
            clicked = true;
            clickY = e.pageY;
        },
        'mouseup': function () {
            clicked = false;
        }
    });

    var alert = $("#alert").first();

    var updateScrollPos = function (e, ui) {
        $($(ui).scrollParent()).scrollTop($(ui).scrollParent().scrollTop() + (clickY - e.pageY));
    }

    $(".dropable").droppable({
        accept: ".draggable",
        activeClass: "ui-state-hover",
        hoverClass: "ui-state-active",
        drop: function (event, ui) {

            var numChildren = $(this).children().length;
            var maxChildren = parseInt($($(this).siblings('input[id*="MaxChoices"]')[0]).val());

            if (numChildren >= maxChildren) {
                $(alert).html("Cannot excede " + maxChildren + " selections.");
                $(alert).show();
                clicked = false;
                setTimeout(function () {
                    $(alert).hide();
                }, 3000);

                return;
            }
            $("#exhibitA_" + ui.draggable.context.id).val($("#exhibitA_" + ui.draggable.context.id).val() + ui.draggable.context.innerText + ";");
            $(ui.draggable).detach().css({ top: 0, left: 0 }).appendTo(this);
            clicked = false;
        }
    });

    $(".dropable-revert").droppable({
        accept: ".draggable",
        activeClass: "ui-state-hover",
        hoverClass: "ui-state-active",
        drop: function (event, ui) {
            var toRemove = ui.draggable.context.innerText + ";";
            var curVal = $("#exhibitA_" + ui.draggable.context.id).val();
            curVal = curVal.replace(toRemove, "");
            $("#exhibitA_" + ui.draggable.context.id).val(curVal);
            $(ui.draggable).detach().css({ top: 0, left: 0 }).appendTo(this);
        }
    });
    
});
