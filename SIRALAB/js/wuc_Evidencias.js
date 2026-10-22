
function cargarEvidencias() {

    $("#reviews-slider").flexslider({
        animation: "slide",
        animationLoop: false,
        itemWidth: 600,
        itemMargin: 0,
        minItems: 5,
        maxItems: 10,
        controlNav: false,
        move: 0
    });

    $(".slides").css("padding-left", "60px");

    if ($("#mdlArchivosAnalisis").size() > 0) {
        $("#mdlArchivosAnalisis").modal("hide");
        $('body').removeClass('modal-open');
        $(".modal-backdrop.in").remove();
    }
}

function cargarEvidencias_pop() {

    $("#reviews-slider_pop").flexslider({
        animation: "slide",
        animationLoop: false,
        itemWidth: 600,
        itemMargin: 0,
        minItems: 5,
        maxItems: 10,
        controlNav: false,
        move: 0
    });

    $(".slides").css("padding-left", "60px");
}

;

