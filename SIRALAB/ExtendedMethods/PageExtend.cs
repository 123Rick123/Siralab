using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using AjaxControlToolkit;

namespace SIRALAB
{
    public static class PageExtend
    {
        public static void PopMensaje(this System.Web.UI.Page currentPage, string mensaje, string titulo,
            controles.Opciones opcion = controles.Opciones.SoloAceptar, controles.Icono icon = controles.Icono.ok,
            string command = null, string[] args = null)
        {
            System.Web.UI.Control wucMsj = currentPage.Master.FindControl("wucMensaje");
            if (wucMsj != null)
            {
                UpdatePanel up = (UpdatePanel)currentPage.Master.FindControl("upMensaje");
                ((controles.wucMensajes)wucMsj).Mostrar(mensaje, titulo, opcion, icon, command, args);
                up.Update();
            }
            else
            {
                UpdatePanel up = (UpdatePanel)currentPage.Master.Master.FindControl("upMensaje");
                wucMsj = currentPage.Master.Master.FindControl("wucMensaje");
                if (wucMsj != null)
                {
                    ((controles.wucMensajes)wucMsj).Mostrar(mensaje, titulo, opcion, icon, command, args);
                    up.Update();
                }
            }
        }
        public static void page_SetFocus(this System.Web.UI.Page currentPage, string ObjectFocusID)
        {
            ScriptManager.GetCurrent(currentPage).SetFocus(ObjectFocusID);
        }
    }
}