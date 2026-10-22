function ModoConsulta() {
    $(document).ready(function () {
        $(".Accion").css("display", "none");
        $("input:radio").attr("disabled", true);
        $("input:checkbox").attr("disabled", true);
        $('[name *= ImageButtonFileUpload]').attr("disabled", true);

    }
    );



}



  