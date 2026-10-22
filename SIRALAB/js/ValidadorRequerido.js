function extendedValidatorUpdateDisplay(obj) {
    if (typeof originalValidatorUpdateDisplay === "function") {
        originalValidatorUpdateDisplay(obj);
    }
    var control = document.getElementById(obj.controltovalidate);
    if (control) {
        var isValid = true;
        for (var i = 0; i < control.Validators.length; i += 1) {
            if (!control.Validators[i].isvalid) {
                isValid = false;
                break;
            }
        }
        if (isValid) {
            $(control).parent().removeClass("has-error");
            $.each(control.Validators, function (index, value) {
                if (control.Validators[index].isvalid) {
                    $(control.Validators[index]).hide();
                }
            });
        } else {
            $(control).parent().addClass("has-error");
            $.each(control.Validators, function (index, value) {
                if (!control.Validators[index].isvalid) {
                    $(control.Validators[index]).css('visibility', 'visible');
                    $(control.Validators[index]).show();
                }
            });
        }
    }
}

$().ready(function () {
    var originalValidatorUpdateDisplay = window.ValidatorUpdateDisplay;
    window.ValidatorUpdateDisplay = extendedValidatorUpdateDisplay;
});