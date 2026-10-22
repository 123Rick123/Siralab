$(document).on("ready", inicio);
function inicio() {
    $('[id*=txtUsuario]').keypress(validarUsuario);
    $('[id*=txtUsuario]').keyup(validarUsuario);
    $('[id*=txtUsuario]').keydown(validarUsuario);
    $('[id*=txtUsuario]').blur(validarUsuario);
    $('[id*=txtPassword]').keypress(validarPassword);
    $('[id*=txtPassword]').keyup(validarPassword);
    $('[id*=txtPassword]').keydown(validarPassword);
    $('[id*=txtPassword]').blur(validarPassword);
    $('[id*=lnkbAceptar]').click(validarCajas);
    $('[id*=txtCorreo]').blur(validarCorreo);
     $('[id*=lnkbEnviar]').click(validarCorreo);
}

function validarUsuario() {
    var text = $('[id*=txtUsuario]').val();
    if (text == "") {
        $('[id*=txtUsuario]').parent().attr("class", "form-group has-error");
        $('[id*=txtUsuario]').attr("class", "form-control form-control-error")
        $('[id*=txtUsuario]').parent().children("span").text("Este campo es obligatorio").show();
        $('[id*=txtUsuario]').parent().children("span").attr("class","form-text form-text-error")
        $('[id*=txtUsuario]').parent().children("label").children("samp").attr("class", "form-text form-text-error");
    }
    else {
        $('[id*=txtUsuario]').parent().attr("class", "form-group");
        $('[id*=txtUsuario]').parent().children("span").hide();
        $('[id*=txtUsuario]').parent().children("label").children("samp").attr("class", "form-text");
        $('[id*=txtUsuario]').attr("class", "form-control");
    }
}
function validarPassword() {
    var text = $('[id*=txtPassword]').val();
    if (text == "") {
        $('[id*=txtPassword]').parent().attr("class", "form-group has-error");
        $('[id*=txtPassword]').attr("class", "form-control form-control-error")
        $('[id*=txtPassword]').parent().children("span").text("Este campo es obligatorio").show();
        $('[id*=txtPassword]').parent().children("label").children("samp").attr("class", "form-text form-text-error");
    }
    else {
        $('[id*=txtPassword]').parent().attr("class", "form-group");
        $('[id*=txtPassword]').parent().children("span").hide();
        $('[id*=txtPassword]').parent().children("label").children("samp").attr("class", "form-text");
        $('[id*=txtPassword]').attr("class", "form-control");
    }

}

function validarCajas() {
    validarUsuario();
    validarPassword();  
    var hijos = $('div.form-group.has-error');
    var IsValido = false;
    if (hijos.length == 0) {
        IsValido = true;
    }
    return IsValido;
}


function validarCorreo() {
    var text = $('[id*=txtCorreo]').val();
    if (text == "") {


           
            console.log("Si llegue");
        $('[id*=txtCorreo]').parent().attr("class", "form-group has-error");
        $('[id*=txtCorreo]').attr("class", "form-control form-control-error")
        $('[id*=txtCorreo]').parent().children("span").text("Este campo es obligatorio").show();
        $('[id*=txtCorreo]').parent().children("label").children("samp").attr("class", "form-text form-text-error");

    }

}