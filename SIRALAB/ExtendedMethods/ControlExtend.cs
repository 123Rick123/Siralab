using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AjaxControlToolkit;

namespace SIRALAB.controles
{
    public static class ControlExtend
    {
        public static void control_SetFocus(this System.Web.UI.Control currentControl, string ObjectFocusID)
        {
            System.Web.UI.ScriptManager.GetCurrent(currentControl.Page).SetFocus(ObjectFocusID);
        }
    }
}