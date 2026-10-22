MenuHandler = {
    serverButtonClick: function (e) {
        document.getElementById(e).click();
    },
    submenu: function (e) {
        if ($('> ul', e).css('display') === 'none') {
            $('> ul', e).slideDown(200);
        } else {
            $('> ul', e).slideUp(200);
        }
    }
};