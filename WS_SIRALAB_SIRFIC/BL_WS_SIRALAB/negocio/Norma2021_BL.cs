using DL_WS_SIRALAB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL_WS_SIRALAB.negocio
{
    public class Norma2021_BL
    {
        private Siralab_Entities Context { get; set; }

        private readonly string cadenaConexion = string.Empty;
        private readonly string ipBaseNom001 = string.Empty;
        public Norma2021_BL()
        {
            cadenaConexion = ConfigurationManager.ConnectionStrings["Nom001_Entities"].ConnectionString;
            Context = new Siralab_Entities();
            ipBaseNom001 = ConfigurationManager.AppSettings["ipBaseNOM001"].ToString();
        }

        public bool ObtenerDatosInscritosNom001(string Anexo, string Titulo, string rfcCliente, DateTime FechaMesDeclaracion)
        {

            int trimestreDeclaracion = (int)Math.Ceiling((double)FechaMesDeclaracion.Month / 3);// obtiene el trimestre de la declaracion


            using (SqlConnection connection = new SqlConnection(cadenaConexion))
            {
                List<InscriosNom001> listaInscritos = new List<InscriosNom001>();
                var resultado = false;
                //List<ListaLaboratoriosBusqueda> listaLab = new List<ListaLaboratoriosBusqueda>();
                string queryString = " SELECT * from "+ ipBaseNom001 + ".[SGPCNOM001].dbo.vw_Nom001 WHERE " + "RFC = '" + rfcCliente + "'" + " AND PermisoDeDescarga='" + Titulo + "'" + " AND NumeroDeAnexo='" + Anexo + "'";



                connection.Open();
                SqlCommand com = new SqlCommand(queryString, connection);

                SqlDataReader registros = com.ExecuteReader();

                while (registros.Read())
                {
                    DateTime? fechaSinEfecto = null;
                    var fechaSinEfectoString = registros["FechaSinEfecto"].ToString();
                    if (fechaSinEfectoString != string.Empty)
                    {
                        fechaSinEfecto = Convert.ToDateTime(registros["FechaSinEfecto"].ToString());
                    }
                    else
                    {
                        fechaSinEfecto = null;
                    }
                    InscriosNom001 lista = new InscriosNom001
                    {
                        RFC = registros["RFC"].ToString(),
                        NombreResponsable = registros["NombreResponsabledescarga"].ToString(),
                        Estatus = registros["Estatus"].ToString(),
                        NumAnexo = registros["NumeroDeAnexo"].ToString(),
                        Titulo = registros["PermisoDeDescarga"].ToString(),
                        Fecha_Periodo_Inicio = Convert.ToDateTime(registros["Fecha_Periodo_Inicio"].ToString()),
                        Fecha_Periodo_Final = Convert.ToDateTime(registros["Fecha_Periodo_Final"].ToString()),
                        FechaSinEfecto = fechaSinEfecto
                    };
                    listaInscritos.Add(lista);
                }
                if (listaInscritos.Count() > 0)
                {
                    var inscrito = listaInscritos.First();
                    var mesPeriodoInicio = 0;
                    var mesPeriodoFin = 0;
                    DateTime? fechaPeriodoInicio = null;
                    var anioPeriodoInicio = 0;
                    var anioPeriodoFin = 0;
                    DateTime? fechaPeriodoFin = null;
                    DateTime? fechaNoCumplio = null;
                    int trimestreFin = 0;



                    if (inscrito.Fecha_Periodo_Inicio != null)
                    {
                        mesPeriodoInicio = inscrito.Fecha_Periodo_Inicio.Value.Month;
                        anioPeriodoInicio = inscrito.Fecha_Periodo_Inicio.Value.Year;
                        fechaPeriodoInicio = Convert.ToDateTime("01/" + mesPeriodoInicio.ToString() + "/" + anioPeriodoInicio.ToString()); 
                    }
                    if (inscrito.Fecha_Periodo_Final != null)
                    {
                        mesPeriodoFin = inscrito.Fecha_Periodo_Final.Value.Month;
                        anioPeriodoFin = inscrito.Fecha_Periodo_Final.Value.Year;
                        trimestreFin = (int)Math.Ceiling((double)mesPeriodoFin / 3) * 3;// obtiene el ultimo mes del trimestre de la fecha del periodo final de vigencia
                        fechaPeriodoFin = Convert.ToDateTime(DateTime.DaysInMonth(anioPeriodoFin, trimestreFin).ToString() + "/" + trimestreFin.ToString() + "/" + anioPeriodoFin.ToString());
                       
                    }

                    if ((inscrito.FechaSinEfecto != null) && (inscrito.FechaSinEfecto < inscrito.Fecha_Periodo_Final))
                    {
                        resultado = false;
                    }
                    //valida que la fecha en el que la declaracion se hizo este dentro del rango de fechas de la nom001
                    else if ((fechaPeriodoInicio != null && FechaMesDeclaracion >= fechaPeriodoInicio) && (fechaPeriodoFin != null && FechaMesDeclaracion <= fechaPeriodoFin))
                    {
                        resultado = true;
                    }

                    

                }
                connection.Close();


                return resultado;

            }


        }

        public bool EstaSinEfectos(string Anexo, string Titulo, string rfcCliente)
        {

            using (SqlConnection connection = new SqlConnection(cadenaConexion))
            {
                List<InscriosNom001> listaInscritos = new List<InscriosNom001>();
                var resultado = false;
                //List<ListaLaboratoriosBusqueda> listaLab = new List<ListaLaboratoriosBusqueda>();
                string queryString = " SELECT * from " + ipBaseNom001 + ".[SGPCNOM001].dbo.vw_Nom001 WHERE " + "RFC = '" + rfcCliente + "'" + " AND PermisoDeDescarga='" + Titulo + "'" + " AND NumeroDeAnexo='" + Anexo + "'";

                connection.Open();
                SqlCommand com = new SqlCommand(queryString, connection);

                SqlDataReader registros = com.ExecuteReader();

                while (registros.Read())
                {
                    DateTime? fechaSinEfecto = null;
                    var fechaSinEfectoString = registros["FechaSinEfecto"].ToString();
                    if (fechaSinEfectoString != string.Empty)
                    {
                        fechaSinEfecto = Convert.ToDateTime(registros["FechaSinEfecto"].ToString());
                    }
                    else
                    {
                        fechaSinEfecto = null;
                    }
                    InscriosNom001 lista = new InscriosNom001
                    {
                        RFC = registros["RFC"].ToString(),
                        NombreResponsable = registros["NombreResponsabledescarga"].ToString(),
                        Estatus = registros["Estatus"].ToString(),
                        NumAnexo = registros["NumeroDeAnexo"].ToString(),
                        Titulo = registros["PermisoDeDescarga"].ToString(),
                        Fecha_Periodo_Inicio = Convert.ToDateTime(registros["Fecha_Periodo_Inicio"].ToString()),
                        Fecha_Periodo_Final = Convert.ToDateTime(registros["Fecha_Periodo_Final"].ToString()),
                        FechaSinEfecto = fechaSinEfecto
                    };
                    listaInscritos.Add(lista);
                }
                if (listaInscritos.Count() > 0)
                {
                    var inscrito = listaInscritos.First();

                    if (inscrito.FechaSinEfecto != null && inscrito.Estatus== "Sin efectos")
                    {
                        resultado = true;
                    }
                    //valida que la fecha en el que la declaracion se hizo este dentro del rango de fechas de la nom001
                    else
                    {
                        resultado = false;
                    }
                }
                connection.Close();


                return resultado;

            }


        }

        public int ObtenerNormaIDByNombre(string nombre)
        {
            try
            {
                var norma = Context.Normas.FirstOrDefault(x => x.Nombre == nombre);
                return norma.NormaID;
            }
            catch (Exception)
            {

                return 1;//norma actual que manda declaragua
            }

        }

        public int ObtenerNormaIDByNombreSSTDQOCOT(string nombre)
        {
            try
            {
                var normas = Context.Normas.ToList();
                var normaID = 3;
                foreach (var item in normas)
                {
                    item.Nombre = item.Nombre.Replace(" ", "");
                    if (item.Nombre == nombre.Replace(" ", ""))
                    {
                        normaID = item.NormaID;
                        break;
                    }
                }
                return normaID;
            }
            catch (Exception)
            {

                return 3;//norma actual que manda declaragua
            }

        }

        public Norma ObtenerNormaByID(int NormaID)
        {
            try
            {
                var norma1 = Context.Normas.FirstOrDefault(x => x.NormaID == NormaID);
                Norma norma = new Norma();
                norma.id = norma1.NormaID;
                norma.Nombre = norma1.Nombre;
                norma.Descripcion = norma1.Descripcion;

                return norma;
            }
            catch (Exception)
            {

                return null;
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
        public DateTime? Fecha_Periodo_Inicio { get; set; }
        public DateTime? Fecha_Periodo_Final { get; set; }
        public DateTime? FechaSinEfecto { get; set; }

    }

}
