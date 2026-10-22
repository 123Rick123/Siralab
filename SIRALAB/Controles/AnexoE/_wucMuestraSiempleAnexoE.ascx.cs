using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using SIRALAB_BL;
using SIRALAB_BL.catalogos;
using SIRALAB_BL.common;
using SIRALAB_DAL;



namespace SIRALAB.Controles.AnexoE
{
    public partial class _wucMuestraSiempleAnexoE : UserControl
    {
        public SIRALAB_DAL.OrdenTrabajo OrdenTrabajo
        {
            get { return (SIRALAB_DAL.OrdenTrabajo)Session["OrdenTrabajoConsulta_"]; }
        }
        public SIRALAB_DAL.Muestra Muestra
        {
            get { return (SIRALAB_DAL.Muestra)Session["MuestraMuestra_"]; }
        }
        public SIRALAB_DAL.Analisi Analisis
        {
            get { return (SIRALAB_DAL.Analisi)Session["AnalisisMuestra_"]; }
        }
        public Usuario Login
        {
            get { return (Usuario)Session["UsuarioLog"]; }
        }

        public LaboratorioSignatario Signatario_Laboratorio_Persona
        {
            get { return (LaboratorioSignatario)Session["Signatario_Laboratorio_Persona_"]; }
        }
        [Browsable(true)]
        public event EventHandler<MuestraCapturaArgs> Guardar;
        [Browsable(true)]
        public event EventHandler<bool> MsIncompleta;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ce_txtMuestraFechaMuestreo.EndDate = DateTime.Now;
                CalendarExtender1.EndDate = DateTime.Now;
                CalendarExtender2.EndDate = DateTime.Now;
                CalendarExtender3.EndDate = DateTime.Now;
                CalendarExtender4.EndDate = DateTime.Now;
                CalendarExtender5.EndDate = DateTime.Now;
                CalendarExtender6.EndDate = DateTime.Now;
            }
        }

        readonly Muestra_BL _muestraBl = new Muestra_BL();
        readonly MuestraValor_BL _muestravalorBl = new MuestraValor_BL();
        List<MuestraValorArgBase_BL> _list = new List<MuestraValorArgBase_BL>();
        readonly Metodo_BL _metBl = new Metodo_BL();
        readonly Laboratorio_BL _labBl = new Laboratorio_BL();

        private void Muestra2(int analisisId)
        {
            var listaMuestra = _muestraBl.Lista_Muestra(analisisId).Where(x => x.NivelID == 1).ToList();
            TextBox1.Text = listaMuestra[0].Fecha != null ? DateTime.Parse(listaMuestra[0].Fecha.ToString()).ToShortDateString() : "";
            TextBox2.Text = listaMuestra[0].Fecha != null ? DateTime.Parse(listaMuestra[0].Fecha.ToString()).ToString("HH:mm") : "";
            TextBox8.Text = listaMuestra[0].Observaciones;
            TextBox9.Text = listaMuestra[0].ValMuestraC == null ? "0" : listaMuestra[0].ValMuestraC.ToString();

            //if (listaMuestra[0].ValHojaCampo == null || listaMuestra[0].ValHojaCampo == 0)
            //{
            //    CheckBox1.Checked = false;
            //    //Button2.Visible = false;
            //    //Gtemp.Visible = true;
            //    //lnkbtnMuestraCompuesta.Visible = false;
            //}
            //else
            //{
            //    //Button2.Visible = true;
            //    //Gtemp.Visible = false;
            //    CheckBox1.Checked = true;
            //    CheckBox1.Enabled = false;
            //    //lnkbtnMuestraCompuesta.Visible = true;
            //}
            if (TextBox1.Text != "" && TextBox2.Text != "")
                CA1.Text = listaMuestra[0].Caudal != 0 ? listaMuestra[0].Caudal.ToString("######.##") : "0";
            else
                CA1.Text = "";

            //TextBox3.Text = TextBox1.Text != "" ? listaMuestra[0].Caudal.ToString("######.##") : "";
            TextBox10.Text = listaMuestra[0].FechaRecepcion != null ? DateTime.Parse(listaMuestra[0].FechaRecepcion.ToString()).ToShortDateString() : "";
            TextBox11.Text = listaMuestra[0].Fecha != null ? DateTime.Parse(listaMuestra[0].FechaRecepcion.ToString()).ToString("HH:mm") : "";

            TextBox12.Text = listaMuestra[1].Fecha != null ? DateTime.Parse(listaMuestra[1].Fecha.ToString()).ToShortDateString() : "";
            TextBox13.Text = listaMuestra[1].Fecha != null ? DateTime.Parse(listaMuestra[1].Fecha.ToString()).ToString("HH:mm") : "";
            TextBox20.Text = listaMuestra[1].Observaciones;
            if (TextBox12.Text != "" && TextBox13.Text != "")
                CA2.Text = listaMuestra[1].Caudal != 0 ? listaMuestra[1].Caudal.ToString("######.##") : "0";
            else
                CA2.Text = "";
            //TextBox14.Text = TextBox12.Text != "" ? listaMuestra[1].Caudal.ToString("######.##") : "";

            var counts = 0;
            foreach (var t in listaMuestra)
            {
                if (OrdenTrabajo.NormaID != null)
                    _list = t.NivelID == (decimal)enumNivel.Simple ? _muestravalorBl.GuardaObten((int)OrdenTrabajo.NormaID, t.MuestraID, t.NivelID, false) : _muestravalorBl.GuardaObtenCompuesta((int)OrdenTrabajo.NormaID, t.MuestraID);
                counts++;

                foreach (var it in _list.Select(t1 => _list.FirstOrDefault(p => p.MuestraValorID == t1.MuestraValorID)))
                {
                    switch (counts)
                    {
                        case 1:
                            switch (it.ParametroID)
                            {
                                case 862:
                                    PH1.Text = it.Valor != 0 ? it.Valor.ToString("####.##") : "0";
                                    if (it.Exento == false && it.NoAnalizado == false)
                                        ddlAction1.SelectedValue = "3";
                                    if (it.Exento == true)
                                        ddlAction1.SelectedValue = "1";
                                    if (it.NoAnalizado == true)
                                        ddlAction1.SelectedValue = "2";
                                    break;
                                case 118:
                                    T1.Text = it.Valor != 0 ? it.Valor.ToString("##.#") : "0";
                                    if (it.Exento == false && it.NoAnalizado == false)
                                        ddlAction2.SelectedValue = "3";
                                    if (it.Exento == true)
                                        ddlAction2.SelectedValue = "1";
                                    if (it.NoAnalizado == true)
                                        ddlAction2.SelectedValue = "2";
                                    break;
                                case 225:
                                    CF1.Text = it.Valor != 0 ? it.Valor.ToString("####") : "0";
                                    var a52 = it.LC_MC != 0 ? it.LC_MC.ToString() : "0";
                                    string[] split = a52.Split(new[] { '.' });
                                    lcmc1.Text = split[0];

                                    if (it.Exento == false && it.NoAnalizado == false)
                                        ddlAction4.SelectedValue = "3";
                                    if (it.Exento == true)
                                        ddlAction4.SelectedValue = "1";
                                    if (it.NoAnalizado == true)
                                        ddlAction4.SelectedValue = "2";
                                    break;
                                case 79:
                                    GA1.Text = it.Valor != 0 ? it.Valor.ToString("####.####") : "0";
                                    var a53 = it.LC_MC != 0 ? it.LC_MC.ToString() : "0";
                                    string[] split1 = a53.Split(new[] { '.' });
                                    if (split1.Count() > 1)
                                        lcmc7.Text = split1[1] == "000000" ? split1[0] : Convert.ToDecimal(a53).ToString("####.####");
                                    else
                                        lcmc7.Text = split1[0];

                                    if (it.Exento == false && it.NoAnalizado == false)
                                        ddlAction5.SelectedValue = "3";
                                    if (it.Exento == true)
                                        ddlAction5.SelectedValue = "1";
                                    if (it.NoAnalizado == true)
                                        ddlAction5.SelectedValue = "2";
                                    break;
                                case 89:
                                    ddlana1.SelectedValue = it.ValorValidacion == null ? "true" : it.ValorValidacion.ToString().ToLower();

                                    if (it.Exento == false && it.NoAnalizado == false)
                                        ddlAction3.SelectedValue = "3";
                                    if (it.Exento == true)
                                        ddlAction3.SelectedValue = "1";
                                    if (it.NoAnalizado == true)
                                        ddlAction3.SelectedValue = "2";
                                    break;
                            }
                            break;
                        case 2:
                            switch (it.ParametroID)
                            {
                                case 862:
                                    PH2.Text = it.Valor != 0 ? it.Valor.ToString("####.##") : "0";
                                    break;
                                case 118:
                                    T2.Text = it.Valor != 0 ? it.Valor.ToString("##.#") : "0";
                                    break;
                                case 225:
                                    CF2.Text = it.Valor != 0 ? it.Valor.ToString("####") : "0";
                                    var a54 = it.LC_MC != 0 ? it.LC_MC.ToString() : "0";
                                    string[] split3 = a54.Split(new[] { '.' });
                                    lcmc2.Text = split3[0];
                                    break;
                                case 79:
                                    GA2.Text = it.Valor != 0 ? it.Valor.ToString("####.####") : "0";
                                    var a55 = it.LC_MC != 0 ? it.LC_MC.ToString() : "0";
                                    string[] split4 = a55.Split(new[] { '.' });
                                    if (split4.Count() > 1)
                                        lcmc8.Text = split4[1] == "000000" ? split4[0] : Convert.ToDecimal(a55).ToString("####.####");
                                    else
                                        lcmc8.Text = split4[0];
                                    break;
                                case 89:
                                    ddlana2.SelectedValue = it.ValorValidacion == null ? "true" : it.ValorValidacion.ToString().ToLower();
                                    break;
                            }
                            break;
                    }
                }

            }
        }
        private void Muestra4(int analisisId)
        {
            var listaMuestra = _muestraBl.Lista_Muestra(analisisId).Where(x => x.NivelID == 1).ToList();


            TextBox17.Text = listaMuestra[2].Fecha != null ? DateTime.Parse(listaMuestra[2].Fecha.ToString()).ToShortDateString() : "";
            TextBox21.Text = listaMuestra[2].Fecha != null ? DateTime.Parse(listaMuestra[2].Fecha.ToString()).ToString("HH:mm") : "";
            TextBox42.Text = listaMuestra[2].Observaciones;
            if (TextBox17.Text != "" && TextBox21.Text != "")
                CA3.Text = listaMuestra[2].Caudal != 0 ? listaMuestra[2].Caudal.ToString("######.##") : "0";
            else
                CA3.Text = "";
            //TextBox22.Text = TextBox17.Text != "" ? listaMuestra[2].Caudal.ToString("######.##") : "";

            TextBox9.Text = listaMuestra[0].ValMuestraC == null ? "0" : listaMuestra[0].ValMuestraC.ToString();


            TextBox25.Text = listaMuestra[3].Fecha != null ? DateTime.Parse(listaMuestra[3].Fecha.ToString()).ToShortDateString() : "";
            TextBox26.Text = listaMuestra[3].Fecha != null ? DateTime.Parse(listaMuestra[3].Fecha.ToString()).ToString("HH:mm") : "";
            TextBox45.Text = listaMuestra[3].Observaciones;
            if (TextBox25.Text != "" && TextBox26.Text != "")
                CA4.Text = listaMuestra[3].Caudal != 0 ? listaMuestra[3].Caudal.ToString("######.##") : "0";
            else
                CA4.Text = "";
            //TextBox27.Text = TextBox25.Text != "" ? listaMuestra[3].Caudal.ToString("######.##") : "";

            var counts = 0;
            foreach (var t in listaMuestra)
            {
                if (OrdenTrabajo.NormaID != null)
                    _list = t.NivelID == (decimal)enumNivel.Simple
                        ? _muestravalorBl.GuardaObten((int)OrdenTrabajo.NormaID, t.MuestraID, t.NivelID, false)
                        : _muestravalorBl.GuardaObtenCompuesta((int)OrdenTrabajo.NormaID, t.MuestraID);

                counts++;
                foreach (var it in _list.Select(t1 => _list.FirstOrDefault(p => p.MuestraValorID == t1.MuestraValorID)))
                {
                    switch (counts)
                    {
                        case 3:
                            switch (it.ParametroID)
                            {
                                case 862:
                                    PH3.Text = it.Valor != 0 ? it.Valor.ToString("####.##") : "0";
                                    break;
                                case 118:
                                    T3.Text = it.Valor != 0 ? it.Valor.ToString("##.#") : "0";
                                    break;
                                case 225:
                                    CF3.Text = it.Valor != 0 ? it.Valor.ToString("####") : "0";
                                    var a56 = it.LC_MC != 0 ? it.LC_MC.ToString() : "0";
                                    string[] split5 = a56.Split(new[] { '.' });
                                    lcmc3.Text = split5[0];
                                    break;
                                case 79:
                                    GA3.Text = it.Valor != 0 ? it.Valor.ToString("####.####") : "0";
                                    var a57 = it.LC_MC != 0 ? it.LC_MC.ToString() : "0";
                                    string[] split6 = a57.Split(new[] { '.' });
                                    if (split6.Count() > 1)
                                        lcmc9.Text = split6[1] == "000000" ? split6[0] : Convert.ToDecimal(a57).ToString("####.####");
                                    else
                                        lcmc9.Text = split6[0];
                                    break;
                                case 89:
                                    ddlana3.SelectedValue = it.ValorValidacion == null ? "true" : it.ValorValidacion.ToString().ToLower();
                                    break;
                            }
                            break;
                        case 4:
                            switch (it.ParametroID)
                            {
                                case 862:
                                    PH4.Text = it.Valor != 0 ? it.Valor.ToString("####.##") : "0";
                                    break;
                                case 118:
                                    T4.Text = it.Valor != 0 ? it.Valor.ToString("##.#") : "0";
                                    break;
                                case 225:
                                    CF4.Text = it.Valor != 0 ? it.Valor.ToString("####") : "0";
                                    var a58 = it.LC_MC != 0 ? it.LC_MC.ToString() : "0";
                                    string[] split7 = a58.Split(new[] { '.' });
                                    lcmc4.Text = split7[0];
                                    break;
                                case 79:
                                    GA4.Text = it.Valor != 0 ? it.Valor.ToString("####.####") : "0";
                                    var a59 = it.LC_MC != 0 ? it.LC_MC.ToString() : "0";
                                    string[] split8 = a59.Split(new[] { '.' });
                                    if (split8.Count() > 1)
                                        l10lcmc.Text = split8[1] == "000000" ? split8[0] : Convert.ToDecimal(a59).ToString("####.####");
                                    else
                                        l10lcmc.Text = split8[0];
                                    break;
                                case 89:
                                    ddlana4.SelectedValue = it.ValorValidacion == null ? "true" : it.ValorValidacion.ToString().ToLower();
                                    break;
                            }
                            break;
                    }
                }

            }
        }
        private void Muestra6(int analisisId)
        {
            var listaMuestra = _muestraBl.Lista_Muestra(analisisId).Where(x => x.NivelID == 1).ToList();
            TextBox30.Text = listaMuestra[4].Fecha != null ? DateTime.Parse(listaMuestra[4].Fecha.ToString()).ToShortDateString() : "";
            TextBox31.Text = listaMuestra[4].Fecha != null ? DateTime.Parse(listaMuestra[4].Fecha.ToString()).ToString("HH:mm") : "";
            TextBox48.Text = listaMuestra[4].Observaciones;
            if (TextBox30.Text != "" && TextBox31.Text != "")
                CA5.Text = listaMuestra[4].Caudal != 0 ? listaMuestra[4].Caudal.ToString("######.##") : "0";
            else
                CA5.Text = "";
            //TextBox32.Text = TextBox30.Text != "" ? listaMuestra[4].Caudal.ToString("######.##") : "";

            TextBox9.Text = listaMuestra[0].ValMuestraC == null ? "0" : listaMuestra[0].ValMuestraC.ToString();

            TextBox35.Text = listaMuestra[5].Fecha != null ? DateTime.Parse(listaMuestra[5].Fecha.ToString()).ToShortDateString() : "";
            TextBox36.Text = listaMuestra[5].Fecha != null ? DateTime.Parse(listaMuestra[5].Fecha.ToString()).ToString("HH:mm") : "";
            TextBox51.Text = listaMuestra[5].Observaciones;
            if (TextBox35.Text != "" && TextBox36.Text != "")
                CA6.Text = listaMuestra[5].Caudal != 0 ? listaMuestra[5].Caudal.ToString("######.##") : "0";
            else
                CA6.Text = "";
            //TextBox37.Text = TextBox35.Text != "" ? listaMuestra[5].Caudal.ToString("######.##") : "";

            var counts = 0;
            foreach (var t in listaMuestra)
            {
                if (OrdenTrabajo.NormaID != null)
                    _list = t.NivelID == (decimal)enumNivel.Simple
                        ? _muestravalorBl.GuardaObten((int)OrdenTrabajo.NormaID, t.MuestraID, t.NivelID, false)
                        : _muestravalorBl.GuardaObtenCompuesta((int)OrdenTrabajo.NormaID, t.MuestraID);

                counts++;
                foreach (var it in _list.Select(t1 => _list.FirstOrDefault(p => p.MuestraValorID == t1.MuestraValorID)))
                {
                    switch (counts)
                    {
                        case 5:
                            switch (it.ParametroID)
                            {
                                case 862:
                                    PH5.Text = it.Valor != 0 ? it.Valor.ToString("####.##") : "0";
                                    break;
                                case 118:
                                    T5.Text = it.Valor != 0 ? it.Valor.ToString("##.#") : "0";
                                    break;
                                case 225:
                                    CF5.Text = it.Valor != 0 ? it.Valor.ToString("####") : "0";
                                    var a60 = it.LC_MC != 0 ? it.LC_MC.ToString() : "0";
                                    string[] split9 = a60.Split(new[] { '.' });
                                    lcmc5.Text = split9[0];
                                    break;
                                case 79:
                                    GA5.Text = it.Valor != 0 ? it.Valor.ToString("####.####") : "0";
                                    var a61 = it.LC_MC != 0 ? it.LC_MC.ToString() : "0";
                                    string[] split10 = a61.Split(new[] { '.' });
                                    if (split10.Count() > 1)
                                        l11lcmc.Text = split10[1] == "000000" ? split10[0] : Convert.ToDecimal(a61).ToString("####.####");
                                    else
                                        l11lcmc.Text = split10[0];
                                    break;
                                case 89:
                                    ddlana5.SelectedValue = it.ValorValidacion == null ? "true" : it.ValorValidacion.ToString().ToLower();
                                    break;
                            }
                            break;
                        case 6:
                            switch (it.ParametroID)
                            {
                                case 862:
                                    PH6.Text = it.Valor != 0 ? it.Valor.ToString("####.##") : "0";
                                    break;
                                case 118:
                                    T6.Text = it.Valor != 0 ? it.Valor.ToString("##.#") : "0";
                                    break;
                                case 225:
                                    CF6.Text = it.Valor != 0 ? it.Valor.ToString("####") : "0";
                                    var a62 = it.LC_MC != 0 ? it.LC_MC.ToString() : "0";
                                    string[] split11 = a62.Split(new[] { '.' });
                                    lcmc6.Text = split11[0];
                                    break;
                                case 79:
                                    GA6.Text = it.Valor != 0 ? it.Valor.ToString("####.####") : "0";
                                    var a63 = it.LC_MC != 0 ? it.LC_MC.ToString() : "0";
                                    string[] split12 = a63.Split(new[] { '.' });
                                    if (split12.Count() > 1)
                                        l12lcmc.Text = split12[1] == "000000" ? split12[0] : Convert.ToDecimal(a63).ToString("####.####");
                                    else
                                        l12lcmc.Text = split12[0];
                                    break;
                                case 89:
                                    ddlana6.SelectedValue = it.ValorValidacion == null ? "true" : it.ValorValidacion.ToString().ToLower();
                                    break;
                            }
                            break;
                    }
                }
            }
        }

        private void Laboratorios(int analisisId)
        {
            var listaMuestra = _muestraBl.Lista_Muestra(analisisId).Where(x => x.NivelID == 1).ToList();
            var counts = 0;
            foreach (var t in listaMuestra)
            {
                if (OrdenTrabajo.NormaID != null)
                    _list = t.NivelID == (decimal)enumNivel.Simple
                        ? _muestravalorBl.GuardaObten((int)OrdenTrabajo.NormaID, t.MuestraID, t.NivelID, false)
                        : _muestravalorBl.GuardaObtenCompuesta((int)OrdenTrabajo.NormaID, t.MuestraID);

                counts++;
                switch (counts)
                {
                    case 1:
                        if (t.ValMuestraC != null)
                        {
                            if (OrdenTrabajo.NormaID != null)
                            {
                                ddlLab1.LoadDrop("Nombre", "LaboratorioID", _labBl.Obten_Laboratorio((int)OrdenTrabajo.NormaID, 862));
                                ddlLab1.ClearSelection();

                                ddlLab1.SelectedValue = _list[0].LaboratorioID == null ? "0" : (_list[0].LaboratorioID).ToString();
                                ddlMet1.LoadDrop("Nombre", "MetodoID", _metBl.Obten_Metodo(int.Parse(ddlLab1.SelectedValue), 862));
                                if (ddlMet1.Items.Count >= 2)
                                    ddlMet1.SelectedValue = (_list[0].ParametroMetodo.MetodoID).ToString(CultureInfo.InvariantCulture);
                                else if (ddlLab1.SelectedValue == "0")
                                    ddlMet1.SelectedValue = "0";
                                else
                                    ddlMet1.SelectedIndex = 1;

                                ddlLab2.LoadDrop("Nombre", "LaboratorioID", _labBl.Obten_Laboratorio((int)OrdenTrabajo.NormaID, 118));
                                ddlLab2.ClearSelection();
                                ddlLab2.SelectedValue = _list[2].LaboratorioID == null ? "0" : (_list[2].LaboratorioID).ToString();
                                ddlMet2.LoadDrop("Nombre", "MetodoID", _metBl.Obten_Metodo(int.Parse(ddlLab2.SelectedValue), 118));
                                if (ddlMet2.Items.Count >= 2)
                                    ddlMet2.SelectedValue = (_list[2].ParametroMetodo.MetodoID).ToString(CultureInfo.InvariantCulture);
                                else if (ddlLab2.SelectedValue == "0")
                                    ddlMet2.SelectedValue = "0";
                                else
                                    ddlMet2.SelectedIndex = 1;

                                ddlLab4.LoadDrop("Nombre", "LaboratorioID", _labBl.Obten_Laboratorio((int)OrdenTrabajo.NormaID, 225));
                                ddlLab4.ClearSelection();
                                ddlLab4.SelectedValue = _list[1].LaboratorioID == null ? "0" : (_list[1].LaboratorioID).ToString();
                                ddlMet4.LoadDrop("Nombre", "MetodoID", _metBl.Obten_Metodo(int.Parse(ddlLab4.SelectedValue), 225));
                                if (ddlMet4.Items.Count >= 2)
                                    ddlMet4.SelectedValue = (_list[1].ParametroMetodo.MetodoID).ToString(CultureInfo.InvariantCulture);
                                else if (ddlLab4.SelectedValue == "0")
                                    ddlMet4.SelectedValue = "0";
                                else
                                    ddlMet4.SelectedIndex = 1;

                                ddlLab5.LoadDrop("Nombre", "LaboratorioID", _labBl.Obten_Laboratorio((int)OrdenTrabajo.NormaID, 79));
                                ddlLab5.ClearSelection();
                                ddlLab5.SelectedValue = _list[3].LaboratorioID == null ? "0" : (_list[3].LaboratorioID).ToString();
                                ddlMet5.LoadDrop("Nombre", "MetodoID", _metBl.Obten_Metodo(int.Parse(ddlLab5.SelectedValue), 79));
                                if (ddlMet5.Items.Count >= 2)
                                    ddlMet5.SelectedValue = (_list[3].ParametroMetodo.MetodoID).ToString(CultureInfo.InvariantCulture);
                                else if (ddlLab5.SelectedValue == "0")
                                    ddlMet5.SelectedValue = "0";
                                else
                                    ddlMet5.SelectedIndex = 1;

                                ddlLab3.LoadDrop("Nombre", "LaboratorioID", _labBl.Obten_Laboratorio((int)OrdenTrabajo.NormaID, 89));
                                ddlLab3.ClearSelection();
                                ddlLab3.SelectedValue = _list[4].LaboratorioID == null ? "0" : (_list[4].LaboratorioID).ToString();
                                ddlMet3.LoadDrop("Nombre", "MetodoID", _metBl.Obten_Metodo(int.Parse(ddlLab3.SelectedValue), 89));
                                if (ddlMet3.Items.Count >= 2)
                                    ddlMet3.SelectedValue = (_list[4].ParametroMetodo.MetodoID).ToString(CultureInfo.InvariantCulture);
                                else if (ddlLab3.SelectedValue == "0")
                                    ddlMet3.SelectedValue = "0";
                                else
                                    ddlMet3.SelectedIndex = 1;
                            }
                        }
                        else
                        {
                            if (OrdenTrabajo.NormaID != null)
                            {
                                ddlLab1.LoadDrop("Nombre", "LaboratorioID", _labBl.Obten_Laboratorio((int)OrdenTrabajo.NormaID, 862));
                                ddlLab1.ClearSelection();
                                ddlLab1.SelectedValue =
                                    ((int)Login.LaboratorioID).ToString(CultureInfo.InvariantCulture);
                                ddlMet1.LoadDrop("Nombre", "MetodoID", _metBl.Obten_Metodo(int.Parse(ddlLab1.SelectedValue), 862));
                                if (ddlMet1.Items.Count == 2) ddlMet1.SelectedIndex = 1;

                                ddlLab2.LoadDrop("Nombre", "LaboratorioID", _labBl.Obten_Laboratorio((int)OrdenTrabajo.NormaID, 118));
                                ddlLab2.ClearSelection();
                                ddlLab2.SelectedValue =
                                    ((int)Login.LaboratorioID).ToString(CultureInfo.InvariantCulture);
                                ddlMet2.LoadDrop("Nombre", "MetodoID", _metBl.Obten_Metodo(int.Parse(ddlLab2.SelectedValue), 118));
                                if (ddlMet2.Items.Count == 2) ddlMet2.SelectedIndex = 1;

                                ddlLab3.LoadDrop("Nombre", "LaboratorioID", _labBl.Obten_Laboratorio((int)OrdenTrabajo.NormaID, 89));
                                ddlLab3.ClearSelection();
                                ddlLab3.SelectedValue =
                                    ((int)Login.LaboratorioID).ToString(CultureInfo.InvariantCulture);
                                ddlMet3.LoadDrop("Nombre", "MetodoID", _metBl.Obten_Metodo(int.Parse(ddlLab3.SelectedValue), 89));
                                if (ddlMet3.Items.Count == 2) ddlMet3.SelectedIndex = 1;

                                ddlLab4.LoadDrop("Nombre", "LaboratorioID", _labBl.Obten_Laboratorio((int)OrdenTrabajo.NormaID, 225));
                                ddlLab4.ClearSelection();
                                ddlLab4.SelectedValue =
                                    ((int)Login.LaboratorioID).ToString(CultureInfo.InvariantCulture);
                                ddlMet4.LoadDrop("Nombre", "MetodoID", _metBl.Obten_Metodo(int.Parse(ddlLab4.SelectedValue), 225));
                                if (ddlMet4.Items.Count == 2) ddlMet4.SelectedIndex = 1;

                                ddlLab5.LoadDrop("Nombre", "LaboratorioID", _labBl.Obten_Laboratorio((int)OrdenTrabajo.NormaID, 79));
                                ddlLab5.ClearSelection();
                                ddlLab5.SelectedValue =
                                    ((int)Login.LaboratorioID).ToString(CultureInfo.InvariantCulture);
                                ddlMet5.LoadDrop("Nombre", "MetodoID", _metBl.Obten_Metodo(int.Parse(ddlLab5.SelectedValue), 79));
                                if (ddlMet5.Items.Count == 2) ddlMet5.SelectedIndex = 1;
                            }
                        }

                        break;

                }
            }
        }
        public void CargaInformacion(int analisisId)
        {
            Session["analisiid"] = analisisId;
            var listaMuestra = _muestraBl.Lista_Muestra(analisisId).Where(x => x.NivelID == 1).ToList();
            Session["cont"] = listaMuestra.Count();
            var muestraCompuesta = _muestraBl.Lista_Muestra(analisisId).Where(x => x.NivelID == 2).ToList();
            if (muestraCompuesta.Count > 0 || !Analisis.OrdenTrabajo.Norma.RequiereCompuesta)
            {
                /* Oculta el boton de generar compuesta sí ya existe una compuesta o si no se requiere de la compuesta */
                lnkbtnMuestraCompuesta.Visible = false;
            }


            switch (listaMuestra.Count)
            {
                case 2:
                    M3.Visible = false;
                    M4.Visible = false;
                    M5.Visible = false;
                    M6.Visible = false;
                    Muestra2(analisisId);
                    Laboratorios(analisisId);
                    break;
                case 4:
                    M5.Visible = false;
                    M6.Visible = false;
                    Muestra2(analisisId);
                    Muestra4(analisisId);
                    Laboratorios(analisisId);
                    break;
                case 6:
                    Muestra2(analisisId);
                    Muestra4(analisisId);
                    Muestra6(analisisId);
                    Laboratorios(analisisId);
                    break;
            }

            Calcular();
        }

        readonly Metodo_BL _metodoBl = new Metodo_BL();
        protected void ddlLab1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlMet1.LoadDrop("Nombre", "MetodoID", _metodoBl.Obten_Metodo(int.Parse(ddlLab1.SelectedValue), 862));
            Calcular();
        }

        protected void ddlLab2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlMet2.LoadDrop("Nombre", "MetodoID", _metodoBl.Obten_Metodo(int.Parse(ddlLab2.SelectedValue), 118));
            Calcular();
        }

        protected void ddlLab3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlMet3.LoadDrop("Nombre", "MetodoID", _metodoBl.Obten_Metodo(int.Parse(ddlLab3.SelectedValue), 89));
            Calcular();
        }

        protected void ddlLab4_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlMet4.LoadDrop("Nombre", "MetodoID", _metodoBl.Obten_Metodo(int.Parse(ddlLab4.SelectedValue), 225));
            Calcular();
        }

        protected void ddlLab5_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlMet5.LoadDrop("Nombre", "MetodoID", _metodoBl.Obten_Metodo(int.Parse(ddlLab5.SelectedValue), 79));
            Calcular();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {


            Calcular();
        }

        public static String Validahora(string hora)
        {

            const string expresion = @"^([0-9]|0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$";            
            if (Regex.IsMatch(hora, expresion))
            {
                return Regex.Replace(hora, expresion, String.Empty).Length == 0 ? "ok" : "ERROR";
            }
            return "ERROR";
        }


        protected void Button2_Click(object sender, EventArgs e)
        {

            var muestravalorBl = new MuestraValor_BL();
            var muestraArgs = new MuestraCapturaArgs();
            var mpBl = new ParametroMetodo_BL();
            var muestraBl = new Muestra_BL();
            var list = new List<MuestraValorArgBase_BL>();
            var muestreadorArgs = new MuestreadorCapturaArgs();
            var muestreadorbl = new Muestreador_BL();
            var errores = "";
            var listaMuestra = _muestraBl.Lista_Muestra((int)Session["analisiid"]).Where(x => x.NivelID == 1).ToList();

            if (Session["cheksignatario"] == null)
                errores += "-Debes seleccionar el signatario </br> ";
            switch (listaMuestra.Count)
            {
                case 2:
                    if (ValidarFecha(TextBox1.Text) || ValidarFecha(TextBox12.Text) || ValidarFecha(TextBox10.Text))
                        errores += "-La fecha de muestreo y/o la fecha de recepción en laboratorio son incorrectas o se encuentran vacías  </br> ";
                    if (TextBox2.Text == "" || TextBox13.Text == "")
                        errores += "-Se encuentra vacio el campo de hora  de muestreo </br> ";
                    if (CA1.Text == "" || CA2.Text == "")
                        errores += "-El caudal debe de ser igual  o mayor a '0' no puede estar vacío </br> ";
                    if (TextBox8.Text.Length > 200 || TextBox20.Text.Length > 200)
                        errores += "-Solo se pueden ingresar 200 caracteres en el campo observaciones </br> ";
                    break;
                case 4:
                    if (ValidarFecha(TextBox1.Text) || ValidarFecha(TextBox12.Text) || ValidarFecha(TextBox17.Text) || ValidarFecha(TextBox25.Text) || ValidarFecha(TextBox10.Text))
                        errores += "-La fecha de muestreo y/o la fecha de recepción en laboratorio son incorrectas o se encuentran vacías  </br> ";
                    if (TextBox2.Text == "" || TextBox13.Text == "" || TextBox21.Text == "" || TextBox26.Text == "")
                        errores += "-Se encuentra vacio el campo de hora  de muestreo </br> ";
                    if (CA1.Text == "" || CA2.Text == "" || CA3.Text == "" || CA4.Text == "")
                        errores += "-El caudal debe de ser igual  o mayor a '0' no puede estar vacío </br> ";
                    if (TextBox8.Text.Length > 200 || TextBox20.Text.Length > 200 || TextBox42.Text.Length > 200 || TextBox45.Text.Length > 200)
                        errores += "-Solo se pueden ingresar 200 caracteres en el campo observaciones </br> ";
                    break;
                case 6:
                    if (ValidarFecha(TextBox1.Text) || ValidarFecha(TextBox12.Text) || ValidarFecha(TextBox17.Text) || ValidarFecha(TextBox25.Text) || ValidarFecha(TextBox30.Text) || ValidarFecha(TextBox35.Text) || ValidarFecha(TextBox10.Text))
                        errores += "-La fecha de muestreo y/o la fecha de recepción en laboratorio son incorrectas o se encuentran vacías  </br> ";
                    if (TextBox2.Text == "" || TextBox13.Text == "" || TextBox21.Text == "" || TextBox26.Text == "" || TextBox31.Text == "" || TextBox36.Text == "")
                        errores += "-Se encuentra vacio el campo de hora  de muestreo </br> ";
                    if (CA1.Text == "" || CA2.Text == "" || CA3.Text == "" || CA4.Text == "" || CA5.Text == "" || CA6.Text == "")
                        errores += "-El caudal debe de ser igual  o mayor a '0' no puede estar vacío </br> ";
                    if (TextBox8.Text.Length > 200 || TextBox20.Text.Length > 200 || TextBox42.Text.Length > 200 || TextBox45.Text.Length > 200 || TextBox48.Text.Length > 200 || TextBox51.Text.Length > 200)
                        errores += "-Solo se pueden ingresar 200 caracteres en el campo observaciones </br> ";
                    break;
            }


            if (TextBox9.Text == "" || TextBox9.Text == "0" || TextBox9.Text == "0.00")
                errores += "-El Valor de la muestra compuesta debe ser mayor a cero </br> ";
            if (CheckBox1.Checked == false)
                errores += "-Debes de confirmar que los valores de la hoja de campo física concuerdan con los de la electrónica </br>";

            if (ddlAction1.SelectedValue != "2")
            {
                if (ddlLab1.SelectedValue == "0")
                    errores += "-Debe seleccionar laboratorio que analizó el parámetro de Ph para las muestras. </br>";
                if (ddlMet1.SelectedValue == "0")
                    errores += "-Debe seleccionar el metodo de las muestras. </br>";
            }
            if (ddlAction2.SelectedValue != "2")
            {
                if (ddlLab2.SelectedValue == "0")
                    errores += "-Debe seleccionar laboratorio que analizó el parámetro de Temperatura para las muestras. </br>";
                if (ddlMet2.SelectedValue == "0")
                    errores += "-Debe seleccionar el metodo de las muestras. </br>";
            }
            if (ddlAction3.SelectedValue != "2")
            {
                if (ddlLab3.SelectedValue == "0")
                    errores += "-Debe seleccionar laboratorio que analizó el parámetro de Materia flotante para las muestras. </br>";
                if (ddlMet3.SelectedValue == "0")
                    errores += "-Debe seleccionar el metodo de las muestras. </br>";
            }
            if (ddlAction4.SelectedValue != "2")
            {
                if (ddlLab4.SelectedValue == "0")
                    errores += "-Debe seleccionar laboratorio que analizó el parámetro de Coliformes fecales para las muestras. </br>";
                if (ddlMet4.SelectedValue == "0")
                    errores += "-Debe seleccionar el metodo de las muestras. </br>";
            }
            if (ddlAction5.SelectedValue != "2")
            {
                if (ddlLab5.SelectedValue == "0")
                    errores += "-Debe seleccionar laboratorio que analizó el parámetro de Grasas y aceites para las muestras. </br>";
                if (ddlMet5.SelectedValue == "0")
                errores += "-Debe seleccionar el metodo de las muestras. </br>";
            }

            if (errores != "")
            {
                Calcular();
                muestraArgs.Descripcion = "ErrorT";
                Session["errort"] = errores;
                Guardar(this, muestraArgs);
                return;
            }
            if (Validahora(TextBox11.Text) == "ERROR")
            {
                muestraArgs.Descripcion = "ErrorHoras24";
                Guardar(this, muestraArgs);
                return;
            }
            #region guardar
            var conts = 1;
            foreach (var t in listaMuestra)
            {
                if (OrdenTrabajo.NormaID != null)
                    list = t.NivelID == (decimal)enumNivel.Simple ? muestravalorBl.GuardaObten((int)OrdenTrabajo.NormaID, (int)t.MuestraID, t.NivelID, false) : muestravalorBl.GuardaObtenCompuesta((int)OrdenTrabajo.NormaID, (int)t.MuestraID);

                switch (conts)
                {
                    case 1:
                        if (Validahora(TextBox2.Text) == "ERROR")
                        {
                            muestraArgs.Descripcion = "ErrorHoras24";
                            Guardar(this, muestraArgs);
                            return;
                        }
                        if (RemoveSpecialCharacters(TextBox2.Text) == "")
                        {
                            muestraArgs.Descripcion = "ErrorHorasV";
                            Guardar(this, muestraArgs);
                            return;
                        }
                        if ((CF1.Text == "" || lcmc1.Text == "") || (Convert.ToDecimal(CF1.Text) < Convert.ToDecimal(lcmc1.Text)))
                        {
                            if (ddlAction4.SelectedValue == "3" )
                            {
                            muestraArgs.Descripcion = "Errorlc";
                            Guardar(this, muestraArgs);
                            return;
                            }
                        }
                        if ((GA1.Text == "" || lcmc7.Text == "") || (Convert.ToDecimal(GA1.Text) < Convert.ToDecimal(lcmc7.Text)))
                        {
                            if (ddlAction5.SelectedValue == "3" )
                            {
                            muestraArgs.Descripcion = "Errorlc";
                            Guardar(this, muestraArgs);
                            return;
                            }
                        }

                        foreach (var it in list.Select(t1 => list.FirstOrDefault(p => p.MuestraValorID == t1.MuestraValorID)))
                        {
                            switch (it.ParametroID)
                            {
                                case 862:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(PH1.Text);
                                    it.LaboratorioID = int.Parse(ddlLab1.SelectedValue);
                                    var mp0 = mpBl.Obten(862, int.Parse(ddlMet1.SelectedValue));
                                    if (mp0 != null)
                                    it.ParametroMetodoID = mp0.ParametroMetodoID;
                                    break;
                                case 118:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(T1.Text);
                                    it.LaboratorioID = int.Parse(ddlLab2.SelectedValue);
                                    var mp1 = mpBl.Obten(118, int.Parse(ddlMet2.SelectedValue));
                                    if (mp1 != null)
                                    it.ParametroMetodoID = mp1.ParametroMetodoID;
                                    break;
                                case 225:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(CF1.Text);
                                    it.LC_MC = lcmc1.Text=="" ? 0 : decimal.Parse(lcmc1.Text);
                                    it.LaboratorioID = int.Parse(ddlLab4.SelectedValue);
                                    var mp2 = mpBl.Obten(225, int.Parse(ddlMet4.SelectedValue));
                                    if (mp2 != null)
                                    it.ParametroMetodoID = mp2.ParametroMetodoID;
                                    break;
                                case 79:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(GA1.Text);
                                    it.LC_MC = lcmc7.Text == "" ? 0 : decimal.Parse(lcmc7.Text);
                                    it.LaboratorioID = int.Parse(ddlLab5.SelectedValue);
                                    var mp3 = mpBl.Obten(79, int.Parse(ddlMet5.SelectedValue));
                                    if (mp3 != null)
                                    it.ParametroMetodoID = mp3.ParametroMetodoID;
                                    break;
                                case 89:
                                    bool tf;
                                    switch (ddlana1.SelectedValue)
                                    {
                                        case "true":
                                            tf = true;
                                            break;
                                        default:
                                            tf = false;
                                            break;
                                    }
                                    it.GeneradoSP = false;
                                    it.ValorValidacion = tf;
                                    it.LaboratorioID = int.Parse(ddlLab3.SelectedValue);
                                    var mp4 = mpBl.Obten(89, int.Parse(ddlMet3.SelectedValue));
                                    if (mp4 != null)
                                    it.ParametroMetodoID = mp4.ParametroMetodoID;
                                    break;

                            }
                            muestravalorBl.Guardar(it);
                        }

                        var muestreador = muestreadorbl.Obten_Muestra_Muestra(t.MuestraID);
                        //Se comprueba si el registro ya existia
                        if (muestreador != null)
                        muestreadorArgs.MuestreadorID = muestreador.MuestreadorID;

                        muestreadorArgs.MuestraID = t.MuestraID;
                        muestreadorArgs.LaboratorioSignatarioID = Signatario_Laboratorio_Persona.LaboratorioSignatarioID;
                        muestreadorbl.Guarda(muestreadorArgs);

                        muestraArgs.Descripcion = "N/A";
                        muestraArgs.Identificacion = "N/A";
                        muestraArgs.Caudal = decimal.Parse(CA1.Text);
                        muestraArgs.Observaciones = TextBox8.Text;
                        if (ValidarFecha(TextBox1.Text) || ValidarFecha(TextBox10.Text))
                            muestraArgs.Descripcion = "ErrorFecha";

                        muestraArgs.Fecha = DateTime.ParseExact(TextBox1.Text + " " + TextBox2.Text, "dd/MM/yyyy H:mm", CultureInfo.InvariantCulture);
                        muestraArgs.FechaRecepcion = DateTime.ParseExact(TextBox10.Text + " " + TextBox11.Text, "dd/MM/yyyy H:mm", CultureInfo.InvariantCulture);

                        break;
                    case 2:
                        if (Validahora(TextBox2.Text) == "ERROR" || Validahora(TextBox13.Text) == "ERROR")
                        {
                            muestraArgs.Descripcion = "ErrorHoras24";
                            Guardar(this, muestraArgs);
                            return;
                        }
                        if (RemoveSpecialCharacters(TextBox2.Text) == "" || RemoveSpecialCharacters(TextBox13.Text) == "")
                        {
                            muestraArgs.Descripcion = "ErrorHorasV";
                            Guardar(this, muestraArgs);
                            return;
                        }
                        //if (Convert.ToDecimal(RemoveSpecialCharacters(TextBox2.Text)) > Convert.ToDecimal(RemoveSpecialCharacters(TextBox13.Text)))
                        //{
                        //    muestraArgs.Descripcion = "ErrorHoras";
                        //    Guardar(this, muestraArgs);
                        //    return;
                        //}
                        if ((CF2.Text == "" || lcmc2.Text == "") || (Convert.ToDecimal(CF2.Text) < Convert.ToDecimal(lcmc2.Text)))
                        {
                            if (ddlAction4.SelectedValue == "3")
                            {
                            muestraArgs.Descripcion = "Errorlc";
                            Guardar(this, muestraArgs);
                            return;
                            }
                        }
                        if ((GA2.Text == "" || lcmc8.Text == "") || (Convert.ToDecimal(GA2.Text) < Convert.ToDecimal(lcmc8.Text)))
                        {
                            if (ddlAction5.SelectedValue == "3")
                            {
                            muestraArgs.Descripcion = "Errorlc";
                            Guardar(this, muestraArgs);
                            return;
                            }
                        }
                        foreach (var it in list.Select(t1 => list.FirstOrDefault(p => p.MuestraValorID == t1.MuestraValorID)))
                        {
                            switch (it.ParametroID)
                            {
                                case 862:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(PH2.Text);
                                    it.LaboratorioID = int.Parse(ddlLab1.SelectedValue);
                                    var mp0 = mpBl.Obten(862, int.Parse(ddlMet1.SelectedValue));
                                    if (mp0 != null)
                                    it.ParametroMetodoID = mp0.ParametroMetodoID;
                                    break;
                                case 118:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(T2.Text);
                                    it.LaboratorioID = int.Parse(ddlLab2.SelectedValue);
                                    var mp1 = mpBl.Obten(118, int.Parse(ddlMet2.SelectedValue));
                                    if (mp1 != null)
                                    it.ParametroMetodoID = mp1.ParametroMetodoID;
                                    break;
                                case 225:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(CF2.Text);
                                    it.LC_MC = lcmc2.Text == "" ? 0 : decimal.Parse(lcmc2.Text);
                                    it.LaboratorioID = int.Parse(ddlLab4.SelectedValue);
                                    var mp2 = mpBl.Obten(225, int.Parse(ddlMet4.SelectedValue));
                                    if (mp2 != null)
                                    it.ParametroMetodoID = mp2.ParametroMetodoID;
                                    break;
                                case 79:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(GA2.Text);
                                    it.LC_MC = lcmc8.Text == "" ? 0 : decimal.Parse(lcmc8.Text);
                                    it.LaboratorioID = int.Parse(ddlLab5.SelectedValue);
                                    var mp3 = mpBl.Obten(79, int.Parse(ddlMet5.SelectedValue));
                                    if (mp3 != null)
                                    it.ParametroMetodoID = mp3.ParametroMetodoID;
                                    break;
                                case 89:
                                    bool tf;

                                    switch (ddlana2.SelectedValue)
                                    {
                                        case "true":
                                            tf = true;
                                            break;
                                        default:
                                            tf = false;
                                            break;
                                    }
                                    it.GeneradoSP = false;
                                    it.ValorValidacion = tf;
                                    it.LaboratorioID = int.Parse(ddlLab3.SelectedValue);
                                    var mp4 = mpBl.Obten(89, int.Parse(ddlMet3.SelectedValue));
                                    if (mp4 != null)
                                    it.ParametroMetodoID = mp4.ParametroMetodoID;
                                    break;

                            }
                            muestravalorBl.Guardar(it);
                        }

                        var muestreadorConsulta = muestreadorbl.Obten_Muestra_Muestra(t.MuestraID);
                        //Se comprueba si el registro ya existia
                        if (muestreadorConsulta != null)
                            muestreadorArgs.MuestreadorID = muestreadorConsulta.MuestreadorID;

                        muestreadorArgs.MuestraID = t.MuestraID;
                        muestreadorArgs.LaboratorioSignatarioID = Signatario_Laboratorio_Persona.LaboratorioSignatarioID;
                        muestreadorbl.Guarda(muestreadorArgs);

                        muestraArgs.Descripcion = "N/A";
                        muestraArgs.Identificacion = "N/A";
                        muestraArgs.Caudal = decimal.Parse(CA2.Text);
                        muestraArgs.Observaciones = TextBox20.Text;
                        if (ValidarFecha(TextBox12.Text) || ValidarFecha(TextBox10.Text))
                            muestraArgs.Descripcion = "ErrorFecha";

                        muestraArgs.Fecha = DateTime.ParseExact(TextBox12.Text + " " + TextBox13.Text, "dd/MM/yyyy H:mm", CultureInfo.InvariantCulture);
                        muestraArgs.FechaRecepcion = DateTime.ParseExact(TextBox10.Text + " " + TextBox11.Text, "dd/MM/yyyy H:mm", CultureInfo.InvariantCulture);
                        break;
                    case 3:
                        if (Validahora(TextBox2.Text) == "ERROR" || Validahora(TextBox13.Text) == "ERROR" || Validahora(TextBox21.Text) == "ERROR")
                        {
                            muestraArgs.Descripcion = "ErrorHoras24";
                            Guardar(this, muestraArgs);
                            return;
                        }
                        var a3 = RemoveSpecialCharacters(TextBox2.Text);
                        var b3 = RemoveSpecialCharacters(TextBox13.Text);
                        var c3 = RemoveSpecialCharacters(TextBox21.Text);
                        if (a3 == "" || b3 == "" || c3 == "")
                        {
                            muestraArgs.Descripcion = "ErrorHorasV";
                            Guardar(this, muestraArgs);
                            return;
                        }
                        //if ((Convert.ToDecimal(a3) > Convert.ToDecimal(b3)) || (Convert.ToDecimal(b3) > Convert.ToDecimal(c3)))
                        //{
                        //    muestraArgs.Descripcion = "ErrorHoras";
                        //    Guardar(this, muestraArgs);
                        //    return;
                        //}
                        if ((CF3.Text == "" || lcmc3.Text == "") || (Convert.ToDecimal(CF3.Text) < Convert.ToDecimal(lcmc3.Text)))
                        {
                            if (ddlAction4.SelectedValue == "3")
                            {
                            muestraArgs.Descripcion = "Errorlc";
                            Guardar(this, muestraArgs);
                            return;
                            }
                        }
                        if ((GA3.Text == "" || lcmc9.Text == "") || (Convert.ToDecimal(GA3.Text) < Convert.ToDecimal(lcmc9.Text)))
                        {
                            if (ddlAction5.SelectedValue == "3")
                            {
                            muestraArgs.Descripcion = "Errorlc";
                            Guardar(this, muestraArgs);
                            return;
                            }
                        }
                        foreach (var it in list.Select(t1 => list.FirstOrDefault(p => p.MuestraValorID == t1.MuestraValorID)))
                        {
                            switch (it.ParametroID)
                            {
                                case 862:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(PH3.Text);
                                    it.LaboratorioID = int.Parse(ddlLab1.SelectedValue);
                                    var mp0 = mpBl.Obten(862, int.Parse(ddlMet1.SelectedValue));
                                    if (mp0 != null)
                                    it.ParametroMetodoID = mp0.ParametroMetodoID;
                                    break;
                                case 118:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(T3.Text);
                                    it.LaboratorioID = int.Parse(ddlLab2.SelectedValue);
                                    var mp1 = mpBl.Obten(118, int.Parse(ddlMet2.SelectedValue));
                                    if (mp1 != null)
                                    it.ParametroMetodoID = mp1.ParametroMetodoID;
                                    break;
                                case 225:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(CF3.Text);
                                    it.LC_MC = lcmc3.Text == "" ? 0 : decimal.Parse(lcmc3.Text);
                                    it.LaboratorioID = int.Parse(ddlLab4.SelectedValue);
                                    var mp2 = mpBl.Obten(225, int.Parse(ddlMet4.SelectedValue));
                                    if (mp2 != null)
                                    it.ParametroMetodoID = mp2.ParametroMetodoID;
                                    break;
                                case 79:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(GA3.Text);
                                    it.LC_MC = lcmc9.Text == "" ? 0 : decimal.Parse(lcmc9.Text);
                                    it.LaboratorioID = int.Parse(ddlLab5.SelectedValue);
                                    var mp3 = mpBl.Obten(79, int.Parse(ddlMet5.SelectedValue));
                                    if (mp3 != null)
                                    it.ParametroMetodoID = mp3.ParametroMetodoID;
                                    break;
                                case 89:
                                    bool tf;

                                    switch (ddlana3.SelectedValue)
                                    {
                                        case "true":
                                            tf = true;
                                            break;
                                        default:
                                            tf = false;
                                            break;
                                    }
                                    it.GeneradoSP = false;
                                    it.ValorValidacion = tf;
                                    it.LaboratorioID = int.Parse(ddlLab3.SelectedValue);
                                    var mp4 = mpBl.Obten(89, int.Parse(ddlMet3.SelectedValue));
                                    if (mp4 != null)
                                    it.ParametroMetodoID = mp4.ParametroMetodoID;
                                    break;

                            }
                            muestravalorBl.Guardar(it);
                        }

                        var muestreadorConsulta2 = muestreadorbl.Obten_Muestra_Muestra(t.MuestraID);
                        //Se comprueba si el registro ya existia
                        if (muestreadorConsulta2 != null)
                            muestreadorArgs.MuestreadorID = muestreadorConsulta2.MuestreadorID;

                        muestreadorArgs.MuestraID = t.MuestraID;
                        muestreadorArgs.LaboratorioSignatarioID = Signatario_Laboratorio_Persona.LaboratorioSignatarioID;
                        muestreadorbl.Guarda(muestreadorArgs);

                        muestraArgs.Descripcion = "N/A";
                        muestraArgs.Identificacion = "N/A";
                        muestraArgs.Caudal = decimal.Parse(CA3.Text);
                        muestraArgs.Observaciones = TextBox42.Text;
                        if (ValidarFecha(TextBox17.Text) || ValidarFecha(TextBox10.Text))
                            muestraArgs.Descripcion = "ErrorFecha";

                        muestraArgs.Fecha = DateTime.ParseExact(TextBox17.Text + " " + TextBox21.Text, "dd/MM/yyyy H:mm", CultureInfo.InvariantCulture);
                        muestraArgs.FechaRecepcion = DateTime.ParseExact(TextBox10.Text + " " + TextBox11.Text, "dd/MM/yyyy H:mm", CultureInfo.InvariantCulture);
                        break;
                    case 4:
                        if (Validahora(TextBox2.Text) == "ERROR" || Validahora(TextBox13.Text) == "ERROR" || Validahora(TextBox21.Text) == "ERROR" || Validahora(TextBox26.Text) == "ERROR")
                        {
                            muestraArgs.Descripcion = "ErrorHoras24";
                            Guardar(this, muestraArgs);
                            return;
                        }
                        var a4 = RemoveSpecialCharacters(TextBox2.Text);
                        var b4 = RemoveSpecialCharacters(TextBox13.Text);
                        var c4 = RemoveSpecialCharacters(TextBox21.Text);
                        var d4 = RemoveSpecialCharacters(TextBox26.Text);
                        if (a4 == "" || b4 == "" || c4 == "" || d4 == "")
                        {
                            muestraArgs.Descripcion = "ErrorHorasV";
                            if (muestraArgs.Descripcion == "ErrorHorasV")
                            {
                                Guardar(this, muestraArgs);
                                return;
                            }
                        }
                        //if ((Convert.ToDecimal(a4) > Convert.ToDecimal(b4)) || (Convert.ToDecimal(b4) > Convert.ToDecimal(c4)) || (Convert.ToDecimal(c4) > Convert.ToDecimal(d4)))
                        //{
                        //    muestraArgs.Descripcion = "ErrorHoras";
                        //    Guardar(this, muestraArgs);
                        //    return;

                        //}
                        if ((CF4.Text == "" || lcmc4.Text == "") || (Convert.ToDecimal(CF4.Text) < Convert.ToDecimal(lcmc4.Text)))
                        {
                            if (ddlAction4.SelectedValue == "3")
                            {
                            muestraArgs.Descripcion = "Errorlc";
                            Guardar(this, muestraArgs);
                            return;
                            }
                        }
                        if ((GA4.Text == "" || l10lcmc.Text == "") || (Convert.ToDecimal(GA4.Text) < Convert.ToDecimal(l10lcmc.Text)))
                        {
                            if (ddlAction5.SelectedValue == "3")
                            {
                            muestraArgs.Descripcion = "Errorlc";
                            Guardar(this, muestraArgs);
                            return;
                            }
                        }
                        foreach (var it in list.Select(t1 => list.FirstOrDefault(p => p.MuestraValorID == t1.MuestraValorID)))
                        {
                            switch (it.ParametroID)
                            {
                                case 862:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(PH4.Text);
                                    it.LaboratorioID = int.Parse(ddlLab1.SelectedValue);
                                    var mp0 = mpBl.Obten(862, int.Parse(ddlMet1.SelectedValue));
                                    if (mp0 != null)
                                    it.ParametroMetodoID = mp0.ParametroMetodoID;
                                    break;
                                case 118:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(T4.Text);
                                    it.LaboratorioID = int.Parse(ddlLab2.SelectedValue);
                                    var mp1 = mpBl.Obten(118, int.Parse(ddlMet2.SelectedValue));
                                    if (mp1 != null)
                                    it.ParametroMetodoID = mp1.ParametroMetodoID;
                                    break;
                                case 225:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(CF4.Text);
                                    it.LC_MC = lcmc4.Text == "" ? 0 : decimal.Parse(lcmc4.Text);
                                    it.LaboratorioID = int.Parse(ddlLab4.SelectedValue);
                                    var mp2 = mpBl.Obten(225, int.Parse(ddlMet4.SelectedValue));
                                    if (mp2 != null)
                                    it.ParametroMetodoID = mp2.ParametroMetodoID;
                                    break;
                                case 79:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(GA4.Text);
                                    it.LC_MC = l10lcmc.Text == "" ? 0 : decimal.Parse(l10lcmc.Text);
                                    it.LaboratorioID = int.Parse(ddlLab5.SelectedValue);
                                    var mp3 = mpBl.Obten(79, int.Parse(ddlMet5.SelectedValue));
                                    if (mp3 != null)
                                    it.ParametroMetodoID = mp3.ParametroMetodoID;
                                    break;
                                case 89:
                                    bool tf;

                                    switch (ddlana4.SelectedValue)
                                    {
                                        case "true":
                                            tf = true;
                                            break;
                                        default:
                                            tf = false;
                                            break;
                                    }
                                    it.GeneradoSP = false;
                                    it.ValorValidacion = tf;
                                    it.LaboratorioID = int.Parse(ddlLab3.SelectedValue);
                                    var mp4 = mpBl.Obten(89, int.Parse(ddlMet3.SelectedValue));
                                    if (mp4 != null)
                                    it.ParametroMetodoID = mp4.ParametroMetodoID;
                                    break;

                            }
                            muestravalorBl.Guardar(it);
                        }

                        var muestreadorConsulta3 = muestreadorbl.Obten_Muestra_Muestra(t.MuestraID);
                        //Se comprueba si el registro ya existia
                        if (muestreadorConsulta3 != null)
                            muestreadorArgs.MuestreadorID = muestreadorConsulta3.MuestreadorID;

                        muestreadorArgs.MuestraID = t.MuestraID;
                        muestreadorArgs.LaboratorioSignatarioID = Signatario_Laboratorio_Persona.LaboratorioSignatarioID;
                        muestreadorbl.Guarda(muestreadorArgs);

                        muestraArgs.Descripcion = "N/A";
                        muestraArgs.Identificacion = "N/A";
                        muestraArgs.Caudal = decimal.Parse(CA4.Text);
                        muestraArgs.Observaciones = TextBox45.Text;
                        if (ValidarFecha(TextBox25.Text) || ValidarFecha(TextBox10.Text))
                            muestraArgs.Descripcion = "ErrorFecha";

                        muestraArgs.Fecha = DateTime.ParseExact(TextBox25.Text + " " + TextBox26.Text, "dd/MM/yyyy H:mm", CultureInfo.InvariantCulture);
                        muestraArgs.FechaRecepcion = DateTime.ParseExact(TextBox10.Text + " " + TextBox11.Text, "dd/MM/yyyy H:mm", CultureInfo.InvariantCulture);
                        break;
                    case 5:
                        if (Validahora(TextBox2.Text) == "ERROR" || Validahora(TextBox13.Text) == "ERROR" || Validahora(TextBox21.Text) == "ERROR" || Validahora(TextBox26.Text) == "ERROR" || Validahora(TextBox31.Text) == "ERROR")
                        {
                            muestraArgs.Descripcion = "ErrorHoras24";
                            Guardar(this, muestraArgs);
                            return;
                        }
                        var a5 = RemoveSpecialCharacters(TextBox2.Text);
                        var b5 = RemoveSpecialCharacters(TextBox13.Text);
                        var c5 = RemoveSpecialCharacters(TextBox21.Text);
                        var d5 = RemoveSpecialCharacters(TextBox26.Text);
                        var e5 = RemoveSpecialCharacters(TextBox31.Text);
                        if (a5 == "" || b5 == "" || c5 == "" || d5 == "" || e5 == "")
                        {
                            muestraArgs.Descripcion = "ErrorHorasV";
                            Guardar(this, muestraArgs);
                            return;
                        }
                        //if ((Convert.ToDecimal(a5) > Convert.ToDecimal(b5)) || (Convert.ToDecimal(b5) > Convert.ToDecimal(c5)) || (Convert.ToDecimal(c5) > Convert.ToDecimal(d5)) || (Convert.ToDecimal(d5) > Convert.ToDecimal(e5)))
                        //{
                        //    muestraArgs.Descripcion = "ErrorHoras";
                        //    Guardar(this, muestraArgs);
                        //    return;
                        //}
                        if ((CF5.Text == "" || lcmc5.Text == "") || (Convert.ToDecimal(CF5.Text) < Convert.ToDecimal(lcmc5.Text)))
                        {
                            if (ddlAction4.SelectedValue == "3")
                            {
                            muestraArgs.Descripcion = "Errorlc";
                            Guardar(this, muestraArgs);
                            return;
                            }
                        } if ((GA5.Text == "" || l11lcmc.Text == "") || (Convert.ToDecimal(GA5.Text) < Convert.ToDecimal(l11lcmc.Text)))
                        {
                            if (ddlAction5.SelectedValue == "3")
                            {
                            muestraArgs.Descripcion = "Errorlc";
                            Guardar(this, muestraArgs);
                            return;
                            }
                        }
                        foreach (var it in list.Select(t1 => list.FirstOrDefault(p => p.MuestraValorID == t1.MuestraValorID)))
                        {
                            switch (it.ParametroID)
                            {
                                case 862:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(PH5.Text);
                                    it.LaboratorioID = int.Parse(ddlLab1.SelectedValue);
                                    var mp0 = mpBl.Obten(862, int.Parse(ddlMet1.SelectedValue));
                                    if (mp0 != null)
                                    it.ParametroMetodoID = mp0.ParametroMetodoID;
                                    break;
                                case 118:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(T5.Text);
                                    it.LaboratorioID = int.Parse(ddlLab2.SelectedValue);
                                    var mp1 = mpBl.Obten(118, int.Parse(ddlMet2.SelectedValue));
                                    if (mp1 != null)
                                    it.ParametroMetodoID = mp1.ParametroMetodoID;
                                    break;
                                case 225:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(CF5.Text);
                                    it.LC_MC = lcmc5.Text == "" ? 0 : decimal.Parse(lcmc5.Text);
                                    it.LaboratorioID = int.Parse(ddlLab4.SelectedValue);
                                    var mp2 = mpBl.Obten(225, int.Parse(ddlMet4.SelectedValue));
                                    if (mp2 != null)
                                    it.ParametroMetodoID = mp2.ParametroMetodoID;
                                    break;
                                case 79:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(GA5.Text);
                                    it.LC_MC = l11lcmc.Text == "" ? 0 : decimal.Parse(l11lcmc.Text);
                                    it.LaboratorioID = int.Parse(ddlLab5.SelectedValue);
                                    var mp3 = mpBl.Obten(79, int.Parse(ddlMet5.SelectedValue));
                                    if (mp3 != null)
                                    it.ParametroMetodoID = mp3.ParametroMetodoID;
                                    break;
                                case 89:
                                    bool tf;

                                    switch (ddlana5.SelectedValue)
                                    {
                                        case "true":
                                            tf = true;
                                            break;
                                        default:
                                            tf = false;
                                            break;
                                    }
                                    it.GeneradoSP = false;
                                    it.ValorValidacion = tf;
                                    it.LaboratorioID = int.Parse(ddlLab3.SelectedValue);
                                    var mp4 = mpBl.Obten(89, int.Parse(ddlMet3.SelectedValue));
                                    if (mp4 != null)
                                    it.ParametroMetodoID = mp4.ParametroMetodoID;
                                    break;

                            }
                            muestravalorBl.Guardar(it);
                        }

                         var muestreadorConsulta4 = muestreadorbl.Obten_Muestra_Muestra(t.MuestraID);
                        //Se comprueba si el registro ya existia
                        if (muestreadorConsulta4 != null)
                            muestreadorArgs.MuestreadorID = muestreadorConsulta4.MuestreadorID;

                        muestreadorArgs.MuestraID = t.MuestraID;
                        muestreadorArgs.LaboratorioSignatarioID = Signatario_Laboratorio_Persona.LaboratorioSignatarioID;
                        muestreadorbl.Guarda(muestreadorArgs);

                        muestraArgs.Descripcion = "N/A";
                        muestraArgs.Identificacion = "N/A";
                        muestraArgs.Caudal = decimal.Parse(CA5.Text);
                        muestraArgs.Observaciones = TextBox48.Text;
                        if (ValidarFecha(TextBox30.Text) || ValidarFecha(TextBox10.Text))
                            muestraArgs.Descripcion = "ErrorFecha";

                        muestraArgs.Fecha = DateTime.ParseExact(TextBox30.Text + " " + TextBox31.Text, "dd/MM/yyyy H:mm", CultureInfo.InvariantCulture);
                        muestraArgs.FechaRecepcion = DateTime.ParseExact(TextBox10.Text + " " + TextBox11.Text, "dd/MM/yyyy H:mm", CultureInfo.InvariantCulture);
                        break;
                    case 6:
                        if (Validahora(TextBox2.Text) == "ERROR" || Validahora(TextBox13.Text) == "ERROR" || Validahora(TextBox21.Text) == "ERROR" || Validahora(TextBox26.Text) == "ERROR" || Validahora(TextBox31.Text) == "ERROR" || Validahora(TextBox36.Text) == "ERROR")
                        {
                            muestraArgs.Descripcion = "ErrorHoras24";
                            Guardar(this, muestraArgs);
                            return;
                        }
                        var a6 = RemoveSpecialCharacters(TextBox2.Text);
                        var b6 = RemoveSpecialCharacters(TextBox13.Text);
                        var c6 = RemoveSpecialCharacters(TextBox21.Text);
                        var d6 = RemoveSpecialCharacters(TextBox26.Text);
                        var e6 = RemoveSpecialCharacters(TextBox31.Text);
                        var f6 = RemoveSpecialCharacters(TextBox36.Text);
                        if (a6 == "" || b6 == "" || c6 == "" || d6 == "" || e6 == "" || f6 == "")
                        {
                            muestraArgs.Descripcion = "ErrorHorasV";
                            Guardar(this, muestraArgs);
                            return;
                        }
                        //if ((Convert.ToDecimal(a6) > Convert.ToDecimal(b6)) || (Convert.ToDecimal(b6) > Convert.ToDecimal(c6)) || (Convert.ToDecimal(c6) > Convert.ToDecimal(d6)) || (Convert.ToDecimal(d6) > Convert.ToDecimal(e6)) || (Convert.ToDecimal(e6) > Convert.ToDecimal(f6)))
                        //{
                        //    muestraArgs.Descripcion = "ErrorHoras";
                        //    Guardar(this, muestraArgs);
                        //    return;
                        //}
                        if ((CF6.Text == "" || lcmc6.Text == "") || (Convert.ToDecimal(CF6.Text) < Convert.ToDecimal(lcmc6.Text)))
                        {
                            if (ddlAction4.SelectedValue == "3")
                            {
                            muestraArgs.Descripcion = "Errorlc";
                            Guardar(this, muestraArgs);
                            return;
                            }
                        }
                        if ((GA6.Text == "" || l12lcmc.Text == "") || (Convert.ToDecimal(GA6.Text) < Convert.ToDecimal(l12lcmc.Text)))
                        {
                            if (ddlAction5.SelectedValue == "3")
                            {
                            muestraArgs.Descripcion = "Errorlc";
                            Guardar(this, muestraArgs);
                            return;
                            }
                        }
                        foreach (var it in list.Select(t1 => list.FirstOrDefault(p => p.MuestraValorID == t1.MuestraValorID)))
                        {
                            switch (it.ParametroID)
                            {
                                case 862:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(PH6.Text);
                                    it.LaboratorioID = int.Parse(ddlLab1.SelectedValue);
                                    var mp0 = mpBl.Obten(862, int.Parse(ddlMet1.SelectedValue));
                                    if (mp0 != null)
                                    it.ParametroMetodoID = mp0.ParametroMetodoID;
                                    break;
                                case 118:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(T6.Text);
                                    it.LaboratorioID = int.Parse(ddlLab2.SelectedValue);
                                    var mp1 = mpBl.Obten(118, int.Parse(ddlMet2.SelectedValue));
                                    if (mp1 != null)
                                    it.ParametroMetodoID = mp1.ParametroMetodoID;
                                    break;
                                case 225:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(CF6.Text);
                                    it.LC_MC = lcmc6.Text == "" ? 0 : decimal.Parse(lcmc6.Text);
                                    it.LaboratorioID = int.Parse(ddlLab4.SelectedValue);
                                    var mp2 = mpBl.Obten(225, int.Parse(ddlMet4.SelectedValue));
                                    if (mp2 != null)
                                    it.ParametroMetodoID = mp2.ParametroMetodoID;
                                    break;
                                case 79:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(GA6.Text);
                                    it.LC_MC = l12lcmc.Text == "" ? 0 : decimal.Parse(l12lcmc.Text);
                                    it.LaboratorioID = int.Parse(ddlLab5.SelectedValue);
                                    var mp3 = mpBl.Obten(79, int.Parse(ddlMet5.SelectedValue));
                                    if (mp3 != null)
                                    it.ParametroMetodoID = mp3.ParametroMetodoID;
                                    break;
                                case 89:
                                    bool tf;

                                    switch (ddlana6.SelectedValue)
                                    {
                                        case "true":
                                            tf = true;
                                            break;
                                        default:
                                            tf = false;
                                            break;
                                    }
                                    it.GeneradoSP = false;
                                    it.ValorValidacion = tf;
                                    it.LaboratorioID = int.Parse(ddlLab3.SelectedValue);
                                    var mp4 = mpBl.Obten(89, int.Parse(ddlMet3.SelectedValue));
                                    if (mp4 != null)
                                    it.ParametroMetodoID = mp4.ParametroMetodoID;
                                    break;

                            }
                            muestravalorBl.Guardar(it);
                        }

                         var muestreadorConsulta5 = muestreadorbl.Obten_Muestra_Muestra(t.MuestraID);
                        //Se comprueba si el registro ya existia
                        if (muestreadorConsulta5 != null)
                            muestreadorArgs.MuestreadorID = muestreadorConsulta5.MuestreadorID;

                        muestreadorArgs.MuestraID = t.MuestraID;
                        muestreadorArgs.LaboratorioSignatarioID = Signatario_Laboratorio_Persona.LaboratorioSignatarioID;
                        muestreadorbl.Guarda(muestreadorArgs);

                        muestraArgs.Descripcion = "N/A";
                        muestraArgs.Identificacion = "N/A";
                        muestraArgs.Caudal = decimal.Parse(CA6.Text);
                        muestraArgs.Observaciones = TextBox51.Text;
                        if (ValidarFecha(TextBox35.Text) || ValidarFecha(TextBox10.Text))
                            muestraArgs.Descripcion = "ErrorFecha";

                        muestraArgs.Fecha = DateTime.ParseExact(TextBox35.Text + " " + TextBox36.Text, "dd/MM/yyyy H:mm", CultureInfo.InvariantCulture);
                        muestraArgs.FechaRecepcion = DateTime.ParseExact(TextBox10.Text + " " + TextBox11.Text, "dd/MM/yyyy H:mm", CultureInfo.InvariantCulture);
                        break;

                }
                muestraArgs.MuestraID = t.MuestraID;
                muestraArgs.AnalisisID = t.AnalisisID;
                muestraArgs.Datum = t.Datum;
                muestraArgs.DireccionID = t.DireccionID;
                muestraArgs.Numero = t.Numero;
                muestraArgs.Latitud = t.Latitud;
                muestraArgs.Longitud = t.Longitud;
                muestraArgs.DireccionID = t.DireccionID;
                muestraArgs.NivelID = t.NivelID;
                muestraArgs.ValMuestraC = Convert.ToDecimal(TextBox9.Text);
                muestraArgs.ValHojaCampo = Convert.ToDecimal(CheckBox1.Checked);

                if (muestraArgs.Descripcion != "ErrorFecha")
                {
                    var muestra = muestraBl.Actualiza(muestraArgs);
                    muestraBl.RecalculaSimple(muestra.MuestraID);
                }

                conts++;
            }

            #endregion


            //TODO: Actualiza 
            muestraBl.RecalculaSimple((int)Session["analisiid"]);
            //TODO: Actualiza
            //CargaInformacion((int)Session["analisiid"]);
            Response.Redirect(Request.Url.AbsoluteUri);
            //Response.Redirect("~/Analisis.aspx");
        }
        private string RemoveSpecialCharacters(string str)
        {
            //return Regex.Replace(str, "[^a-zA-Z0-9_.- ]+", "", RegexOptions.Compiled);
            str = str.Replace(":", "");
            return str;
        }

        private void Calcular()
        {
            var listaMuestra = _muestraBl.Lista_Muestra((int)Session["analisiid"]).Where(x => x.NivelID == 1).ToList();
            //switch (listaMuestra.Count)
            //{
            //    case 2:
            //        #region 2
            //        var tex3_2 = TextBox3.Text != "" ? Convert.ToDecimal(TextBox3.Text) : 0;
            //        var tex142 = TextBox14.Text != "" ? Convert.ToDecimal(TextBox14.Text) : 0;


            //        var l42 = (tex3_2 + tex142).ToString("#####.####");
            //        if (l42 == "") l42 = "0";

            //        Label1.Text = l42 == "0" ? "0" : (tex3_2 / Convert.ToDecimal(l42)).ToString("##.####");
            //        Label10.Text = l42 == "0" ? "0" : (tex142 / Convert.ToDecimal(l42)).ToString("##.####");


            //        if (Label1.Text == "")
            //            Label1.Text = "0";
            //        if (Label10.Text == "")
            //            Label10.Text = "0";
            //        if (Label13.Text == "")
            //            Label13.Text = "0";
            //        if (Label16.Text == "")
            //            Label16.Text = "0";
            //        if (Label19.Text == "")
            //            Label19.Text = "0";
            //        if (Label22.Text == "")
            //            Label22.Text = "0";

            //        Label6.Text = (Convert.ToDecimal(Label1.Text) + Convert.ToDecimal(Label10.Text) +
            //                       Convert.ToDecimal(Label13.Text) + Convert.ToDecimal(Label16.Text) +
            //                       Convert.ToDecimal(Label19.Text) + Convert.ToDecimal(Label22.Text)).ToString("#####.####");
            //        if (Label6.Text == "")
            //            Label6.Text = "0.0000";

            //        var tb92 = TextBox9.Text == "" ? 0 : Convert.ToDecimal(TextBox9.Text);

            //        Label2.Text = (tb92 * Convert.ToDecimal(Label1.Text)).ToString("####.##");
            //        Label11.Text = (tb92 * Convert.ToDecimal(Label10.Text)).ToString("####.##");
            //        Label14.Text = (tb92 * Convert.ToDecimal(Label13.Text)).ToString("####.##");
            //        Label17.Text = (tb92 * Convert.ToDecimal(Label16.Text)).ToString("####.##");
            //        Label20.Text = (tb92 * Convert.ToDecimal(Label19.Text)).ToString("####.##");
            //        Label23.Text = (tb92 * Convert.ToDecimal(Label22.Text)).ToString("####.##");

            //        if (Label2.Text == "")
            //            Label2.Text = "0";
            //        if (Label11.Text == "")
            //            Label11.Text = "0";


            //        Label7.Text = (Convert.ToDecimal(Label2.Text) + Convert.ToDecimal(Label11.Text)).ToString("####.##");
            //        if (Label7.Text == "")
            //            Label7.Text = "0.00";

            //        var arr32 = new List<decimal>();
            //        if (TextBox3.Text != "" && TextBox3.Text != "0")
            //        {
            //            arr32.Add(1);
            //            if (ddlAction1.SelectedValue == "1" || ddlAction1.SelectedValue == "2")
            //                TextBox4.Enabled = false;
            //            else
            //                TextBox4.Enabled = true;
            //            if (ddlAction2.SelectedValue == "1" || ddlAction2.SelectedValue == "2")
            //                TextBox5.Enabled = false;
            //            else
            //                TextBox5.Enabled = true;
            //            if (ddlAction4.SelectedValue == "1" || ddlAction4.SelectedValue == "2")
            //                TextBox6.Enabled = false;
            //            else
            //                TextBox6.Enabled = true;
            //            if (ddlAction5.SelectedValue == "1" || ddlAction5.SelectedValue == "2")
            //                TextBox7.Enabled = false;
            //            else
            //                TextBox7.Enabled = true;
            //            if (ddlAction3.SelectedValue == "1" || ddlAction3.SelectedValue == "2")
            //                ddlana1.Enabled = false;
            //            else
            //                ddlana1.Enabled = true;
            //            TextBox52.Enabled = true;
            //            TextBox53.Enabled = true;
            //        }
            //        if (TextBox3.Text == "0")
            //        {
            //            TextBox4.Enabled = false;
            //            TextBox4.Text = "0";
            //            TextBox5.Enabled = false;
            //            TextBox5.Text = "0";
            //            TextBox6.Enabled = false;
            //            TextBox6.Text = "0";
            //            TextBox7.Enabled = false;
            //            TextBox7.Text = "0";
            //            TextBox52.Enabled = false;
            //            TextBox52.Text = "0";
            //            TextBox53.Enabled = false;
            //            TextBox53.Text = "0";
            //            ddlana1.Enabled = false;

            //        }
            //        if (TextBox14.Text != "" && TextBox14.Text != "0")
            //        {
            //            arr32.Add(1);
            //            if (ddlAction1.SelectedValue == "1" || ddlAction1.SelectedValue == "2")
            //                TextBox15.Enabled = false;
            //            else
            //                TextBox15.Enabled = true;
            //            if (ddlAction2.SelectedValue == "1" || ddlAction2.SelectedValue == "2")
            //                TextBox16.Enabled = false;
            //            else
            //                TextBox16.Enabled = true;
            //            if (ddlAction4.SelectedValue == "1" || ddlAction4.SelectedValue == "2")
            //                TextBox18.Enabled = false;
            //            else
            //                TextBox18.Enabled = true;
            //            if (ddlAction5.SelectedValue == "1" || ddlAction5.SelectedValue == "2")
            //                TextBox19.Enabled = false;
            //            else
            //                TextBox19.Enabled = true;
            //            if (ddlAction3.SelectedValue == "1" || ddlAction3.SelectedValue == "2")
            //                ddlana2.Enabled = false;
            //            else
            //                ddlana2.Enabled = true;
            //            TextBox54.Enabled = true;
            //            TextBox55.Enabled = true;
            //        }
            //        if (TextBox14.Text == "0")
            //        {
            //            TextBox15.Enabled = false;
            //            TextBox15.Text = "0";
            //            TextBox16.Enabled = false;
            //            TextBox16.Text = "0";
            //            TextBox18.Enabled = false;
            //            TextBox18.Text = "0";
            //            TextBox19.Enabled = false;
            //            TextBox19.Text = "0";
            //            TextBox54.Enabled = false;
            //            TextBox54.Text = "0";
            //            TextBox55.Enabled = false;
            //            TextBox55.Text = "0";
            //            ddlana2.Enabled = false;

            //        }

            //        if (arr32.Count != 2)
            //        {
            //            try
            //            { Label4.Text = (Convert.ToDecimal(l42) / arr32.Count).ToString("#####.##"); }
            //            catch (Exception)
            //            { Label4.Text = "0"; }
            //            arr32.Clear();
            //        }
            //        else
            //        {
            //            Label4.Text = (Convert.ToDecimal(l42) / 6).ToString("#####.##");
            //            arr32.Clear();
            //        }

            //        var arrC2 = new List<decimal>();
            //        if (TextBox5.Text != "0" && TextBox5.Text != "")
            //            arrC2.Add(1);
            //        if (TextBox16.Text != "0" && TextBox5.Text != "")
            //            arrC2.Add(1);


            //        var l52 = (Convert.ToDecimal(TextBox5.Text) + Convert.ToDecimal(TextBox16.Text)).ToString("#####.####");

            //        Label5.Text = (l52 == "0" || l52 == "") ? "0.0000" : (Convert.ToDecimal(l52) / arrC2.Count).ToString("####.####");
            //        arrC2.Clear();

            //        var arrmG32 = new List<double>();
            //        if (TextBox6.Text != "0" && TextBox6.Text != "")
            //            arrmG32.Add(Convert.ToDouble(TextBox6.Text));
            //        if (TextBox18.Text != "0" && TextBox18.Text != "")
            //            arrmG32.Add(Convert.ToDouble(TextBox18.Text));


            //        double r1632;
            //        switch (arrmG32.Count)
            //        {


            //            case 1:
            //                r1632 = arrmG32[0];
            //                r1632 = Math.Pow(r1632, 1);
            //                Label8.Text = r1632.ToString("######");
            //                break;
            //            case 2:
            //                r1632 = arrmG32[0] * arrmG32[1];
            //                r1632 = Math.Pow(r1632, 0.5);
            //                Label8.Text = r1632.ToString("######");
            //                break;

            //        }
            //        arrmG32.Clear();
            //        if (Label8.Text == "")
            //            Label8.Text = "0000";



            //        Label3.Text = (Convert.ToDecimal(TextBox7.Text) * Convert.ToDecimal(Label1.Text)).ToString("####.####");
            //        if (Label3.Text == "") Label3.Text = "0";
            //        Label12.Text = (Convert.ToDecimal(TextBox19.Text) * Convert.ToDecimal(Label10.Text)).ToString("####.####");
            //        if (Label12.Text == "") Label12.Text = "0";
            //        Label15.Text = (Convert.ToDecimal(TextBox41.Text) * Convert.ToDecimal(Label13.Text)).ToString("####.####");
            //        if (Label15.Text == "") Label15.Text = "0";
            //        Label18.Text = (Convert.ToDecimal(TextBox44.Text) * Convert.ToDecimal(Label16.Text)).ToString("####.####");
            //        if (Label18.Text == "") Label18.Text = "0";
            //        Label21.Text = (Convert.ToDecimal(TextBox47.Text) * Convert.ToDecimal(Label19.Text)).ToString("####.####");
            //        if (Label21.Text == "") Label21.Text = "0";
            //        Label24.Text = (Convert.ToDecimal(TextBox50.Text) * Convert.ToDecimal(Label22.Text)).ToString("####.####");
            //        if (Label24.Text == "") Label24.Text = "0";

            //        Label9.Text = (Convert.ToDecimal(Label3.Text) + Convert.ToDecimal(Label12.Text) +
            //                                  Convert.ToDecimal(Label15.Text) + Convert.ToDecimal(Label18.Text) +
            //                                  Convert.ToDecimal(Label21.Text) + Convert.ToDecimal(Label24.Text)).ToString("####.####");
            //        if (Label9.Text == "")
            //            Label9.Text = "0.0000";
            //        #endregion
            //        break;
            //    case 4:
            //        #region 4
            //        var tex34 = TextBox3.Text != "" ? Convert.ToDecimal(TextBox3.Text) : 0;
            //        var tex144 = TextBox14.Text != "" ? Convert.ToDecimal(TextBox14.Text) : 0;
            //        var tex224 = TextBox22.Text != "" ? Convert.ToDecimal(TextBox22.Text) : 0;
            //        var tex274 = TextBox27.Text != "" ? Convert.ToDecimal(TextBox27.Text) : 0;
            //        var tex324 = TextBox32.Text != "" ? Convert.ToDecimal(TextBox32.Text) : 0;
            //        var tex374 = TextBox37.Text != "" ? Convert.ToDecimal(TextBox37.Text) : 0;

            //        var l44 = (tex34 + tex144 + tex224 + tex274 + tex324 + tex374).ToString("#####.####");
            //        if (l44 == "") l44 = "0";

            //        Label1.Text = l44 == "0" ? "0" : (tex34 / Convert.ToDecimal(l44)).ToString("##.####");
            //        Label10.Text = l44 == "0" ? "0" : (tex144 / Convert.ToDecimal(l44)).ToString("##.####");
            //        Label13.Text = l44 == "0" ? "0" : (tex224 / Convert.ToDecimal(l44)).ToString("##.####");
            //        Label16.Text = l44 == "0" ? "0" : (tex274 / Convert.ToDecimal(l44)).ToString("##.####");


            //        if (Label1.Text == "")
            //            Label1.Text = "0";
            //        if (Label10.Text == "")
            //            Label10.Text = "0";
            //        if (Label13.Text == "")
            //            Label13.Text = "0";
            //        if (Label16.Text == "")
            //            Label16.Text = "0";

            //        Label6.Text = (Convert.ToDecimal(Label1.Text) + Convert.ToDecimal(Label10.Text) +
            //                       Convert.ToDecimal(Label13.Text) + Convert.ToDecimal(Label16.Text)).ToString("#####.####");
            //        if (Label6.Text == "")
            //            Label6.Text = "0.0000";

            //        var tb94 = TextBox9.Text == "" ? 0 : Convert.ToDecimal(TextBox9.Text);

            //        Label2.Text = (tb94 * Convert.ToDecimal(Label1.Text)).ToString("####.##");
            //        Label11.Text = (tb94 * Convert.ToDecimal(Label10.Text)).ToString("####.##");
            //        Label14.Text = (tb94 * Convert.ToDecimal(Label13.Text)).ToString("####.##");
            //        Label17.Text = (tb94 * Convert.ToDecimal(Label16.Text)).ToString("####.##");


            //        if (Label2.Text == "")
            //            Label2.Text = "0";
            //        if (Label11.Text == "")
            //            Label11.Text = "0";
            //        if (Label14.Text == "")
            //            Label14.Text = "0";
            //        if (Label17.Text == "")
            //            Label17.Text = "0";


            //        Label7.Text = (Convert.ToDecimal(Label2.Text) + Convert.ToDecimal(Label11.Text) +
            //                      Convert.ToDecimal(Label14.Text) + Convert.ToDecimal(Label17.Text)).ToString("####.##");
            //        if (Label7.Text == "")
            //            Label7.Text = "0.00";

            //        var arr34 = new List<decimal>();
            //        if (TextBox3.Text != "" && TextBox3.Text != "0")
            //        {
            //            arr34.Add(1);
            //            if (ddlAction1.SelectedValue == "1" || ddlAction1.SelectedValue == "2")
            //                TextBox4.Enabled = false;
            //            else
            //                TextBox4.Enabled = true;
            //            if (ddlAction2.SelectedValue == "1" || ddlAction2.SelectedValue == "2")
            //                TextBox5.Enabled = false;
            //            else
            //                TextBox5.Enabled = true;
            //            if (ddlAction4.SelectedValue == "1" || ddlAction4.SelectedValue == "2")
            //                TextBox6.Enabled = false;
            //            else
            //                TextBox6.Enabled = true;
            //            if (ddlAction5.SelectedValue == "1" || ddlAction5.SelectedValue == "2")
            //                TextBox7.Enabled = false;
            //            else
            //                TextBox7.Enabled = true;
            //            if (ddlAction3.SelectedValue == "1" || ddlAction3.SelectedValue == "2")
            //                ddlana1.Enabled = false;
            //            else
            //                ddlana1.Enabled = true;
            //            TextBox52.Enabled = true;
            //            TextBox53.Enabled = true;
            //        }
            //        if (TextBox3.Text == "0")
            //        {
            //            TextBox4.Enabled = false;
            //            TextBox4.Text = "0";
            //            TextBox5.Enabled = false;
            //            TextBox5.Text = "0";
            //            TextBox6.Enabled = false;
            //            TextBox6.Text = "0";
            //            TextBox7.Enabled = false;
            //            TextBox7.Text = "0";
            //            TextBox52.Enabled = false;
            //            TextBox52.Text = "0";
            //            TextBox53.Enabled = false;
            //            TextBox53.Text = "0";
            //            ddlana1.Enabled = false;

            //        }
            //        if (TextBox14.Text != "" && TextBox14.Text != "0")
            //        {
            //            arr34.Add(1);
            //            if (ddlAction1.SelectedValue == "1" || ddlAction1.SelectedValue == "2")
            //                TextBox15.Enabled = false;
            //            else
            //                TextBox15.Enabled = true;
            //            if (ddlAction2.SelectedValue == "1" || ddlAction2.SelectedValue == "2")
            //                TextBox16.Enabled = false;
            //            else
            //                TextBox16.Enabled = true;
            //            if (ddlAction4.SelectedValue == "1" || ddlAction4.SelectedValue == "2")
            //                TextBox18.Enabled = false;
            //            else
            //                TextBox18.Enabled = true;
            //            if (ddlAction5.SelectedValue == "1" || ddlAction5.SelectedValue == "2")
            //                TextBox19.Enabled = false;
            //            else
            //                TextBox19.Enabled = true;
            //            if (ddlAction3.SelectedValue == "1" || ddlAction3.SelectedValue == "2")
            //                ddlana2.Enabled = false;
            //            else
            //                ddlana2.Enabled = true;
            //            TextBox54.Enabled = true;
            //            TextBox55.Enabled = true;
            //        }
            //        if (TextBox14.Text == "0")
            //        {
            //            TextBox15.Enabled = false;
            //            TextBox15.Text = "0";
            //            TextBox16.Enabled = false;
            //            TextBox16.Text = "0";
            //            TextBox18.Enabled = false;
            //            TextBox18.Text = "0";
            //            TextBox19.Enabled = false;
            //            TextBox19.Text = "0";
            //            TextBox54.Enabled = false;
            //            TextBox54.Text = "0";
            //            TextBox55.Enabled = false;
            //            TextBox55.Text = "0";
            //            ddlana2.Enabled = false;

            //        }
            //        if (TextBox22.Text != "" && TextBox22.Text != "0")
            //        {
            //            arr34.Add(1);
            //            if (ddlAction1.SelectedValue == "1" || ddlAction1.SelectedValue == "2")
            //                TextBox23.Enabled = false;
            //            else
            //                TextBox23.Enabled = true;
            //            if (ddlAction2.SelectedValue == "1" || ddlAction2.SelectedValue == "2")
            //                TextBox24.Enabled = false;
            //            else
            //                TextBox24.Enabled = true;
            //            if (ddlAction4.SelectedValue == "1" || ddlAction4.SelectedValue == "2")
            //                TextBox40.Enabled = false;
            //            else
            //                TextBox40.Enabled = true;
            //            if (ddlAction5.SelectedValue == "1" || ddlAction5.SelectedValue == "2")
            //                TextBox41.Enabled = false;
            //            else
            //                TextBox41.Enabled = true;
            //            if (ddlAction3.SelectedValue == "1" || ddlAction3.SelectedValue == "2")
            //                ddlana3.Enabled = false;
            //            else
            //                ddlana3.Enabled = true;
            //            TextBox56.Enabled = true;
            //            TextBox57.Enabled = true;
            //        }
            //        if (TextBox22.Text == "0")
            //        {
            //            TextBox23.Enabled = false;
            //            TextBox23.Text = "0";
            //            TextBox24.Enabled = false;
            //            TextBox24.Text = "0";
            //            TextBox40.Enabled = false;
            //            TextBox40.Text = "0";
            //            TextBox41.Enabled = false;
            //            TextBox41.Text = "0";
            //            TextBox56.Enabled = false;
            //            TextBox56.Text = "0";
            //            TextBox57.Enabled = false;
            //            TextBox57.Text = "0";
            //            ddlana3.Enabled = false;

            //        }
            //        if (TextBox27.Text != "" && TextBox27.Text != "0")
            //        {
            //            arr34.Add(1);
            //            if (ddlAction1.SelectedValue == "1" || ddlAction1.SelectedValue == "2")
            //                TextBox28.Enabled = false;
            //            else
            //                TextBox28.Enabled = true;
            //            if (ddlAction2.SelectedValue == "1" || ddlAction2.SelectedValue == "2")
            //                TextBox29.Enabled = false;
            //            else
            //                TextBox29.Enabled = true;
            //            if (ddlAction4.SelectedValue == "1" || ddlAction4.SelectedValue == "2")
            //                TextBox43.Enabled = false;
            //            else
            //                TextBox43.Enabled = true;
            //            if (ddlAction5.SelectedValue == "1" || ddlAction5.SelectedValue == "2")
            //                TextBox44.Enabled = false;
            //            else
            //                TextBox44.Enabled = true;
            //            if (ddlAction3.SelectedValue == "1" || ddlAction3.SelectedValue == "2")
            //                ddlana4.Enabled = false;
            //            else
            //                ddlana4.Enabled = true;
            //            TextBox58.Enabled = true;
            //            TextBox59.Enabled = true;
            //        }
            //        if (TextBox27.Text == "0")
            //        {
            //            TextBox28.Enabled = false;
            //            TextBox28.Text = "0";
            //            TextBox29.Enabled = false;
            //            TextBox29.Text = "0";
            //            TextBox43.Enabled = false;
            //            TextBox43.Text = "0";
            //            TextBox44.Enabled = false;
            //            TextBox44.Text = "0";
            //            TextBox58.Enabled = false;
            //            TextBox58.Text = "0";
            //            TextBox59.Enabled = false;
            //            TextBox59.Text = "0";
            //            ddlana4.Enabled = false;

            //        }



            //        if (arr34.Count != 4)
            //        {
            //            try
            //            { Label4.Text = (Convert.ToDecimal(l44) / arr34.Count).ToString("#####.##"); }
            //            catch (Exception)
            //            { Label4.Text = "0"; }
            //            arr34.Clear();
            //        }
            //        else
            //        {
            //            Label4.Text = (Convert.ToDecimal(l44) / 4).ToString("#####.##");
            //            arr34.Clear();
            //        }

            //        var arrC4 = new List<decimal>();
            //        if (TextBox5.Text != "0" && TextBox5.Text != "")
            //            arrC4.Add(1);
            //        if (TextBox16.Text != "0" && TextBox5.Text != "")
            //            arrC4.Add(1);
            //        if (TextBox24.Text != "0" && TextBox5.Text != "")
            //            arrC4.Add(1);
            //        if (TextBox29.Text != "0" && TextBox5.Text != "")
            //            arrC4.Add(1);


            //        var l54 = (Convert.ToDecimal(TextBox5.Text) + Convert.ToDecimal(TextBox16.Text) +
            //                         Convert.ToDecimal(TextBox24.Text) + Convert.ToDecimal(TextBox29.Text)).ToString("#####.####");

            //        Label5.Text = (l54 == "0" || l54 == "") ? "0.0000" : (Convert.ToDecimal(l54) / arrC4.Count).ToString("####.####");
            //        arrC4.Clear();

            //        var arrmG34 = new List<double>();
            //        if (TextBox6.Text != "0" && TextBox6.Text != "")
            //            arrmG34.Add(Convert.ToDouble(TextBox6.Text));
            //        if (TextBox18.Text != "0" && TextBox18.Text != "")
            //            arrmG34.Add(Convert.ToDouble(TextBox18.Text));
            //        if (TextBox40.Text != "0" && TextBox40.Text != "")
            //            arrmG34.Add(Convert.ToDouble(TextBox40.Text));
            //        if (TextBox43.Text != "0" && TextBox43.Text != "")
            //            arrmG34.Add(Convert.ToDouble(TextBox43.Text));


            //        double r1634;
            //        switch (arrmG34.Count)
            //        {


            //            case 1:
            //                r1634 = arrmG34[0];
            //                r1634 = Math.Pow(r1634, 1);
            //                Label8.Text = r1634.ToString("######");
            //                break;
            //            case 2:
            //                r1634 = arrmG34[0] * arrmG34[1];
            //                r1634 = Math.Pow(r1634, 0.5);
            //                Label8.Text = r1634.ToString("######");
            //                break;
            //            case 3:
            //                r1634 = arrmG34[0] * arrmG34[1] * arrmG34[2];
            //                r1634 = Math.Pow(r1634, 0.33333333333333333333333333333333);
            //                Label8.Text = r1634.ToString("######");
            //                break;
            //            case 4:
            //                r1634 = arrmG34[0] * arrmG34[1] * arrmG34[2] * arrmG34[3];
            //                r1634 = Math.Pow(r1634, 0.25);
            //                Label8.Text = r1634.ToString("######");
            //                break;

            //        }
            //        arrmG34.Clear();
            //        if (Label8.Text == "")
            //            Label8.Text = "0000";



            //        Label3.Text = (Convert.ToDecimal(TextBox7.Text) * Convert.ToDecimal(Label1.Text)).ToString("####.####");
            //        if (Label3.Text == "") Label3.Text = "0";
            //        Label12.Text = (Convert.ToDecimal(TextBox19.Text) * Convert.ToDecimal(Label10.Text)).ToString("####.####");
            //        if (Label12.Text == "") Label12.Text = "0";
            //        Label15.Text = (Convert.ToDecimal(TextBox41.Text) * Convert.ToDecimal(Label13.Text)).ToString("####.####");
            //        if (Label15.Text == "") Label15.Text = "0";
            //        Label18.Text = (Convert.ToDecimal(TextBox44.Text) * Convert.ToDecimal(Label16.Text)).ToString("####.####");
            //        if (Label18.Text == "") Label18.Text = "0";



            //        Label9.Text = (Convert.ToDecimal(Label3.Text) + Convert.ToDecimal(Label12.Text) +
            //                                  Convert.ToDecimal(Label15.Text) + Convert.ToDecimal(Label18.Text)
            //                                 ).ToString("####.####");
            //        if (Label9.Text == "")
            //            Label9.Text = "0.0000";
            //        #endregion
            //        break;
            //    case 6:
            //        #region 6
            //        var tex3 = TextBox3.Text != "" ? Convert.ToDecimal(TextBox3.Text) : 0;
            //        var tex14 = TextBox14.Text != "" ? Convert.ToDecimal(TextBox14.Text) : 0;
            //        var tex22 = TextBox22.Text != "" ? Convert.ToDecimal(TextBox22.Text) : 0;
            //        var tex27 = TextBox27.Text != "" ? Convert.ToDecimal(TextBox27.Text) : 0;
            //        var tex32 = TextBox32.Text != "" ? Convert.ToDecimal(TextBox32.Text) : 0;
            //        var tex37 = TextBox37.Text != "" ? Convert.ToDecimal(TextBox37.Text) : 0;

            //        var l4 = (tex3 + tex14 + tex22 + tex27 + tex32 + tex37).ToString("#####.####");
            //        if (l4 == "") l4 = "0";

            //        Label1.Text = l4 == "0" ? "0" : (tex3 / Convert.ToDecimal(l4)).ToString("##.####");
            //        Label10.Text = l4 == "0" ? "0" : (tex14 / Convert.ToDecimal(l4)).ToString("##.####");
            //        Label13.Text = l4 == "0" ? "0" : (tex22 / Convert.ToDecimal(l4)).ToString("##.####");
            //        Label16.Text = l4 == "0" ? "0" : (tex27 / Convert.ToDecimal(l4)).ToString("##.####");
            //        Label19.Text = l4 == "0" ? "0" : (tex32 / Convert.ToDecimal(l4)).ToString("##.####");
            //        Label22.Text = l4 == "0" ? "0" : (tex37 / Convert.ToDecimal(l4)).ToString("##.####");

            //        if (Label1.Text == "")
            //            Label1.Text = "0";
            //        if (Label10.Text == "")
            //            Label10.Text = "0";
            //        if (Label13.Text == "")
            //            Label13.Text = "0";
            //        if (Label16.Text == "")
            //            Label16.Text = "0";
            //        if (Label19.Text == "")
            //            Label19.Text = "0";
            //        if (Label22.Text == "")
            //            Label22.Text = "0";

            //        Label6.Text = (Convert.ToDecimal(Label1.Text) + Convert.ToDecimal(Label10.Text) +
            //                       Convert.ToDecimal(Label13.Text) + Convert.ToDecimal(Label16.Text) +
            //                       Convert.ToDecimal(Label19.Text) + Convert.ToDecimal(Label22.Text)).ToString("#####.####");
            //        if (Label6.Text == "")
            //            Label6.Text = "0.0000";

            //        var tb9 = TextBox9.Text == "" ? 0 : Convert.ToDecimal(TextBox9.Text);

            //        Label2.Text = (tb9 * Convert.ToDecimal(Label1.Text)).ToString("####.##");
            //        Label11.Text = (tb9 * Convert.ToDecimal(Label10.Text)).ToString("####.##");
            //        Label14.Text = (tb9 * Convert.ToDecimal(Label13.Text)).ToString("####.##");
            //        Label17.Text = (tb9 * Convert.ToDecimal(Label16.Text)).ToString("####.##");
            //        Label20.Text = (tb9 * Convert.ToDecimal(Label19.Text)).ToString("####.##");
            //        Label23.Text = (tb9 * Convert.ToDecimal(Label22.Text)).ToString("####.##");

            //        if (Label2.Text == "")
            //            Label2.Text = "0";
            //        if (Label11.Text == "")
            //            Label11.Text = "0";
            //        if (Label14.Text == "")
            //            Label14.Text = "0";
            //        if (Label17.Text == "")
            //            Label17.Text = "0";
            //        if (Label20.Text == "")
            //            Label20.Text = "0";
            //        if (Label23.Text == "")
            //            Label23.Text = "0";

            //        Label7.Text = (Convert.ToDecimal(Label2.Text) + Convert.ToDecimal(Label11.Text) +
            //                      Convert.ToDecimal(Label14.Text) + Convert.ToDecimal(Label17.Text) +
            //                      Convert.ToDecimal(Label20.Text) + Convert.ToDecimal(Label23.Text)).ToString("####.##");
            //        if (Label7.Text == "")
            //            Label7.Text = "0.00";

            //        var arr3 = new List<decimal>();
            //        if (TextBox3.Text != "" && TextBox3.Text != "0")
            //        {
            //            arr3.Add(1);
            //            if (ddlAction1.SelectedValue == "1" || ddlAction1.SelectedValue == "2")
            //                TextBox4.Enabled = false;
            //            else
            //                TextBox4.Enabled = true;
            //            if (ddlAction2.SelectedValue == "1" || ddlAction2.SelectedValue == "2")
            //                TextBox5.Enabled = false;
            //            else
            //                TextBox5.Enabled = true;
            //            if (ddlAction4.SelectedValue == "1" || ddlAction4.SelectedValue == "2")
            //                TextBox6.Enabled = false;
            //            else
            //                TextBox6.Enabled = true;
            //            if (ddlAction5.SelectedValue == "1" || ddlAction5.SelectedValue == "2")
            //                TextBox7.Enabled = false;
            //            else
            //                TextBox7.Enabled = true;
            //            if (ddlAction3.SelectedValue == "1" || ddlAction3.SelectedValue == "2")
            //                ddlana1.Enabled = false;
            //            else
            //                ddlana1.Enabled = true;
            //            TextBox52.Enabled = true;
            //            TextBox53.Enabled = true;
            //        }
            //        if (TextBox3.Text == "0")
            //        {
            //            TextBox4.Enabled = false;
            //            TextBox4.Text = "0";
            //            TextBox5.Enabled = false;
            //            TextBox5.Text = "0";
            //            TextBox6.Enabled = false;
            //            TextBox6.Text = "0";
            //            TextBox7.Enabled = false;
            //            TextBox7.Text = "0";
            //            TextBox52.Enabled = false;
            //            TextBox52.Text = "0";
            //            TextBox53.Enabled = false;
            //            TextBox53.Text = "0";
            //            ddlana1.Enabled = false;

            //        }
            //        if (TextBox14.Text != "" && TextBox14.Text != "0")
            //        {
            //            arr3.Add(1);
            //            if (ddlAction1.SelectedValue == "1" || ddlAction1.SelectedValue == "2")
            //                TextBox15.Enabled = false;
            //            else
            //                TextBox15.Enabled = true;
            //            if (ddlAction2.SelectedValue == "1" || ddlAction2.SelectedValue == "2")
            //                TextBox16.Enabled = false;
            //            else
            //                TextBox16.Enabled = true;
            //            if (ddlAction4.SelectedValue == "1" || ddlAction4.SelectedValue == "2")
            //                TextBox18.Enabled = false;
            //            else
            //                TextBox18.Enabled = true;
            //            if (ddlAction5.SelectedValue == "1" || ddlAction5.SelectedValue == "2")
            //                TextBox19.Enabled = false;
            //            else
            //                TextBox19.Enabled = true;
            //            if (ddlAction3.SelectedValue == "1" || ddlAction3.SelectedValue == "2")
            //                ddlana2.Enabled = false;
            //            else
            //                ddlana2.Enabled = true;
            //            TextBox54.Enabled = true;
            //            TextBox55.Enabled = true;
            //        }
            //        if (TextBox14.Text == "0")
            //        {
            //            TextBox15.Enabled = false;
            //            TextBox15.Text = "0";
            //            TextBox16.Enabled = false;
            //            TextBox16.Text = "0";
            //            TextBox18.Enabled = false;
            //            TextBox18.Text = "0";
            //            TextBox19.Enabled = false;
            //            TextBox19.Text = "0";
            //            TextBox54.Enabled = false;
            //            TextBox54.Text = "0";
            //            TextBox55.Enabled = false;
            //            TextBox55.Text = "0";
            //            ddlana2.Enabled = false;

            //        }
            //        if (TextBox22.Text != "" && TextBox22.Text != "0")
            //        {
            //            arr3.Add(1);
            //            if (ddlAction1.SelectedValue == "1" || ddlAction1.SelectedValue == "2")
            //                TextBox23.Enabled = false;
            //            else
            //                TextBox23.Enabled = true;
            //            if (ddlAction2.SelectedValue == "1" || ddlAction2.SelectedValue == "2")
            //                TextBox24.Enabled = false;
            //            else
            //                TextBox24.Enabled = true;
            //            if (ddlAction4.SelectedValue == "1" || ddlAction4.SelectedValue == "2")
            //                TextBox40.Enabled = false;
            //            else
            //                TextBox40.Enabled = true;
            //            if (ddlAction5.SelectedValue == "1" || ddlAction5.SelectedValue == "2")
            //                TextBox41.Enabled = false;
            //            else
            //                TextBox41.Enabled = true;
            //            if (ddlAction3.SelectedValue == "1" || ddlAction3.SelectedValue == "2")
            //                ddlana3.Enabled = false;
            //            else
            //                ddlana3.Enabled = true;
            //            TextBox56.Enabled = true;
            //            TextBox57.Enabled = true;
            //        }
            //        if (TextBox22.Text == "0")
            //        {
            //            TextBox23.Enabled = false;
            //            TextBox23.Text = "0";
            //            TextBox24.Enabled = false;
            //            TextBox24.Text = "0";
            //            TextBox40.Enabled = false;
            //            TextBox40.Text = "0";
            //            TextBox41.Enabled = false;
            //            TextBox41.Text = "0";
            //            TextBox56.Enabled = false;
            //            TextBox56.Text = "0";
            //            TextBox57.Enabled = false;
            //            TextBox57.Text = "0";
            //            ddlana3.Enabled = false;

            //        }
            //        if (TextBox27.Text != "" && TextBox27.Text != "0")
            //        {
            //            arr3.Add(1);
            //            if (ddlAction1.SelectedValue == "1" || ddlAction1.SelectedValue == "2")
            //                TextBox28.Enabled = false;
            //            else
            //                TextBox28.Enabled = true;
            //            if (ddlAction2.SelectedValue == "1" || ddlAction2.SelectedValue == "2")
            //                TextBox29.Enabled = false;
            //            else
            //                TextBox29.Enabled = true;
            //            if (ddlAction4.SelectedValue == "1" || ddlAction4.SelectedValue == "2")
            //                TextBox43.Enabled = false;
            //            else
            //                TextBox43.Enabled = true;
            //            if (ddlAction5.SelectedValue == "1" || ddlAction5.SelectedValue == "2")
            //                TextBox44.Enabled = false;
            //            else
            //                TextBox44.Enabled = true;
            //            if (ddlAction3.SelectedValue == "1" || ddlAction3.SelectedValue == "2")
            //                ddlana4.Enabled = false;
            //            else
            //                ddlana4.Enabled = true;
            //            TextBox58.Enabled = true;
            //            TextBox59.Enabled = true;
            //        }
            //        if (TextBox27.Text == "0")
            //        {
            //            TextBox28.Enabled = false;
            //            TextBox28.Text = "0";
            //            TextBox29.Enabled = false;
            //            TextBox29.Text = "0";
            //            TextBox43.Enabled = false;
            //            TextBox43.Text = "0";
            //            TextBox44.Enabled = false;
            //            TextBox44.Text = "0";
            //            TextBox58.Enabled = false;
            //            TextBox58.Text = "0";
            //            TextBox59.Enabled = false;
            //            TextBox59.Text = "0";
            //            ddlana4.Enabled = false;

            //        }
            //        if (TextBox32.Text != "" && TextBox32.Text != "0")
            //        {
            //            arr3.Add(1);
            //            if (ddlAction1.SelectedValue == "1" || ddlAction1.SelectedValue == "2")
            //                TextBox33.Enabled = false;
            //            else
            //                TextBox33.Enabled = true;
            //            if (ddlAction2.SelectedValue == "1" || ddlAction2.SelectedValue == "2")
            //                TextBox34.Enabled = false;
            //            else
            //                TextBox34.Enabled = true;
            //            if (ddlAction4.SelectedValue == "1" || ddlAction4.SelectedValue == "2")
            //                TextBox46.Enabled = false;
            //            else
            //                TextBox46.Enabled = true;
            //            if (ddlAction5.SelectedValue == "1" || ddlAction5.SelectedValue == "2")
            //                TextBox47.Enabled = false;
            //            else
            //                TextBox47.Enabled = true;
            //            if (ddlAction3.SelectedValue == "1" || ddlAction3.SelectedValue == "2")
            //                ddlana5.Enabled = false;
            //            else
            //                ddlana5.Enabled = true;
            //            TextBox60.Enabled = true;
            //            TextBox61.Enabled = true;
            //        }
            //        if (TextBox32.Text == "0")
            //        {
            //            TextBox33.Enabled = false;
            //            TextBox33.Text = "0";
            //            TextBox34.Enabled = false;
            //            TextBox34.Text = "0";
            //            TextBox46.Enabled = false;
            //            TextBox46.Text = "0";
            //            TextBox47.Enabled = false;
            //            TextBox47.Text = "0";
            //            TextBox60.Enabled = false;
            //            TextBox60.Text = "0";
            //            TextBox61.Enabled = false;
            //            TextBox61.Text = "0";
            //            ddlana5.Enabled = false;

            //        }
            //        if (TextBox37.Text != "" && TextBox37.Text != "0")
            //        {
            //            arr3.Add(1);
            //            if (ddlAction1.SelectedValue == "1" || ddlAction1.SelectedValue == "2")
            //                TextBox38.Enabled = false;
            //            else
            //                TextBox38.Enabled = true;
            //            if (ddlAction2.SelectedValue == "1" || ddlAction2.SelectedValue == "2")
            //                TextBox39.Enabled = false;
            //            else
            //                TextBox39.Enabled = true;
            //            if (ddlAction4.SelectedValue == "1" || ddlAction4.SelectedValue == "2")
            //                TextBox49.Enabled = false;
            //            else
            //                TextBox49.Enabled = true;
            //            if (ddlAction5.SelectedValue == "1" || ddlAction5.SelectedValue == "2")
            //                TextBox50.Enabled = false;
            //            else
            //                TextBox50.Enabled = true;
            //            if (ddlAction3.SelectedValue == "1" || ddlAction3.SelectedValue == "2")
            //                ddlana6.Enabled = false;
            //            else
            //                ddlana6.Enabled = true;
            //            TextBox62.Enabled = true;
            //            TextBox63.Enabled = true;
            //        }
            //        if (TextBox37.Text == "0")
            //        {
            //            TextBox38.Enabled = false;
            //            TextBox38.Text = "0";
            //            TextBox39.Enabled = false;
            //            TextBox39.Text = "0";
            //            TextBox49.Enabled = false;
            //            TextBox49.Text = "0";
            //            TextBox50.Enabled = false;
            //            TextBox50.Text = "0";
            //            TextBox62.Enabled = false;
            //            TextBox62.Text = "0";
            //            TextBox63.Enabled = false;
            //            TextBox63.Text = "0";
            //            ddlana6.Enabled = false;

            //        }
            //        if (arr3.Count != 6)
            //        {
            //            try
            //            { Label4.Text = (Convert.ToDecimal(l4) / arr3.Count).ToString("#####.##"); }
            //            catch (Exception)
            //            { Label4.Text = "0"; }
            //            arr3.Clear();
            //        }
            //        else
            //        {
            //            Label4.Text = (Convert.ToDecimal(l4) / 6).ToString("#####.##");
            //            arr3.Clear();
            //        }

            //        var arrC = new List<decimal>();
            //        if (TextBox5.Text != "0" && TextBox5.Text != "")
            //            arrC.Add(1);
            //        if (TextBox16.Text != "0" && TextBox5.Text != "")
            //            arrC.Add(1);
            //        if (TextBox24.Text != "0" && TextBox5.Text != "")
            //            arrC.Add(1);
            //        if (TextBox29.Text != "0" && TextBox5.Text != "")
            //            arrC.Add(1);
            //        if (TextBox34.Text != "0" && TextBox5.Text != "")
            //            arrC.Add(1);
            //        if (TextBox39.Text != "0" && TextBox5.Text != "")
            //            arrC.Add(1);

            //        var l5 = (Convert.ToDecimal(TextBox5.Text) + Convert.ToDecimal(TextBox16.Text) +
            //                         Convert.ToDecimal(TextBox24.Text) + Convert.ToDecimal(TextBox29.Text) +
            //                         Convert.ToDecimal(TextBox34.Text) + Convert.ToDecimal(TextBox39.Text)).ToString("#####.####");

            //        Label5.Text = (l5 == "0" || l5 == "") ? "0.0000" : (Convert.ToDecimal(l5) / arrC.Count).ToString("####.####");
            //        arrC.Clear();

            //        var arrmG3 = new List<double>();
            //        if (TextBox6.Text != "0" && TextBox6.Text != "")
            //            arrmG3.Add(Convert.ToDouble(TextBox6.Text));
            //        if (TextBox18.Text != "0" && TextBox18.Text != "")
            //            arrmG3.Add(Convert.ToDouble(TextBox18.Text));
            //        if (TextBox40.Text != "0" && TextBox40.Text != "")
            //            arrmG3.Add(Convert.ToDouble(TextBox40.Text));
            //        if (TextBox43.Text != "0" && TextBox43.Text != "")
            //            arrmG3.Add(Convert.ToDouble(TextBox43.Text));
            //        if (TextBox46.Text != "0" && TextBox46.Text != "")
            //            arrmG3.Add(Convert.ToDouble(TextBox46.Text));
            //        if (TextBox49.Text != "0" && TextBox49.Text != "0")
            //            arrmG3.Add(Convert.ToDouble(TextBox49.Text));


            //        double r163;
            //        switch (arrmG3.Count)
            //        {


            //            case 1:
            //                r163 = arrmG3[0];
            //                r163 = Math.Pow(r163, 1);
            //                Label8.Text = r163.ToString("######");
            //                break;
            //            case 2:
            //                r163 = arrmG3[0] * arrmG3[1];
            //                r163 = Math.Pow(r163, 0.5);
            //                Label8.Text = r163.ToString("######");
            //                break;
            //            case 3:
            //                r163 = arrmG3[0] * arrmG3[1] * arrmG3[2];
            //                r163 = Math.Pow(r163, 0.33333333333333333333333333333333);
            //                Label8.Text = r163.ToString("######");
            //                break;
            //            case 4:
            //                r163 = arrmG3[0] * arrmG3[1] * arrmG3[2] * arrmG3[3];
            //                r163 = Math.Pow(r163, 0.25);
            //                Label8.Text = r163.ToString("######");
            //                break;
            //            case 5:
            //                r163 = arrmG3[0] * arrmG3[1] * arrmG3[2] * arrmG3[3] * arrmG3[4];
            //                r163 = Math.Pow(r163, 0.2);
            //                Label8.Text = r163.ToString("######");
            //                break;
            //            case 6:
            //                r163 = arrmG3[0] * arrmG3[1] * arrmG3[2] * arrmG3[3] * arrmG3[4] * arrmG3[5];
            //                r163 = Math.Pow(r163, 0.16666666666666666666666666666667);
            //                Label8.Text = r163.ToString("######");

            //                break;
            //        }
            //        arrmG3.Clear();
            //        if (Label8.Text == "")
            //            Label8.Text = "0000";



            //        Label3.Text = (Convert.ToDecimal(TextBox7.Text) * Convert.ToDecimal(Label1.Text)).ToString("####.####");
            //        if (Label3.Text == "") Label3.Text = "0";
            //        Label12.Text = (Convert.ToDecimal(TextBox19.Text) * Convert.ToDecimal(Label10.Text)).ToString("####.####");
            //        if (Label12.Text == "") Label12.Text = "0";
            //        Label15.Text = (Convert.ToDecimal(TextBox41.Text) * Convert.ToDecimal(Label13.Text)).ToString("####.####");
            //        if (Label15.Text == "") Label15.Text = "0";
            //        Label18.Text = (Convert.ToDecimal(TextBox44.Text) * Convert.ToDecimal(Label16.Text)).ToString("####.####");
            //        if (Label18.Text == "") Label18.Text = "0";
            //        Label21.Text = (Convert.ToDecimal(TextBox47.Text) * Convert.ToDecimal(Label19.Text)).ToString("####.####");
            //        if (Label21.Text == "") Label21.Text = "0";
            //        Label24.Text = (Convert.ToDecimal(TextBox50.Text) * Convert.ToDecimal(Label22.Text)).ToString("####.####");
            //        if (Label24.Text == "") Label24.Text = "0";

            //        Label9.Text = (Convert.ToDecimal(Label3.Text) + Convert.ToDecimal(Label12.Text) +
            //                                  Convert.ToDecimal(Label15.Text) + Convert.ToDecimal(Label18.Text) +
            //                                  Convert.ToDecimal(Label21.Text) + Convert.ToDecimal(Label24.Text)).ToString("####.####");
            //        if (Label9.Text == "")
            //            Label9.Text = "0.0000";
            //        #endregion
            //        break;

            //}

            var textbox = new List<string>();

            var textboxga = new List<string>();
            var textboxmg = new List<string>();
            var textboxtemp = new List<string>();
            switch (listaMuestra.Count)
            {
                case 2:
                    textbox.Add(CA1.Text); textbox.Add(CA2.Text);
                    textboxga.Add(GA1.Text); textboxga.Add(GA2.Text);
                    textboxmg.Add(CF1.Text); textboxmg.Add(CF2.Text);
                    textboxtemp.Add(T1.Text); textboxtemp.Add(T2.Text);
                    break;
                case 4:
                    textbox.Add(CA1.Text); textbox.Add(CA2.Text); textbox.Add(CA3.Text); textbox.Add(CA4.Text);
                    textboxga.Add(GA1.Text); textboxga.Add(GA2.Text); textboxga.Add(GA3.Text); textboxga.Add(GA4.Text);
                    textboxmg.Add(CF1.Text); textboxmg.Add(CF2.Text); textboxmg.Add(CF3.Text); textboxmg.Add(CF4.Text);
                    textboxtemp.Add(T1.Text); textboxtemp.Add(T2.Text); textboxtemp.Add(T3.Text); textboxtemp.Add(T4.Text);
                    break;
                case 6:
                    textbox.Add(CA1.Text); textbox.Add(CA2.Text); textbox.Add(CA3.Text); textbox.Add(CA4.Text); textbox.Add(CA5.Text); textbox.Add(CA6.Text);
                    textboxga.Add(GA1.Text); textboxga.Add(GA2.Text); textboxga.Add(GA3.Text); textboxga.Add(GA4.Text); textboxga.Add(GA5.Text); textboxga.Add(GA6.Text);
                    textboxmg.Add(CF1.Text); textboxmg.Add(CF2.Text); textboxmg.Add(CF3.Text); textboxmg.Add(CF4.Text); textboxmg.Add(CF5.Text); textboxmg.Add(CF6.Text);
                    textboxtemp.Add(T1.Text); textboxtemp.Add(T2.Text); textboxtemp.Add(T3.Text); textboxtemp.Add(T4.Text); textboxtemp.Add(T5.Text); textboxtemp.Add(T6.Text);
                    break;
            }
            var accionddl = new List<decimal>
            {
                Convert.ToDecimal(ddlAction1.SelectedValue),
                Convert.ToDecimal(ddlAction2.SelectedValue),
                Convert.ToDecimal(ddlAction3.SelectedValue),
                Convert.ToDecimal(ddlAction4.SelectedValue),
                Convert.ToDecimal(ddlAction5.SelectedValue)
            };

            switch (listaMuestra.Count)
            {
                case 2:
                    if (textbox[0] == "" || textbox[0] != "0")
                    {

                        PH1.Enabled = accionddl[0] != 3 ? false : true;
                        T1.Enabled = accionddl[1] != 3 ? false : true;
                        ddlana1.Enabled = accionddl[2] != 3 ? false : true;
                        CF1.Enabled = accionddl[3] != 3 ? false : true;
                        GA1.Enabled = accionddl[4] != 3 ? false : true;

                        lcmc1.Enabled = true;
                        lcmc7.Enabled = true;
                    }
                    if (textbox[0] == "0")
                    {
                        PH1.Enabled = false;
                        //TextBox1.Text = "0";
                        T1.Enabled = false;
                        //TextBox1.Text = "0";
                        CF1.Enabled = false;
                        //TextBox1.Text = "0";
                        GA1.Enabled = false;
                        //TextBox1.Text = "0";
                        lcmc1.Enabled = false;
                        //TextBox1.Text = "0";
                        lcmc7.Enabled = false;
                        //TextBox1.Text = "0";
                        ddlana1.Enabled = false;

                    }
                    if (textbox[1] == "" || textbox[1] != "0")
                    {
                        PH2.Enabled = accionddl[0] != 3 ? false : true;
                        T2.Enabled = accionddl[1] != 3 ? false : true;
                        ddlana2.Enabled = accionddl[2] != 3 ? false : true;
                        CF2.Enabled = accionddl[3] != 3 ? false : true;
                        GA2.Enabled = accionddl[4] != 3 ? false : true;
                        lcmc2.Enabled = true;
                        lcmc8.Enabled = true;

                    }
                    if (textbox[1] == "0")
                    {
                        PH2.Enabled = false;
                        //TextBox1.Text = "0";
                        T2.Enabled = false;
                        //TextBox1.Text = "0";
                        CF2.Enabled = false;
                        //TextBox1.Text = "0";
                        GA2.Enabled = false;
                        //TextBox1.Text = "0";
                        lcmc2.Enabled = false;
                        //TextBox1.Text = "0";
                        lcmc8.Enabled = false;
                        //TextBox1.Text = "0";
                        ddlana2.Enabled = false;

                    }

                    break;
                case 4:
                    if (textbox[0] == "" || textbox[0] != "0")
                    {

                        PH1.Enabled = accionddl[0] != 3 ? false : true;
                        T1.Enabled = accionddl[1] != 3 ? false : true;
                        ddlana1.Enabled = accionddl[2] != 3 ? false : true;
                        CF1.Enabled = accionddl[3] != 3 ? false : true;
                        GA1.Enabled = accionddl[4] != 3 ? false : true;

                        lcmc1.Enabled = true;
                        lcmc7.Enabled = true;
                    }
                    if (textbox[0] == "0")
                    {
                        PH1.Enabled = false;
                        //TextBox1.Text = "0";
                        T1.Enabled = false;
                        //TextBox1.Text = "0";
                        CF1.Enabled = false;
                        //TextBox1.Text = "0";
                        GA1.Enabled = false;
                        //TextBox1.Text = "0";
                        lcmc1.Enabled = false;
                        //TextBox1.Text = "0";
                        lcmc7.Enabled = false;
                        //TextBox1.Text = "0";
                        ddlana1.Enabled = false;

                    }
                    if (textbox[1] == "" || textbox[1] != "0")
                    {
                        PH2.Enabled = accionddl[0] != 3 ? false : true;
                        T2.Enabled = accionddl[1] != 3 ? false : true;
                        ddlana2.Enabled = accionddl[2] != 3 ? false : true;
                        CF2.Enabled = accionddl[3] != 3 ? false : true;
                        GA2.Enabled = accionddl[4] != 3 ? false : true;
                        lcmc2.Enabled = true;
                        lcmc8.Enabled = true;

                    }
                    if (textbox[1] == "0")
                    {
                        PH2.Enabled = false;
                        //TextBox1.Text = "0";
                        T2.Enabled = false;
                        //TextBox1.Text = "0";
                        CF2.Enabled = false;
                        //TextBox1.Text = "0";
                        GA2.Enabled = false;
                        //TextBox1.Text = "0";
                        lcmc2.Enabled = false;
                        //TextBox1.Text = "0";
                        lcmc8.Enabled = false;
                        //TextBox1.Text = "0";
                        ddlana2.Enabled = false;

                    }

                    if (textbox[2] == "" || textbox[2] != "0")
                    {
                        PH3.Enabled = accionddl[0] != 3 ? false : true;
                        T3.Enabled = accionddl[1] != 3 ? false : true;
                        ddlana3.Enabled = accionddl[2] != 3 ? false : true;
                        CF3.Enabled = accionddl[3] != 3 ? false : true;
                        GA3.Enabled = accionddl[4] != 3 ? false : true;
                        lcmc3.Enabled = true;
                        lcmc9.Enabled = true;



                    }
                    if (textbox[2] == "0")
                    {
                        PH3.Enabled = false;
                        //TextBox1.Text = "0";
                        T3.Enabled = false;
                        //TextBox1.Text = "0";
                        CF3.Enabled = false;
                        //TextBox1.Text = "0";
                        GA3.Enabled = false;
                        //TextBox1.Text = "0";
                        lcmc3.Enabled = false;
                        //TextBox1.Text = "0";
                        lcmc9.Enabled = false;
                        //TextBox1.Text = "0";
                        ddlana3.Enabled = false;

                    }
                    if (textbox[3] == "" || textbox[3] != "0")
                    {
                        PH4.Enabled = accionddl[0] != 3 ? false : true;
                        T4.Enabled = accionddl[1] != 3 ? false : true;
                        ddlana4.Enabled = accionddl[2] != 3 ? false : true;
                        CF4.Enabled = accionddl[3] != 3 ? false : true;
                        GA4.Enabled = accionddl[4] != 3 ? false : true;
                        lcmc4.Enabled = true;
                        l10lcmc.Enabled = true;

                    }
                    if (textbox[3] == "0")
                    {
                        PH4.Enabled = false;
                        //TextBox1.Text = "0";
                        T4.Enabled = false;
                        //TextBox1.Text = "0";
                        CF4.Enabled = false;
                        //TextBox1.Text = "0";
                        GA4.Enabled = false;
                        //TextBox1.Text = "0";
                        lcmc4.Enabled = false;
                        //TextBox1.Text = "0";
                        l10lcmc.Enabled = false;
                        //TextBox1.Text = "0";
                        ddlana4.Enabled = false;

                    }

                    break;
                case 6:
                    if (textbox[0] == "" || textbox[0] != "0")
                    {

                        PH1.Enabled = accionddl[0] != 3 ? false : true;
                        T1.Enabled = accionddl[1] != 3 ? false : true;
                        ddlana1.Enabled = accionddl[2] != 3 ? false : true;
                        CF1.Enabled = accionddl[3] != 3 ? false : true;
                        GA1.Enabled = accionddl[4] != 3 ? false : true;

                        lcmc1.Enabled = true;
                        lcmc7.Enabled = true;
                    }
                    if (textbox[0] == "0")
                    {
                        PH1.Enabled = false;
                        //TextBox1.Text = "0";
                        T1.Enabled = false;
                        //TextBox1.Text = "0";
                        CF1.Enabled = false;
                        //TextBox1.Text = "0";
                        GA1.Enabled = false;
                        //TextBox1.Text = "0";
                        lcmc1.Enabled = false;
                        //TextBox1.Text = "0";
                        lcmc7.Enabled = false;
                        //TextBox1.Text = "0";
                        ddlana1.Enabled = false;

                    }
                    if (textbox[1] == "" || textbox[1] != "0")
                    {
                        PH2.Enabled = accionddl[0] != 3 ? false : true;
                        T2.Enabled = accionddl[1] != 3 ? false : true;
                        ddlana2.Enabled = accionddl[2] != 3 ? false : true;
                        CF2.Enabled = accionddl[3] != 3 ? false : true;
                        GA2.Enabled = accionddl[4] != 3 ? false : true;
                        lcmc2.Enabled = true;
                        lcmc8.Enabled = true;

                    }
                    if (textbox[1] == "0")
                    {
                        PH2.Enabled = false;
                        //TextBox1.Text = "0";
                        T2.Enabled = false;
                        //TextBox1.Text = "0";
                        CF2.Enabled = false;
                        //TextBox1.Text = "0";
                        GA2.Enabled = false;
                        //TextBox1.Text = "0";
                        lcmc2.Enabled = false;
                        //TextBox1.Text = "0";
                        lcmc8.Enabled = false;
                        //TextBox1.Text = "0";
                        ddlana2.Enabled = false;

                    }

                    if (textbox[2] == "" || textbox[2] != "0")
                    {
                        PH3.Enabled = accionddl[0] != 3 ? false : true;
                        T3.Enabled = accionddl[1] != 3 ? false : true;
                        ddlana3.Enabled = accionddl[2] != 3 ? false : true;
                        CF3.Enabled = accionddl[3] != 3 ? false : true;
                        GA3.Enabled = accionddl[4] != 3 ? false : true;
                        lcmc3.Enabled = true;
                        lcmc9.Enabled = true;
                    }
                    if (textbox[2] == "0")
                    {
                        PH3.Enabled = false;
                        //TextBox1.Text = "0";
                        T3.Enabled = false;
                        //TextBox1.Text = "0";
                        CF3.Enabled = false;
                        //TextBox1.Text = "0";
                        GA3.Enabled = false;
                        //TextBox1.Text = "0";
                        lcmc3.Enabled = false;
                        //TextBox1.Text = "0";
                        lcmc9.Enabled = false;
                        //TextBox1.Text = "0";
                        ddlana3.Enabled = false;

                    }
                    if (textbox[3] == "" || textbox[3] != "0")
                    {
                        PH4.Enabled = accionddl[0] != 3 ? false : true;
                        T4.Enabled = accionddl[1] != 3 ? false : true;
                        ddlana4.Enabled = accionddl[2] != 3 ? false : true;
                        CF4.Enabled = accionddl[3] != 3 ? false : true;
                        GA4.Enabled = accionddl[4] != 3 ? false : true;
                        lcmc4.Enabled = true;
                        l10lcmc.Enabled = true;

                    }
                    if (textbox[3] == "0")
                    {
                        PH4.Enabled = false;
                        //TextBox1.Text = "0";
                        T4.Enabled = false;
                        //TextBox1.Text = "0";
                        CF4.Enabled = false;
                        //TextBox1.Text = "0";
                        GA4.Enabled = false;
                        //TextBox1.Text = "0";
                        lcmc4.Enabled = false;
                        //TextBox1.Text = "0";
                        l10lcmc.Enabled = false;
                        //TextBox1.Text = "0";
                        ddlana4.Enabled = false;

                    }
                    if (textbox[4] == "" || textbox[4] != "0")
                    {
                        PH5.Enabled = accionddl[0] != 3 ? false : true;
                        T5.Enabled = accionddl[1] != 3 ? false : true;
                        ddlana5.Enabled = accionddl[2] != 3 ? false : true;
                        CF5.Enabled = accionddl[3] != 3 ? false : true;
                        GA5.Enabled = accionddl[4] != 3 ? false : true;
                        lcmc5.Enabled = true;
                        l11lcmc.Enabled = true;


                    }
                    if (textbox[4] == "0")
                    {
                        PH5.Enabled = false;
                        //TextBox1.Text = "0";
                        T5.Enabled = false;
                        //TextBox1.Text = "0";
                        CF5.Enabled = false;
                        //TextBox1.Text = "0";
                        GA5.Enabled = false;
                        //TextBox1.Text = "0";
                        l11lcmc.Enabled = false;
                        //TextBox1.Text = "0";
                        lcmc5.Enabled = false;
                        //TextBox1.Text = "0";
                        ddlana5.Enabled = false;

                    }
                    if (textbox[5] == "" || textbox[5] != "0")
                    {
                        PH6.Enabled = accionddl[0] != 3 ? false : true;
                        T6.Enabled = accionddl[1] != 3 ? false : true;
                        ddlana6.Enabled = accionddl[2] != 3 ? false : true;
                        CF6.Enabled = accionddl[3] != 3 ? false : true;
                        GA6.Enabled = accionddl[4] != 3 ? false : true;
                        lcmc6.Enabled = true;
                        l12lcmc.Enabled = true;


                    }
                    if (textbox[5] == "0")
                    {
                        PH6.Enabled = false;
                        //TextBox1.Text = "0";
                        T6.Enabled = false;
                        //TextBox1.Text = "0";
                        CF6.Enabled = false;
                        //TextBox1.Text = "0";
                        GA6.Enabled = false;
                        //TextBox1.Text = "0";
                        lcmc6.Enabled = false;
                        //TextBox1.Text = "0";
                        l12lcmc.Enabled = false;
                        //TextBox1.Text = "0";
                        ddlana6.Enabled = false;

                    }
                    break;
            }


            var caudal = textbox;

            var countCaudal = new List<decimal>();
            decimal sumCaudal = 0;
            foreach (var t in caudal)
            {
                if (t != "" && t != "0")
                {
                    countCaudal.Add(1);
                    sumCaudal = Convert.ToDecimal(sumCaudal) + Convert.ToDecimal(t);
                    var resCaudal = Convert.ToDecimal(sumCaudal) / Convert.ToDecimal(countCaudal.Count);

                    Label4.Text = (substring2(resCaudal)).ToString(CultureInfo.InvariantCulture);

                }
            }
            var qi = new List<decimal>();
            decimal resqisum = 0;
            switch (listaMuestra.Count)
            {
                case 2:
                    if (caudal[0] != "")
                        qi.Add(Convert.ToDecimal(caudal[0]) / Convert.ToDecimal(substring6(sumCaudal)));
                    else
                        qi.Add(0);
                    if (caudal[1] != "")
                        qi.Add(Convert.ToDecimal(caudal[1]) / Convert.ToDecimal(substring6(sumCaudal)));
                    else
                        qi.Add(0);
                    resqisum = substring4(Convert.ToDecimal(substring6(qi[0])) + Convert.ToDecimal(substring6(qi[1])));
                    Label1.Text = (substring4(qi[0])).ToString(CultureInfo.InvariantCulture);
                    Label10.Text = (substring4(qi[1])).ToString(CultureInfo.InvariantCulture);
                    break;
                case 4:
                if (caudal[0] != "")
                        qi.Add(Convert.ToDecimal(caudal[0]) / Convert.ToDecimal(substring6(sumCaudal)));
                    else
                        qi.Add(0);
                    if (caudal[1] != "")
                        qi.Add(Convert.ToDecimal(caudal[1]) / Convert.ToDecimal(substring6(sumCaudal)));
                    else
                        qi.Add(0);
                    if (caudal[2] != "")
                        qi.Add(Convert.ToDecimal(caudal[2]) / Convert.ToDecimal(substring6(sumCaudal)));
                    else
                        qi.Add(0);
                    if (caudal[3] != "")
                        qi.Add(Convert.ToDecimal(caudal[3]) / Convert.ToDecimal(substring6(sumCaudal)));
                    else
                        qi.Add(0);
                    resqisum = substring4(Convert.ToDecimal(substring6(qi[0])) + Convert.ToDecimal(substring6(qi[1])) + Convert.ToDecimal(substring6(qi[2])) + Convert.ToDecimal(substring6(qi[3])));
                    Label1.Text = (substring4(qi[0])).ToString(CultureInfo.InvariantCulture);
                    Label10.Text = (substring4(qi[1])).ToString(CultureInfo.InvariantCulture);
                    Label13.Text = (substring4(qi[2]).ToString(CultureInfo.InvariantCulture));
                    Label16.Text = (substring4(qi[3])).ToString(CultureInfo.InvariantCulture);

                    break;
                case 6:
                    if (caudal[0] != "")
                        qi.Add(Convert.ToDecimal(caudal[0]) / Convert.ToDecimal(substring6(sumCaudal)));
                    else
                        qi.Add(0);
                    if (caudal[1] != "")
                        qi.Add(Convert.ToDecimal(caudal[1]) / Convert.ToDecimal(substring6(sumCaudal)));
                    else
                        qi.Add(0);
                    if (caudal[2] != "")
                        qi.Add(Convert.ToDecimal(caudal[2]) / Convert.ToDecimal(substring6(sumCaudal)));
                    else
                        qi.Add(0);
                    if (caudal[3] != "")
                        qi.Add(Convert.ToDecimal(caudal[3]) / Convert.ToDecimal(substring6(sumCaudal)));
                    else
                        qi.Add(0);
                    if (caudal[4] != "")
                        qi.Add(Convert.ToDecimal(caudal[4]) / Convert.ToDecimal(substring6(sumCaudal)));
                    else
                        qi.Add(0);
                    if (caudal[5] != "")
                        qi.Add(Convert.ToDecimal(caudal[5]) / Convert.ToDecimal(substring6(sumCaudal)));
                    else
                        qi.Add(0);
                    resqisum = substring4(Convert.ToDecimal(substring6(qi[0])) + Convert.ToDecimal(substring6(qi[1])) + Convert.ToDecimal(substring6(qi[2])) + Convert.ToDecimal(substring6(qi[3])) + Convert.ToDecimal(substring6(qi[4])) + Convert.ToDecimal(substring6(qi[5])));
                    Label1.Text = (substring4(qi[0])).ToString(CultureInfo.InvariantCulture);
                    Label10.Text = (substring4(qi[1])).ToString(CultureInfo.InvariantCulture);
                    Label13.Text = (substring4(qi[2]).ToString(CultureInfo.InvariantCulture));
                    Label16.Text = (substring4(qi[3])).ToString(CultureInfo.InvariantCulture);
                    Label19.Text = (substring4(qi[4])).ToString(CultureInfo.InvariantCulture);
                    Label22.Text = (substring4(qi[5])).ToString(CultureInfo.InvariantCulture);
                    break;
            }
            var totalqq = resqisum;
            Label6.Text = totalqq.ToString(CultureInfo.InvariantCulture);

            var tb9 = "0";
            if (TextBox9.Text != "")
            {
                tb9 = TextBox9.Text;
            }
            var vm = new List<decimal>();
            decimal resvmsum = 0;
            switch (listaMuestra.Count)
            {
                case 2:
                    vm.Add(Convert.ToDecimal(tb9) * Convert.ToDecimal(substring6(qi[0])));
                    vm.Add(Convert.ToDecimal(tb9) * Convert.ToDecimal(substring6(qi[1])));
                    resvmsum =
                        substring4(Convert.ToDecimal(substring6(vm[0])) + Convert.ToDecimal(substring6(vm[1])));
                    Label2.Text = (substring2(vm[0])).ToString(CultureInfo.InvariantCulture);
                    Label11.Text = (substring2(vm[1])).ToString(CultureInfo.InvariantCulture);

                    break;
                case 4:
                    vm.Add(Convert.ToDecimal(tb9) * Convert.ToDecimal(substring6(qi[0])));
                    vm.Add(Convert.ToDecimal(tb9) * Convert.ToDecimal(substring6(qi[1])));
                    vm.Add(Convert.ToDecimal(tb9) * Convert.ToDecimal(substring6(qi[2])));
                    vm.Add(Convert.ToDecimal(tb9) * Convert.ToDecimal(substring6(qi[3])));
                    resvmsum =

                            substring4(Convert.ToDecimal(substring6(vm[0])) + Convert.ToDecimal(substring6(vm[1])) +
                                       Convert.ToDecimal(substring6(vm[2])) + Convert.ToDecimal(substring6(vm[3])));
                    Label2.Text = (substring2(vm[0])).ToString(CultureInfo.InvariantCulture);
                    Label11.Text = (substring2(vm[1])).ToString(CultureInfo.InvariantCulture);
                    Label14.Text = (substring2(vm[2])).ToString(CultureInfo.InvariantCulture);
                    Label17.Text = (substring2(vm[3])).ToString(CultureInfo.InvariantCulture);

                    break;
                case 6:
                    vm.Add(Convert.ToDecimal(tb9) * Convert.ToDecimal(substring6(qi[0])));
                    vm.Add(Convert.ToDecimal(tb9) * Convert.ToDecimal(substring6(qi[1])));
                    vm.Add(Convert.ToDecimal(tb9) * Convert.ToDecimal(substring6(qi[2])));
                    vm.Add(Convert.ToDecimal(tb9) * Convert.ToDecimal(substring6(qi[3])));
                    vm.Add(Convert.ToDecimal(tb9) * Convert.ToDecimal(substring6(qi[4])));
                    vm.Add(Convert.ToDecimal(tb9) * Convert.ToDecimal(substring6(qi[5])));
                    resvmsum =

                            substring4(Convert.ToDecimal(substring6(vm[0])) + Convert.ToDecimal(substring6(vm[1])) +
                                       Convert.ToDecimal(substring6(vm[2])) + Convert.ToDecimal(substring6(vm[3])) +
                                       Convert.ToDecimal(substring6(vm[4])) + Convert.ToDecimal(substring6(vm[5])));
                    Label2.Text = (substring2(vm[0])).ToString(CultureInfo.InvariantCulture);
                    Label11.Text = (substring2(vm[1])).ToString(CultureInfo.InvariantCulture);
                    Label14.Text = (substring2(vm[2])).ToString(CultureInfo.InvariantCulture);
                    Label17.Text = (substring2(vm[3])).ToString(CultureInfo.InvariantCulture);
                    Label20.Text = (substring2(vm[4])).ToString(CultureInfo.InvariantCulture);
                    Label23.Text = (substring2(vm[5])).ToString(CultureInfo.InvariantCulture);


                    break;


            }

            var totalVm = resvmsum;
            Label7.Text = (substring2(totalVm)).ToString(CultureInfo.InvariantCulture);



            var ga = textboxga;
            var gares = new List<decimal>();
            decimal resgasum = 0;
            switch (listaMuestra.Count)
            {
                case 2:

                    gares.Add(Convert.ToDecimal(ga[0]) * Convert.ToDecimal(substring6(qi[0])));
                    gares.Add(Convert.ToDecimal(ga[1]) * Convert.ToDecimal(substring6(qi[1])));

                    Label3.Text = (substring4(gares[0])).ToString(CultureInfo.InvariantCulture);
                    Label12.Text = (substring4(gares[1])).ToString(CultureInfo.InvariantCulture);
                    resgasum = substring4(Convert.ToDecimal(substring6(gares[0])) + Convert.ToDecimal(substring6(gares[1])));

                    break;
                case 4:
                    gares.Add(Convert.ToDecimal(ga[0]) * Convert.ToDecimal(substring6(qi[0])));
                    gares.Add(Convert.ToDecimal(ga[1]) * Convert.ToDecimal(substring6(qi[1])));
                    gares.Add(Convert.ToDecimal(ga[2]) * Convert.ToDecimal(substring6(qi[2])));
                    gares.Add(Convert.ToDecimal(ga[3]) * Convert.ToDecimal(substring6(qi[3])));
                    Label3.Text = (substring4(gares[0])).ToString(CultureInfo.InvariantCulture);
                    Label12.Text = (substring4(gares[1])).ToString(CultureInfo.InvariantCulture);
                    Label15.Text = (substring4(gares[2])).ToString(CultureInfo.InvariantCulture);
                    Label18.Text = (substring4(gares[3])).ToString(CultureInfo.InvariantCulture);
                    resgasum = substring4(Convert.ToDecimal(substring6(gares[0])) + Convert.ToDecimal(substring6(gares[1])) + Convert.ToDecimal(substring6(gares[2])) + Convert.ToDecimal(substring6(gares[3])));

                    break;
                case 6:
                    gares.Add(Convert.ToDecimal(ga[0]) * Convert.ToDecimal(substring6(qi[0])));
                    gares.Add(Convert.ToDecimal(ga[1]) * Convert.ToDecimal(substring6(qi[1])));
                    gares.Add(Convert.ToDecimal(ga[2]) * Convert.ToDecimal(substring6(qi[2])));
                    gares.Add(Convert.ToDecimal(ga[3]) * Convert.ToDecimal(substring6(qi[3])));
                    gares.Add(Convert.ToDecimal(ga[4]) * Convert.ToDecimal(substring6(qi[4])));
                    gares.Add(Convert.ToDecimal(ga[5]) * Convert.ToDecimal(substring6(qi[5])));

                    Label3.Text = (substring4(gares[0])).ToString(CultureInfo.InvariantCulture);
                    Label12.Text = (substring4(gares[1])).ToString(CultureInfo.InvariantCulture);
                    Label15.Text = (substring4(gares[2])).ToString(CultureInfo.InvariantCulture);
                    Label18.Text = (substring4(gares[3])).ToString(CultureInfo.InvariantCulture);
                    Label21.Text = (substring4(gares[4])).ToString(CultureInfo.InvariantCulture);
                    Label24.Text = (substring4(gares[5])).ToString(CultureInfo.InvariantCulture);
                    resgasum = substring4(Convert.ToDecimal(substring6(gares[0])) + Convert.ToDecimal(substring6(gares[1])) + Convert.ToDecimal(substring6(gares[2])) + Convert.ToDecimal(substring6(gares[3])) + Convert.ToDecimal(substring6(gares[4])) + Convert.ToDecimal(substring6(gares[5])));


                    break;

            }
            var totalppfc = resgasum;
            Label9.Text = (totalppfc).ToString();



            var temperatura = textboxtemp;
            var counttemp = new List<decimal>();
            decimal sumtemp = 0;
            decimal restemp = 0;
            foreach (var ti in temperatura)
            {
                if (ti != "" && ti != "0")
                {
                    counttemp.Add(1);
                    sumtemp = Convert.ToDecimal(sumtemp) + Convert.ToDecimal(ti);
                    restemp = (Convert.ToDecimal(sumtemp) / Convert.ToDecimal(counttemp.Count));
                    Label5.Text = (substring4(restemp)).ToString(CultureInfo.InvariantCulture);

                }
            }
            var mg = textboxmg;
            var countmg = new List<decimal>();
            double mulmg = 1;
            double resmg = 0;
            foreach (var ts in mg)
            {
                if (ts != "" && ts != "0")
                {
                    countmg.Add(Convert.ToDecimal(ts));
                    mulmg = mulmg * Convert.ToDouble(ts);
                }
            }
            switch (countmg.Count)
            {
                case 1:
                    resmg = Math.Pow(Convert.ToDouble(countmg[0]), 1);
                    Label8.Text = (substring0(Convert.ToDecimal(resmg))).ToString(CultureInfo.InvariantCulture);

                    break;
                case 2:
                    resmg = Convert.ToDouble(countmg[0] * countmg[1]);
                    resmg = Math.Pow((double)substring6(Convert.ToDecimal(resmg)), 0.5);
                    Label8.Text = (substring0(Convert.ToDecimal(resmg))).ToString(CultureInfo.InvariantCulture);
                    break;
                case 3:
                    resmg = Convert.ToDouble(countmg[0] * countmg[1] * countmg[2]);
                    resmg = Math.Pow((double)substring6(Convert.ToDecimal(resmg)), 0.33333333333333333333333333333333);
                    Label8.Text = (substring0(Convert.ToDecimal(resmg))).ToString(CultureInfo.InvariantCulture);

                    break;
                case 4:
                   // resmg = Convert.ToDouble(countmg[0] * countmg[1] * countmg[2] * countmg[3]);
                    resmg = Math.Pow(mulmg, 0.25);
                    Label8.Text = (substring0(Convert.ToDecimal(resmg))).ToString(CultureInfo.InvariantCulture);

                    break;
                case 5:
                   // resmg = Convert.ToDouble(countmg[0] * countmg[1] * countmg[2] * countmg[3] * countmg[4]);
                    resmg = Math.Pow(mulmg, 0.2);
                    Label8.Text = (substring0(Convert.ToDecimal(resmg))).ToString(CultureInfo.InvariantCulture);
                    break;
                case 6:
                   // resmg = Convert.ToDouble(countmg[0] * countmg[1] * countmg[2] * countmg[3] * countmg[4] * countmg[5]);
                    resmg = Math.Pow(mulmg, 0.16666666666666666666666666666667);
                    Label8.Text = (substring0(Convert.ToDecimal(resmg))).ToString(CultureInfo.InvariantCulture);

                    break;
            }

        }

        private decimal substring0(decimal numero)
        {
            if (numero != null)
            {
                var num = numero.ToString();
                string[] split = num.Split(new[] { '.' });
                if (split.Count() == 2)
                {

                    numero = Convert.ToDecimal(split[0]);
                }
            }
            else
            {
                numero = 0;
            }
            return numero;
        }

        private decimal substring2(decimal numero)
        {
            if (numero != null)
            {
                var num = numero.ToString();
                string[] split = num.Split(new[] { '.' });
                if (split.Count() == 2)
                {
                    var a = split[1].ToString(CultureInfo.InvariantCulture);
                    var b = a.Length > 2 ? a.Substring(0, 2) : a;
                    numero = Convert.ToDecimal(split[0] + "." + b);
                }
            }
            else
            {
                numero = 0;
            }
            return numero;
        }

        private decimal substring4(decimal numero)
        {
            if (numero != null)
            {
                var num = numero.ToString();
                string[] split = num.Split(new[] { '.' });
                if (split.Count() == 2)
                {
                    var a = split[1].ToString(CultureInfo.InvariantCulture);
                    var b = a.Length > 4 ? a.Substring(0, 4) : a;
                    numero = Convert.ToDecimal(split[0] + "." + b);
                }
            }
            else
            {
                numero = 0;
            }
            return numero;
        }

        private decimal substring6(decimal numero)
        {
            if (numero != null)
            {
                var num = numero.ToString();
                string[] split = num.Split(new[] { '.' });
                if (split.Count() == 2)
                {
                    var a = split[1].ToString(CultureInfo.InvariantCulture);
                    var b = a.Length > 6 ? a.Substring(0, 6) : a;

                    numero = Convert.ToDecimal(split[0] + "." + b);
                }
            }
            else
            {
                numero = 0;
            }
            return numero;
        }
        private bool ValidarFecha(string fechaXValidarCadena)
        {
            var validacion = true;
            var fechaActual = DateTime.Now;
            DateTime fechaXValidar;
            if (DateTime.TryParse(fechaXValidarCadena, out fechaXValidar))
            {
                if (fechaXValidar.Date <= fechaActual.Date && fechaXValidar.Year >= 2014)
                {
                    validacion = false;
                }
            }
            return validacion;
        }
        protected void lnkbtnMuestraCompuesta_Click(object sender, EventArgs e)
        {
            var avanzaCompuesta = false;
            decimal compuestaCaudal = 0;
            //Metodo para generar la muestra compuesta
            var muestraBl = new Muestra_BL();
            var ncBl = new Norma_BL();
            var muestraArgs = new MuestraCapturaArgs();
            var m = new Muestra();

            var muestravalorBl = new MuestraValor_BL();

            if (Analisis != null)
            {
                //Se realiza la validación para no permitir que se genere la muestra compuesta hasta que esten completas las muestars simples
                var listaMuestra = muestraBl.Lista_Muestra(Analisis.AnalisisID);

                if (listaMuestra.Where(p => p.Caudal != 0).Count() > 0)
                {
                    compuestaCaudal = listaMuestra.Where(p => p.Caudal != 0).Average(x => x.Caudal);
                }
                //Validación utilizada para comprobar que la lista tiene registros
                if (listaMuestra.Count > 0)
                {
                    foreach (var muestra in listaMuestra)
                    {
                        var listaValores = muestravalorBl.Lista_MuestraValor(muestra.MuestraID);
                        var listaValoresConfig = ncBl.Obtener((int)muestra.Analisi.OrdenTrabajo.NormaID);

                        //Validación utilizada para comprobar que existen registros en muestra valor
                        if (listaValores.Count > 0 && listaValoresConfig.NormaConfiguracions.Where(p => p.NivelID == enumNivel.Simple.ConvertToInt()).Count() > 0)
                        {
                            foreach (var val in listaValores)
                            {
                                if (val.GeneradoSP == false)
                                {
                                    avanzaCompuesta = true;
                                }
                                else
                                {
                                    avanzaCompuesta = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (!Analisis.OrdenTrabajo.Norma.RequiereCaudal && muestra.Caudal == -1)
                            {
                                avanzaCompuesta = true;
                            }
                            else
                            {
                                if (muestra.Cancelado)
                                {
                                    avanzaCompuesta = true;
                                }
                                else
                                {
                                    avanzaCompuesta = listaValoresConfig.NormaConfiguracions.Where(p => p.NivelID == enumNivel.Simple.ConvertToInt()).Count() == 0;
                                    break;
                                }
                            }

                        }
                    }
                }

                //TODO: Realizar una validacion para que verifique que los caudales son diferentes de cero todos.
                if (avanzaCompuesta)
                {
                    var suma = 0;
                    foreach (var verifCaudal in listaMuestra)
                    {
                        if (verifCaudal.Caudal <= 0)
                        {
                            suma += 1;
                        }
                    }
                    if (suma == listaMuestra.Count)
                    {
                        avanzaCompuesta = false;
                    }
                }
                //TODO: Realizar una validacion para que verifique que los caudales son diferentes de cero todos.


                //SE REALIZA UN CICLO CON EL NUMERO DE MUESTRAS CONFIGURADO EN LA ORDEN DE TRABAJO
                if (avanzaCompuesta)
                {
                    //INICIO Funcionalidad para modificar los parametros, no analizado y exento
                    var listaMuestraValor = muestravalorBl.Lista_MuestraValor_Analisis(this.Analisis.AnalisisID);

                    foreach (var val in listaMuestraValor)
                    {
                        var listaParametroNoAnalizado = listaMuestraValor.Where(x => x.ParametroID == val.ParametroID && x.NoAnalizado == true);
                        if (listaParametroNoAnalizado.Count() > 0)
                        {
                            var listaACtualizarNoAnalizado = listaMuestraValor.Where(q => q.ParametroID == val.ParametroID).ToList();

                            foreach (var ma in listaACtualizarNoAnalizado)
                            {
                                var muestra = muestravalorBl.Actualiza_NoAnalizado(ma.MuestraValorID, true);
                            }
                        }

                        var listaParametroNoExento = listaMuestraValor.Where(x => x.ParametroID == val.ParametroID && x.Exento == true);
                        if (listaParametroNoExento.Count() > 0)
                        {
                            var listaACtualizarExento = listaMuestraValor.Where(q => q.ParametroID == val.ParametroID).ToList();

                            foreach (var exe in listaACtualizarExento)
                            {
                                var muestra = muestravalorBl.Actualiza_Exento(exe.MuestraValorID, true);
                            }
                        }


                    }
                    //FIN Funcionalidad para modificar los parametros, no analizado y exento

                    var unicamuestra = muestraBl.Lista_Muestra(this.Analisis.AnalisisID).FirstOrDefault();

                    muestraArgs.AnalisisID = Analisis.AnalisisID;
                    muestraArgs.DireccionID = unicamuestra.DireccionID;
                    muestraArgs.Numero = unicamuestra.Numero;
                    muestraArgs.Descripcion = "";
                    muestraArgs.Identificacion = "";
                    muestraArgs.Caudal = compuestaCaudal;
                    muestraArgs.Latitud = unicamuestra.Latitud;
                    muestraArgs.Longitud = unicamuestra.Longitud;
                    muestraArgs.Datum = unicamuestra.Datum;
                    muestraArgs.Fecha = null;
                    muestraArgs.EsCompuesta = true;
                    muestraArgs.FechaRecepcion = null;
                    muestraArgs.Cancelado = false;
                    muestraArgs.Observaciones = "";
                    muestraArgs.NivelID = enumNivel.Compuesta.ConvertToInt();

                    m = muestraBl.Guarda(muestraArgs);
                    if (m != null)
                        CargaInformacion((int)Session["analisiid"]);



                    lnkbtnMuestraCompuesta.Visible = false;

                    Guardar(this, m != null ? muestraArgs : null);
                }
                else
                {
                    //Mandamos evento indicando que las muestras simples estan incompletas
                    muestraArgs.Descripcion = "incompleta";
                    Guardar(this, muestraArgs);
                    Calcular();
                    return;
                   
                   // MsIncompleta(this, false);
                }
                Response.Redirect(Request.Url.AbsoluteUri);
            }
        }
        public class jsList
        {
            public class Row
            {
                public int id { get; set; }
                public List<string> cell { get; set; }

                public Row()
                {
                    cell = new List<string>();
                }
            }

            public int page { get; set; }
            public int total { get; set; }
            public int records { get; set; }
            public List<Row> rows { get; set; }

            public jsList()
            {
                rows = new List<Row>();
            }
        }

        protected void Gtemp_Click(object sender, EventArgs e)
        {
            var muestravalorBl = new MuestraValor_BL();
            var muestraArgs = new MuestraCapturaArgs();
            var mpBl = new ParametroMetodo_BL();
            var muestraBl = new Muestra_BL();
            var list = new List<MuestraValorArgBase_BL>();

            var listaMuestra = _muestraBl.Lista_Muestra((int)Session["analisiid"]).Where(x => x.NivelID == 1).ToList();
            #region guardar
            var conts = 1;
            foreach (var t in listaMuestra)
            {
                if (OrdenTrabajo.NormaID != null)
                    list = t.NivelID == (decimal)enumNivel.Simple ? muestravalorBl.GuardaObten((int)OrdenTrabajo.NormaID, (int)t.MuestraID, t.NivelID, false) : muestravalorBl.GuardaObtenCompuesta((int)OrdenTrabajo.NormaID, (int)t.MuestraID);

                switch (conts)
                {
                    case 1:
                        if (ValidarFecha(TextBox1.Text) || ValidarFecha(TextBox10.Text))
                            muestraArgs.Descripcion = "ErrorFecha";
                        if (muestraArgs.Descripcion == "ErrorFecha")
                        {
                            Guardar(this, muestraArgs);
                            return;
                        }
                        if (TextBox9.Text == "" || TextBox9.Text == "0" || TextBox9.Text == "0.00")
                        {
                            muestraArgs.Descripcion = "ErrorMuestra";
                            Guardar(this, muestraArgs);
                            return;
                        }

                        var a1 = RemoveSpecialCharacters(TextBox2.Text);

                        if (a1 == "")
                        {
                            muestraArgs.Descripcion = "ErrorHoras";
                            if (muestraArgs.Descripcion == "ErrorHoras")
                            {
                                Guardar(this, muestraArgs);
                                return;
                            }
                        }
                        if (CA1.Text == "")
                        {
                            muestraArgs.Descripcion = "Errorcaudal";
                            Guardar(this, muestraArgs);
                            return;
                        }
                        foreach (var it in list.Select(t1 => list.FirstOrDefault(p => p.MuestraValorID == t1.MuestraValorID)))
                        {
                            switch (it.ParametroID)
                            {
                                case 862:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(PH1.Text);
                                    it.LaboratorioID = int.Parse(ddlLab1.SelectedValue);
                                    var mp0 = mpBl.Obten(862, int.Parse(ddlMet1.SelectedValue));
                                    it.ParametroMetodoID = mp0.ParametroMetodoID;
                                    break;
                                case 118:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(T1.Text);
                                    it.LaboratorioID = int.Parse(ddlLab2.SelectedValue);
                                    var mp1 = mpBl.Obten(118, int.Parse(ddlMet2.SelectedValue));
                                    it.ParametroMetodoID = mp1.ParametroMetodoID;
                                    break;
                                case 225:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(CF1.Text);
                                    it.LaboratorioID = int.Parse(ddlLab4.SelectedValue);
                                    var mp2 = mpBl.Obten(225, int.Parse(ddlMet4.SelectedValue));
                                    it.ParametroMetodoID = mp2.ParametroMetodoID;
                                    break;
                                case 79:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(GA1.Text);
                                    it.LaboratorioID = int.Parse(ddlLab5.SelectedValue);
                                    var mp3 = mpBl.Obten(79, int.Parse(ddlMet5.SelectedValue));
                                    it.ParametroMetodoID = mp3.ParametroMetodoID;
                                    break;
                                case 89:
                                    bool tf;
                                    switch (ddlana1.SelectedValue)
                                    {
                                        case "true":
                                            tf = true;
                                            break;
                                        default:
                                            tf = false;
                                            break;
                                    }
                                    it.GeneradoSP = false;
                                    it.ValorValidacion = tf;
                                    it.LaboratorioID = int.Parse(ddlLab3.SelectedValue);
                                    var mp4 = mpBl.Obten(89, int.Parse(ddlMet3.SelectedValue));
                                    it.ParametroMetodoID = mp4.ParametroMetodoID;
                                    break;

                            }
                            muestravalorBl.Guardar(it);
                        }

                        muestraArgs.Descripcion = "N/A";
                        muestraArgs.Identificacion = "N/A";
                        muestraArgs.Caudal = decimal.Parse(CA1.Text);
                        muestraArgs.Observaciones = TextBox8.Text;
                        if (ValidarFecha(TextBox1.Text) || ValidarFecha(TextBox10.Text))
                            muestraArgs.Descripcion = "ErrorFecha";

                        muestraArgs.Fecha = DateTime.ParseExact(TextBox1.Text + " " + TextBox2.Text, "dd/MM/yyyy H:mm", CultureInfo.InvariantCulture);
                        muestraArgs.FechaRecepcion = DateTime.ParseExact(TextBox10.Text + " " + TextBox11.Text, "dd/MM/yyyy H:mm", CultureInfo.InvariantCulture);

                        break;
                    case 2:

                        var a = RemoveSpecialCharacters(TextBox2.Text);
                        var b = RemoveSpecialCharacters(TextBox13.Text);
                        if (a == "" || b == "")
                        {
                            muestraArgs.Descripcion = "ErrorHoras";
                            if (muestraArgs.Descripcion == "ErrorHoras")
                            {
                                Guardar(this, muestraArgs);
                                return;
                            }
                        }
                        if (Convert.ToDecimal(a) > Convert.ToDecimal(b))
                        {
                            muestraArgs.Descripcion = "ErrorHoras";
                            if (muestraArgs.Descripcion == "ErrorHoras")
                            {
                                Guardar(this, muestraArgs);
                                return;
                            }
                        }

                        if (ValidarFecha(TextBox12.Text) || ValidarFecha(TextBox10.Text))
                            muestraArgs.Descripcion = "ErrorFecha";
                        if (muestraArgs.Descripcion == "ErrorFecha")
                        {
                            Guardar(this, muestraArgs);
                            return;
                        }
                        if (CA2.Text == "")
                        {
                            muestraArgs.Descripcion = "Errorcaudal";
                            Guardar(this, muestraArgs);
                            return;
                        }
                        foreach (var it in list.Select(t1 => list.FirstOrDefault(p => p.MuestraValorID == t1.MuestraValorID)))
                        {
                            switch (it.ParametroID)
                            {
                                case 862:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(PH2.Text);
                                    it.LaboratorioID = int.Parse(ddlLab1.SelectedValue);
                                    var mp0 = mpBl.Obten(862, int.Parse(ddlMet1.SelectedValue));
                                    it.ParametroMetodoID = mp0.ParametroMetodoID;
                                    break;
                                case 118:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(T2.Text);
                                    it.LaboratorioID = int.Parse(ddlLab2.SelectedValue);
                                    var mp1 = mpBl.Obten(118, int.Parse(ddlMet2.SelectedValue));
                                    it.ParametroMetodoID = mp1.ParametroMetodoID;
                                    break;
                                case 225:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(CF2.Text);
                                    it.LaboratorioID = int.Parse(ddlLab4.SelectedValue);
                                    var mp2 = mpBl.Obten(225, int.Parse(ddlMet4.SelectedValue));
                                    it.ParametroMetodoID = mp2.ParametroMetodoID;
                                    break;
                                case 79:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(GA2.Text);
                                    it.LaboratorioID = int.Parse(ddlLab5.SelectedValue);
                                    var mp3 = mpBl.Obten(79, int.Parse(ddlMet5.SelectedValue));
                                    it.ParametroMetodoID = mp3.ParametroMetodoID;
                                    break;
                                case 89:
                                    bool tf;

                                    switch (ddlana2.SelectedValue)
                                    {
                                        case "true":
                                            tf = true;
                                            break;
                                        default:
                                            tf = false;
                                            break;
                                    }
                                    it.GeneradoSP = false;
                                    it.ValorValidacion = tf;
                                    it.LaboratorioID = int.Parse(ddlLab3.SelectedValue);
                                    var mp4 = mpBl.Obten(89, int.Parse(ddlMet3.SelectedValue));
                                    it.ParametroMetodoID = mp4.ParametroMetodoID;
                                    break;

                            }
                            muestravalorBl.Guardar(it);
                        }
                        muestraArgs.Descripcion = "N/A";
                        muestraArgs.Identificacion = "N/A";
                        muestraArgs.Caudal = decimal.Parse(CA2.Text);
                        muestraArgs.Observaciones = TextBox20.Text;
                        if (ValidarFecha(TextBox12.Text) || ValidarFecha(TextBox10.Text))
                            muestraArgs.Descripcion = "ErrorFecha";

                        muestraArgs.Fecha = DateTime.ParseExact(TextBox12.Text + " " + TextBox13.Text, "dd/MM/yyyy H:mm", CultureInfo.InvariantCulture);
                        muestraArgs.FechaRecepcion = DateTime.ParseExact(TextBox10.Text + " " + TextBox11.Text, "dd/MM/yyyy H:mm", CultureInfo.InvariantCulture);
                        break;
                    case 3:
                        if (ValidarFecha(TextBox17.Text) || ValidarFecha(TextBox10.Text))
                            muestraArgs.Descripcion = "ErrorFecha";
                        if (muestraArgs.Descripcion == "ErrorFecha")
                        {
                            Guardar(this, muestraArgs);
                            return;
                        }
                        var a3 = RemoveSpecialCharacters(TextBox2.Text);
                        var b3 = RemoveSpecialCharacters(TextBox13.Text);
                        var c3 = RemoveSpecialCharacters(TextBox21.Text);
                        if (a3 == "" || b3 == "" || c3 == "")
                        {
                            muestraArgs.Descripcion = "ErrorHoras";
                            if (muestraArgs.Descripcion == "ErrorHoras")
                            {
                                Guardar(this, muestraArgs);
                                return;
                            }
                        }
                        if ((Convert.ToDecimal(a3) > Convert.ToDecimal(b3)) || (Convert.ToDecimal(b3) > Convert.ToDecimal(c3)))
                        {
                            muestraArgs.Descripcion = "ErrorHoras";
                            if (muestraArgs.Descripcion == "ErrorHoras")
                            {
                                Guardar(this, muestraArgs);
                                return;
                            }
                        }
                        if (CA3.Text == "")
                        {
                            muestraArgs.Descripcion = "Errorcaudal";
                            Guardar(this, muestraArgs);
                            return;
                        }
                        foreach (var it in list.Select(t1 => list.FirstOrDefault(p => p.MuestraValorID == t1.MuestraValorID)))
                        {
                            switch (it.ParametroID)
                            {
                                case 862:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(PH3.Text);
                                    it.LaboratorioID = int.Parse(ddlLab1.SelectedValue);
                                    var mp0 = mpBl.Obten(862, int.Parse(ddlMet1.SelectedValue));
                                    it.ParametroMetodoID = mp0.ParametroMetodoID;
                                    break;
                                case 118:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(T3.Text);
                                    it.LaboratorioID = int.Parse(ddlLab2.SelectedValue);
                                    var mp1 = mpBl.Obten(118, int.Parse(ddlMet2.SelectedValue));
                                    it.ParametroMetodoID = mp1.ParametroMetodoID;
                                    break;
                                case 225:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(CF3.Text);
                                    it.LaboratorioID = int.Parse(ddlLab4.SelectedValue);
                                    var mp2 = mpBl.Obten(225, int.Parse(ddlMet4.SelectedValue));
                                    it.ParametroMetodoID = mp2.ParametroMetodoID;
                                    break;
                                case 79:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(GA3.Text);
                                    it.LaboratorioID = int.Parse(ddlLab5.SelectedValue);
                                    var mp3 = mpBl.Obten(79, int.Parse(ddlMet5.SelectedValue));
                                    it.ParametroMetodoID = mp3.ParametroMetodoID;
                                    break;
                                case 89:
                                    bool tf;

                                    switch (ddlana3.SelectedValue)
                                    {
                                        case "true":
                                            tf = true;
                                            break;
                                        default:
                                            tf = false;
                                            break;
                                    }
                                    it.GeneradoSP = false;
                                    it.ValorValidacion = tf;
                                    it.LaboratorioID = int.Parse(ddlLab3.SelectedValue);
                                    var mp4 = mpBl.Obten(89, int.Parse(ddlMet3.SelectedValue));
                                    it.ParametroMetodoID = mp4.ParametroMetodoID;
                                    break;

                            }
                            muestravalorBl.Guardar(it);
                        }
                        muestraArgs.Descripcion = "N/A";
                        muestraArgs.Identificacion = "N/A";
                        muestraArgs.Caudal = decimal.Parse(CA3.Text);
                        muestraArgs.Observaciones = TextBox42.Text;
                        if (ValidarFecha(TextBox17.Text) || ValidarFecha(TextBox10.Text))
                            muestraArgs.Descripcion = "ErrorFecha";

                        muestraArgs.Fecha = DateTime.ParseExact(TextBox17.Text + " " + TextBox21.Text, "dd/MM/yyyy H:mm", CultureInfo.InvariantCulture);
                        muestraArgs.FechaRecepcion = DateTime.ParseExact(TextBox10.Text + " " + TextBox11.Text, "dd/MM/yyyy H:mm", CultureInfo.InvariantCulture);
                        break;
                    case 4:
                        if (ValidarFecha(TextBox25.Text) || ValidarFecha(TextBox10.Text))
                            muestraArgs.Descripcion = "ErrorFecha";
                        if (muestraArgs.Descripcion == "ErrorFecha")
                        {
                            Guardar(this, muestraArgs);
                            return;
                        }
                        var a4 = RemoveSpecialCharacters(TextBox2.Text);
                        var b4 = RemoveSpecialCharacters(TextBox13.Text);
                        var c4 = RemoveSpecialCharacters(TextBox21.Text);
                        var d4 = RemoveSpecialCharacters(TextBox26.Text);
                        if (a4 == "" || b4 == "" || c4 == "" || d4 == "")
                        {
                            muestraArgs.Descripcion = "ErrorHoras";
                            if (muestraArgs.Descripcion == "ErrorHoras")
                            {
                                Guardar(this, muestraArgs);
                                return;
                            }
                        }
                        if ((Convert.ToDecimal(a4) > Convert.ToDecimal(b4)) || (Convert.ToDecimal(b4) > Convert.ToDecimal(c4)) || (Convert.ToDecimal(c4) > Convert.ToDecimal(d4)))
                        {
                            muestraArgs.Descripcion = "ErrorHoras";
                            if (muestraArgs.Descripcion == "ErrorHoras")
                            {
                                Guardar(this, muestraArgs);
                                return;
                            }
                        }
                        if (CA4.Text == "")
                        {
                            muestraArgs.Descripcion = "Errorcaudal";
                            Guardar(this, muestraArgs);
                            return;
                        }
                        foreach (var it in list.Select(t1 => list.FirstOrDefault(p => p.MuestraValorID == t1.MuestraValorID)))
                        {
                            switch (it.ParametroID)
                            {
                                case 862:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(PH4.Text);
                                    it.LaboratorioID = int.Parse(ddlLab1.SelectedValue);
                                    var mp0 = mpBl.Obten(862, int.Parse(ddlMet1.SelectedValue));
                                    it.ParametroMetodoID = mp0.ParametroMetodoID;
                                    break;
                                case 118:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(T4.Text);
                                    it.LaboratorioID = int.Parse(ddlLab2.SelectedValue);
                                    var mp1 = mpBl.Obten(118, int.Parse(ddlMet2.SelectedValue));
                                    it.ParametroMetodoID = mp1.ParametroMetodoID;
                                    break;
                                case 225:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(CF4.Text);
                                    it.LaboratorioID = int.Parse(ddlLab4.SelectedValue);
                                    var mp2 = mpBl.Obten(225, int.Parse(ddlMet4.SelectedValue));
                                    it.ParametroMetodoID = mp2.ParametroMetodoID;
                                    break;
                                case 79:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(GA4.Text);
                                    it.LaboratorioID = int.Parse(ddlLab5.SelectedValue);
                                    var mp3 = mpBl.Obten(79, int.Parse(ddlMet5.SelectedValue));
                                    it.ParametroMetodoID = mp3.ParametroMetodoID;
                                    break;
                                case 89:
                                    bool tf;

                                    switch (ddlana4.SelectedValue)
                                    {
                                        case "true":
                                            tf = true;
                                            break;
                                        default:
                                            tf = false;
                                            break;
                                    }
                                    it.GeneradoSP = false;
                                    it.ValorValidacion = tf;
                                    it.LaboratorioID = int.Parse(ddlLab3.SelectedValue);
                                    var mp4 = mpBl.Obten(89, int.Parse(ddlMet3.SelectedValue));
                                    it.ParametroMetodoID = mp4.ParametroMetodoID;
                                    break;

                            }
                            muestravalorBl.Guardar(it);
                        }
                        muestraArgs.Descripcion = "N/A";
                        muestraArgs.Identificacion = "N/A";
                        muestraArgs.Caudal = decimal.Parse(CA4.Text);
                        muestraArgs.Observaciones = TextBox45.Text;
                        if (ValidarFecha(TextBox25.Text) || ValidarFecha(TextBox10.Text))
                            muestraArgs.Descripcion = "ErrorFecha";

                        muestraArgs.Fecha = DateTime.ParseExact(TextBox25.Text + " " + TextBox26.Text, "dd/MM/yyyy H:mm", CultureInfo.InvariantCulture);
                        muestraArgs.FechaRecepcion = DateTime.ParseExact(TextBox10.Text + " " + TextBox11.Text, "dd/MM/yyyy H:mm", CultureInfo.InvariantCulture);
                        break;
                    case 5:
                        if (ValidarFecha(TextBox30.Text) || ValidarFecha(TextBox10.Text))
                            muestraArgs.Descripcion = "ErrorFecha";
                        if (muestraArgs.Descripcion == "ErrorFecha")
                        {
                            Guardar(this, muestraArgs);
                            return;
                        }
                        var a5 = RemoveSpecialCharacters(TextBox2.Text);
                        var b5 = RemoveSpecialCharacters(TextBox13.Text);
                        var c5 = RemoveSpecialCharacters(TextBox21.Text);
                        var d5 = RemoveSpecialCharacters(TextBox26.Text);
                        var e5 = RemoveSpecialCharacters(TextBox31.Text);
                        if (a5 == "" || b5 == "" || c5 == "" || d5 == "" || e5 == "")
                        {
                            muestraArgs.Descripcion = "ErrorHoras";
                            if (muestraArgs.Descripcion == "ErrorHoras")
                            {
                                Guardar(this, muestraArgs);
                                return;
                            }
                        }
                        if ((Convert.ToDecimal(a5) > Convert.ToDecimal(b5)) || (Convert.ToDecimal(b5) > Convert.ToDecimal(c5)) || (Convert.ToDecimal(c5) > Convert.ToDecimal(d5)) || (Convert.ToDecimal(d5) > Convert.ToDecimal(e5)))
                        {
                            muestraArgs.Descripcion = "ErrorHoras";
                            if (muestraArgs.Descripcion == "ErrorHoras")
                            {
                                Guardar(this, muestraArgs);
                                return;
                            }
                        }
                        if (CA5.Text == "")
                        {
                            muestraArgs.Descripcion = "Errorcaudal";
                            Guardar(this, muestraArgs);
                            return;
                        }
                        foreach (var it in list.Select(t1 => list.FirstOrDefault(p => p.MuestraValorID == t1.MuestraValorID)))
                        {
                            switch (it.ParametroID)
                            {
                                case 862:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(PH5.Text);
                                    it.LaboratorioID = int.Parse(ddlLab1.SelectedValue);
                                    var mp0 = mpBl.Obten(862, int.Parse(ddlMet1.SelectedValue));
                                    it.ParametroMetodoID = mp0.ParametroMetodoID;
                                    break;
                                case 118:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(T5.Text);
                                    it.LaboratorioID = int.Parse(ddlLab2.SelectedValue);
                                    var mp1 = mpBl.Obten(118, int.Parse(ddlMet2.SelectedValue));
                                    it.ParametroMetodoID = mp1.ParametroMetodoID;
                                    break;
                                case 225:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(CF5.Text);
                                    it.LaboratorioID = int.Parse(ddlLab4.SelectedValue);
                                    var mp2 = mpBl.Obten(225, int.Parse(ddlMet4.SelectedValue));
                                    it.ParametroMetodoID = mp2.ParametroMetodoID;
                                    break;
                                case 79:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(GA5.Text);
                                    it.LaboratorioID = int.Parse(ddlLab5.SelectedValue);
                                    var mp3 = mpBl.Obten(79, int.Parse(ddlMet5.SelectedValue));
                                    it.ParametroMetodoID = mp3.ParametroMetodoID;
                                    break;
                                case 89:
                                    bool tf;

                                    switch (ddlana5.SelectedValue)
                                    {
                                        case "true":
                                            tf = true;
                                            break;
                                        default:
                                            tf = false;
                                            break;
                                    }
                                    it.GeneradoSP = false;
                                    it.ValorValidacion = tf;
                                    it.LaboratorioID = int.Parse(ddlLab3.SelectedValue);
                                    var mp4 = mpBl.Obten(89, int.Parse(ddlMet3.SelectedValue));
                                    it.ParametroMetodoID = mp4.ParametroMetodoID;
                                    break;

                            }
                            muestravalorBl.Guardar(it);
                        }
                        muestraArgs.Descripcion = "N/A";
                        muestraArgs.Identificacion = "N/A";
                        muestraArgs.Caudal = decimal.Parse(CA5.Text);
                        muestraArgs.Observaciones = TextBox48.Text;
                        if (ValidarFecha(TextBox30.Text) || ValidarFecha(TextBox10.Text))
                            muestraArgs.Descripcion = "ErrorFecha";

                        muestraArgs.Fecha = DateTime.ParseExact(TextBox30.Text + " " + TextBox31.Text, "dd/MM/yyyy H:mm", CultureInfo.InvariantCulture);
                        muestraArgs.FechaRecepcion = DateTime.ParseExact(TextBox10.Text + " " + TextBox11.Text, "dd/MM/yyyy H:mm", CultureInfo.InvariantCulture);
                        break;
                    case 6:
                        if (ValidarFecha(TextBox35.Text) || ValidarFecha(TextBox10.Text))
                            muestraArgs.Descripcion = "ErrorFecha";
                        if (muestraArgs.Descripcion == "ErrorFecha")
                        {
                            Guardar(this, muestraArgs);
                            return;
                        }
                        var a6 = RemoveSpecialCharacters(TextBox2.Text);
                        var b6 = RemoveSpecialCharacters(TextBox13.Text);
                        var c6 = RemoveSpecialCharacters(TextBox21.Text);
                        var d6 = RemoveSpecialCharacters(TextBox26.Text);
                        var e6 = RemoveSpecialCharacters(TextBox31.Text);
                        var f6 = RemoveSpecialCharacters(TextBox36.Text);
                        if (a6 == "" || b6 == "" || c6 == "" || d6 == "" || e6 == "" || f6 == "")
                        {
                            muestraArgs.Descripcion = "ErrorHoras";
                            if (muestraArgs.Descripcion == "ErrorHoras")
                            {
                                Guardar(this, muestraArgs);
                                return;
                            }
                        }
                        if ((Convert.ToDecimal(a6) > Convert.ToDecimal(b6)) || (Convert.ToDecimal(b6) > Convert.ToDecimal(c6)) || (Convert.ToDecimal(c6) > Convert.ToDecimal(d6)) || (Convert.ToDecimal(d6) > Convert.ToDecimal(e6)) || (Convert.ToDecimal(e6) > Convert.ToDecimal(f6)))
                        {
                            muestraArgs.Descripcion = "ErrorHoras";
                            if (muestraArgs.Descripcion == "ErrorHoras")
                            {
                                Guardar(this, muestraArgs);
                                return;
                            }
                        }
                        if (CA6.Text == "")
                        {
                            muestraArgs.Descripcion = "Errorcaudal";
                            Guardar(this, muestraArgs);
                            return;
                        }
                        foreach (var it in list.Select(t1 => list.FirstOrDefault(p => p.MuestraValorID == t1.MuestraValorID)))
                        {
                            switch (it.ParametroID)
                            {
                                case 862:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(PH6.Text);
                                    it.LaboratorioID = int.Parse(ddlLab1.SelectedValue);
                                    var mp0 = mpBl.Obten(862, int.Parse(ddlMet1.SelectedValue));
                                    it.ParametroMetodoID = mp0.ParametroMetodoID;
                                    break;
                                case 118:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(T6.Text);
                                    it.LaboratorioID = int.Parse(ddlLab2.SelectedValue);
                                    var mp1 = mpBl.Obten(118, int.Parse(ddlMet2.SelectedValue));
                                    it.ParametroMetodoID = mp1.ParametroMetodoID;
                                    break;
                                case 225:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(CF6.Text);
                                    it.LaboratorioID = int.Parse(ddlLab4.SelectedValue);
                                    var mp2 = mpBl.Obten(225, int.Parse(ddlMet4.SelectedValue));
                                    it.ParametroMetodoID = mp2.ParametroMetodoID;
                                    break;
                                case 79:
                                    it.GeneradoSP = false;
                                    it.Valor = decimal.Parse(GA6.Text);
                                    it.LaboratorioID = int.Parse(ddlLab5.SelectedValue);
                                    var mp3 = mpBl.Obten(79, int.Parse(ddlMet5.SelectedValue));
                                    it.ParametroMetodoID = mp3.ParametroMetodoID;
                                    break;
                                case 89:
                                    bool tf;

                                    switch (ddlana6.SelectedValue)
                                    {
                                        case "true":
                                            tf = true;
                                            break;
                                        default:
                                            tf = false;
                                            break;
                                    }
                                    it.GeneradoSP = false;
                                    it.ValorValidacion = tf;
                                    it.LaboratorioID = int.Parse(ddlLab3.SelectedValue);
                                    var mp4 = mpBl.Obten(89, int.Parse(ddlMet3.SelectedValue));
                                    it.ParametroMetodoID = mp4.ParametroMetodoID;
                                    break;

                            }
                            muestravalorBl.Guardar(it);
                        }
                        muestraArgs.Descripcion = "N/A";
                        muestraArgs.Identificacion = "N/A";
                        muestraArgs.Caudal = decimal.Parse(CA6.Text);
                        muestraArgs.Observaciones = TextBox51.Text;
                        if (ValidarFecha(TextBox35.Text) || ValidarFecha(TextBox10.Text))
                            muestraArgs.Descripcion = "ErrorFecha";

                        muestraArgs.Fecha = DateTime.ParseExact(TextBox35.Text + " " + TextBox36.Text, "dd/MM/yyyy H:mm", CultureInfo.InvariantCulture);
                        muestraArgs.FechaRecepcion = DateTime.ParseExact(TextBox10.Text + " " + TextBox11.Text, "dd/MM/yyyy H:mm", CultureInfo.InvariantCulture);
                        break;

                }
                muestraArgs.MuestraID = t.MuestraID;
                muestraArgs.AnalisisID = t.AnalisisID;
                muestraArgs.Datum = t.Datum;
                muestraArgs.DireccionID = t.DireccionID;
                muestraArgs.Numero = t.Numero;
                muestraArgs.Latitud = t.Latitud;
                muestraArgs.Longitud = t.Longitud;
                muestraArgs.DireccionID = t.DireccionID;
                muestraArgs.NivelID = t.NivelID;
                muestraArgs.ValMuestraC = Convert.ToDecimal(TextBox9.Text);
                muestraArgs.ValHojaCampo = Convert.ToDecimal(CheckBox1.Checked);

                if (muestraArgs.Descripcion != "ErrorFecha")
                {
                    var muestra = muestraBl.Actualiza(muestraArgs);
                    muestraBl.RecalculaSimple(muestra.MuestraID);
                }

                conts++;
            }

            #endregion


            //CargaInformacion((int)Session["analisiid"]);
            Response.Redirect(Request.Url.AbsoluteUri);
            //Response.Redirect("~/Analisis.aspx");
        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            //if (CheckBox1.Checked)
            //{
            //    //Gtemp.Visible = false;
            //    Button2.Visible = true;
            //}
            //else
            //{
            //    //Gtemp.Visible = true;
            //    Button2.Visible = false;
            //}
        }

        protected void ddlAction1_SelectedIndexChanged(object sender, EventArgs e)
        {

            var muestravalorBl = new MuestraValor_BL();
            var list = new List<MuestraValorArgBase_BL>();

            var listaMuestra = _muestraBl.Lista_Muestra((int)Session["analisiid"]).Where(x => x.NivelID == 1).ToList();

            const int conts = 1;
            foreach (var t in listaMuestra)
            {
                if (OrdenTrabajo.NormaID != null)
                    list = t.NivelID == (decimal)enumNivel.Simple
                        ? muestravalorBl.GuardaObten((int)OrdenTrabajo.NormaID, (int)t.MuestraID, t.NivelID,
                            false)
                        : muestravalorBl.GuardaObtenCompuesta((int)OrdenTrabajo.NormaID, (int)t.MuestraID);
                switch (conts)
                {
                    case 1:
                        var list1 = list;
                        foreach (
                            var it in
                                list.Select(
                                    t1 => list1.FirstOrDefault(p => p.MuestraValorID == t1.MuestraValorID)))
                        {
                            switch (it.ParametroID)
                            {
                                case 862:
                                    if (it.MuestraValorID != null)
                                    {
                                        object muestra;
                                        switch (ddlAction1.SelectedValue)
                                        {
                                            case "1":
                                                muestra = muestravalorBl.Actualiza_Exento((int)it.MuestraValorID, true);
                                                muestravalorBl.Actualiza_NoAnalizado((int)it.MuestraValorID, false);
                                                if (muestra != null)
                                                {
                                                    PH1.Enabled = false;
                                                    PH2.Enabled = false;
                                                    PH3.Enabled = false;
                                                    PH4.Enabled = false;
                                                    PH5.Enabled = false;
                                                    PH6.Enabled = false;
                                                    ddlLab1.Enabled = false;
                                                    ddlMet1.Enabled = false;
                                                }
                                                break;
                                            case "2":
                                                muestra = muestravalorBl.Actualiza_NoAnalizado((int)it.MuestraValorID, true);
                                                muestravalorBl.Actualiza_Exento((int)it.MuestraValorID, false);
                                                if (muestra != null)
                                                {
                                                    PH1.Enabled = false;
                                                    PH2.Enabled = false;
                                                    PH3.Enabled = false;
                                                    PH4.Enabled = false;
                                                    PH5.Enabled = false;
                                                    PH6.Enabled = false;
                                                    ddlLab1.Enabled = false;
                                                    ddlMet1.Enabled = false;
                                                }
                                                break;
                                            case "3":
                                                muestravalorBl.Actualiza_NoAnalizado((int)it.MuestraValorID, false);
                                                muestravalorBl.Actualiza_Exento((int)it.MuestraValorID, false);
                                                PH1.Enabled = true;
                                                PH2.Enabled = true;
                                                PH3.Enabled = true;
                                                PH4.Enabled = true;
                                                PH5.Enabled = true;
                                                PH6.Enabled = true;
                                                ddlLab1.Enabled = true;
                                                ddlMet1.Enabled = true;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                }
            }
            const string sScript = "$(function llamarsum() { sum(); });";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "messageScroll", sScript, true);
            Calcular();
        }

        protected void ddlAction2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var muestravalorBl = new MuestraValor_BL();
            var list = new List<MuestraValorArgBase_BL>();

            var listaMuestra = _muestraBl.Lista_Muestra((int)Session["analisiid"]).Where(x => x.NivelID == 1).ToList();

            const int conts = 1;
            foreach (var t in listaMuestra)
            {
                if (OrdenTrabajo.NormaID != null)
                    list = t.NivelID == (decimal)enumNivel.Simple
                        ? muestravalorBl.GuardaObten((int)OrdenTrabajo.NormaID, (int)t.MuestraID, t.NivelID,
                            false)
                        : muestravalorBl.GuardaObtenCompuesta((int)OrdenTrabajo.NormaID, (int)t.MuestraID);
                switch (conts)
                {
                    case 1:
                        var list1 = list;
                        foreach (
                            var it in
                                list.Select(
                                    t1 => list1.FirstOrDefault(p => p.MuestraValorID == t1.MuestraValorID)))
                        {
                            switch (it.ParametroID)
                            {
                                case 118:
                                    if (it.MuestraValorID != null)
                                    {
                                        object muestra;
                                        switch (ddlAction2.SelectedValue)
                                        {
                                            case "1":
                                                muestra = muestravalorBl.Actualiza_Exento((int)it.MuestraValorID, true);
                                                muestravalorBl.Actualiza_NoAnalizado((int)it.MuestraValorID, false);
                                                if (muestra != null)
                                                {
                                                    T1.Enabled = false;
                                                    T2.Enabled = false;
                                                    T3.Enabled = false;
                                                    T4.Enabled = false;
                                                    T5.Enabled = false;
                                                    T6.Enabled = false;
                                                    ddlLab2.Enabled = false;
                                                    ddlMet2.Enabled = false;
                                                }
                                                break;
                                            case "2":
                                                muestra = muestravalorBl.Actualiza_NoAnalizado((int)it.MuestraValorID, true);
                                                muestravalorBl.Actualiza_Exento((int)it.MuestraValorID, false);
                                                if (muestra != null)
                                                {
                                                    T1.Enabled = false;
                                                    T2.Enabled = false;
                                                    T3.Enabled = false;
                                                    T4.Enabled = false;
                                                    T5.Enabled = false;
                                                    T6.Enabled = false;
                                                    ddlLab2.Enabled = false;
                                                    ddlMet2.Enabled = false;
                                                }
                                                break;
                                            case "3":
                                                muestravalorBl.Actualiza_NoAnalizado((int)it.MuestraValorID, false);
                                                muestravalorBl.Actualiza_Exento((int)it.MuestraValorID, false);
                                                T1.Enabled = true;
                                                T2.Enabled = true;
                                                T3.Enabled = true;
                                                T4.Enabled = true;
                                                T5.Enabled = true;
                                                T6.Enabled = true;
                                                ddlLab2.Enabled = true;
                                                ddlMet2.Enabled = true;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                }
            }
            var compuesta = _muestraBl.Lista_Muestra((int)Session["analisiid"]).Where(x => x.NivelID == 2).ToList();
            foreach (var i in compuesta.SelectMany(t => t.MuestraValors))
            {
                switch (i.ParametroID)
                {
                    case 118:
                        object muestra;
                        switch (ddlAction2.SelectedValue)
                        {
                            case "1":
                                muestra = muestravalorBl.Actualiza_Exento((int)i.MuestraValorID, true);
                                muestravalorBl.Actualiza_NoAnalizado((int)i.MuestraValorID, false);
                                break;
                            case "2":
                                muestra = muestravalorBl.Actualiza_NoAnalizado((int)i.MuestraValorID, true);
                                muestravalorBl.Actualiza_Exento((int)i.MuestraValorID, false);
                                break;
                            case "3":
                                muestravalorBl.Actualiza_NoAnalizado((int)i.MuestraValorID, false);
                                muestravalorBl.Actualiza_Exento((int)i.MuestraValorID, false);
                                break;
                        }
                        break;
                }
            }
            const string sScript = "$(function llamarsum() { sum(); });";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "messageScroll", sScript, true);
            Calcular();
        }

        protected void ddlAction3_SelectedIndexChanged(object sender, EventArgs e)
        {
            var muestravalorBl = new MuestraValor_BL();
            var list = new List<MuestraValorArgBase_BL>();

            var listaMuestra = _muestraBl.Lista_Muestra((int)Session["analisiid"]).Where(x => x.NivelID == 1).ToList();

            const int conts = 1;
            foreach (var t in listaMuestra)
            {
                if (OrdenTrabajo.NormaID != null)
                    list = t.NivelID == (decimal)enumNivel.Simple
                        ? muestravalorBl.GuardaObten((int)OrdenTrabajo.NormaID, (int)t.MuestraID, t.NivelID,
                            false)
                        : muestravalorBl.GuardaObtenCompuesta((int)OrdenTrabajo.NormaID, (int)t.MuestraID);
                switch (conts)
                {
                    case 1:
                        var list1 = list;
                        foreach (
                            var it in
                                list.Select(
                                    t1 => list1.FirstOrDefault(p => p.MuestraValorID == t1.MuestraValorID)))
                        {
                            switch (it.ParametroID)
                            {
                                case 89:
                                    if (it.MuestraValorID != null)
                                    {
                                        object muestra;
                                        switch (ddlAction3.SelectedValue)
                                        {
                                            case "1":
                                                muestra = muestravalorBl.Actualiza_Exento((int)it.MuestraValorID, true);
                                                muestravalorBl.Actualiza_NoAnalizado((int)it.MuestraValorID, false);
                                                if (muestra != null)
                                                {
                                                    ddlana1.Enabled = false;
                                                    ddlana2.Enabled = false;
                                                    ddlana3.Enabled = false;
                                                    ddlana4.Enabled = false;
                                                    ddlana5.Enabled = false;
                                                    ddlana6.Enabled = false;
                                                    ddlLab3.Enabled = false;
                                                    ddlMet3.Enabled = false;
                                                }
                                                break;
                                            case "2":
                                                muestra = muestravalorBl.Actualiza_NoAnalizado((int)it.MuestraValorID, true);
                                                muestravalorBl.Actualiza_Exento((int)it.MuestraValorID, false);
                                                if (muestra != null)
                                                {
                                                    ddlana1.Enabled = false;
                                                    ddlana2.Enabled = false;
                                                    ddlana3.Enabled = false;
                                                    ddlana4.Enabled = false;
                                                    ddlana5.Enabled = false;
                                                    ddlana6.Enabled = false;
                                                    ddlLab3.Enabled = false;
                                                    ddlMet3.Enabled = false;
                                                }
                                                break;
                                            case "3":
                                                muestravalorBl.Actualiza_NoAnalizado((int)it.MuestraValorID, false);
                                                muestravalorBl.Actualiza_Exento((int)it.MuestraValorID, false);
                                                ddlana1.Enabled = true;
                                                ddlana2.Enabled = true;
                                                ddlana3.Enabled = true;
                                                ddlana4.Enabled = true;
                                                ddlana5.Enabled = true;
                                                ddlana6.Enabled = true;
                                                ddlLab3.Enabled = true;
                                                ddlMet3.Enabled = true;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                }
            }
            const string sScript = "$(function llamarsum() { sum(); });";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "messageScroll", sScript, true);
            Calcular();
        }

        protected void ddlAction4_SelectedIndexChanged(object sender, EventArgs e)
        {
            var muestravalorBl = new MuestraValor_BL();
            var list = new List<MuestraValorArgBase_BL>();

            var listaMuestra = _muestraBl.Lista_Muestra((int)Session["analisiid"]).Where(x => x.NivelID == 1).ToList();

            const int conts = 1;
            foreach (var t in listaMuestra)
            {
                if (OrdenTrabajo.NormaID != null)
                    list = t.NivelID == (decimal)enumNivel.Simple
                        ? muestravalorBl.GuardaObten((int)OrdenTrabajo.NormaID, (int)t.MuestraID, t.NivelID,
                            false)
                        : muestravalorBl.GuardaObtenCompuesta((int)OrdenTrabajo.NormaID, (int)t.MuestraID);
                switch (conts)
                {
                    case 1:
                        var list1 = list;
                        foreach (
                            var it in
                                list.Select(
                                    t1 => list1.FirstOrDefault(p => p.MuestraValorID == t1.MuestraValorID)))
                        {
                            switch (it.ParametroID)
                            {
                                case 225:
                                    if (it.MuestraValorID != null)
                                    {
                                        object muestra;
                                        switch (ddlAction4.SelectedValue)
                                        {
                                            case "1":
                                                muestra = muestravalorBl.Actualiza_Exento((int)it.MuestraValorID, true);
                                                muestravalorBl.Actualiza_NoAnalizado((int)it.MuestraValorID, false);
                                                if (muestra != null)
                                                {
                                                    CF1.Enabled = false;
                                                    CF2.Enabled = false;
                                                    CF3.Enabled = false;
                                                    CF4.Enabled = false;
                                                    CF5.Enabled = false;
                                                    CF6.Enabled = false;
                                                    ddlLab4.Enabled = false;
                                                    ddlMet4.Enabled = false;
                                                }
                                                lcmc1.Text = "0"; lcmc2.Text = "0"; lcmc3.Text = "0"; lcmc4.Text = "0"; lcmc5.Text = "0"; lcmc6.Text = "0";
                                                lcmc1.Enabled = false; lcmc2.Enabled = false; lcmc3.Enabled = false; lcmc4.Enabled = false; lcmc5.Enabled = false; lcmc6.Enabled = false;
                                                break;
                                            case "2":
                                                muestra = muestravalorBl.Actualiza_NoAnalizado((int)it.MuestraValorID, true);
                                                muestravalorBl.Actualiza_Exento((int)it.MuestraValorID, false);
                                                if (muestra != null)
                                                {
                                                    CF1.Enabled = false;
                                                    CF2.Enabled = false;
                                                    CF3.Enabled = false;
                                                    CF4.Enabled = false;
                                                    CF5.Enabled = false;
                                                    CF6.Enabled = false;
                                                    ddlLab4.Enabled = false;
                                                    ddlMet4.Enabled = false;
                                                }
                                                lcmc1.Text = "0"; lcmc2.Text = "0"; lcmc3.Text = "0"; lcmc4.Text = "0"; lcmc5.Text = "0"; lcmc6.Text = "0";
                                                lcmc1.Enabled = false; lcmc2.Enabled = false; lcmc3.Enabled = false; lcmc4.Enabled = false; lcmc5.Enabled = false; lcmc6.Enabled = false;
                                                break;
                                            case "3":
                                                muestravalorBl.Actualiza_NoAnalizado((int)it.MuestraValorID, false);
                                                muestravalorBl.Actualiza_Exento((int)it.MuestraValorID, false);
                                                CF1.Enabled = true;
                                                CF2.Enabled = true;
                                                CF3.Enabled = true;
                                                CF4.Enabled = true;
                                                CF5.Enabled = true;
                                                CF6.Enabled = true;
                                                ddlLab4.Enabled = true;
                                                ddlMet4.Enabled = true;
                                                lcmc1.Text = ""; lcmc2.Text = ""; lcmc3.Text = ""; lcmc4.Text = ""; lcmc5.Text = ""; lcmc6.Text = "";
                                                lcmc1.Enabled = true; lcmc2.Enabled = true; lcmc3.Enabled = true; lcmc4.Enabled = true; lcmc5.Enabled = true; lcmc6.Enabled = true;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                }
            }
            var compuesta = _muestraBl.Lista_Muestra((int)Session["analisiid"]).Where(x => x.NivelID == 2).ToList();
            foreach (var i in compuesta.SelectMany(t => t.MuestraValors))
            {
                switch (i.ParametroID)
                {
                    case 225:
                        object muestra;
                        switch (ddlAction4.SelectedValue)
                        {
                            case "1":
                                muestra = muestravalorBl.Actualiza_Exento((int)i.MuestraValorID, true);
                                muestravalorBl.Actualiza_NoAnalizado((int)i.MuestraValorID, false);
                                break;
                            case "2":
                                muestra = muestravalorBl.Actualiza_NoAnalizado((int)i.MuestraValorID, true);
                                muestravalorBl.Actualiza_Exento((int)i.MuestraValorID, false);
                                break;
                            case "3":
                                muestravalorBl.Actualiza_NoAnalizado((int)i.MuestraValorID, false);
                                muestravalorBl.Actualiza_Exento((int)i.MuestraValorID, false);
                                break;
                        }
                        break;
                }
            }
            const string sScript = "$(function llamarsum() { sum(); });";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "messageScroll", sScript, true);
            Calcular();
        }

        protected void ddlAction5_SelectedIndexChanged(object sender, EventArgs e)
        {
            var muestravalorBl = new MuestraValor_BL();
            var list = new List<MuestraValorArgBase_BL>();

            var listaMuestra = _muestraBl.Lista_Muestra((int)Session["analisiid"]).Where(x => x.NivelID == 1).ToList();

            const int conts = 1;
            foreach (var t in listaMuestra)
            {
                if (OrdenTrabajo.NormaID != null)
                    list = t.NivelID == (decimal)enumNivel.Simple
                        ? muestravalorBl.GuardaObten((int)OrdenTrabajo.NormaID, (int)t.MuestraID, t.NivelID,
                            false)
                        : muestravalorBl.GuardaObtenCompuesta((int)OrdenTrabajo.NormaID, (int)t.MuestraID);
                switch (conts)
                {
                    case 1:
                        var list1 = list;
                        foreach (
                            var it in
                                list.Select(
                                    t1 => list1.FirstOrDefault(p => p.MuestraValorID == t1.MuestraValorID)))
                        {
                            switch (it.ParametroID)
                            {
                                case 79:
                                    if (it.MuestraValorID != null)
                                    {
                                        object muestra;
                                        switch (ddlAction5.SelectedValue)
                                        {
                                            case "1":
                                                muestra = muestravalorBl.Actualiza_Exento((int)it.MuestraValorID, true);
                                                muestravalorBl.Actualiza_NoAnalizado((int)it.MuestraValorID, false);
                                                if (muestra != null)
                                                {
                                                    GA1.Enabled = false;
                                                    GA2.Enabled = false;
                                                    GA3.Enabled = false;
                                                    GA4.Enabled = false;
                                                    GA5.Enabled = false;
                                                    GA6.Enabled = false;
                                                    ddlLab5.Enabled = false;
                                                    ddlMet5.Enabled = false;
                                                }
                                                lcmc7.Text = "0"; lcmc8.Text = "0"; lcmc9.Text = "0"; l10lcmc.Text = "0"; l11lcmc.Text = "0"; l12lcmc.Text = "0";
                                                lcmc7.Enabled = false; lcmc8.Enabled = false; lcmc9.Enabled = false; l10lcmc.Enabled = false; l11lcmc.Enabled = false; l12lcmc.Enabled = false;
                                                break;
                                            case "2":
                                                muestra = muestravalorBl.Actualiza_NoAnalizado((int)it.MuestraValorID, true);
                                                muestravalorBl.Actualiza_Exento((int)it.MuestraValorID, false);
                                                if (muestra != null)
                                                {
                                                    GA1.Enabled = false;
                                                    GA2.Enabled = false;
                                                    GA3.Enabled = false;
                                                    GA4.Enabled = false;
                                                    GA5.Enabled = false;
                                                    GA6.Enabled = false;
                                                    ddlLab5.Enabled = false;
                                                    ddlMet5.Enabled = false;
                                                }
                                                lcmc7.Text = "0"; lcmc8.Text = "0"; lcmc9.Text = "0"; l10lcmc.Text = "0"; l11lcmc.Text = "0"; l12lcmc.Text = "0";
                                                lcmc7.Enabled = false; lcmc8.Enabled = false; lcmc9.Enabled = false; l10lcmc.Enabled = false; l11lcmc.Enabled = false; l12lcmc.Enabled = false;
                                                break;
                                            case "3":
                                                muestravalorBl.Actualiza_NoAnalizado((int)it.MuestraValorID, false);
                                                muestravalorBl.Actualiza_Exento((int)it.MuestraValorID, false);
                                                GA1.Enabled = true;
                                                GA2.Enabled = true;
                                                GA3.Enabled = true;
                                                GA4.Enabled = true;
                                                GA5.Enabled = true;
                                                GA6.Enabled = true;
                                                ddlLab5.Enabled = true;
                                                ddlMet5.Enabled = true;
                                                lcmc7.Text = ""; lcmc8.Text = ""; lcmc9.Text = ""; l10lcmc.Text = ""; l11lcmc.Text = ""; l12lcmc.Text = "";
                                                lcmc7.Enabled = true; lcmc8.Enabled = true; lcmc9.Enabled = true; l10lcmc.Enabled = true; l11lcmc.Enabled = true; l12lcmc.Enabled = true;
                                                break;
                                        }
                                    }
                                    break;
                            }
                        }
                        break;
                }
            }
            var compuesta = _muestraBl.Lista_Muestra((int)Session["analisiid"]).Where(x => x.NivelID == 2).ToList();
            foreach (var i in compuesta.SelectMany(t => t.MuestraValors))
            {
                switch (i.ParametroID)
                {
                    case 79:
                        object muestra;
                        switch (ddlAction5.SelectedValue)
                        {
                            case "1":
                                muestra = muestravalorBl.Actualiza_Exento((int)i.MuestraValorID, true);
                                muestravalorBl.Actualiza_NoAnalizado((int)i.MuestraValorID, false);
                                break;
                            case "2":
                                muestra = muestravalorBl.Actualiza_NoAnalizado((int)i.MuestraValorID, true);
                                muestravalorBl.Actualiza_Exento((int)i.MuestraValorID, false);
                                break;
                            case "3":
                                muestravalorBl.Actualiza_NoAnalizado((int)i.MuestraValorID, false);
                                muestravalorBl.Actualiza_Exento((int)i.MuestraValorID, false);
                                break;
                        }
                        break;
                }
            }
            const string sScript = "$(function llamarsum() { sum(); });";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "messageScroll", sScript, true);
            Calcular();
        }

        protected void ex_Click(object sender, EventArgs e)
        {
            const string sScript = "$(function llamarsum() { sum(); });";
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "messageScroll", sScript, true);
        }

     


    }
}