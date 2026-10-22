function passRecovery(a, b) {
    var offs = $('#PanelContrasenia').css('display');

    if (offs == 'none') {
        $('#PanelContrasenia').css('display', 'block');
        $('#PanelLogin').css('display', 'none');
    }
    else {
        $('#PanelContrasenia').css('display', 'none');
        $('#PanelLogin').css('display', 'block');
    }
}

function errorMail() {
    $('.login-mode').css('margin-left', "-360px");
}

