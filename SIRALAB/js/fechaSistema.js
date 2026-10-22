
ServerTime = {
    diasSemana: function () { _diasSemana = ["Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo"]; return _diasSemana; },
    mesesAnio: function () { _mesesAnio = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"]; return _mesesAnio; },
    getDayName: function (weekDay) {
        dia = ServerTime.diasSemana();
        return dia[weekDay];
    },
    getMonthName: function (month) {
        mes = ServerTime.mesesAnio();
        return mes[month];
    },
    init: function (wd, d, M, y, h, m, s, elem) {
        ServerTime.ViewTime(wd, d, M, y, h, m, s, elem);
    },
    ViewTime: function (wd, d, M, y, h, m, s, elem) {
        clearTimeout(ServerTime.timeOutHandler);
        newDate = new Date(y, M, d, h, m, s);
        DT = "";
        if (h > 12) {
            h = h - 12;
            DT = "p.m.";
        }
        else {
            DT = "a.m.";
        }
        hora = ServerTime.padLeft(h, "0", 2);
        minuto = ServerTime.padLeft(m, "0", 2);
        segundo = ServerTime.padLeft(s, "0", 2);

        resultado = ServerTime.getDayName(wd - 1) + " " + d + " de " + ServerTime.getMonthName(M - 1) + " del " + y + " " + hora + ":" + minuto + ":" + segundo + " " + DT;

        newDate.setSeconds(newDate.getSeconds() + 1);
        $(elem).html(resultado);

        ServerTime.timeOutHandler = setTimeout(function () { ServerTime.ViewTime(wd, d, M, y, newDate.getHours(), newDate.getMinutes(), newDate.getSeconds(), elem); }, 1000);
    },
    padLeft: function (s, c, i) {
        str = s.toString();
        pad = "";
        for (j = 0; j < i; j++) {
            pad = pad.concat(c);
        }

        return pad.substring(0, pad.length - str.length) + str;
    },
    timeOutHandler: 0
}