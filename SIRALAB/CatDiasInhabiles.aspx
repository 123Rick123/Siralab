<%@ Page Title="" Language="C#" MasterPageFile="~/Application.master" AutoEventWireup="true" CodeBehind="CatDiasInhabiles.aspx.cs" Inherits="SIRALAB.CatDiasInhabiles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <link href="style/Formularios.css" rel="stylesheet" type="text/css" />
    <link href="style/iconos.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .pagination-ys {
            /*display: inline-block;*/
            padding-left: 0;
            margin: 20px 0;
            border-radius: 4px;
        }

        .pagination-ys table > tbody > tr > td {
            display: inline;
        }

        .pagination-ys table > tbody > tr > td > a,
        .pagination-ys table > tbody > tr > td > span {
            position: relative;
            float: left;
            padding: 8px 12px;
            line-height: 1.42857143;
            text-decoration: none;
            color: #003300;
            background-color: #ffffff;
            border: 1px solid #dddddd;
            margin-left: -1px;
        }

        .pagination-ys table > tbody > tr > td > span {
            position: relative;
            float: left;
            padding: 8px 12px;
            line-height: 1.42857143;
            text-decoration: none;    
            margin-left: -1px;
            z-index: 2;
            color: #003300;
            background-color: #f5f5f5;
            border-color: #dddddd;
            cursor: default;
        }

        .pagination-ys table > tbody > tr > td:first-child > a,
        .pagination-ys table > tbody > tr > td:first-child > span {
            margin-left: 0;
            border-bottom-left-radius: 4px;
            border-top-left-radius: 4px;
        }

        .pagination-ys table > tbody > tr > td:last-child > a,
        .pagination-ys table > tbody > tr > td:last-child > span {
            border-bottom-right-radius: 4px;
            border-top-right-radius: 4px;
        }

        .pagination-ys table > tbody > tr > td > a:hover,
        .pagination-ys table > tbody > tr > td > span:hover,
        .pagination-ys table > tbody > tr > td > a:focus,
        .pagination-ys table > tbody > tr > td > span:focus {
            color: #003300;
            background-color: #eeeeee;
            border-color: #dddddd;
        }
    </style>
    <script type="text/javascript">

        $(document).ready(function () {
            $.datepicker.regional.es = {
                closeText: 'Cerrar',
                prevText: 'Ant',
                nextText: 'Sig',
                currentText: 'Hoy',
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                dayNames: ['Domingo', 'Lunes', 'Martes', 'Mi&eacute;rcoles', 'Jueves', 'Viernes', 'S&aacute;bado'],
                dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mi&eacute;', 'Juv', 'Vie', 'S&aacute;b'],
                dayNamesMin: ['Dom', 'Lun', 'Mar', 'Mie', 'Jue', 'Vie', 'S&aacute;b'],
                weekHeader: 'Sm',
                dateFormat: 'dd/mm/yy',
                firstDay: 1,
                isRTL: false,
                showMonthAfterYear: false,
                yearSuffix: ''
            };
            $.datepicker.setDefaults($.datepicker.regional.es);


            $("input[id*='txtDiaInhabilAdd']").on("blur", function () {
                var fecha = $(this).val();
                if (fecha === "") {
                    $(".spnAdd").empty().html("Debe introducir un valor");
                    $(".divSave").show();
                } else if (!FechaValida(fecha)) {
                    $(".spnAdd").empty().html("Formato de fecha incorrecto");
                    $(".divSave").show();
                    enviar = false;
                } else {
                    $(".divSave").hide();
                }
            });


        });

        function setDatepicker() {
            $(".fecha").datepicker({ autoclose: true });
        }


        function FechaValida(fechaFormatoHispano) {
            var regEx = /^\d{2}\/\d{2}\/\d{4}$/;
            return fechaFormatoHispano.match(regEx);
        }

        function validaFecha() {

        }

        function validaEdicion(e) {
            var enviar = true;
            var fecha = $("input[id*='txtDiaInhabilEdit']").val();
            if (fecha === "") {
                $(".spnUpdate").empty().html("Debe introducir un valor").show();
                $("divUpdate").show();
                enviar = false;

            } else if (!FechaValida(fecha)) {
                $(".spnUpdate").empty().html("Formato de fecha incorrecto").show();
                $(".divUpdate").show();
                enviar = false;
            } else if (!transformaFecha(fecha)) {
                $(".spnUpdate").empty().html("Fecha inválida").show();
                $(".divUpdate").show();
                enviar = false;
            } else {
                $(".divUpdate").hide();
            }

            if (!enviar) {
                e.stopPropagation();
                e.preventDefault();
                return false;
            }
        }


        function validaGuardar(e) {
            var enviar = true;
            var fecha = $("input[id*='txtDiaInhabilAdd']").val();
            if (fecha === "") {
                $(".spnAdd").empty().html("Debe introducir un valor").show();
                $(".divSave").show();
                enviar = false;

            } else if (!FechaValida(fecha)) {
                $(".spnAdd").empty().html("Formato de fecha incorrecto").show();
                $(".divSave").show();
                enviar = false;
            } else if (!transformaFecha(fecha)) {
                $(".spnSave").empty().html("Fecha inválida").show();
                $(".divSave").show();
                enviar = false;
            } else {
                $(".divSave").hide();
            }

            if (!enviar) {
                e.stopPropagation();
                e.preventDefault();

                return false;
            }
        }

        function validaOnBlurSave(objeto) {
            var fecha = $(objeto).val();
            if (fecha === "") {
                $(".spnAdd").empty().html("Debe introducir un valor").show();
                $(".divSave").show();
            } else if (!FechaValida(fecha)) {
                $(".spnAdd").empty().html("Formato de fecha incorrecto").show();
                $(".divSave").show();
                enviar = false;
            } else if (!transformaFecha(fecha)) {
                $(".spnAdd").empty().html("Fecha inválida").show();
                $(".divSave").show();
            } else {
                $(".divSave").hide();
            }
        }

        function validaOnBlurEdicion(objeto) {

            var fecha = $(objeto).val();
            if (fecha === "") {
                $(".spnUpdate").empty().html("Debe introducir un valor").show();
                $(".divUpdate").show();
            } else if (!FechaValida(fecha)) {
                $(".spnUpdate").empty().html("Formato de fecha incorrecto").show();
                $(".divUpdate").show();
            } else if (!transformaFecha(fecha)) {
                $(".spnUpdate").empty().html("Fecha inválida").show();
                $(".divUpdate").show();
            } else {
                $(".divUpdate").hide();
            }
        }

        function transformaFecha(fecha) {
            var arr = fecha.split("/");
            var fechaTransformada = arr[2] + "-" + arr[1] + "-" + arr[0];

            return Date.parse(fechaTransformada) ? true : false;
        }


    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphNeasted" runat="server">


    <div class="container">
        <br />
        <div class="row">
            <div class="col-sm-1">
                <asp:LinkButton ID="lnkbtnback" runat="server" CssClass="btn btn-default btn-ms" OnClick="lnkbtnback_Click" ToolTip="Regresar a la pagina anterior">
                        <span class="glyphicon glyphicon-chevron-left"></span>
                </asp:LinkButton>
            </div>
        </div>

        <div class="row">
            <div class="col-sm-12">
                <h2>Días inhábiles</h2>
                <hr class="red">
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <div class="row">
                            <div class="col-md-11">
                                <h5><span>Días inhábiles</span></h5>
                            </div>
                            <div class="col-md-1">
                                <asp:UpdatePanel ID="upAgregar" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                                    <ContentTemplate>
                                        <asp:LinkButton ID="lnkAddCatDiasInhabiles" runat="server" ToolTip="Agregar" CssClass="btn btn-default btn-sm" OnClick="lnkAddCatDiaInhabil_Click" CausesValidation="true">Añadir</asp:LinkButton>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-2"></div>
                            <div class="col-md-8">
                                <div class="table-responsive">
                                    <asp:UpdatePanel ID="upAccion" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true" ClientIDMode="AutoID">
                                        <ContentTemplate>
                                            <asp:GridView ID="GridViewCatDiasInhabiles" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                                                AllowSorting="True" Visible="true" ShowHeaderWhenEmpty="true" PageSize="12"
                                                DataKeyNames="DiaInhabilId"
                                                OnRowCommand="GridViewCatDiasInhabiles_RowCommand"
                                                OnRowDataBound="GridViewCatDiasInhabiles_RowDataBound"
                                                OnRowCancelingEdit="GridViewCatDiasInhabiles_RowCancelingEdit"
                                                OnRowDeleting="GridViewCatDiasInhabiles_RowDeleting"
                                                OnRowEditing="GridViewCatDiasInhabiles_RowEditing"
                                                OnRowUpdating="GridViewCatDiasInhabiles_RowUpdating"
                                                OnPageIndexChanging="GridViewCatDiasInhabiles_PageIndexChanging"
                                                OnSelectedIndexChanged="GridViewCatDiasInhabiles_SelectedIndexChanged"
                                                Width="100%" CssClass="table table-condensed table-striped table-sm text-center" CellSpacing="0" BorderWidth="0" GridLines="None">
                                                <PagerStyle CssClass="pagination-ys"/>
                                                <Columns>
                                                    <%--Campo: Nombre--%>
                                                    <asp:TemplateField HeaderText="Día" HeaderStyle-CssClass="text-center">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="LblNombre" Text='<%# Eval("DiaInhabil","{0:dd/MM/yyyy}") %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <div class="col-md-3"></div>
                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <div class="form-group datepicker-group">
                                                                        <asp:TextBox ID="txtDiaInhabilEdit" runat="server" Style="text-align: center;" Text='<%# Eval("DiaInhabil","{0:dd/MM/yyyy}") %>' CssClass="form-control fecha" MaxLength="10" Width="100%" OnBlur="validaOnBlurEdicion(this);" />
                                                                        <i class="glyphicon glyphicon-calendar form-control-feedback"></i>
                                                                        <div class="divUpdate" style="display: none">
                                                                            <span class="text-danger spnUpdate" style="display: none;"></span>
                                                                        </div>
                                                                    </div>

                                                                </div>
                                                            </div>
                                                            <div class="col-md-3"></div>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <div class="col-md-3"></div>
                                                            <div class="col-md-6">
                                                                <div class="form-group">
                                                                    <div class="form-group datepicker-group">
                                                                        <asp:TextBox ID="txtDiaInhabilAdd" runat="server" Style="text-align: center;" CssClass="form-control fecha" MaxLength="10" Width="100%" OnBlur="validaOnBlurSave(this);" />
                                                                        <i class="glyphicon glyphicon-calendar"></i>
                                                                        <div class="divSave" style="display: none">
                                                                            <span class="text-danger spnAdd" style="display: none;"></span>
                                                                        </div>
                                                                    </div>

                                                                </div>
                                                            </div>
                                                            <div class="col-md-3"></div>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                    <%--Botones de Acción--%>
                                                    <asp:TemplateField HeaderText="Acciones" HeaderStyle-Width="50px">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="Edit" ToolTip="Editar" CssClass="btn btn-default btn-xs">
                                                        <span class="glyphicon glyphicon-pencil"></span>
                                                            </asp:LinkButton>
                                                            <asp:LinkButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="Delete" ToolTip="Eliminar" CssClass="btn btn-default btn-xs">
                                                        <span class="glyphicon glyphicon-trash"></span>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:LinkButton ID="btnUpdateSave" ToolTip="Guardar" runat="server" CommandName="Update" CssClass="btn btn-default btn-xs" OnClientClick="validaEdicion(event);"> 
                                                        <span class="fas fa-save fa-lg"></span>
                                                            </asp:LinkButton>
                                                            <asp:LinkButton ID="btnUpdateCancel" ToolTip="Cancelar" runat="server" CommandName="Cancel" CausesValidation="false" CssClass="btn btn-default btn-xs">
                                                        <span class="glyphicon glyphicon-minus"></span>
                                                            </asp:LinkButton>
                                                        </EditItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:LinkButton ID="btnAddSave" ToolTip="Guardar" runat="server" CommandName="AddNew" CssClass="btn btn-default btn-xs" OnClientClick="validaGuardar(event);"> 
                                                        <span class="fas fa-save fa-lg"></span>
                                                            </asp:LinkButton>
                                                            <asp:LinkButton ID="btnAddCancel" ToolTip="Cancelar" runat="server" CommandName="Cancel" CausesValidation="false" CssClass="btn btn-default btn-xs">
                                                        <span class="glyphicon glyphicon-minus"></span>
                                                            </asp:LinkButton>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                            </div>
                            <div class="col-md-2"></div>
                        </div>
                    </div>
                </div>
    </div>
        </div>
    </div>

</asp:Content>
