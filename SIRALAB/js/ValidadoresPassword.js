$(document).on("ready", inicio);
function inicio()
{
    $('[id*=txtEmail]').keypress(validarCorreo);
    $('[id*=txtEmail]').keyup(validarCorreo);
    $('[id*=txtEmail]').keydown(validarCorreo);
    $('[id*=txtEmail]').blur(validarCorreo);

    $('[id*=txtPassword]').keypress(validarPasswordActual);
    $('[id*=txtPassword]').keyup(validarPasswordActual);
    $('[id*=txtPassword]').keydown(validarPasswordActual);
    $('[id*=txtPassword]').blur(validarPasswordActual);


    $('[id*=txtPasswordNuevo]').keypress(validarPasswordNuevo);
    $('[id*=txtPasswordNuevo]').keyup(validarPasswordNuevo);
    $('[id*=txtPasswordNuevo]').keydown(validarPasswordNuevo);
    $('[id*=txtPasswordNuevo]').blur(validarPasswordNuevo);

    $('[id*=txtPasswordRepite]').keypress(validarPasswordRepite);
    $('[id*=txtPasswordRepite]').keyup(validarPasswordRepite);
    $('[id*=txtPasswordRepite]').keydown(validarPasswordRepite);
    $('[id*=txtPasswordRepite]').blur(validarPasswordRepite);
    $('[id*=lnkbGuardar]').click(validarCajas);

}
function validarCorreo()
{
    var texto = $('[id*=txtEmail]').val();
    var val = /^[a-zA-Z0-9\._-]+@[a-zA-Z0-9-]{2,}[.][a-zA-Z]{2,4}$/;
    if (texto != "") {
        if (!val.test(texto)) {
            $('[id*=txtEmail]').parent().attr("class", "form-group has-error");
            $('[id*=txtEmail]').attr("class", "form-control form-control-error");
            $('[id*=txtEmail]').parent().children("span").text("*Correo no valido").show();
            $('[id*=txtEmail]').parent().children("label").children("samp").attr("class", "form-text form-text-error");

        }
        else {
            $('[id*=txtEmail]').parent().attr("class", "form-group");
            $('[id*=txtEmail]').parent().children("span").hide();
            $('[id*=txtEmail]').parent().children("label").children("samp").attr("class", "form-text");
            $('[id*=txtEmail]').attr("class", "form-control");
        }
    }
    else
    {
        $('[id*=txtEmail]').parent().attr("class", "form-group has-error");
        $('[id*=txtEmail]').parent().children("span").text("Este campo es obligatorio").show();
    }
   
}
function validarPasswordActual() {
    var text = $('[id*=txtPassword]').val();
    if(text=="")
    {
        $('[id*=txtPassword]').parent().attr("class", "form-group has-error");
        $('[id*=txtPassword]').attr("class", "form-control form-control-error")
        $('[id*=txtPassword]').parent().children("span").text("Este campo es obligatorio ").show();
        $('[id*=txtPassword]').parent().children("label").children("samp").attr("class", "form-text form-text-error");
    }
    else {
        $('[id*=txtPassword]').parent().attr("class", "form-group");
        $('[id*=txtPassword]').parent().children("span").hide();
    }
}
function validarPasswordNuevo() {
    var text = $('[id*=txtPasswordNuevo]').val();
    if (text == "") {
        $('[id*=txtPasswordNuevo]').parent().attr("class", "form-group has-error");
        $('[id*=txtPasswordNuevo]').attr("class", "form-control form-control-error")
        $('[id*=txtPasswordNuevo]').parent().children("span").text("Este campo es obligatorio ").show();
        $('[id*=txtPasswordNuevo]').parent().children("label").children("samp").attr("class", "form-text form-text-error");
    }
    else {
        $('[id*=txtPasswordNuevo]').parent().attr("class", "form-group");
        $('[id*=txtPasswordNuevo]').parent().children("span").hide();
    }
}
function validarPasswordRepite() {
    var text = $('[id*=txtPasswordRepite]').val();
    if (text == "") {
        $('[id*=txtPasswordRepite]').parent().attr("class", "form-group has-error");
        $('[id*=txtPasswordRepite]').attr("class", "form-control form-control-error")
        $('[id*=txtPasswordRepite]').parent().children("span").text("Este campo es obligatorio ").show();
        $('[id*=txtPasswordRepite]').parent().children("label").children("samp").attr("class", "form-text form-text-error");
    }
    else {

        if ($('[id*=txtPasswordRepite]').val() == $('[id*=txtPasswordNuevo]').val())
        {
            $('[id*=txtPasswordRepite]').parent().attr("class", "form-group");
            $('[id*=txtPasswordRepite]').parent().children("span").hide();
        }
        else {
            $('[id*=txtPasswordRepite]').parent().attr("class", "form-group has-error");
            $('[id*=txtPasswordRepite]').parent().children("span").text("*Las contraseña no coinciden ").show();
        }
       
    }


}
function validarCajas() {
    validarCorreo();
    validarPasswordActual();
    validarPasswordNuevo();
    validarPasswordRepite();
    var hijos = $('div.form-group.has-error');
    var IsValido = false;
    if (hijos.length == 0)
    {
        IsValido = true;
    }
    return IsValido;
}