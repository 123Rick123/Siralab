<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="wucListaOrdenCanceladas.ascx.cs" Inherits="SIRALAB.Controles.wucListaOrdenCanceladas" %>

<%--<link href="../style/Master.css" rel="stylesheet" type="text/css" />--%>
<link href="../style/Formularios.css" rel="stylesheet" type="text/css" />
<link href="../style/titulo.css" rel="stylesheet" type="text/css" />
<link href="../style/botones.css" rel="stylesheet" type="text/css" />
<link href="../style/grid.css" rel="stylesheet" type="text/css" />
<%--<link href="../style/paginador.css" rel="stylesheet" />--%>
<link href="../style/iconos.css" rel="stylesheet" type="text/css" />
<style type="text/css">
    .table-list > tbody > tr:nth-child(even) > td {
        background-color: #F2F2F2;
    }

    .pager-style {
        padding: 7px 4px;
        display: block;
    }

        .pager-style > ul {
            margin: 0;
            padding: 7px 4px;
            display: block;
        }

            .pager-style > ul > li {
                display: inline-block;
                margin: 0 2px;
            }

                .pager-style > ul > li:first-child > a, .pager-style > ul > li:last-child > a {
                    padding: 4px 12px;
                }

                .pager-style > ul > li > a {
                    padding: 4px 8px;
                    border-radius: 3px;
                    -webkit-border-radius: 3px;
                    -moz-border-radius: 3px;
                    -o-border-radius: 3px;
                    line-height: 1.4285;
                    color: #4A966A;
                    text-decoration: none;
                    background-color: #fff;
                    border: 1px solid #ddd;
                    -webkit-box-sizing: border-box;
                    -moz-box-sizing: border-box;
                    box-sizing: border-box;
                    font: 8pt Segoe UI;
                }

                .pager-style > ul > li > a {
                    background-color: #fff;
                }

                    .pager-style > ul > li > a:hover {
                        background-color: #e2e2e2;
                    }

                    .pager-style > ul > li > a:active, .pager-style > ul > li > span {
                        background-color: #ccc;
                    }

                    .pager-style > ul > li > a[selected-page="true"] {
                        background-color: #ccc;
                    }

    .page-previous {
        background: url('/image/anterior-16.png') no-repeat center;
    }

    .page-next {
        background: url('/image/siguiente-16.png') no-repeat center;
    }

    .page-previous:hover {
        background: url('/image/anterior-16hover.png') no-repeat center;
    }

    .page-next:hover {
        background: url('/image/siguiente-16hover.png') no-repeat center;
    }
</style>

<!-- JqGrid -->
<link href="JQGridReq/ui.jqgrid.css" rel="stylesheet" />
<%--<link href="JQGridReq/jquery-ui.css" rel="stylesheet" />--%>
<link href="JQGridReq/jquery-ui-1.9.2.custom.css" rel="stylesheet" />
<script src="JQGridReq/jquery.jqGrid.min.js"></script>
<script src="JQGridReq/grid.locale-es.js"></script>
<script src="JQGridReq/jquery-ui.min.js"></script>

<script type="text/javascript">
    jQuery(document).ready(function () {

        /* Ordenes de trabajo NO Firmadas*/

        function getData_OT_P(pdata) {



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
                url: "Orden.aspx/ListaOrdenesTrabajoCanceladas",
                data: JSON.stringify(params),
                dataType: "json",
                success: function (data, textStatus) {
                    if (textStatus == "success") {
                        var thegrid = $("#gridC")[0];
                        thegrid.addJSONData(data.d);
                    }
                },
                error: function (data, textStatus) {
                    alert(data);
                    alert(textStatus);
                }
            });
        }

        $("#gridC").jqGrid(
        {

            datatype: function (pdata) {
                getData_OT_P(pdata);
            },
            colNames: ["id", "Número Informe", "Titulo", "Anexo", "RFC del Cliente", "Norma", "Fecha Emisión", " Acción"],
            colModel: [
                           { name: 'id', key: true, hidden: true },
                           {
                               name: "Folio", index: "Folio", width: 120, searchoptions: { sopt: ["cn"] }
                           },
                           {
                               name: "Titulo", index: "Titulo", width: 120, searchoptions: { sopt: ["cn"] }
                           },
                           {
                               name: "Anexo", index: "Anexo", width: 50, searchoptions: { sopt: ["cn"] }
                           },
                           {
                               name: "RFCCliente", index: "RFCCliente", width: 100, searchoptions: { sopt: ["cn"] }
                           },
                           {
                               name: "Norma", index: "Norma", width: 100, searchoptions: { sopt: ["cn"] }
                           },
                           { name: "Registro", index: "Registro", width: 100, searchoptions: { sopt: ["cn"] } },
                           { name: 'RowActions', width: 60, fixed: true, sortable: false, resize: false, formatter: formatCustomED, search: false }



            ],
            odata: ['contains'],
            rowNum: 10,
            rowList: [10, 20, 50, 100, 200],
            pager: "#pagerC",
            sortname: "id",
            sortorder: "ASC",
            viewrecords: true,
            height: "100%",
            caption: "",
            regional: 'es'

        }).navGrid("#pagerC",
            {
                edit: false,
                add: false,
                search: true,
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
                alert(error.responseText)
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
                            alert(response.d)
                        },
                        error: function (error) {
                            alert(error.responseText)
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
        //alert(JSON.stringify(rowObject));
        //return "<div style=\"float: left; cursor: pointer;padding-left:8px\" class=\"ui-pg-div ui-inline-edit\" onmouseover=\"jQuery(this).addClass('ui-state-hover');\" title=\"Edit selected row\" onmouseout=\"jQuery(this).removeClass('ui-state-hover')\" onclick='goEdit(" + rowObject[0] + ")'><span class=\"ui-icon ui-icon-pencil\"></span></div><div style=\"margin-left: 5px; float: left;\" class=\"ui-pg-div ui-inline-del\" onmouseover=\"jQuery(this).addClass('ui-state-hover');\" title=\"Delete selected row\" onmouseout=\"jQuery(this).removeClass('ui-state-hover');\" onclick=\"doDelete(" + rowObject[0] + ")\"><span class=\"ui-icon ui-icon-trash\"></span></div>";
        return "<div style=\"float: left; cursor: pointer;padding-left:20px\" class=\"ui-pg-div ui-inline-edit\" onmouseover=\"jQuery(this).addClass('ui-state-hover');\" title=\"Edit selected row\" onmouseout=\"jQuery(this).removeClass('ui-state-hover')\" onclick='goEdit(" + rowObject[0] + ")'><span class=\"ui-icon ui-icon-pencil\"></span></div>";

    }





</script>

<table id="gridC"></table>
<div id="pagerC"></div>