<%@ Page Title="" Language="C#" MasterPageFile="~/Application.master" AutoEventWireup="true" CodeBehind="Validaciones.aspx.cs" Inherits="SIRALAB.Validaciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">


    <%--<link href="style/Master.css" rel="stylesheet" type="text/css" />--%>
    <link href="style/Formularios.css" rel="stylesheet" type="text/css" />
    <link href="style/titulo.css" rel="stylesheet" type="text/css" />
    <link href="style/botones.css" rel="stylesheet" type="text/css" />
    <link href="style/grid.css" rel="stylesheet" type="text/css" />
   <%-- <link href="style/paginador.css" rel="stylesheet" />--%>
    <link href="style/iconos.css" rel="stylesheet" type="text/css" />

    <link href="style/Mensajes/bootstrap.min.css" rel="stylesheet">
    <!-- Custom CSS -->
    <link href="style/Mensajes/main.css" rel="stylesheet">
    <!-- Scroll Menu -->
    <link href="style/Mensajes/sweetalert.css" rel="stylesheet">
    <!-- jQuery (necessary for Bootstrap's JavaScript plugins) -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.11.0/jquery.min.js"></script>
    <!-- Include all compiled plugins (below), or include individual files as needed -->
    <script src="js/Mensajes/bootstrap.js"></script>

    <!-- Custom functions file -->
    <script src="js/Mensajes/functions.js"></script>
    <!-- Sweet Alert Script -->
    <script src="js/Mensajes/sweetalert.min.js"></script>
    <style type="text/css">
        .table-list > tbody > tr:nth-child(even) > td {
            background-color: #F2F2F2;
        }

        .titulo-campo, .titulo-seccion, .titulo-pagina {
            color: #4A966A;
        }

        .auto-style1 {
            text-align: right;
            width: 358px;
        }

        .auto-style2 {
            width: 358px;
        }
    </style>
    <script type="text/javascript">

        $('#busqueda').hide();

        function errorcatchkey() {
            swal("¡Error!:", "la llave no corresponde con el certificado", "error");
        }
        function errorcatchcer() {
            swal("¡Error!:", "Certificado no corresponde a la llave privada", "error");
        }
        function errorcatchpw() {
            swal("¡Error!:", "Contraseña incorrecta", "error");
        }
        function errorcatch() {
            swal("¡Error!:", "Hay un inconveniente, por el momento no se puede realizar tu solicitud.", "error");
        }
        function errorfile1() {
            swal("¡Error!:", "Ingresa un certificado", "error");
        }
        function errorfile2() {
            swal("¡Error!:", "Ingresa la llave de un certificado", "error");
        }
        function errortxt() {
            swal("¡Error!:", "Ingresa una contraseña", "error");
        }
        function bien() {
            swal("¡Bien!", "Se realizó el cambio con éxito", "success");
        }

        jQuery(document).ready(function () {
            $("#clear").click(function () {
                $('#<%= TextBox1.ClientID %>').val('');
                $('#<%= TextBox2.ClientID %>').val('');
                $('#<%= TextBox3.ClientID %>').val('');
                $('#<%= TextBox5.ClientID %>').val('');
                $('#<%= TextBox6.ClientID %>').val('');
                $('#<%= CheckBox1.ClientID %>').prop("checked",false);
                $('#<%= CheckBox2.ClientID %>').prop("checked", false);
                $('#busqueda').hide();
            });
            $("#hhload").click(function () {
                var result =
                    $('#<%= TextBox2.ClientID %>').val() + ',' +
                         $('#<%= CheckBox1.ClientID %>').is(":checked") + ',' +
                         $('#<%= CheckBox2.ClientID %>').is(":checked") + ',' +
                        $('#<%= TextBox5.ClientID %>').val() + ',' +
                        $('#<%= TextBox6.ClientID %>').val();


                if ($('#<%= TextBox2.ClientID %>').val() == "" || $('#<%= TextBox5.ClientID %>').val() == "" || $('#<%= TextBox6.ClientID %>').val() == "") {
                    swal("¡Datos faltantes!:", "Faltan parámetro para poder guardar");
                } else {
                    $.ajax({
                        type: "POST",
                        url: "Validaciones.aspx/Habilitar",
                        data: "{'busqueda' : " + JSON.stringify(result) + "}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: resultado,
                        error: errores
                    });
                }

            });

            function resultado(msg) {
                //msg.d tiene el resultado devuelto por el método
                if (msg.d != null) {
                    $('#busqueda').hide();
                    $("#grid").trigger("reloadGrid");
                    bien();
                } else {
                    // swal("¡Bien!", "Se realizó el cambio con éxito", "success");
                    swal("¡Error!:", "Hay un inconveniente, por el momento no se puede realizar tu solicitud.", "error");
                }

                // alert('ok: ' + msg.d);
            }
            function errores(msg) {
                //msg.responseText tiene el mensaje de error enviado por el servidor
                swal("¡Error!:", msg.responseText);
            }

            $("#btnSearch").click(function () {

                var result = $('#<%= TextBox1.ClientID %>').val();

                if (result == "") {
                    $('#busqueda').hide();
                    swal("¡Datos faltantes!:", "Ingresa al menos un parámetro para realizar la búsqueda...");
                } else {
                    $.ajax({
                        type: "POST",
                        url: "Validaciones.aspx/Busquedarfclab",
                        data: "{'busqueda' : " + JSON.stringify(result) + "}",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            if (response.d == null) {
                                $('#busqueda').hide();
                                swal("¡Datos ingresados!:", "El parámetro ingresado no trajo ningún resultado, realiza otra búsqueda", 'info');
                            } else {
                                $('#busqueda').show('slow');
                                $('#<%= TextBox2.ClientID %>').val(response.d[0]);
                                $('#<%= TextBox3.ClientID %>').val(response.d[1]);
                            }
                        },
                        error: function (error) {
                            alert(error.responseText);
                        },
                        failure: function (response) {
                            alert(response.d);
                        }
                    });
                }

            });

            function getData1(customer) {



                var params = new Object();
                params.page = 1;
                params.pageSize = 10;
                params.sortIndex = 'id';
                params.sortDirection = 'ASC';
                /*Búsqueda*/
                params._search = true;
                params.searchField = '';
                params.searchString = '';
                params.searchOper = '';
                if (params._search) {
                    params.searchField = '';
                    params.searchString = customer;
                    params.searchOper = '';
                }


                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "Validaciones.aspx/Busquedarfclab",
                    data: JSON.stringify(params),
                    dataType: "json",
                    success: function (data, textStatus) {
                        if (textStatus == "success") {
                            var thegrid = $("#grid")[0];
                            if (data.d.records == 0) {
                                swal("¡Datos ingresados!:", "El parámetro ingresado no trajo ningún resultado, realiza otra búsqueda", 'info');
                            } else {
                                thegrid.addJSONData(data.d);
                            }

                        }
                    },
                    error: function (data, textStatus) {
                        alert(data);
                        alert(textStatus);
                    }
                });
            }
            /* Ordenes de trabajo NO Firmadas*/

            function getDataOtP(pdata) {
                $('#busqueda').hide();


                var params = new Object();
                params.page = pdata.page;
                params.pageSize = pdata.rows;
                params.sortIndex = pdata.sidx;
                params.sortDirection = pdata.sord;
                /*Búsqueda*/
                params._search = pdata._search;
                params.searchField = '';
                params.searchString = '';
                params.searchOper = '';
                if (pdata._search) {
                    params.searchField = pdata.searchField;
                    params.searchString = pdata.searchString;
                    params.searchOper = pdata.searchOper;
                }


                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "Validaciones.aspx/Busquedarfc",
                    data: JSON.stringify(params),
                    dataType: "json",
                    success: function (data, textStatus) {
                        if (textStatus == "success") {
                            var thegrid = $("#grid")[0];
                            thegrid.addJSONData(data.d);
                        }
                    },
                    error: function (data, textStatus) {
                        alert(data);
                        alert(textStatus);
                    }
                });
            }

            $("#grid").jqGrid(
            {

                datatype: function (pdata) {
                    getDataOtP(pdata);
                },
                colNames: ['ID', 'Id Laboratorio', "RFC Laboratorio", "Validacion Correo", "Validacion Extemporaneos", "Fecha Inicio", "Fecha Fin"],
                colModel: [
                     { name: 'id', index: "id", key: true },
                    { name: 'IdLaboratorio', index: "IdLaboratorio", width: 150 },
                    {
                        name: "RfcLaboratorio", index: "RfcLaboratorio", width: 150, searchoptions: { sopt: ["cn"] }
                    },
                    {
                        name: "ValCorreo", index: "ValCorreo", width: 150, searchoptions: { sopt: ["cn"] }
                    },
                    {
                        name: "ValExtemporaneo", index: "ValExtemporaneo", width: 150, searchoptions: { sopt: ["cn"] }
                    },
                    {
                        name: "FechaInicio", index: "FechaInicio", width: 150, searchoptions: { sopt: ["cn"] }
                    },
                    {
                        name: "FechaFin", index: "FechaFin", width: 150, searchoptions: { sopt: ["cn"] }
                    }



                ],
                odata: ['contains'],
                rowNum: 10,
                rowList: [10, 20, 50, 100, 200],
                pager: "#pager",
                sortname: "id",
                sortorder: "ASC",
                viewrecords: true,
                height: "100%",
                // multiselect: true,
                caption: "",
                regional: 'es',
                rownumbers: true


            }).navGrid("#pager",
                {
                    edit: false,
                    add: false,
                    search: false,
                    del: false

                }, {}, {}, {}, {
                    closeAfterSearch: true

                }

            );

        });




        /*Edita la Orden de Trabajo*/
        function goEdit(id) {

            //alert("edit" + id);
            $.ajax({
                type: "POST",
                url: "Orden.aspx/EditOrdenTrabajo",
                data: "{'Id' : " + id + ", 'url' : '" + location.href + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    window.location.href = "OrdenTrabajo.aspx";
                },
                error: function (error) {
                    alert(error.responseText);
                },
                failure: function (response) {
                    alert(response.d);
                }
            });
        }

        /*Elimina la Orden de Trabajo*/
        function doDelete(id) {
            $("#dialog-confirm").attr('style', 'z-index:10000;');
            $("#dialog-confirm").dialog({
                resizable: false,
                modal: true,
                title: "Modal",
                height: 250,
                width: 400,
                buttons: {
                    "Yes": function () {
                        $(this).dialog('close');
                        $.ajax({
                            type: "POST",
                            url: "Orden.aspx/EliminarOrdenTrabajo",
                            data: "{'Id' : " + id + "}",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (response) {
                                // window.location.href = "OrdenTrabajo.aspx";
                                alert(response.d);
                            },
                            error: function (error) {
                                alert(error.responseText);
                            },
                            failure: function (response) {
                                alert(response.d);
                            }
                        });
                    },
                    "No": function () {
                        $(this).dialog('close');
                        callback(false);
                    }
                }
            });




        }


        formatCustomED = function (cellvalue, options, rowObject) {
            return "<div style=\"float: left; cursor: pointer;padding-left:20px\" class=\"ui-pg-div ui-inline-edit\" onmouseover=\"jQuery(this).addClass('ui-state-hover');\" title=\"Edit selected row\" onmouseout=\"jQuery(this).removeClass('ui-state-hover')\" onclick='goEdit(" + rowObject[0] + ")'><span class=\"ui-icon ui-icon-pencil\"></span></div>";

        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphNeasted" runat="server">
    <link rel="stylesheet" href="style/Mensajes/bootstrap.min.css">
    <link rel="stylesheet" href="style/Mensajes/bootstrap-theme.min.css">
    <link rel="stylesheet" href="style/Mensajes/jquery.simplefileinput.css">
    <script type="text/javascript" src="js/Mensajes/jquery.simplefileinput.min.js"></script>


    <link href="JQGridReq/ui.jqgrid.css" rel="stylesheet" />
    <%--<link href="JQGridReq/jquery-ui.css" rel="stylesheet" />--%>
    <link href="JQGridReq/jquery-ui-1.9.2.custom.css" rel="stylesheet" />
    <script src="JQGridReq/jquery.jqGrid.min.js"></script>
    <script src="JQGridReq/grid.locale-es.js"></script>
    <script src="JQGridReq/jquery-ui.min.js"></script>

    <div style="min-height: 400px;">
        <div class="titulo-pagina">Validaciones Correo y Extemporaneos</div>
        <div class="formulario-seccion">
            <asp:Panel runat="server" Width="100%">
                <div align="center">
                    <table style="width: 80%;">
                        <tr>
                            <td colspan="3">&nbsp;</td>
                        </tr>
                        <tr>
                            <td>RFC Laboratorio:</td>
                            <td>
                                <asp:TextBox ID="TextBox1" runat="server" CssClass="control-campo" Width="300px"></asp:TextBox>
                            </td>
                            <td>
                                <input id="btnSearch" class="boton" style="width: 150px" type="button" value="Buscar" />
                            </td>
                        </tr>
                    </table>

                </div>
            </asp:Panel>
        </div>


        <div class="formulario-seccion" id="busqueda">
            <table style="width: 80%;">
                <tr>
                    <td class="auto-style2">&nbsp;</td>
                    <td style="text-align: left">&nbsp;</td>
                    <td style="text-align: left">&nbsp;</td>
                    <td style="text-align: left">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 300px; text-align: right;">RFC:</td>
                    <td style="text-align: left">
                        <asp:TextBox ID="TextBox2" runat="server" CssClass="control-campo" Enabled="False" Width="264px"></asp:TextBox>
                    </td>
                    <td style="text-align: left">&nbsp;</td>
                    <td style="text-align: left">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style1">Nombre laboratorio:</td>
                    <td style="text-align: left">
                        <asp:TextBox ID="TextBox3" runat="server" CssClass="control-campo" Width="500px" Enabled="False"></asp:TextBox>
                    </td>
                    <td style="text-align: left">&nbsp;</td>
                    <td style="text-align: left">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style1">Enviar Correo</td>
                    <td style="text-align: left">
                        <asp:CheckBox ID="CheckBox1" runat="server" CssClass="control-campo" />
                    </td>
                    <td style="text-align: left">&nbsp;</td>
                    <td style="text-align: left">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style1">Es Extemporaneo</td>
                    <td style="text-align: left">
                        <asp:CheckBox ID="CheckBox2" runat="server" CssClass="control-campo" />
                    </td>
                    <td style="text-align: left">&nbsp;</td>
                    <td style="text-align: left">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style1">Fecha Inicial</td>
                    <td style="text-align: left">
                        <asp:TextBox ID="TextBox5" runat="server" CssClass="control-campo"></asp:TextBox>
                        <asp:CalendarExtender ID="TextBox5_CalendarExtender" runat="server" Format="dd/MM/yyyy" TargetControlID="TextBox5">
                        </asp:CalendarExtender>
                    </td>
                    <td style="text-align: left">&nbsp;</td>
                    <td style="text-align: left">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style1">Fecha Final</td>
                    <td style="text-align: left">
                        <asp:TextBox ID="TextBox6" runat="server" CssClass="control-campo"></asp:TextBox>
                        <asp:CalendarExtender ID="TextBox6_CalendarExtender" runat="server" Format="dd/MM/yyyy" TargetControlID="TextBox6">
                        </asp:CalendarExtender>
                    </td>
                    <td style="text-align: left">&nbsp;</td>
                    <td style="text-align: left">&nbsp;</td>
                </tr>
                <tr>
                    <td class="auto-style2">&nbsp;</td>
                    <td style="text-align: left">&nbsp;</td>
                    <td style="text-align: left">
                        <input id="hhload" type="button" value="Habilitar" class="boton" style="width: 150px" />
                    </td>
                    <td style="text-align: left">
                        <input id="clear" type="button" value="Limpiar" class="boton" style="width: 150px" /></td>
                </tr>
            </table>
        </div>
        <div align="center">
            <div id="select" runat="server"></div>
            <table id="grid"></table>
            <div id="pager"></div>

        </div>
    </div>

</asp:Content>
