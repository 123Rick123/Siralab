using SIRALAB_BL;
using SIRALAB_DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SIRALAB.Controles
{
    public partial class wucConductividadElectrica : System.Web.UI.UserControl
    {
        public int OrdenTrabajoID
        {
            get { return (int)Session["OrdenTrabajoID_"] == 0 ? 0 : (int)Session["OrdenTrabajoID_"]; }
            set { Session["OrdenTrabajoID_"] = value; }
        }

        public int AnalisisID
        {
            get { return (int)Session["AnalisisID_"] == 0 ? 0 : (int)Session["AnalisisID_"]; }
            set { Session["AnalisisID_"] = value; }
        }

        public Analisi Analisis_
        {
            get { return (SIRALAB_DAL.Analisi)Session["AnalisisMuestra_"]; }
        }

        public string SesionEcoliEnterococos
        {
            get { return (string)Session["SesionEcoliEnterococos"]; }
        }

        public Muestra Muestra
        {
            get { return (SIRALAB_DAL.Muestra)Session["MuestraMuestra_"]; }
        }

        [Browsable(true)]
        public event EventHandler<Dictionary<bool, string>> CheckEcoliEnterococos;
        protected void Page_Load(object sender, EventArgs e)
        {
            wucMuestrasSimples1.CheckEcoliEnterococos += wucMuestrasSimples1_CheckEcoliEnterococos;
            if (!IsPostBack)
            {
                Analisis_BL analisisBL = new Analisis_BL();
                var an = analisisBL.Obten_Analisis(AnalisisID);

                if (an.Conductividad != string.Empty && an.Conductividad != null)
                {
                    if (an.Conductividad == "Enterococos")
                    {
                        chkEnterococos.Checked = true;

                        chkEColi.Checked = false;
                        chkEnterococos_CheckedChanged(this,null);
                    }
                    else if (an.Conductividad == "Escherichia")
                    {
                        chkEnterococos.Checked = false;
              
                        chkEColi.Checked = true;
                        chkEColi_CheckedChanged(this, null);
                    }
                }



            }


        }
        protected void chkEColi_CheckedChanged(object sender, EventArgs e)
        {
            Dictionary<bool, string> valores;
            if (chkEColi.Checked)
            {
                chkEnterococos.Checked = false;
                wucMuestrasSimples1.ChkEcoli(chkEColi.Checked);
                valores = ValidaEColi();
                hfConductividad.Value = "Escherichia";
                if (valores.First().Key && valores.First().Value == "SinFecha")
                {
                    hfGuardaConductividad.Value = "true";
                    CheckEcoliEnterococos(this, valores);
                }
                else
                {
                    CheckEcoliEnterococos(this, valores);
                   
                    if (valores.First().Key)
                    {
                        hfGuardaConductividad.Value = "false";
                    }
                    else
                    {
                        hfGuardaConductividad.Value = "true";
                    }
                }
            }
            else if (this.Analisis_.OrdenTrabajo.Analisis.Any())
            {
                chkEnterococos.Checked = true;
                wucMuestrasSimples1.ChkEnterococos(chkEnterococos.Checked);
                valores = ValidaEnterococos();
                hfConductividad.Value = "Enterococos";
                if (valores.First().Key && valores.First().Value == "SinFecha")
                {
                    hfGuardaConductividad.Value = "true";
                    CheckEcoliEnterococos(this, valores);
                }
                else
                {
                    CheckEcoliEnterococos(this, valores);

                    if (valores.First().Key)
                    {
                        hfGuardaConductividad.Value = "false";
                    }
                    else
                    {
                        hfGuardaConductividad.Value = "true";
                    }
                }
            }
            else
            {
                wucMuestrasSimples1.ChkEcoli(chkEColi.Checked);
            }
        }
        protected void chkEnterococos_CheckedChanged(object sender, EventArgs e)
        {
            Dictionary<bool, string> valores;
            if (chkEnterococos.Checked)
            {
                chkEColi.Checked = false;
                wucMuestrasSimples1.ChkEnterococos(chkEnterococos.Checked);
                valores = ValidaEnterococos();
                hfConductividad.Value = "Enterococos";
                if (valores.First().Key && valores.First().Value == "SinFecha")
                {
                    hfGuardaConductividad.Value = "true";
                    CheckEcoliEnterococos(this, valores);
                }
                else
                {
                    CheckEcoliEnterococos(this, valores);

                    if (valores.First().Key)
                    {
                        hfGuardaConductividad.Value = "false";
                    }
                    else
                    {
                        hfGuardaConductividad.Value = "true";
                    }
                }
            }
            else if (this.Analisis_.OrdenTrabajo.Analisis.Any())
            {
                chkEColi.Checked = true;
                wucMuestrasSimples1.ChkEcoli(chkEColi.Checked);
                valores = ValidaEColi();
                hfConductividad.Value = "Escherichia";
                if (valores.First().Key && valores.First().Value == "SinFecha")
                {
                    hfGuardaConductividad.Value = "true";
                    CheckEcoliEnterococos(this, valores);
                }
                else
                {
                    CheckEcoliEnterococos(this, valores);

                    if (valores.First().Key)
                    {
                        hfGuardaConductividad.Value = "false";
                    }
                    else
                    {
                        hfGuardaConductividad.Value = "true";
                    }
                }
            }
            else
            {
                wucMuestrasSimples1.ChkEnterococos(chkEnterococos.Checked);
            }
        }
        public Dictionary<bool, string> ValidaEColi()
        {
            Siralab_Entities siralab_Entities = new Siralab_Entities();
            var valores = new Dictionary<bool, string>();
            Analisis_BL analisisBLL = new Analisis_BL();
            var an = analisisBLL.Obten_Analisis(AnalisisID);
            if (an.Muestras.First().FechaConformacion == null)
            {
                valores.Add(true, "SinFecha");
            }
            else 
            {
                var fechaConformacion = an.Muestras.First().FechaConformacion.Value;
                int trimestreMuestra = (int)Math.Ceiling((double)fechaConformacion.Month / 3);// validar que fecha poner 
                var query = from ot in siralab_Entities.OrdenTrabajoes
                            join _a in siralab_Entities.Analisis on ot.OrdenTrabajoID equals _a.OrdenTrabajoID
                            join m in siralab_Entities.Muestras on _a.AnalisisID equals m.AnalisisID
                            join n in siralab_Entities.Normas on ot.NormaID equals n.NormaID
                            where ot.NormaID == 15 && ot.ClienteID == an.OrdenTrabajo.ClienteID
                            && m.FechaConformacion.Value.Year == fechaConformacion.Year // validar que fecha poner
                            && _a.Conductividad != null
                            && _a.EstatusID != 4 && ot.ConsecionID == an.OrdenTrabajo.ConsecionID
                            && ot.AnexoID == this.Analisis_.OrdenTrabajo.AnexoID
                            orderby _a.AnalisisID descending
                            select new
                            {
                                _a.AnalisisID,
                                Fecha = m.FechaConformacion,// validar que fecha poner 
                                Trimestre = (int)Math.Ceiling((double)m.FechaConformacion.Value.Month / 3),// validar que fecha poner 
                                _a.Conductividad
                            };
                
                if (query != null)
                {
                    var prueba = query.Where(x => x.Trimestre == trimestreMuestra && x.AnalisisID != this.AnalisisID).FirstOrDefault();
                    if (prueba != null)
                    {
                        valores.Add(prueba.Conductividad == "Enterococos", "Escherichia");
                        hfEColiEntero.Value = valores.First().Key ? "Enterococos" : "Escherichia";
                    }
                    else
                    {
                        valores.Add(false, "");
                        hfEColiEntero.Value = "Escherichia";
                    }
                }
                else
                {
                    valores.Add(false, "");
                    hfEColiEntero.Value = "Escherichia";
                }
            }
            return valores;
        }
        public Dictionary<bool, string> ValidaEnterococos()
        {
            Siralab_Entities siralab_Entities = new Siralab_Entities();
            var valores = new Dictionary<bool, string>();
            Analisis_BL analisisBLL = new Analisis_BL();
            var an = analisisBLL.Obten_Analisis(AnalisisID);
            if (an.Muestras.First().FechaConformacion == null)
            {
                valores.Add(true, "SinFecha");
            }
            else
            {
                var fechaConformacion = an.Muestras.First().FechaConformacion.Value;
                int trimestreMuestra = (int)Math.Ceiling((double)fechaConformacion.Month / 3);// validar que fecha poner 
                var query = from ot in siralab_Entities.OrdenTrabajoes
                            join _a in siralab_Entities.Analisis on ot.OrdenTrabajoID equals _a.OrdenTrabajoID
                            join m in siralab_Entities.Muestras on _a.AnalisisID equals m.AnalisisID
                            join n in siralab_Entities.Normas on ot.NormaID equals n.NormaID
                            where ot.NormaID == 15 && ot.ClienteID == an.OrdenTrabajo.ClienteID
                            && m.FechaConformacion.Value.Year == fechaConformacion.Year // validar que fecha poner
                            && _a.Conductividad != null
                            && _a.EstatusID != 4 && ot.ConsecionID == an.OrdenTrabajo.ConsecionID
                            && ot.AnexoID == an.OrdenTrabajo.AnexoID
                            orderby _a.AnalisisID descending
                            select new
                            {
                                _a.AnalisisID,
                                Fecha = m.FechaConformacion,// validar que fecha poner 
                                Trimestre = (int)Math.Ceiling((double)m.FechaConformacion.Value.Month / 3),// validar que fecha poner 
                                _a.Conductividad
                            };

                if (query != null)
                {
                    var prueba = query.Where(x => x.Trimestre == trimestreMuestra && x.AnalisisID != this.AnalisisID).FirstOrDefault();
                    if (prueba != null)
                    {
                        valores.Add(prueba.Conductividad == "Escherichia", "Enterococos");
                        hfEColiEntero.Value = valores.First().Key ? "Escherichia" : "Enterococos";
                    }
                    else
                    {
                        valores.Add(false, "");
                        hfEColiEntero.Value = "Enterococos";
                    }
                }
                else
                {
                    valores.Add(false, "");
                    hfEColiEntero.Value = "Enterococos";
                }
            }
            return valores;
        }

        protected void wucMuestrasSimples1_CheckEcoliEnterococos(object sender, Dictionary<bool, string> e)
        {
            CheckEcoliEnterococos?.Invoke(sender, e);
        }



    }
}
