using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SIRALAB_DAL;
using SIRALAB_BL;
using SIRALAB_BL.common;
using SIRALAB_BL.catalogos;
using SIRALAB.Common;
using SIRALAB.Controles;
using AjaxControlToolkit;
using SIRALAB_BL.negocio;
using System.Linq;

namespace SIRALAB
{
    public partial class NormaConfiguracion : System.Web.UI.Page
    {
        #region Atributos

        public bool? RequiereUsoDeAgua
        {
            set { Session["RequiereUsoDeAgua"] = value; }
            get { return Session["RequiereUsoDeAgua"] == null ? null : (bool?)Session["RequiereUsoDeAgua"]; }
        }
        public bool? Nom2021
        {
            set { Session["Nom2021"] = value; }
            get { return Session["Nom2021"] == null ? null : (bool?)Session["Nom2021"]; }
        }
        public string NombreNorma2021
        {
            set { Session["NombreNorma2021"] = value; }
            get { return Session["NombreNorma2021"] == null ? null : (string)Session["NombreNorma2021"]; }
        }
        public int? NormaID
        {
            set
            {
                Session["NormaID_Editar"] = value;
            }
            get { return Session["NormaID_Editar"] == null ? null : (int?)Session["NormaID_Editar"]; }
        }

        

        public enumNivel NivelType
        {
            get { return (tcConfiguraciones.ActiveTabIndex + 1).ConvertToNivel(); }
        }
        #endregion Atributos

        Main main => (Main)this.Master.Master;

        protected void Page_Load(object sender, EventArgs e)
        {
            main.MensajeDevolucion += main_MensajeDevolucion;
            if (!IsPostBack)
            {
                if (this.NormaID != null)
                {
                    /* Acá vamos a cargar la norma  */
                    CargarNorma((int)this.NormaID);
                }
            }
        }

        void main_MensajeDevolucion(object sender, controles.MensajeArgs e)
        {
            switch (e.Command)
            {
                case "EliminarParametro":
                    if (e.Opcion == controles.Opcion.Aceptar)
                    {
                        NormaConfiguracion_BL ncBL = new NormaConfiguracion_BL();

                        ncBL.Eliminar(int.Parse(e.Args[0]));

                        if (ncBL.Mensaje != null && ncBL.Mensaje.Trim() != "")
                        {
                            /// No se eliminó de forma correcta
                            /// mostramos un mensaje con el motivo
                            this.PopMensaje(ncBL.Mensaje, "Configuración de Norma", controles.Opciones.SoloAceptar, controles.Icono.error);
                        }
                        else
                        {
                            wucListaNormaConfiguracionSimpleCompuesta.Mostrar((int)this.NormaID, wucFiltroNormaConfiguracion.Filtro.CuerpoReceptorID, wucFiltroNormaConfiguracion.Filtro.UsoAguaID, enumNivel.Compuesta.ConvertToInt());
                            wucListaNormaConfiguracionCompuestaMensual.Mostrar((int)this.NormaID, wucFiltroNormaConfiguracion.Filtro.CuerpoReceptorID, wucFiltroNormaConfiguracion.Filtro.UsoAguaID);
                            wucListaNormaConfiguracion.Mostrar((int)this.NormaID, wucFiltroNormaConfiguracion.Filtro.CuerpoReceptorID, wucFiltroNormaConfiguracion.Filtro.UsoAguaID, enumNivel.Simple.ConvertToInt());
                            wucListaNormaConfiguracion1.Mostrar((int)this.NormaID, wucFiltroNormaConfiguracion.Filtro.CuerpoReceptorID, wucFiltroNormaConfiguracion.Filtro.UsoAguaID, enumNivel.Compuesta.ConvertToInt());
                            //wucListaNormaConfiguracionInstantaneo.Mostrar((int)this.NormaID, wucFiltroNormaConfiguracion.Filtro.CuerpoReceptorID, wucFiltroNormaConfiguracion.Filtro.UsoAguaID, enumNivel.ValorInstantaneo.ConvertToInt());
                            upConfiguraciones.Update();
                        }
                    }
                    break;
            }
            main.MensajeDevolucion -= main_MensajeDevolucion;
        }

        #region Norma
        protected void btnEditNorma_Click(object sender, EventArgs e)
        {
            wucNormaCaptura.Editar_Norma();
            pnlNormaConfiguracion.Visible = false;
            upConfiguraciones.Update();
        }
        private void CargarNorma(int NormaID)
        {
            Norma_BL nBL = new Norma_BL();
            nBL.Obtener(NormaID);
            if (nBL.Entidad.RequiereUsoDeAgua == null)//RGG Se agregó validación para que no truene cuando se cargue otra norma que no sea la 2021
            {

                nBL.Entidad = nBL.UpdateRequiereUsoAndNom2021(nBL.Entidad.NormaID);
            }
            if (nBL.Entidad != null)
            {
                tabCompuesta.Visible = nBL.Entidad.RequiereCompuesta;
                tabMensual.Visible = nBL.Entidad.RequiereMensual;
                /* if (nBL.Entidad.RequiereValorInstantaneo != null || nBL.Entidad.RequiereUsoDeAgua != null)*///RGG Se agregó validación para que no truene cuando se cargue otra norma que no sea la 2021
                if (nBL.Entidad.RequiereUsoDeAgua != null)//RGG Se agregó validación para que no truene cuando se cargue otra norma que no sea la 2021
                {
                    //tabValorInstantaneo.Visible = nBL.Entidad.RequiereValorInstantaneo.Value;
                    RequiereUsoDeAgua = nBL.Entidad.RequiereUsoDeAgua;
                }
                else
                {
                    RequiereUsoDeAgua = true;
                }
                if (nBL.Entidad.Nom2021 != null)
                {
                    Nom2021 = nBL.Entidad.Nom2021;
                }
                else
                {
                    Nom2021 = false;
                }

                NombreNorma2021 = nBL.Entidad.Nombre;
                wucFiltroNormaConfiguracion.CargarDrops2021(NombreNorma2021);
            }

            pnlNormaConfiguracion.Visible = true;

            wucNormaCaptura.Cargar(NormaID);
            upConfiguraciones.Update();
        }
        protected void wucNormaCaptura_Guardar(object sender, NormaArg e)
        {
            Norma_BL nBL = new Norma_BL();
            nBL.Guardar(e);
            if (nBL.Mensaje!=string.Empty && nBL.Mensaje.Contains("Exito"))
            {
                this.PopMensaje("Datos guardados correctamente", "Exito");
            }
            else if (nBL.Mensaje.Contains("Error"))
            {
                this.PopMensaje("No se pudo guardar la información, " + nBL.Mensaje.Replace("Error", ""), "Error", controles.Opciones.SoloAceptar, controles.Icono.error);
            }
            this.NormaID = nBL.Entidad.NormaID;
            CargarNorma((int)nBL.Entidad.NormaID);
        }
        protected void wucNormaCaptura_Cancelar(object sender, EventArgs e)
        {
            if (this.NormaID != null)
            {
                pnlNormaConfiguracion.Visible = true;
                upConfiguraciones.Update();
            }
        }
        protected void btnBackPage_Click(object sender, EventArgs e)
        {
            Session["NormaID_Editar"] = null;
            Response.Redirect("~/Norma.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }
        protected void lnkbNuevaNormaConfiguracion_Click(object sender, EventArgs e)
        {
            wucNormaConfiguracion.AllowGroup = NivelType != enumNivel.Simple;
            //if (NivelType == enumNivel.ValorInstantaneo)
            //{
            //    wucNormaConfiguracion.AllowGroup = false;
            //}
            if (NivelType == enumNivel.Mensual)
            {
                this.PopMensaje("No se pueden agregar parámetros que no existan en la muestra compuesta.", "Configuración de norma", controles.Opciones.SoloAceptar, controles.Icono.information);
                return;
            }
            wucNormaConfiguracion.Mostrar((int)this.NormaID, wucFiltroNormaConfiguracion.Filtro.CuerpoReceptorID, wucFiltroNormaConfiguracion.Filtro.UsoAguaID, this.NivelType.ConvertToInt());
        }
        protected void tcConfiguraciones_ActiveTabChanged(object sender, EventArgs e)
        {
            wucListaNormaConfiguracionSimpleCompuesta.Mostrar((int)this.NormaID, wucFiltroNormaConfiguracion.Filtro.CuerpoReceptorID, wucFiltroNormaConfiguracion.Filtro.UsoAguaID, this.NivelType.ConvertToInt());
            wucListaNormaConfiguracionCompuestaMensual.Mostrar((int)this.NormaID, wucFiltroNormaConfiguracion.Filtro.CuerpoReceptorID, wucFiltroNormaConfiguracion.Filtro.UsoAguaID);
            //wucListaNormaConfiguracionInstantaneo.Mostrar((int)this.NormaID, wucFiltroNormaConfiguracion.Filtro.CuerpoReceptorID, wucFiltroNormaConfiguracion.Filtro.UsoAguaID, this.NivelType.ConvertToInt());

        }
        protected void wucListaNormaConfiguracionSimpleCompuesta_CheckItem(object sender, EventArgs e)
        {
            upConfiguraciones.Update();
        }
        protected void wucListaNormaConfiguracionCompuestaMensual_EvalItem(object sender, EventArgs e)
        {
            upConfiguraciones.Update();
        }

        #endregion Norma

        #region NormaConfiguracion

        #region Búsqueda
        protected void wucFiltroNormaConfiguracion_After_Load(object sender, EventArgs e)
        {
            if (this.NormaID != null)
            {
                wucListaNormaConfiguracion.Mostrar((int)this.NormaID, wucFiltroNormaConfiguracion.Filtro.CuerpoReceptorID, wucFiltroNormaConfiguracion.Filtro.UsoAguaID, enumNivel.Simple.ConvertToInt());
                wucListaNormaConfiguracion1.Mostrar((int)this.NormaID, wucFiltroNormaConfiguracion.Filtro.CuerpoReceptorID, wucFiltroNormaConfiguracion.Filtro.UsoAguaID, enumNivel.Compuesta.ConvertToInt());
                wucListaNormaConfiguracionSimpleCompuesta.Mostrar((int)this.NormaID, wucFiltroNormaConfiguracion.Filtro.CuerpoReceptorID, wucFiltroNormaConfiguracion.Filtro.UsoAguaID, enumNivel.Compuesta.ConvertToInt());
                wucListaNormaConfiguracionCompuestaMensual.Mostrar((int)this.NormaID, wucFiltroNormaConfiguracion.Filtro.CuerpoReceptorID, wucFiltroNormaConfiguracion.Filtro.UsoAguaID);
                upConfiguraciones.Update();
            }
        }
        protected void wucFiltroNormaConfiguracion_Buscar(object sender, FiltroNormaConfiguracionArg normaCfg)
        {
            normaCfg.NivelID = this.NivelType.ConvertToInt();

            wucListaNormaConfiguracion.Mostrar((int)this.NormaID, wucFiltroNormaConfiguracion.Filtro.CuerpoReceptorID, wucFiltroNormaConfiguracion.Filtro.UsoAguaID, enumNivel.Simple.ConvertToInt());
            wucListaNormaConfiguracion1.Mostrar((int)this.NormaID, wucFiltroNormaConfiguracion.Filtro.CuerpoReceptorID, wucFiltroNormaConfiguracion.Filtro.UsoAguaID, enumNivel.Compuesta.ConvertToInt());
            wucListaNormaConfiguracionSimpleCompuesta.Mostrar((int)this.NormaID, wucFiltroNormaConfiguracion.Filtro.CuerpoReceptorID, wucFiltroNormaConfiguracion.Filtro.UsoAguaID, enumNivel.Compuesta.ConvertToInt());
            wucListaNormaConfiguracionCompuestaMensual.Mostrar((int)this.NormaID, wucFiltroNormaConfiguracion.Filtro.CuerpoReceptorID, wucFiltroNormaConfiguracion.Filtro.UsoAguaID);
            //wucListaNormaConfiguracionInstantaneo.Mostrar((int)this.NormaID, wucFiltroNormaConfiguracion.Filtro.CuerpoReceptorID, wucFiltroNormaConfiguracion.Filtro.UsoAguaID, enumNivel.ValorInstantaneo.ConvertToInt());
            upConfiguraciones.Update();
        }
        protected void wucFiltroNormaConfiguracion_Limpiar(object sender, FiltroNormaConfiguracionArg normaCfg)
        {

        }
        #endregion

        #region CRUD
        protected void wucNormaConfiguracion_Cancelar(object sender, EventArgs e)
        {

        }
        protected void wucNormaConfiguracion_Guardar(object sender, NormaConfiguracionArgs e)
        {
            e.NivelID = this.NivelType.ConvertToInt();

            /* Acá vamos a guardar la configuración y tenemos que recargar el listado en la pantalla. */
            NormaConfiguracion_BL ncBL = new NormaConfiguracion_BL();

            ncBL.Guardar(e);

            if (ncBL.Entidad == null && ncBL.Mensaje != "" && !ncBL.Mensaje.Contains("Error"))
            {
                this.PopMensaje(ncBL.Mensaje, "Configuración de Norma");
            }
            else
            {
                pnlNormaConfiguracion.Visible = true;
                wucListaNormaConfiguracion.Mostrar((int)this.NormaID, wucFiltroNormaConfiguracion.Filtro.CuerpoReceptorID, wucFiltroNormaConfiguracion.Filtro.UsoAguaID, enumNivel.Simple.ConvertToInt());
                wucListaNormaConfiguracion1.Mostrar((int)this.NormaID, wucFiltroNormaConfiguracion.Filtro.CuerpoReceptorID, wucFiltroNormaConfiguracion.Filtro.UsoAguaID, enumNivel.Compuesta.ConvertToInt());
                //wucListaNormaConfiguracionInstantaneo.Mostrar((int)this.NormaID, wucFiltroNormaConfiguracion.Filtro.CuerpoReceptorID, wucFiltroNormaConfiguracion.Filtro.UsoAguaID, enumNivel.ValorInstantaneo.ConvertToInt());
                upConfiguraciones.Update();
            }

            if (ncBL.Mensaje.Contains("Exito"))
            {
                this.PopMensaje("Datos guardados correctamente.", "Exitoso");

                ScriptManager.RegisterClientScriptBlock(this.Page, typeof(Page), "ShowModal", "$('#modalConfiguracion').modal('hide');", true);

                //Actualizar();

            }
            else if (ncBL.Mensaje.Contains("Error"))
            {
                this.PopMensaje("No se pudo guardar la información, " + ncBL.Mensaje.Replace("Error", ""), "Error", controles.Opciones.SoloAceptar, controles.Icono.error);
            }
        }
        protected void wucListaNormaConfiguracion_Editar(object sender, NormaConfiguracionItemArg e)
        {
            NormaConfiguracion_BL ncBL = new NormaConfiguracion_BL();
            var ncItem = ncBL.Obtener(e.NormaConfiguracionID);
            var NormaConfiguracionID = e.NormaConfiguracionID;
            switch (e.Accion)
            {
                case NormaConfiguracionItemArg.enumAccion.Editar:
                    if (this.NivelType == enumNivel.Simple)
                    {
                        wucNormaConfiguracion.AllowGroup = false;
                    }
                    if (this.NivelType == enumNivel.Compuesta)
                    {
                        wucNormaConfiguracion.AllowGroup = true;
                    }
                    //if (this.NivelType == enumNivel.ValorInstantaneo)
                    //{
                    //    wucNormaConfiguracion.AllowGroup = false;
                    //}
                    if (ncItem.Grupo)
                        wucGrupoConfiguracion.Mostrar(NormaConfiguracionID, true);
                    else
                        wucNormaConfiguracion.Mostrar(NormaConfiguracionID);

                    break;
                case NormaConfiguracionItemArg.enumAccion.Activar:
                    ncBL.Guardar(NormaConfiguracionID);
                    break;
            }
        }
        protected void wucListaNormaConfiguracion_ModificarOrden(object sender, NormaConfiguracionItemArg e)
        {
            NormaConfiguracion_BL ncBL = new NormaConfiguracion_BL();

            if (e.Accion == NormaConfiguracionItemArg.enumAccion.Bajar || e.Accion == NormaConfiguracionItemArg.enumAccion.Subir)
                ncBL.CambiarOrden(e.NormaConfiguracionID, e.Accion == NormaConfiguracionItemArg.enumAccion.Subir);
            if (e.Accion == NormaConfiguracionItemArg.enumAccion.Activar)
            {
                this.PopMensaje("Está seguro de eliminar el parámetro", "Configuración de Norma", controles.Opciones.SiNo, controles.Icono.warning, "EliminarParametro", new string[] { e.NormaConfiguracionID.ToString() });
                return;
            }

            wucListaNormaConfiguracion.Mostrar((int)this.NormaID, wucFiltroNormaConfiguracion.Filtro.CuerpoReceptorID, wucFiltroNormaConfiguracion.Filtro.UsoAguaID, enumNivel.Simple.ConvertToInt());
            wucListaNormaConfiguracion1.Mostrar((int)this.NormaID, wucFiltroNormaConfiguracion.Filtro.CuerpoReceptorID, wucFiltroNormaConfiguracion.Filtro.UsoAguaID, enumNivel.Compuesta.ConvertToInt());
            //wucListaNormaConfiguracionInstantaneo.Mostrar((int)this.NormaID, wucFiltroNormaConfiguracion.Filtro.CuerpoReceptorID, wucFiltroNormaConfiguracion.Filtro.UsoAguaID, enumNivel.ValorInstantaneo.ConvertToInt());

            upConfiguraciones.Update();
        }
        protected void wucNormaConfiguracion_AgregarGrupo(object sender, EventArgs e)
        {
            wucGrupoConfiguracion.Mostrar((int)this.NormaID, wucFiltroNormaConfiguracion.Filtro.CuerpoReceptorID, wucFiltroNormaConfiguracion.Filtro.UsoAguaID, this.NivelType.ConvertToInt());
        }
        protected void wucGrupoConfiguracion_Guardar(object sender, GrupoConfiguracionArgs e)
        {
            NormaConfiguracion_BL ncBL = new NormaConfiguracion_BL();
            e.NivelID = this.NivelType.ConvertToInt();
            ncBL.Guardar(e);

            if (ncBL.Entidad == null && ncBL.Mensaje != "")
                this.PopMensaje(ncBL.Mensaje, "Configuración de Norma");
            else
            {
                pnlNormaConfiguracion.Visible = true;
                if (wucGrupoConfiguracion.Lista)
                {
                    wucNormaConfiguracion.ActualizarGrupo((int)this.NormaID, wucFiltroNormaConfiguracion.Filtro.CuerpoReceptorID, wucFiltroNormaConfiguracion.Filtro.UsoAguaID, enumNivel.Compuesta.ConvertToInt());
                    wucGrupoConfiguracion.Lista = false;
                }
                else
                {
                    wucNormaConfiguracion.ActualizarGrupo();
                }
                wucListaNormaConfiguracion.Mostrar((int)this.NormaID, wucFiltroNormaConfiguracion.Filtro.CuerpoReceptorID, wucFiltroNormaConfiguracion.Filtro.UsoAguaID, enumNivel.Simple.ConvertToInt());
                wucListaNormaConfiguracion1.Mostrar((int)this.NormaID, wucFiltroNormaConfiguracion.Filtro.CuerpoReceptorID, wucFiltroNormaConfiguracion.Filtro.UsoAguaID, enumNivel.Compuesta.ConvertToInt());
                upConfiguraciones.Update();
            }
        }
        protected void wucListaNormaConfiguracionSimpleCompuesta_Agregar(object sender, AgregarSimplesArgs e)
        {
            if (e.Listado.Count > 0)
            {
                try
                {
                    NormaConfiguracion_BL ncBL = new NormaConfiguracion_BL();
                    foreach (var Item in e.Listado)
                    {
                        ncBL.Guardar(Item);
                    }
                }
                catch
                {

                }

                wucListaNormaConfiguracion1.Mostrar((int)this.NormaID, (int)wucFiltroNormaConfiguracion.Filtro.CuerpoReceptorID, (int)wucFiltroNormaConfiguracion.Filtro.UsoAguaID, this.NivelType.ConvertToInt());
                wucListaNormaConfiguracionSimpleCompuesta.Mostrar((int)this.NormaID, (int)wucFiltroNormaConfiguracion.Filtro.CuerpoReceptorID, (int)wucFiltroNormaConfiguracion.Filtro.UsoAguaID, this.NivelType.ConvertToInt());
                upConfiguraciones.Update();
            }
        }
        protected void wucListaNormaConfiguracionCompuestaMensual_Agregar(object sender, AgregarSimplesArgs e)
        {
            if (e.Listado.Count > 0)
            {
                try
                {
                    NormaConfiguracion_BL ncBL = new NormaConfiguracion_BL();
                    foreach (var Item in e.Listado)
                    {
                        ncBL.Guardar(Item);
                    }
                    this.PopMensaje("Los registros han sido guardados correctamente.", "Configuración de norma");
                }
                catch
                {

                }
                wucListaNormaConfiguracionCompuestaMensual.Mostrar((int)this.NormaID, (int)wucFiltroNormaConfiguracion.Filtro.CuerpoReceptorID, (int)wucFiltroNormaConfiguracion.Filtro.UsoAguaID);
                upConfiguraciones.Update();
            }
        }
        protected void wucListaNormaConfiguracionCompuestaMensual_Activar(object sender, NormaConfiguracionItemArg e)
        {
            this.PopMensaje("Está seguro de eliminar el parámetro", "Configuración de Norma", controles.Opciones.SiNo, controles.Icono.warning, "EliminarParametro", new string[] { e.NormaConfiguracionID.ToString() });
        }
        #endregion

        #endregion NormaConfiguracion
    }

}