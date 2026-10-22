using SIRALAB_BL.common;
using SIRALAB_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRALAB_BL.catalogos
{
    public class Norma_BL : Base_BL<Norma>
    {
        public Norma_BL() : base() { }
        
        public IEnumerable<Norma> ObtenerTodos()
        {
            return Repositorio.GetAll();
        }

        public List<Norma> ObtenerTodosFuenteAbastecimiento()
        {

            List<Norma> resultado = new List<Norma>();
            resultado = Repositorio.Find(a => a.FuenteAbastecimiento ?? false == true).ToList();
            return resultado;
        }

        public Norma Obtener(int normaID)
        {
            this.Entidad = Repositorio.Single(a => a.NormaID == normaID);
            return Entidad;
        }

        public Norma Guardar(SIRALAB_BL.common.NormaArgBase_BL argBase)
        {
            try
            {
                Norma normaTemp = new Norma();

                if (argBase.NormaID != null)
                {
                    normaTemp = Obtener((int)argBase.NormaID);
                    this.Entidad = new Norma();
                }
                else
                    this.Entidad = null;

                normaTemp.ClienteID = argBase.ClienteID;
                normaTemp.Nombre = argBase.Nombre;
                normaTemp.Descripcion = argBase.Descripcion;
                normaTemp.FechaInicio = argBase.FechaInicio;
                normaTemp.FechaFinal = argBase.FechaFinal;
                normaTemp.ExentaParametros = argBase.ExentaParametros;
                normaTemp.RequiereCaudal = argBase.RequiereCaudal;
                normaTemp.RequiereCompuesta = argBase.RequiereCompuesta;
                normaTemp.RequiereMensual = argBase.RequiereMensual;
                normaTemp.Evaluacion = argBase.Evaluacion;
                normaTemp.CPD = argBase.CPD;
                normaTemp.Activo = argBase.Activo;
                normaTemp.FuenteAbastecimiento = argBase.FuenteAbastecimiento;
                normaTemp.MuestrasMultiple = argBase.MuestrasMultiple;
                normaTemp.NormaPadreID = argBase.NormaPadreID == -1 || argBase.NormaPadreID == 0 ? null : argBase.NormaPadreID;
                normaTemp.Desplazamiento = argBase.Desplazamiento;
                normaTemp.RequiereHora = argBase.RequiereHora;
                normaTemp.RequiereObservaciones = argBase.RequiereObservaciones;
                //normaTemp.RequiereValorInstantaneo = argBase.RequiereValorInstantaneo;
                normaTemp.RequiereUsoDeAgua = argBase.RequiereUsoDeAgua;
                normaTemp.Nom2021 = argBase.Nom2021;
                if (this.Entidad != null) //Actualiza
                {
                    if (ObtenerTodos().Where(a => a.Nombre.ToUpper().Equals(argBase.Nombre.ToUpper())).Count() > 0 && normaTemp.NormaID != argBase.NormaID) //Verifica duplicidad
                    {
                        throw new Exception("No se puede guardar más de una norma con el mismo nombre. Intente con un nombre diferente.");
                    }
                    else
                    {
                        if (argBase.Activo == true && argBase.FechaFinal != null)
                            normaTemp.FechaFinal = null;

                        Repositorio.SaveChanges();
                        this.Mensaje = "Exito";
                        this.Entidad = normaTemp;
                    }
                }
                else  //Nuevo
                {
                    if (ObtenerTodos().Where(a => a.Nombre.ToUpper().Equals(argBase.Nombre.ToUpper())).Count() > 0) //Verifica duplicidad
                    {
                        throw new Exception("Ya existe una norma con el mismo nombre. Intente con un nombre diferente.");
                    }
                    else
                    {
                        normaTemp.NormaID = Repositorio.NextNewID(x => x.NormaID);
                        normaTemp.FechaFinal = null;
                        normaTemp.FechaRegistro = DateTime.Now;

                        Repositorio.Add(normaTemp);
                        Repositorio.SaveChanges();
                        this.Mensaje = "Exito";
                        this.Entidad = normaTemp;
                    }
                }
            }
            catch (Exception ex)
            {
                this.Mensaje = "Error" + ex.Message;
                this.Entidad = null;
            }

            return this.Entidad;
        }

        public bool Elimina(int normaID)
        {
            bool result = false;
            try
            {

                Obtener((int)normaID);

                if (this.Entidad != null)
                {
                    if (Entidad.Activo == false)
                    {
                        Entidad.Activo = true;
                        Entidad.FechaFinal = null;
                    }
                    else
                    {
                        Entidad.Activo = false;
                        Entidad.FechaFinal = DateTime.Now;
                    }

                    Repositorio.SaveChanges();
                    result = true;
                }
                else
                    result = false;

            }
            catch (Exception ex)
            {
                result = false;
                throw new Exception(ex.Message, ex.InnerException);
            }

            return result;
        }

        public List<Norma> Filtrar(common.NormaArgBase_BL filtro)
        {

            List<Norma> normaResp = this.Repositorio.GetAll().ToList();

            normaResp = normaResp.Where(p =>
                ((filtro.Nombre == null || filtro.Nombre == "") ? true : p.Nombre.Like(filtro.Nombre.FilterSQLData())) &&
                ((filtro.Descripcion == null || filtro.Descripcion == "") ? true : p.Descripcion.Like(filtro.Descripcion.FilterSQLData())) &&
                ((filtro.FechaInicio == (new DateTime())) ? true : p.FechaInicio == filtro.FechaInicio)
                ).ToList();

            return normaResp;
        }

        public Norma UpdateRequiereUsoAndNom2021(int idNorma)
        {
            try
            { 
                Entidad = Repositorio.First(x => x.NormaID == idNorma);
                Entidad.RequiereUsoDeAgua = true;
                Entidad.Nom2021 = false;
                Repositorio.SaveChanges();
                return Entidad;
            }
            catch (Exception ex)
            {

                return null;
            }

        }

        //public bool ObtieneDatosInscritosNom001(string NombreTitulo, string numeroAnexo, string rfcCliente)
        //{
        //    var resultado = false;
        //    var datos2021 = dbContext.tblProgramaCumplimientoNom001.Where(x => x.NombreTitulo == NombreTitulo && x.numeroAnexo== numeroAnexo && x.RFC==rfcCliente);
        //    if (datos2021.Any())
        //    {
        //        resultado = true;
        //    }
        //    return resultado;
        //}
        public bool ObtieneDatosInscritosNom001(string NombreTitulo, string numeroAnexo, string rfcCliente)
        {
            SqlConnection conn = null;
            var cadenaDeConexion = ConfigurationManager.ConnectionStrings["ConnectionNom001"].ConnectionString;
            conn = new SqlConnection(cadenaDeConexion);
            ApplicationSettings applicationSettings = new ApplicationSettings();
            var ipBaseNOM001 = applicationSettings.ipBaseNOM001.ToString();
            List<InscriosNom001> listaInscritos = new List<InscriosNom001>();
            var resultado = false;
            //List<ListaLaboratoriosBusqueda> listaLab = new List<ListaLaboratoriosBusqueda>();
            string queryString = " SELECT * from " + ipBaseNOM001 + ".[SGPCNOM001].dbo.vw_Nom001 WHERE " + "RFC = '" + rfcCliente + "'" + " AND PermisoDeDescarga='" + NombreTitulo + "'" + " AND NumeroDeAnexo='" + numeroAnexo + "'" + " AND Estatus='Vigente'";

            conn.Open();
            SqlCommand com = new SqlCommand(queryString, conn);

            SqlDataReader registros = com.ExecuteReader();

            while (registros.Read())
            {
                InscriosNom001 lista = new InscriosNom001
                {
                    RFC = registros["RFC"].ToString(),
                    NombreResponsable = registros["NombreResponsabledescarga"].ToString(),
                    Estatus = registros["Estatus"].ToString(),
                    NumAnexo = registros["NumeroDeAnexo"].ToString(),
                    Titulo = registros["PermisoDeDescarga"].ToString(),

                };
                listaInscritos.Add(lista);
            }
            if (listaInscritos.Count() > 0)
            {
                resultado = true;
            }
            conn.Close();


            return resultado;
        }
        public bool RequierePrograma()
        {
            var datos2021 = dbContext.MensajePCNOMs.First().Activo;
            return datos2021 ?? false;
        }
    }
}

public class InscriosNom001
{
    public string NombreResponsable { get; set; }
    public string Titulo { get; set; }
    public string NumAnexo { get; set; }
    public string Estatus { get; set; }
    public string RFC { get; set; }
}