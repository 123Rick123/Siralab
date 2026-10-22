using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SIRALAB_BL;
using SIRALAB_DAL;
using SIRALAB_BL.common;
using SIRALAB_BL.catalogos;
using System.ComponentModel;

namespace SIRALAB.Controles
{
    public partial class wucListaNormaConfiguracion : System.Web.UI.UserControl
    {
        [Browsable(true)]
        public event EventHandler<NormaConfiguracionItemArg> ModificarOrden;
        [Browsable(true)]
        public event EventHandler<NormaConfiguracionItemArg> Editar;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Mostrar(int NormaID, int CuerpoReceptorID, int UsoAguaID, int NivelID)
        {
            NormaConfiguracion_BL ncBL = new NormaConfiguracion_BL();
            var Configuracion = ncBL.ObtenerNormaConfiguracion(NormaID, CuerpoReceptorID, UsoAguaID, NivelID).Select(p => new NormaConfiguracionItem
            {
                NormaConfiguracionID = p.NormaConfiguracionID,
                NormaConfiguracionPadreID = p.NormaConfiguracionPadreID,
                NormaID = p.NormaID,
                ParametroID = p.ParametroID,
                CuerpoReceptorID = p.CuerpoReceptorID,
                UsoAguaID = p.UsoAguaID,
                NivelID = p.NivelID,
                Grupo = p.Grupo,
                Orden = p.Orden,
                NombreParametro = p.ParametroNombre,
                NombreGrupo = p.NormaConfiguracionPadreID != null ? p.NormaConfiguracion2.Grupo ? p.NormaConfiguracion2.Parametro.Nombre : "-" : "-",
                AceptaValor = p.AceptaValor,
                Limite = p.Evalua ? p.AceptaValor ? (p.LimiteInferior == null ? p.LimiteSuperior.ToString() : string.Format("[{0} - {1}]", p.LimiteInferior, p.LimiteSuperior)) : "Presencia" : "No evalúa",
                LimiteLFD = p.LFD ? p.Evalua ? p.AceptaValor ? (p.LimiteInferiorLFD == null ? p.LimiteSuperiorLFD.ToString() : string.Format("[{0} - {1}]", p.LimiteInferiorLFD, p.LimiteSuperiorLFD)) : "Presencia" : "No evalúa" : "-",
                Exenta = p.Exenta,
                Activo = p.Activo,
                Padre = p.NormaConfiguracion2,
                Truncamiento = p.Truncamiento
            });

            //Recuperar norma, para saber desplazamiento.
            Norma_BL normaBL = new Norma_BL();
            SIRALAB_DAL.Norma norma = normaBL.Obtener((int)NormaID);

            //Corregir desplazamiento si es mayor que el número de parámetros.
            norma.Desplazamiento = norma.Desplazamiento >= Configuracion.Count()
                ? Configuracion.Count()
                : norma.Desplazamiento;

            Configuracion = NivelID == 1 ? agregarParametrosVirtuales(Configuracion, norma) : Configuracion;

            rptrNormaConfiguracion.DataSource = Configuracion;
            rptrNormaConfiguracion.DataBind();
        }

        protected void lnkbEditar_Click(object sender, EventArgs e)
        {
            var NormaConfiguracionID = int.Parse(((LinkButton)sender).CommandArgument);
            Editar?.Invoke(this, new NormaConfiguracionItemArg
            {
                NormaConfiguracionID = NormaConfiguracionID,
                Accion = NormaConfiguracionItemArg.enumAccion.Editar
            });
        }

        protected void lnkbSubir_Click(object sender, EventArgs e)
        {
            var NormaConfiguracionID = int.Parse(((LinkButton)sender).CommandArgument);
            if (ModificarOrden != null)
            {
                ModificarOrden(this, new NormaConfiguracionItemArg
                {
                    NormaConfiguracionID = NormaConfiguracionID,
                    Accion = NormaConfiguracionItemArg.enumAccion.Subir
                });
            }
        }

        protected void lnkbBajar_Click(object sender, EventArgs e)
        {
            var NormaConfiguracionID = int.Parse(((LinkButton)sender).CommandArgument);
            if (ModificarOrden != null)
            {
                ModificarOrden(this, new NormaConfiguracionItemArg
                {
                    NormaConfiguracionID = NormaConfiguracionID,
                    Accion = NormaConfiguracionItemArg.enumAccion.Bajar
                });
            }
        }

        protected void lnkbEliminar_Click(object sender, EventArgs e)
        {
            var NormaConfiguracionID = int.Parse(((LinkButton)sender).CommandArgument);
            if (ModificarOrden != null)
            {
                ModificarOrden(this, new NormaConfiguracionItemArg
                {
                    NormaConfiguracionID = NormaConfiguracionID,
                    Accion = NormaConfiguracionItemArg.enumAccion.Activar
                });
            }
        }

        protected void rptrNormaConfiguracion_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var lnk = (LinkButton)e.Item.FindControl("lnkbActivar");
            var item = (NormaConfiguracionItem)e.Item.DataItem;
            if (lnk != null)
            {
                //lnk.CssClass = item.Activo ? "icono-okgreen-16" : "icono-cerrarrojo-16";
                //lnk.ToolTip = item.Activo ? "Click para desactivar." : "Click para activar.";

                if (item.NormaConfiguracionPadreID != null && !item.Grupo && !item.Padre.Grupo)
                {
                    var lnkbEditar = (LinkButton)e.Item.FindControl("lnkbEditar");
                    //lnkbEditar.Visible = false;
                }
            }

            if (item != null)
            {
                var lnkbSubir = (LinkButton)e.Item.FindControl("lnkbSubir");
                var lnkbBajar = (LinkButton)e.Item.FindControl("lnkbBajar");
                var lnkbEditar = (LinkButton)e.Item.FindControl("lnkbEditar");
                var lnkbEliminar = (LinkButton)e.Item.FindControl("lnkbEliminar");
                var lblbInfo = (Label)e.Item.FindControl("lblbInfo");

                lnkbSubir.Visible = item.ParametroID > 0 && item.Orden > 1;
                lnkbBajar.Visible = item.ParametroID > 0;
                lnkbEditar.Visible = item.ParametroID > 0;
                lnkbEliminar.Visible = item.ParametroID > 0;
                lblbInfo.Visible = item.ParametroID == 0;
            }


        }

        private List<NormaConfiguracionItem> agregarParametrosVirtuales(
            IEnumerable<NormaConfiguracionItem> configuracion, SIRALAB_DAL.Norma norma)
        {

            var caudal = new NormaConfiguracionItem
            {
                NormaConfiguracionID = 0,
                NormaConfiguracionPadreID = 0,
                NormaID = norma.NormaID,
                ParametroID = 0,
                CuerpoReceptorID = 0,
                UsoAguaID = 0,
                NivelID = 1,
                Grupo = false,
                Orden = 0,
                NombreParametro = "Caudal (l/s)",
                NombreGrupo = "-",
                AceptaValor = true,
                Limite = "No Aplica",
                LimiteLFD = "-",
                Exenta = false,
                Activo = true,
                Padre = null
            };

            var participacionCaudal = new NormaConfiguracionItem
            {
                NormaConfiguracionID = 0,
                NormaConfiguracionPadreID = 0,
                NormaID = norma.NormaID,
                ParametroID = 0,
                CuerpoReceptorID = 0,
                UsoAguaID = 0,
                NivelID = 1,
                Grupo = false,
                Orden = 0,
                NombreParametro = "Participación del Caudal (Qi/Qt)",
                NombreGrupo = "-",
                AceptaValor = true,
                Limite = "No Aplica",
                LimiteLFD = "-",
                Exenta = false,
                Activo = true,
                Padre = null
            };

            var volMuestraSimple = new NormaConfiguracionItem
            {
                NormaConfiguracionID = 0,
                NormaConfiguracionPadreID = 0,
                NormaID = norma.NormaID,
                ParametroID = 0,
                CuerpoReceptorID = 0,
                UsoAguaID = 0,
                NivelID = 1,
                Grupo = false,
                Orden = 0,
                NombreParametro = "Volumen de Muestra Simple",
                NombreGrupo = "-",
                AceptaValor = true,
                Limite = "No Aplica",
                LimiteLFD = "-",
                Exenta = false,
                Activo = true,
                Padre = null
            };

            var configuracionConCaudal = new List<NormaConfiguracionItem>();
            NormaConfiguracionItem[] arrayConfiguracion = configuracion.ToArray();
            if (configuracion.Count() > 0)
            {
                for (var i = 0; i < configuracion.Count(); i++)
                {
                    if (i == 0 && norma.RequiereCaudal) //Agrega caudal a la lista.
                        configuracionConCaudal.Add(caudal);

                    if (norma.RequiereCompuesta && norma.Desplazamiento != null && i == norma.Desplazamiento) //Agrega participación del caudal y volumen de muestra simple
                    {
                        configuracionConCaudal.Add(participacionCaudal);
                        configuracionConCaudal.Add(volMuestraSimple);
                    }

                    configuracionConCaudal.Add(configuracion.FirstOrDefault(c => c.Orden == arrayConfiguracion[i].Orden)); //RGG
                    if (norma.RequiereCompuesta && norma.Desplazamiento != null && norma.Desplazamiento == configuracion.Count() && norma.Desplazamiento == i + 1) //Cuando se ubican en la última posición.
                    {
                        configuracionConCaudal.Add(participacionCaudal);
                        configuracionConCaudal.Add(volMuestraSimple);
                    }
                }
            }
            else
            {
                if (norma.RequiereCaudal)
                {
                    configuracionConCaudal.Add(caudal);

                    if (norma.RequiereCompuesta)
                    {
                        configuracionConCaudal.Add(participacionCaudal);
                        configuracionConCaudal.Add(volMuestraSimple);
                    }
                }
            }
            return configuracionConCaudal;
        }
    }

    public class NormaConfiguracionItemArg : EventArgs
    {
        public int NormaConfiguracionID { get; set; }
        public enumAccion Accion { get; set; }
        public enum enumAccion
        {
            Subir,
            Bajar,
            Editar,
            Activar
        }
    }
    public class NormaConfiguracionItem
    {
        public SIRALAB_DAL.NormaConfiguracion Padre { get; set; }
        public int NormaConfiguracionID { get; set; }
        public int? NormaConfiguracionPadreID { get; set; }
        public int NormaID { get; set; }
        public int ParametroID { get; set; }
        public int CuerpoReceptorID { get; set; }
        public int UsoAguaID { get; set; }
        public int NivelID { get; set; }
        public int? OperacionID { get; set; }
        public int Orden { get; set; }
        public string NombreParametro { get; set; }
        public string NombreGrupo { get; set; }
        public string NombreOperacion { get; set; }
        public bool AceptaValor { get; set; }
        public string Limite { get; set; }
        public decimal? LimiteInferior { get; set; }
        public decimal? LimiteSuperior { get; set; }
        public string LimiteLFD { get; set; }
        public decimal? LimiteInferiorLFD { get; set; }
        public decimal? LimiteSuperiorLFD { get; set; }
        public bool Exenta { get; set; }
        public bool Evalua { get; set; }
        public bool Activo { get; set; }
        public bool LFD { get; set; }
        public bool Grupo { get; set; }
        public bool? Truncamiento { get; set; }

    }
}