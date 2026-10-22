using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Xml.Linq;
using SIRALAB;
using SIRALAB_BL.common;

namespace SIRALAB.recursos
{
    public static class MenuXml
    {
        public static string ReadXML(enumUsuarioRol rol, string file, UserControl Ctrl)
        {
            var MenuItems = from c in XElement.Load(file).Elements("item") select c;

            string chain = getMenu(MenuItems, rol, Ctrl);
            return chain;
        }

        public static string getMenu(IEnumerable<XElement> xmlList, enumUsuarioRol rol, UserControl Ctrl)
        {
            string result = "<ul class=\"menu\">";
            foreach (var item in xmlList)
            {
                var Element = (from c in item.Elements("rol") where int.Parse(c.Attribute("type").Value) == rol.ParseToInt() select c).FirstOrDefault();
                if (Element != null)
                {
                    var nombre = item.Attribute("name").Value;
                    var url = item.Attribute("url").Value;
                    var cssclass = item.Attribute("class").Value;
                    string liItem = "<li>";
                    var MenuItems = from c in item.Elements("item") select c;

                    if (MenuItems.Count() > 0)
                    {
                        liItem += "<a onClick=\"Submenu($(this))\" class=\"" + cssclass + "\">" + nombre + "</a>";
                        string resSubMenu = getMenu(MenuItems, rol, Ctrl).Replace("<ul class=\"menu\">", "<ul class=\"submenu\">");
                        liItem += resSubMenu;
                    }
                    else
                    {
                        string urlTemp = url.Contains("javascript:") ? url : Ctrl.ResolveUrl("~/" + url);
                        liItem += "<a href=\"" + urlTemp + "\" class=\"" + cssclass + "\">" + nombre + "</a>";
                    }
                    liItem += "</li>";
                    result += liItem;
                }
            }

            result += "</ul>";
            return result;
        }
    }

    public class MenuElementos
    {
        public List<MenuElementos> submenu { get; set; }
        public string nombre { get; set; }
        public string url { get; set; }
        public string estilo { get; set; }
    }
}