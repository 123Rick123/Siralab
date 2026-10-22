using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using SIRALAB_DAL;
using SIRALAB_BL;
using SIRALAB_BL.catalogos;
using SIRALAB_BL.negocio;


namespace SIRALAB_BL.common
{
    public sealed class LoadDrops
    {
        //private Siralab_Entities sdb;
        public static LoadDrops instance = new LoadDrops();

        private LoadDrops()
        {
            //sdb = new Siralab_Entities();
        }

        public void Estado_Load(DropDownList ddl)
        {
            using (Siralab_Entities sdb = new Siralab_Entities())
            {
                var estado = sdb.usp_ListaEntidadesFederativas().OrderBy(p => p.Nombre);
                ddl.LoadDrop("Nombre", "EstadoID", estado);
            }
        }

        public void Municipio_Load(DropDownList ddl, int EstadoID)
        {
            using (Siralab_Entities sdb = new Siralab_Entities())
            {
                var municipio = sdb.usp_ListaMunicipios(EstadoID).OrderBy(p => p.Nombre);
                ddl.LoadDrop("Nombre", "MunicipioID", municipio);
            }
        }

        public void Localidad(DropDownList ddl, int EstadoID, int MunicipioID)
        {
            using (Siralab_Entities sdb = new Siralab_Entities())
            {
                var localidad = sdb.usp_ListaLocalidades(EstadoID, MunicipioID).OrderBy(p => p.Nombre);
                ddl.LoadDrop("Nombre", "LocalidadID", localidad);
            }
        }

        public void Colonia(DropDownList ddl, int EstadoID, int MunicipioID)
        {
            using (Siralab_Entities sdb = new Siralab_Entities())
            {
                var localidad = sdb.usp_ListaColonias(EstadoID, MunicipioID).OrderBy(p => p.Nombre);
                ddl.LoadDrop("Nombre", "ColoniaID", localidad);
            }
        }

        public void Clientes(DropDownList ddl)
        {
            Cliente_BL cliente=new Cliente_BL();
            var lista = cliente.ObtenerTodos().OrderBy(p => p.RFC);
            ddl.LoadDrop("RFC", "ClienteID", lista);
        }

        public void Conseciones(DropDownList ddl, int ClienteID)
        {
            Consecion_BL consecion_bl = new Consecion_BL();
            var listado = consecion_bl.Listado_Consecion(ClienteID).OrderBy(p => p.Titulo);
            ddl.LoadDrop("Titulo", "ConsecionID", listado);
        }

        public void Anexos(DropDownList ddl, int ConsecionID)
        {
            Anexo_BL anexo_bl = new Anexo_BL();
            var anexo = anexo_bl.Listado_Anexo(ConsecionID).OrderBy(p => p.Anexo1);
            ddl.LoadDrop("Anexo1", "AnexoID", anexo);
        }

        public void TipoCuerpoReceptor(DropDownList ddl)
        {
            CatTipoCuerpoReceptor_BL ctcr_bl = new CatTipoCuerpoReceptor_BL();
            var tipoCuerpoReceptores = ctcr_bl.ObtenerTodos().OrderBy(p => p.Nombre);
            ddl.LoadDrop("Nombre", "TipoCuerpoReceptorID", tipoCuerpoReceptores);
        }

        public void CuerpoReceptor(DropDownList ddl)
        {
            CuerpoReceptor_BL ctcr_bl = new CuerpoReceptor_BL();
            var cuerposReceptores = ctcr_bl.ObtenerTodos().Where(x => x.Norma != "2021").OrderBy(p => p.Nombre);
            ddl.LoadDrop("Nombre", "CuerpoReceptorID", cuerposReceptores);
        }
        public void CuerpoReceptor2021(DropDownList ddl)
        {
            CuerpoReceptor_BL ctcr_bl = new CuerpoReceptor_BL();
            var cuerposReceptores = ctcr_bl.ObtenerTodos().Where(x=>x.Norma== "2021").OrderBy(p => p.Nombre);
            ddl.LoadDrop("Nombre", "CuerpoReceptorID", cuerposReceptores);
        }
        public void CuerpoReceptorCOT(DropDownList ddl)
        {
            CuerpoReceptor_BL ctcr_bl = new CuerpoReceptor_BL();
            var cuerposReceptores = ctcr_bl.ObtenerTodos().Where(x => x.Nombre == "N/A").OrderBy(p => p.Nombre);
            ddl.LoadDrop("Nombre", "CuerpoReceptorID", cuerposReceptores);
        }
        public void UsoAgua(DropDownList ddl)
        {
            CatUsoAgua_BL ctusoAgua = new CatUsoAgua_BL();
            var usoAgua = ctusoAgua.ObtenerTodos().OrderBy(p => p.Nombre);
            ddl.LoadDrop("Nombre", "UsoAguaID", usoAgua);
        }

        public void ParametroMetodo(DropDownList ddl)
        {
            ParametroMetodo_BL ctParametroMetodo = new ParametroMetodo_BL();
            var parametroMetodo = ctParametroMetodo.ObtenerTodos().OrderBy(p => p.NombreParametroMetodo);
            ddl.LoadDrop("NombreParametroMetodo", "ParametroMetodoID", parametroMetodo);
        }

        public void Parametro(DropDownList ddl)
        {
            Parametro_BL ctParametro = new Parametro_BL();
            var parametro = ctParametro.ObtenerQueryable(p => true).OrderBy(p => p.Nombre);
            ddl.LoadDrop("Nombre", "ParametroID", parametro);
        }

        public void ParametroConMetodos(DropDownList ddl)
        {
            Parametro_BL ctParametro = new Parametro_BL();
            var parametro = ctParametro.ObtenerQueryable(q => q.ParametroMetodoes.Count > 0).OrderBy(p => p.Nombre);
            ddl.LoadDrop("Nombre", "ParametroID", parametro);
        }

        public void ParametroGrupo(DropDownList ddl, int NormaID, int CuerpoReceptorID, int UsoAguaID, int NivelID)
        {
            Parametro_BL ctParametro = new Parametro_BL();
            NormaConfiguracion_BL ctNormaConfiguracion = new NormaConfiguracion_BL();

            var parametro = ctParametro.ObtenerQueryable(q => q.ParametroMetodoes.Count > 0);
            /// Los ya agregados como grupo
            var parametroGrupo = ctNormaConfiguracion.ObtenerTodos().Where(q => q.NormaID == NormaID && q.CuerpoReceptorID == CuerpoReceptorID && q.UsoAguaID == UsoAguaID && q.NivelID == NivelID);

            /// Hay que obtener los parámetros que no han sido agregados como grupo ni como parámetro
            /// con la finalidad de no duplicar estos

            var resultado = from p in parametro
                            join q in parametroGrupo on p equals q.Parametro into grp
                                from r in grp.DefaultIfEmpty() where r == null
                            select p;

            ddl.LoadDrop("Nombre", "ParametroID", resultado);
        }

        public void Norma(DropDownList ddl)
        {
            Norma_BL norma_bl = new Norma_BL();
            var norma = norma_bl.ObtenerTodos().Where(x => x.Activo).OrderBy(p => p.Nombre);
            ddl.LoadDrop("Nombre", "NormaID", norma);
        }

        public void NormaCaptura(DropDownList ddl)
        {
            Norma_BL norma_bl = new Norma_BL();
            var norma = norma_bl.ObtenerTodos().Where(x => x.Activo && x.Norma2 == null && x.FuenteAbastecimiento != true).OrderBy(p => p.Nombre);
            ddl.LoadDrop("Nombre", "NormaID", norma);
        }
        public void NormaPredecesora(DropDownList ddl)
        {
            Norma_BL norma_bl = new Norma_BL();
            var norma = norma_bl.ObtenerTodos().Where(x => x.Activo && x.NormaPadreID == null).OrderBy(p => p.Nombre);
            ddl.LoadDrop("Nombre", "NormaID", norma);
        }
        public void NormaHijo(DropDownList ddl, int normaID)
        {
            Norma_BL norma_bl = new Norma_BL();
            var norma = norma_bl.ObtenerTodos().Where(x => x.Activo && x.NormaPadreID != null).OrderBy(p => p.Nombre);
            //var norma = norma_bl.ObtenerTodos().Where(x => x.Activo && x.NormaPadreID == normaID).OrderBy(p => p.Nombre);
            ddl.LoadDrop("Nombre", "NormaID", norma);
        }

        public void Laboratorio(DropDownList ddl)
        {
            Laboratorio_BL lab = new Laboratorio_BL();
            var laboratorio = lab.ObtenerLaboratorios().OrderBy(p => p.Nombre);
            ddl.LoadDrop("Nombre", "LaboratorioID", laboratorio);
        }
        public void CatTipoDocumentoAnexoSinSel(DropDownList ddl)
        {
            CatTipoDocumentoAnexo_BL cat_bl = new CatTipoDocumentoAnexo_BL();
            var cat = cat_bl.ObtenerTodos().Where(x => x.TipoDocumentoAnexoID != 3 && x.TipoDocumentoAnexoID != 4).OrderBy(p => p.Nombre);
            ddl.LoadDropSinSel("Nombre", "TipoDocumentoAnexoID", cat);
        }

        public void CatTipoDocumentoAnalisisMuestraSimple(DropDownList ddl)
        {
            CatTipoDocumentoMuestra_BL cat_bl = new CatTipoDocumentoMuestra_BL();
            var cat = cat_bl.ObtenerTodos().Where(a => a.TipoDocumentoMuestraID == 1).OrderBy(p => p.Nombre);
            ddl.LoadDropSinSel("Nombre", "TipoDocumentoMuestraID", cat);
        }
        public void CatTipoDocumentoMuestraSimpleSinSel(DropDownList ddl)
        {
            CatTipoDocumentoMuestra_BL cat_bl = new CatTipoDocumentoMuestra_BL();
            var cat = cat_bl.ObtenerTodos().Where(a => a.TipoDocumentoMuestraID != 1).OrderBy(p => p.Nombre);
            ddl.LoadDropSinSel("Nombre", "TipoDocumentoMuestraID", cat);
        }
        public void CatTipoDocumentoMuestraCompuestaSinSel(DropDownList ddl)
        {
            CatTipoDocumentoMuestra_BL cat_bl = new CatTipoDocumentoMuestra_BL();
            var cat = cat_bl.ObtenerTodos().Where(a => a.TipoDocumentoMuestraID == 3).OrderBy(p => p.Nombre);
            ddl.LoadDropSinSel("Nombre", "TipoDocumentoMuestraID", cat);
        }

        public void ListaLaboratorio(DropDownList ddl)
        {
            Analisis_BL analisis_bl = new Analisis_BL();
            var ListaLaboratorio = analisis_bl.BuscaLaboratorio().OrderBy(p => p.Nombre).OrderBy(p => p.Nombre);
            ddl.LoadDrop("Nombre", "LaboratorioId", ListaLaboratorio);
        }

        public void ListaMuestreador(DropDownList ddl, int LaboratorioID)
        {
            Cliente_BL cliente_bl = new Cliente_BL();
            var ListaPer = cliente_bl.BuscaMuestreador(LaboratorioID).OrderBy(p => p.Nombre).OrderBy(p => p.Nombre);
            ddl.LoadDrop("Nombre", "PersonaID", ListaPer);
        }

        public void GrupoConfiguracion(DropDownList ddl, int NormaID, int CuerpoReceptorID, int UsoAguaID, int NivelID)
        {
            NormaConfiguracion_BL ncBL = new NormaConfiguracion_BL();

            var ListadoGrupo = ncBL.Obtener_Grupos(NormaID, CuerpoReceptorID, UsoAguaID, NivelID);
            var lista = ListadoGrupo.OrderBy(p => p.Parametro.Nombre).Select(a => new { GrupoID = a.NormaConfiguracionID, Nombre = a.Parametro.Nombre });

            ddl.LoadDrop("Nombre", "GrupoID", lista);
        }

        public void OperacionConsolidado(DropDownList ddl)
        {
            OperacionConsolidado_BL gbl = new OperacionConsolidado_BL();
            var lista = gbl.Obtener().OrderBy(p => p.Nombre).OrderBy(p => p.Nombre);
            ddl.LoadDrop("Nombre", "OperacionID", lista);
        }

        public void Estatus(DropDownList ddl, enumEstatusTipo? estatusTipo = null, enumUsuarioRol? usuarioRol = null)
        {
            Estatus_BL ebl = new Estatus_BL();
            List<Estatu> lista = new List<Estatu>();
            if (estatusTipo == null)
                lista = ebl.Obtener().ToList();
            else
                lista = ebl.Obtener((enumEstatusTipo)estatusTipo).ToList();

            if (usuarioRol == enumUsuarioRol.Recaudacion)
            {
                var estatusCaptura = lista.FirstOrDefault(p => p.EstatusID == enumEstatusCancelacion.Capturada.ParseToInt());
                lista.Remove(estatusCaptura);
            }
            ddl.LoadDrop("Nombre", "EstatusID", lista);
        }
    }

    public static class DrowDownExtender
    {
        public static void LoadDrop<T>(this DropDownList drop, string TextField, string ValueField, IEnumerable<T> list)
        {
            drop.DataSource = null;
            drop.ClearSelection();

            if (drop.SelectedIndex == 0) drop.SelectedIndex = -1;

            drop.DataTextField = TextField;
            drop.DataValueField = ValueField;
            drop.DataSource = list.ToList();
            drop.DataBind();

           
            if (drop.Items.FindByValue("0") == null && drop.Items.FindByText("Seleccione") == null)
            {
                ListItem li = new ListItem { Value = "0", Text = "Seleccione", Selected = true };
                drop.Items.Insert(0, li);
            }
        }

        public static void LoadDropSinSel<T>(this DropDownList drop, string TextField, string ValueField, IEnumerable<T> list)
        {
            drop.DataSource = null;
            drop.ClearSelection();
            drop.DataTextField = TextField;
            drop.DataValueField = ValueField;
            drop.DataSource = list.ToList();
            drop.DataBind();

        }
    }
}
