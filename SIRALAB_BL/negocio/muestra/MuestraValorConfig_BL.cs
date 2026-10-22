using SIRALAB_BL.catalogos;
using SIRALAB_BL.common;
using System.Web.UI.WebControls;

namespace SIRALAB_BL
{
    public class MuestraValorConfig_BL
    {
        public MuestraValorConfig_BL()
        {
        }

        /// <summary>
        /// Metodo utilizado para cargar los valores del repeater
        /// </summary>
        /// <param name="ddlLaboratorio"></param>
        public void ConfiguraParametros(Label lblTituloRepeater, LinkButton lnkbtnParametroID, DropDownList ddlLaboratorio, TextBox txtResultado, DropDownList ddlResultado, DropDownList ddlMetodo, LinkButton lnkbtnExentar, LinkButton lnkbtnEditar, LinkButton lnkbtnNoAnalizado, TextBox txtLC, Label lblResultado, bool EsCompuesta, int NormaID, int ParametroID, int LaboratorioLoginID, MuestraValorArgBase_BL MuestraValorItem)
        {
            if (lblTituloRepeater != null)
            {
                if (EsCompuesta)
                    lblTituloRepeater.Text = "Muestra Compuesta";
                else
                    lblTituloRepeater.Text = "Muestra Simple";
            }

            Laboratorio_BL laboratorio_bl = new Laboratorio_BL();
            var list = laboratorio_bl.Obten_Laboratorio(NormaID, ParametroID);

            MuestraValorConfig_BL MuestravalorConfiguracion_bl = new MuestraValorConfig_BL();
            Metodo_BL metodo_bl = new Metodo_BL();

            //Cargamos el drop con el nombre del laboratorio que ha iniciado sesión en el sistema

            ddlLaboratorio.LoadDrop("Nombre", "LaboratorioID", list);
            ddlLaboratorio.SelectedValue = LaboratorioLoginID.ToString();

            //Posteriormente cargamos los metodos del laboratorio que inicio sesión y que tiene metodos asociados a dicho parámetro.
            var lista_MetodosIniciales = metodo_bl.Obten_Metodo(int.Parse(ddlLaboratorio.SelectedValue.ToString()), int.Parse(lnkbtnParametroID.CommandArgument));
            ddlMetodo.LoadDrop("Nombre", "MetodoID", lista_MetodosIniciales);

            //Obtenemos la muestra valor de cada item
            var mu = MuestraValorItem;

            //Configuramos el boton de exentar
            if ((bool)mu.Exenta)
                lnkbtnExentar.Visible = true;
            else
                lnkbtnExentar.Visible = false;

            //Configuramos si el parametro esta exento o esta en no analizado
            if (mu.Exento == true || mu.NoAnalizado == true)
            {
                lnkbtnEditar.Visible = true;
                lnkbtnEditar.Enabled = true;
                lblResultado.Visible = true;
                lnkbtnExentar.Visible = false;
                lnkbtnNoAnalizado.Visible = false;
                ddlLaboratorio.Enabled = false;
                ddlMetodo.Enabled = false;
                txtResultado.Enabled = false;
                txtResultado.Visible = false;
                ddlResultado.Enabled = false;
                ddlResultado.Visible = false;
                txtLC.Enabled = false;

                if (mu.Exento == true)
                    lblResultado.Text = "Exento";
                else
                    lblResultado.Text = "No Analizado";
            }
            else
            {
                //En caso que se trate de una muestra que requiera de la validacion de DDl, se cargaran los drops
                //Configuramos si se trata de una configuración acerca del materia flotanto u otro parametro
                if (mu.Parametro.Nombre.Contains("Flotante"))
                {
                    ListItem li = new ListItem { Value = "false", Text = "Presente", Selected = true };
                    ddlResultado.Items.Insert(0, li);
                    ListItem li2 = new ListItem { Value = "true", Text = "Ausente" };
                    ddlResultado.Items.Insert(1, li2);
                }
                else
                {
                    ListItem li = new ListItem { Value = "true", Text = "Aprueba" };
                    ddlResultado.Items.Insert(0, li);
                    ListItem li2 = new ListItem { Value = "false", Text = "No Aprueba", Selected = true };
                    ddlResultado.Items.Insert(1, li2);
                }
                //Fin de carga de drops

                //Validación utilizada para verificar si el parametro puede exentar parametros
                if ((bool)mu.Exenta)
                    lnkbtnExentar.Visible = true;
                else
                    lnkbtnExentar.Visible = false;

                //Validación utilizada para verificar si el parámetro acepta valores, en caso afirmativo, se ponen en visible el textbox para captura de valor.
                //En caso negativo se pone el drop en visible para que seleccione la opción deseada.
                if (mu.AceptaValor)
                {
                    ddlResultado.Visible = false;
                    txtResultado.Visible = true;
                }
                else
                {
                    ddlResultado.Visible = true;
                    txtResultado.Visible = false;
                }

                //Validacion utilizada para verificar si el parametro ya tiene un parametroMetodo, en caso que ya contenga un parametroMetodo significa que fue guardado anteriormente y ya tiene valores.
                if (mu.ParametroMetodo != null)
                {
                    lnkbtnEditar.Visible = true;
                    lnkbtnExentar.Visible = false;
                    lnkbtnNoAnalizado.Visible = false;

                    //Aqui se inactivan los registros que ya fueron cargados sus valores previamente
                    txtResultado.Enabled = false;
                    txtLC.Enabled = false;
                    ddlLaboratorio.Enabled = false;
                    ddlMetodo.Enabled = false;

                    //Validación utilizada para verificar que el parametro acepta valor
                    if (mu.AceptaValor)
                    {
                        //Pones en invisible dl ddlResultado, ya que se capturara un valor numerico
                        ddlResultado.Visible = false;
                        txtResultado.Visible = true;

                        ddlLaboratorio.SelectedValue = mu.LaboratorioID.ToString();
                        txtResultado.Text = mu.Valor.ToString();

                        if (mu.LC_MC != null)
                            txtLC.Text = mu.LC_MC.ToString();

                        var lista_Metodos = metodo_bl.Obten_Metodo(int.Parse(ddlLaboratorio.SelectedValue.ToString()), int.Parse(lnkbtnParametroID.CommandArgument));
                        ddlMetodo.LoadDrop("Nombre", "MetodoID", lista_Metodos);
                        ddlMetodo.SelectedValue = mu.ParametroMetodo.Metodo.MetodoID.ToString();
                    }
                    else
                    {
                        ddlResultado.Visible = true;
                        txtResultado.Visible = false;

                        ddlLaboratorio.SelectedValue = mu.LaboratorioID.ToString();
                        txtResultado.Text = mu.Valor.ToString();

                        if (mu.LC_MC != null)
                            txtLC.Text = mu.LC_MC.ToString();

                        var lista_Metodos = metodo_bl.Obten_Metodo(int.Parse(ddlLaboratorio.SelectedValue.ToString()), int.Parse(lnkbtnParametroID.CommandArgument));
                        //var lista_Metodos = metodo_bl.Obten_Metodo(1, int.Parse(lnkbtnParametroID.CommandArgument));
                        ddlMetodo.LoadDrop("Nombre", "MetodoID", lista_Metodos);
                        ddlMetodo.SelectedValue = mu.ParametroMetodo.Metodo.MetodoID.ToString();
                        ddlResultado.SelectedValue = mu.ValorValidacion == true ? "true" : "false";
                    }
                }


                else
                {
                    lnkbtnEditar.Visible = false;
                    lnkbtnNoAnalizado.Visible = true;
                }
                //Configuración de botones para el caso de GeneradoSP=NULL

                if (mu.GeneradoSP == null)
                {
                    lnkbtnEditar.Visible = false;
                    lnkbtnExentar.Visible = false;
                    lnkbtnNoAnalizado.Visible = false;
                    txtResultado.Visible = false;
                    ddlResultado.Visible = false;
                    lblResultado.Text = mu.Valor.ToString();
                    lblResultado.Visible = true;
                    txtLC.Enabled = true;
                    txtLC.Visible = true;
                }
            }
        }
        public void ConfiguraParametros(int ParametroID, DropDownList ddlLaboratorio, TextBox txtResultado, DropDownList ddlResultado, DropDownList ddlMetodo, LinkButton lnkbtnExentar, LinkButton lnkbtnEditar, LinkButton lnkbtnNoAnalizado, TextBox txtLC, Label lblResultado, bool EsCompuesta, int NormaID, int LaboratorioLoginID, MuestraValorArgBase_BL MuestraValorItem)
        {
            Laboratorio_BL laboratorio_bl = new Laboratorio_BL();
            Metodo_BL metodo_bl = new Metodo_BL();
            MuestraValorConfig_BL MuestravalorConfiguracion_bl = new MuestraValorConfig_BL();

            var listLabParametro = laboratorio_bl.Obten_Laboratorio(NormaID, ParametroID);
            var listLabMetodo = metodo_bl.Obten_Metodo(int.Parse(ddlLaboratorio.SelectedValue.ToString()), ParametroID);

            //Cargamos el drop con el nombre del laboratorio que ha iniciado sesión en el sistema
            ddlLaboratorio.LoadDrop("Nombre", "LaboratorioID", listLabParametro);
            ddlLaboratorio.SelectedValue = LaboratorioLoginID.ToString();

            //Posteriormente cargamos los metodos del laboratorio que inicio sesión y que tiene metodos asociados a dicho parámetro.
            ddlMetodo.LoadDrop("Nombre", "MetodoID", listLabMetodo);



            /*-----------------------------------------------------------------------------------------------------------------*/
            /*-----------------------------------------------------------------------------------------------------------------*/
            /*-----------------------------------------------------------------------------------------------------------------*/

            //Obtenemos la muestra valor de cada item
            var mu = MuestraValorItem;

            //Configuramos el boton de exentar
            if ((bool)mu.Exenta)
                lnkbtnExentar.Visible = true;
            else
                lnkbtnExentar.Visible = false;

            //Configuramos si el parametro esta exento o esta en no analizado
            if (mu.Exento == true || mu.NoAnalizado == true)
            {
                lnkbtnEditar.Visible = true;
                lnkbtnEditar.Enabled = true;
                lblResultado.Visible = true;
                lnkbtnExentar.Visible = false;
                lnkbtnNoAnalizado.Visible = false;
                ddlLaboratorio.Enabled = false;
                ddlMetodo.Enabled = false;
                txtResultado.Enabled = false;
                txtResultado.Visible = false;
                ddlResultado.Enabled = false;
                ddlResultado.Visible = false;
                txtLC.Enabled = false;

                if (mu.Exento == true)
                    lblResultado.Text = "Exento";
                else
                    lblResultado.Text = "No Analizado";
            }
            else
            {
                //En caso que se trate de una muestra que requiera de la validacion de DDl, se cargaran los drops
                //Configuramos si se trata de una configuración acerca del materia flotanto u otro parametro
                if (mu.Parametro.Nombre.Contains("Flotante"))
                {
                    ListItem li = new ListItem { Value = "false", Text = "Presente", Selected = true };
                    ddlResultado.Items.Insert(0, li);
                    ListItem li2 = new ListItem { Value = "true", Text = "Ausente" };
                    ddlResultado.Items.Insert(1, li2);
                }
                else
                {
                    ListItem li = new ListItem { Value = "true", Text = "Aprueba" };
                    ddlResultado.Items.Insert(0, li);
                    ListItem li2 = new ListItem { Value = "false", Text = "No Aprueba", Selected = true };
                    ddlResultado.Items.Insert(1, li2);
                }
                //Fin de carga de drops

                //Validación utilizada para verificar si el parametro puede exentar parametros
                if ((bool)mu.Exenta)
                    lnkbtnExentar.Visible = true;
                else
                    lnkbtnExentar.Visible = false;

                //Validación utilizada para verificar si el parámetro acepta valores, en caso afirmativo, se ponen en visible el textbox para captura de valor.
                //En caso negativo se pone el drop en visible para que seleccione la opción deseada.
                if (mu.AceptaValor)
                {
                    ddlResultado.Visible = false;
                    txtResultado.Visible = true;
                }
                else
                {
                    ddlResultado.Visible = true;
                    txtResultado.Visible = false;
                }

                //Validacion utilizada para verificar si el parametro ya tiene un parametroMetodo, en caso que ya contenga un parametroMetodo significa que fue guardado anteriormente y ya tiene valores.
                if (mu.ParametroMetodo != null)
                {
                    lnkbtnEditar.Visible = true;
                    lnkbtnExentar.Visible = false;
                    lnkbtnNoAnalizado.Visible = false;

                    //Aqui se inactivan los registros que ya fueron cargados sus valores previamente
                    txtResultado.Enabled = false;
                    txtLC.Enabled = false;
                    ddlLaboratorio.Enabled = false;
                    ddlMetodo.Enabled = false;

                    //Validación utilizada para verificar que el parametro acepta valor
                    if (mu.AceptaValor)
                    {
                        //Pones en invisible dl ddlResultado, ya que se capturara un valor numerico
                        ddlResultado.Visible = false;
                        txtResultado.Visible = true;

                        ddlLaboratorio.SelectedValue = mu.LaboratorioID.ToString();
                        txtResultado.Text = mu.Valor.ToString();

                        if (mu.LC_MC != null)
                            txtLC.Text = mu.LC_MC.ToString();

                        var lista_Metodos = metodo_bl.Obten_Metodo(int.Parse(ddlLaboratorio.SelectedValue.ToString()), ParametroID);
                        ddlMetodo.LoadDrop("Nombre", "MetodoID", lista_Metodos);
                        ddlMetodo.SelectedValue = mu.ParametroMetodo.Metodo.MetodoID.ToString();
                    }
                    else
                    {
                        ddlResultado.Visible = true;
                        txtResultado.Visible = false;

                        ddlLaboratorio.SelectedValue = mu.LaboratorioID.ToString();
                        txtResultado.Text = mu.Valor.ToString();

                        if (mu.LC_MC != null)
                            txtLC.Text = mu.LC_MC.ToString();

                        var lista_Metodos = metodo_bl.Obten_Metodo(int.Parse(ddlLaboratorio.SelectedValue.ToString()), ParametroID);
                        //var lista_Metodos = metodo_bl.Obten_Metodo(1, int.Parse(lnkbtnParametroID.CommandArgument));
                        ddlMetodo.LoadDrop("Nombre", "MetodoID", lista_Metodos);
                        ddlMetodo.SelectedValue = mu.ParametroMetodo.Metodo.MetodoID.ToString();
                        ddlResultado.SelectedValue = mu.ValorValidacion == true ? "true" : "false";
                    }
                }


                else
                {
                    lnkbtnEditar.Visible = false;
                    lnkbtnNoAnalizado.Visible = true;
                }

                //Configuración de botones para el caso de GeneradoSP=NULL

                if (mu.GeneradoSP == null)
                {
                    lnkbtnEditar.Visible = false;
                    lnkbtnExentar.Visible = false;
                    lnkbtnNoAnalizado.Visible = false;
                    txtResultado.Visible = false;
                    ddlResultado.Visible = false;
                    lblResultado.Text = mu.Valor.ToString();
                    lblResultado.Visible = true;
                    txtLC.Enabled = true;
                    txtLC.Visible = true;
                }
            }
        }
        public void GuardarPmtos(LinkButton lnkbtnMuestraValor, LinkButton lnkbtnParametroID, TextBox txtResultado, DropDownList ddlResultado, DropDownList ddlLaboratorio, DropDownList ddlMetodo, LinkButton lnkbtnEditar, LinkButton lnkbtnGuardar, TextBox txtLC, int NivelID, int NormaID, int CuerpoReceptorID, int UsoAguaID, int MuestraID, LinkButton lnkbtnExentar, LinkButton lnkbtnNoAnalizado, Label lblResultado)
        {
            if (txtResultado.Visible == true && ddlResultado.Visible == false)
            {
                if (txtResultado.Text != "")
                {
                    ParametroMetodo_BL parametrometodo_bl = new ParametroMetodo_BL();
                    var parametroMetodo = parametrometodo_bl.Obten(int.Parse(lnkbtnParametroID.CommandArgument), int.Parse(ddlMetodo.SelectedValue));

                    if (parametroMetodo != null)
                    {
                        int PromedioID = NivelID;

                        NormaConfiguracion_BL normaconfi_bl = new NormaConfiguracion_BL();
                        var normaConfig = normaconfi_bl.Obtener_ParametroMetodo((int)parametroMetodo.ParametroID, (int)NormaID, (int)CuerpoReceptorID, (int)UsoAguaID, PromedioID);
                        MuestraValor_BL muestravalor_bl = new MuestraValor_BL();

                        MuestraValor_BL mv_bl = new MuestraValor_BL();
                        var muestravalor = mv_bl.Obten(int.Parse(lnkbtnMuestraValor.CommandArgument));

                        //Se llenan los argumentos
                        MuestraValorArgBase_BL muestravalorArgs = new MuestraValorArgBase_BL();
                        muestravalorArgs.MuestraValorID = int.Parse(lnkbtnMuestraValor.CommandArgument);
                        muestravalorArgs.MuestraID = MuestraID;
                        muestravalorArgs.ParametroID = int.Parse(lnkbtnParametroID.CommandArgument);
                        muestravalorArgs.LaboratorioID = int.Parse(ddlLaboratorio.SelectedValue);


                        muestravalorArgs.Valor = decimal.Parse(txtResultado.Text);
                        muestravalorArgs.ParametroMetodoID = (int)parametroMetodo.ParametroMetodoID;
                        muestravalorArgs.LC_MC = decimal.Parse(txtLC.Text);

                        muestravalorArgs.Exenta = normaConfig.Exenta;

                        muestravalorArgs.Exento = muestravalor.Exento;
                        muestravalorArgs.NoAnalizado = muestravalor.NoAnalizado;

                        muestravalorArgs.AceptaValor = normaConfig.AceptaValor;
                        muestravalorArgs.LimiteInferior = normaConfig.LimiteInferior;
                        muestravalorArgs.LimiteSuperior = normaConfig.LimiteSuperior;

                        muestravalorArgs.GeneradoSP = false;

                        if (normaConfig.LimiteInferior != null)
                        {
                            //Validaciones para comprobar si se cumple con el parametro
                            if (decimal.Parse(txtResultado.Text) < normaConfig.LimiteInferior || decimal.Parse(txtResultado.Text) > normaConfig.LimiteSuperior)
                                muestravalorArgs.ValorValidacion = false;
                            else
                                muestravalorArgs.ValorValidacion = true;
                        }
                        else
                        {
                            //Validaciones para comprobar si se cumple con el parametro
                            if (decimal.Parse(txtResultado.Text) > normaConfig.LimiteSuperior)
                                muestravalorArgs.ValorValidacion = false;
                            else
                                muestravalorArgs.ValorValidacion = true;
                        }

                        muestravalor_bl.Guardar(muestravalorArgs);

                        //Desactivamos los botones para que se conviertan en solo lectura
                        lnkbtnEditar.Visible = true;
                        //lnkbtnGuardar.Visible = false;
                        ddlLaboratorio.Enabled = false;
                        ddlMetodo.Enabled = false;
                        txtResultado.Enabled = false;
                        txtLC.Enabled = false;
                        lnkbtnExentar.Visible = false;
                        lnkbtnNoAnalizado.Visible = false;
                    }
                }
            }
            //Cuando el ddlResultado este visible se ejecutará esta parte de codigo


            if (txtResultado.Visible == false && ddlResultado.Visible == true)
            {
                ParametroMetodo_BL parametrometodo_bl = new ParametroMetodo_BL();
                //-------var parametroMetodo = parametrometodo_bl.Obten(int.Parse(lnkbtnParametroID.CommandArgument), int.Parse(ddlMetodo.SelectedValue));
                var parametroMetodo = parametrometodo_bl.Obten(int.Parse(lnkbtnParametroID.CommandArgument), 1);

                if (parametroMetodo != null)
                {
                    int PromedioID = NivelID;

                    NormaConfiguracion_BL normaconfi_bl = new NormaConfiguracion_BL();
                    var normaConfig = normaconfi_bl.Obtener_ParametroMetodo((int)parametroMetodo.ParametroID, NormaID, CuerpoReceptorID, UsoAguaID, PromedioID);
                    MuestraValor_BL muestravalor_bl = new MuestraValor_BL();
                    var muestravalor = muestravalor_bl.Obten(int.Parse(lnkbtnMuestraValor.CommandArgument));

                    //Se llenan los argumentos
                    MuestraValorArgBase_BL muestravalorArgs = new MuestraValorArgBase_BL();
                    muestravalorArgs.MuestraValorID = int.Parse(lnkbtnMuestraValor.CommandArgument);
                    muestravalorArgs.MuestraID = MuestraID;
                    muestravalorArgs.ParametroID = int.Parse(lnkbtnParametroID.CommandArgument);
                    muestravalorArgs.LaboratorioID = int.Parse(ddlLaboratorio.SelectedValue);
                    //muestravalorArgs.LaboratorioID = 1;
                    muestravalorArgs.Valor = decimal.Parse("0");
                    muestravalorArgs.ParametroMetodoID = (int)parametroMetodo.ParametroMetodoID;

                    muestravalorArgs.AceptaValor = normaConfig.AceptaValor;
                    muestravalorArgs.LimiteInferior = normaConfig.LimiteInferior;
                    muestravalorArgs.LimiteSuperior = normaConfig.LimiteSuperior;

                    //Seleccionamos el valor del ddl Para guardar el valor seleccionado
                    muestravalorArgs.ValorValidacion = bool.Parse(ddlResultado.SelectedValue);

                    /*----------------------------------------------------------------------------------------------------------*/
                    muestravalorArgs.LC_MC = decimal.Parse(txtLC.Text);

                    muestravalorArgs.Exenta = normaConfig.Exenta;

                    muestravalorArgs.Exento = muestravalor.Exento;
                    muestravalorArgs.NoAnalizado = muestravalor.NoAnalizado;

                    muestravalorArgs.GeneradoSP = false;
                    /*----------------------------------------------------------------------------------------------------------*/
                    muestravalor_bl.Guardar(muestravalorArgs);

                    //Desactivamos los botones para que se conviertan en solo lectura
                    lnkbtnEditar.Visible = true;
                    //lnkbtnGuardar.Visible = false;
                    ddlLaboratorio.Enabled = false;
                    ddlMetodo.Enabled = false;
                    txtResultado.Enabled = false;
                    txtLC.Enabled = false;
                    lnkbtnExentar.Visible = false;
                    lnkbtnNoAnalizado.Visible = false;
                }
            }

            //Cuando el TEXT y el Drop estan invisibles

            if (txtResultado.Visible == false && ddlResultado.Visible == false && lblResultado.Text != "No Analizado")
            {
                if (lblResultado.Text != "Exento")
                {
                    ParametroMetodo_BL parametrometodo_bl = new ParametroMetodo_BL();
                    var parametroMetodo = parametrometodo_bl.Obten(int.Parse(lnkbtnParametroID.CommandArgument), int.Parse(ddlMetodo.SelectedValue));

                    if (parametroMetodo != null)
                    {
                        int PromedioID = NivelID;

                        NormaConfiguracion_BL normaconfi_bl = new NormaConfiguracion_BL();
                        var normaConfig = normaconfi_bl.Obtener_ParametroMetodo((int)parametroMetodo.ParametroID, (int)NormaID, (int)CuerpoReceptorID, (int)UsoAguaID, PromedioID);
                        MuestraValor_BL muestravalor_bl = new MuestraValor_BL();

                        MuestraValor_BL mv_bl = new MuestraValor_BL();
                        var muestravalor = mv_bl.Obten(int.Parse(lnkbtnMuestraValor.CommandArgument));

                        //Se llenan los argumentos
                        MuestraValorArgBase_BL muestravalorArgs = new MuestraValorArgBase_BL();
                        muestravalorArgs.MuestraValorID = int.Parse(lnkbtnMuestraValor.CommandArgument);
                        muestravalorArgs.MuestraID = MuestraID;
                        muestravalorArgs.ParametroID = int.Parse(lnkbtnParametroID.CommandArgument);
                        muestravalorArgs.LaboratorioID = int.Parse(ddlLaboratorio.SelectedValue);


                        muestravalorArgs.Valor = decimal.Parse(lblResultado.Text);
                        muestravalorArgs.ParametroMetodoID = (int)parametroMetodo.ParametroMetodoID;
                        muestravalorArgs.LC_MC = txtLC.Text == "0" || txtLC.Text.Trim() == "" ? null : (decimal?)decimal.Parse(txtLC.Text);

                        muestravalorArgs.Exenta = normaConfig.Exenta;

                        muestravalorArgs.Exento = muestravalor.Exento;
                        muestravalorArgs.NoAnalizado = muestravalor.NoAnalizado;

                        muestravalorArgs.AceptaValor = normaConfig.AceptaValor;
                        muestravalorArgs.LimiteInferior = normaConfig.LimiteInferior;
                        muestravalorArgs.LimiteSuperior = normaConfig.LimiteSuperior;

                        muestravalorArgs.GeneradoSP = null;

                        muestravalorArgs.ValorValidacion = muestravalor.ValorValidacion;

                        //if (normaConfig.LimiteInferior != null)
                        //{
                        //    //Validaciones para comprobar si se cumple con el parametro
                        //    if (decimal.Parse(txtResultado.Text) < normaConfig.LimiteInferior || decimal.Parse(txtResultado.Text) > normaConfig.LimiteSuperior)
                        //        muestravalorArgs.ValorValidacion = false;
                        //    else
                        //        muestravalorArgs.ValorValidacion = true;
                        //}
                        //else
                        //{
                        //    //Validaciones para comprobar si se cumple con el parametro
                        //    if (decimal.Parse(txtResultado.Text) > normaConfig.LimiteSuperior)
                        //        muestravalorArgs.ValorValidacion = false;
                        //    else
                        //        muestravalorArgs.ValorValidacion = true;
                        //}

                        muestravalor_bl.Guardar(muestravalorArgs);

                        //Desactivamos los botones para que se conviertan en solo lectura
                        lnkbtnEditar.Visible = true;
                        //lnkbtnGuardar.Visible = false;
                        ddlLaboratorio.Enabled = false;
                        ddlMetodo.Enabled = false;
                        txtResultado.Enabled = false;
                        txtLC.Enabled = false;
                        lnkbtnExentar.Visible = false;
                        lnkbtnNoAnalizado.Visible = false;
                    }
                }
            }
        }
    }
}