using SIRALAB_BL.negocio.Comun;
using SIRALAB_BL.negocio.LaboratoriosApercibidos.Enum;
using SIRALAB_DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SIRALAB_BL.negocio.LaboratoriosApercibidos.LabApercibidosYReincidentes;


namespace SIRALAB_BL.negocio.LaboratoriosApercibidos
{
    public class LabApercibidosYReincidentesBLL
    {

        string fmt = "00";
        private readonly string cadenaConexion = string.Empty;
        public Siralab_Entities Context { get; private set; }

        public Laboratorios_Apercibidos LAbApercibido { get; private set; }

        private Repository<Laboratorios_Apercibidos> Repositorio;
        private Repository<tbl_Reincidencia> RepositorioReincidencia;
        private Repository<CatGerenteYSubgerente> RepositorioGerenteYSubgerente;
        private Repository<NomenclaturaMemorando> RepositorioNomenclaturaMemorando;
        private Repository<LaboratorioVigencia> RepositorioNumerosYVigencias;
        private Repository<tblImpugnacion> RepositorioImpugnacion;

        public LabApercibidosYReincidentesBLL()
        {
            Context = new Siralab_Entities();
            cadenaConexion = ConfigurationManager.ConnectionStrings["SiralabDbContext"].ConnectionString;
            Repositorio = new Repository<Laboratorios_Apercibidos>(Context);
            RepositorioGerenteYSubgerente = new Repository<CatGerenteYSubgerente>(Context);
            RepositorioNomenclaturaMemorando = new Repository<NomenclaturaMemorando>(Context);
            RepositorioReincidencia = new Repository<tbl_Reincidencia>(Context);
            RepositorioNumerosYVigencias = new Repository<LaboratorioVigencia>(Context);
            RepositorioImpugnacion = new Repository<tblImpugnacion>(Context);


        }

        public List<Laboratorios_Apercibidos> GetAllApercibidos()
        {
            try
            {
                var resultado = Repositorio.GetAll().ToList();
                return resultado;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public List<Laboratorios_Apercibidos> BusquedaApercibido(string rfc, string nombreLaboratorio, DateTime? fechaInicio, DateTime? fechaFin, string numeroAprobacion, string status)
        {
            rfc = string.IsNullOrEmpty(rfc) ? null : rfc.ToLower();
            nombreLaboratorio = string.IsNullOrEmpty(nombreLaboratorio) ? null : nombreLaboratorio.ToLower();
            var lista = Repositorio.FindQueryable(tabla => tabla.RFC.ToLower().Contains(rfc ?? tabla.RFC));

            if (nombreLaboratorio != null)
            {
                lista = lista.Where(tabla => tabla.Nombre.ToLower().Contains(nombreLaboratorio));
            }

            if (fechaInicio != null && fechaFin != null)
            {
                lista = lista.Where(tabla => tabla.Fecha_Resolucion >= fechaInicio && tabla.Fecha_Resolucion <= fechaFin);
            }

            if (status != null && status == "apercibido")
            {
                lista = lista.Where(tabla => tabla.Estatus == 1 || tabla.Estatus == 2);
            }

            lista = lista.Where(a => a.Activo == true);

            return lista.ToList();
        }

        public List<tbl_Reincidencia> BusquedaReincidencias(string rfc, string nombreLaboratorio, DateTime? fechaInicio, DateTime? fechaFin, string numeroAprobacion, string status)
        {
            rfc = string.IsNullOrEmpty(rfc) ? null : rfc.ToLower();
            nombreLaboratorio = string.IsNullOrEmpty(nombreLaboratorio) ? null : nombreLaboratorio.ToLower();
            var lista = RepositorioReincidencia.FindQueryable(tabla => tabla.RFC.ToLower().Contains(rfc ?? tabla.RFC));

            if (nombreLaboratorio != null)
            {
                lista = lista.Where(tabla => tabla.Nombre.ToLower().Contains(nombreLaboratorio));
            }

            if (fechaInicio != null && fechaFin != null)
            {
                lista = lista.Where(tabla => tabla.Fecha_Resolucion >= fechaInicio && tabla.Fecha_Resolucion <= fechaFin);
            }
            if (status != null && status == "reincidente")
            {
                lista = lista.Where(tabla => tabla.Estatus == 3 || tabla.Estatus == 4);
            }
            lista = lista.Where(a => a.Activo == true);

            return lista.ToList();
        }



        public List<Laboratorio> ObtenerLaboratorios()
        {
            try
            {
                var listaanalisis = Context.Laboratorios.ToList();

                return listaanalisis;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public List<ListaLaboratoriosBusqueda> ObtenerListaLab(string rfc, string nombre)
        {
            using (SqlConnection connection = new SqlConnection(cadenaConexion))
            {
                List<ListaLaboratoriosBusqueda> listaLab = new List<ListaLaboratoriosBusqueda>();
                string queryString = " SELECT LaboratorioID AS idLaboratorio, Nombre AS NombreLaboratorio, RFC AS RFCLaboratorio from Laboratorio WHERE Activo IS NOT NULL ";

                if (rfc != string.Empty)
                {
                    queryString += " AND RFC = '" + rfc + "'";
                }
                if (nombre != string.Empty)
                {
                    queryString += " AND Nombre LIKE '%" + nombre + "%'";
                }
                queryString += " ORDER BY 2 DESC";
                connection.Open();
                SqlCommand com = new SqlCommand(queryString, connection);

                SqlDataReader registros = com.ExecuteReader();
                while (registros.Read())
                {
                    ListaLaboratoriosBusqueda lista = new ListaLaboratoriosBusqueda
                    {
                        idLaboratorio = int.Parse(registros["idLaboratorio"].ToString()),
                        NombreLaboratorio = registros["NombreLaboratorio"].ToString(),
                        RFCLaboratorio = registros["RFCLaboratorio"].ToString()

                    };
                    listaLab.Add(lista);
                }
                connection.Close();
                return listaLab;

            }

        }



        public string GuardarApercibimiento(int idLaboratorio, string NombreLaboratorio, string RFC, string idIC2, string numOficio, string fechaOficioRes, string fechaNotificacion, int trimestre, int anio, string observaciones, string iniciales, string noExpediente, string nombreCompleto)
        {
            try
            {
                var complementoNomenclatura = "PAA.";
                var nomenclatura = Nomenclatura(complementoNomenclatura);
                Laboratorios_Apercibidos NuevoApercibimiento = new Laboratorios_Apercibidos();
                NuevoApercibimiento.Id_Laboratorio = idLaboratorio;
                NuevoApercibimiento.Nombre = NombreLaboratorio;
                NuevoApercibimiento.RFC = RFC;
                NuevoApercibimiento.Evidencia = idIC2;
                NuevoApercibimiento.Numero_Oficio = numOficio;
                NuevoApercibimiento.Fecha_Resolucion = Convert.ToDateTime(fechaOficioRes);
                NuevoApercibimiento.Fecha_Notificacion = Convert.ToDateTime(fechaNotificacion);
                NuevoApercibimiento.Trimestre = trimestre;
                NuevoApercibimiento.Año = anio;
                NuevoApercibimiento.Observaciones = observaciones;
                NuevoApercibimiento.Fecha_captura = DateTime.Now;
                NuevoApercibimiento.Activo = true;
                NuevoApercibimiento.Estatus = (int)EnumEstatusApercibidosReincidentes.Apercibido;
                NuevoApercibimiento.NoExpediente = noExpediente;
                NuevoApercibimiento.InicialesResponsable = iniciales;
                NuevoApercibimiento.Memorando = nomenclatura;
                NuevoApercibimiento.NombreResponsable = nombreCompleto;
                //falta memorando
                var mesTrimestre = 0;
                switch (trimestre)
                {
                    case 1:
                        mesTrimestre = 01;
                        break;

                    case 2:
                        mesTrimestre = 04;
                        break;
                    case 3:
                        mesTrimestre = 07;
                        break;
                    case 4:
                        mesTrimestre = 10;
                        break;
                }
                DateTime FechaInicioVig = new DateTime(anio, mesTrimestre, 1);
                var fechaFinVig = FechaInicioVig.AddYears(5);
                fechaFinVig = fechaFinVig.AddMonths(3);
                fechaFinVig = fechaFinVig.AddDays(-1);

                NuevoApercibimiento.Fecha_Vigencia = fechaFinVig;
                Repositorio.Add(NuevoApercibimiento);
                Repositorio.SaveChanges();

                return NuevoApercibimiento.Id.ToString();
            }
            catch (Exception ex)
            {

                return "0";
            }

        }

        public string ActualizaApercibimiento(int idApercibimiento, int idLaboratorio, string NombreLaboratorio, string RFC, string idIC2, string numOficio, string fechaOficioRes, string fechaNotificacion, int trimestre, int anio, string observaciones, string iniciales, string noExpediente, string nombreCompleto)
        {
            try
            {
                var Apercibimiento = BuscarApercibimientoByID(idApercibimiento);





                Apercibimiento.Evidencia = idIC2;
                Apercibimiento.Numero_Oficio = numOficio;
                Apercibimiento.Fecha_Resolucion = Convert.ToDateTime(fechaOficioRes);
                Apercibimiento.Fecha_Notificacion = Convert.ToDateTime(fechaNotificacion);
                Apercibimiento.Trimestre = trimestre;
                Apercibimiento.Año = anio;
                Apercibimiento.Observaciones = observaciones;
                Apercibimiento.NoExpediente = noExpediente;
                Apercibimiento.InicialesResponsable = iniciales;
                Apercibimiento.NombreResponsable = nombreCompleto;

                var mesTrimestre = 0;
                switch (trimestre)
                {
                    case 1:
                        mesTrimestre = 01;
                        break;

                    case 2:
                        mesTrimestre = 04;
                        break;
                    case 3:
                        mesTrimestre = 07;
                        break;
                    case 4:
                        mesTrimestre = 10;
                        break;
                }
                DateTime FechaInicioVig = new DateTime(anio, mesTrimestre, 1);
                var fechaFinVig = FechaInicioVig.AddYears(5);
                fechaFinVig = fechaFinVig.AddMonths(3);
                fechaFinVig = fechaFinVig.AddDays(-1);

                Apercibimiento.Fecha_Vigencia = fechaFinVig;

                Repositorio.SaveChanges();

                return Apercibimiento.Id.ToString();
            }
            catch (Exception ex)
            {

                return "0";
            }

        }

        public Laboratorios_Apercibidos BuscarApercibimientoByID(int id)
        {
            try
            {
                var Apercibimiento = Repositorio.Find(x => x.Id == id).FirstOrDefault();
                return Apercibimiento;
            }
            catch (Exception ex)
            {

                return null;
            }

        }
        public CatGerenteYSubgerente BuscarGerenteYSubgerente()
        {
            try
            {
                var busqueda = RepositorioGerenteYSubgerente.GetAll().FirstOrDefault();
                return busqueda;
            }
            catch (Exception ex)
            {

                return null;
            }

        }



        public bool laboratorioRepetido(int idLaboratorio)
        {
            try
            {
                var laboratorio = Repositorio.First(x => x.Id_Laboratorio == idLaboratorio && x.Activo == true);
                if (laboratorio != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public bool laboratorioEnReincidenciaRepetido(int idLaboratorio)
        {
            try
            {
                var laboratorio = RepositorioReincidencia.First(x => x.Id_Laboratorio == idLaboratorio && x.Activo == true);
                if (laboratorio != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public List<LaboratorioVigencia> ObtenerNumerosYVigenciasDeAprobacion(int idLaboratorio)
        {
            var busqueda = RepositorioNumerosYVigencias.Find(x => x.LaboratorioID == idLaboratorio && x.Activo == true).ToList();
            return busqueda;
        }

        public bool desactivaApercibimiento(int idApercibimiento)
        {
            try
            {
                var Actializo = Repositorio.Find(x => x.Id == idApercibimiento && x.Activo == true).FirstOrDefault();
                Actializo.Activo = false;
                Repositorio.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;

            }


        }
        public bool ActivaApercibimiento(int idApercibimiento)
        {
            try
            {
                var Actializo = Repositorio.Find(x => x.Id == idApercibimiento).FirstOrDefault();
                Actializo.Activo = true;
                Repositorio.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;

            }


        }
        #region Reincidencias
        public string GuardarReincidencia(int idApercibimiento, int idLaboratorio, string NombreLaboratorio, string RFC, string idIC2, string numOficio, string fechaOficioRes, string fechaNotificacion, string observaciones, string iniciales, string noExpediente, string nombreCompleto, int trimestre, int anio)
        {
            try
            {
                var ListaLaboratorioVigencia = ObtenerNumerosYVigenciasDeAprobacion(idLaboratorio);
                var LabVigencia = ListaLaboratorioVigencia.SingleOrDefault();
                var complementoNomenclatura = "PAR.";
                var nomenclatura = Nomenclatura(complementoNomenclatura);
                tbl_Reincidencia NuevaReincidencia = new tbl_Reincidencia();
                NuevaReincidencia.idApercibimiento = idApercibimiento;//obtener el id del apercibimiento seleccionado 
                NuevaReincidencia.Id_Laboratorio = idLaboratorio;
                NuevaReincidencia.Nombre = NombreLaboratorio;
                NuevaReincidencia.RFC = RFC;
                NuevaReincidencia.Evidencia = idIC2;
                NuevaReincidencia.Numero_Oficio = numOficio;
                NuevaReincidencia.Fecha_Resolucion = Convert.ToDateTime(fechaOficioRes);
                NuevaReincidencia.Fecha_Notificacion = Convert.ToDateTime(fechaNotificacion);

                NuevaReincidencia.Observaciones = observaciones;
                NuevaReincidencia.Fecha_captura = DateTime.Now;
                NuevaReincidencia.Activo = true;
                NuevaReincidencia.Estatus = (int)EnumEstatusApercibidosReincidentes.Reincidencia;
                NuevaReincidencia.NoExpediente = noExpediente;
                NuevaReincidencia.InicialesResponsable = iniciales;
                NuevaReincidencia.Memorando = nomenclatura;
                NuevaReincidencia.NombreResponsable = nombreCompleto;
                if (LabVigencia != null)
                {
                    if (LabVigencia.FechaFin != null)
                    {
                        if (LabVigencia.FechaFin.Value != null)
                        {
                            NuevaReincidencia.Fecha_Vigencia = LabVigencia.FechaFin.Value;
                        }
                        else
                        {
                            NuevaReincidencia.Fecha_Vigencia = null;
                        }
                    }
                    else
                    {
                        NuevaReincidencia.Fecha_Vigencia = null;
                    }
                }
                else
                {
                    NuevaReincidencia.Fecha_Vigencia = null;
                }

                NuevaReincidencia.Trimestre = trimestre;
                NuevaReincidencia.Año = anio;
                RepositorioReincidencia.Add(NuevaReincidencia);
                RepositorioReincidencia.SaveChanges();

                bool Actializo = desactivaApercibimiento(NuevaReincidencia.idApercibimiento.Value);
                //var ApercibimientoImpugnado = ObtenerImpugnacionByIdApercibimiento(NuevaReincidencia.idApercibimiento.Value);
                //if (ApercibimientoImpugnado != null)
                //{
                //    bool DesactivaImpugnacion = desactivaImpugnacion(NuevaReincidencia.idApercibimiento.Value);
                //}

                return NuevaReincidencia.Id.ToString();
            }
            catch (Exception ex)
            {

                return "0";
            }

        }

        public string ActualizarReincidencia(int idReincidencia, int idLaboratorio, string NombreLaboratorio, string RFC, string idIC2, string numOficio, string fechaOficioRes, string fechaNotificacion, string observaciones, string iniciales, string noExpediente, string nombreCompleto, int trimestre, int anio)
        {
            try
            {
                var ListaLaboratorioVigencia = ObtenerNumerosYVigenciasDeAprobacion(idLaboratorio);
                var LabVigencia = ListaLaboratorioVigencia.SingleOrDefault();
                var Reincidencia = BuscarReincidenciaByID(idReincidencia);



                Reincidencia.Evidencia = idIC2;
                Reincidencia.Numero_Oficio = numOficio;
                Reincidencia.Fecha_Resolucion = Convert.ToDateTime(fechaOficioRes);
                Reincidencia.Fecha_Notificacion = Convert.ToDateTime(fechaNotificacion);
                Reincidencia.Observaciones = observaciones;
                Reincidencia.NoExpediente = noExpediente;
                Reincidencia.InicialesResponsable = iniciales;
                Reincidencia.NombreResponsable = nombreCompleto;
                Reincidencia.Trimestre = trimestre;
                Reincidencia.Año = anio;

                if (LabVigencia != null)
                {
                    if (LabVigencia.FechaFin != null)
                    {
                        if (LabVigencia.FechaFin.Value != null)
                        {
                            Reincidencia.Fecha_Vigencia = LabVigencia.FechaFin.Value;
                        }
                        else
                        {
                            Reincidencia.Fecha_Vigencia = null;
                        }
                    }
                    else
                    {
                        Reincidencia.Fecha_Vigencia = null;
                    }
                }
                else
                {
                    Reincidencia.Fecha_Vigencia = null;
                }

                RepositorioReincidencia.SaveChanges();



                return Reincidencia.Id.ToString();
            }
            catch (Exception ex)
            {

                return "0";
            }

        }



        public tbl_Reincidencia BuscarReincidenciaByID(int id)
        {
            try
            {
                var Reincidencia = RepositorioReincidencia.Find(x => x.Id == id).FirstOrDefault();
                return Reincidencia;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public tbl_Reincidencia BuscarReincidenciaByIdApercibimiento(int idApercibimiento)
        {
            try
            {
                var Reincidencia = RepositorioReincidencia.Find(x => x.idApercibimiento == idApercibimiento && x.Activo == true).FirstOrDefault();
                return Reincidencia;
            }
            catch (Exception ex)
            {

                return null;
            }

        }

        public bool desactivaReincidencia(int idReincidencia)
        {
            try
            {
                var Actializo = RepositorioReincidencia.Find(x => x.Id == idReincidencia && x.Activo == true).FirstOrDefault();
                Actializo.Activo = false;
                RepositorioReincidencia.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;

            }


        }

        public List<tbl_Reincidencia> GetAllReincidencias()
        {
            try
            {
                var resultado = RepositorioReincidencia.GetAll().ToList();
                return resultado;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        #endregion
        public string Nomenclatura(string complementoNomenclatura)
        {
            try
            {
                var folioEncontrado = RepositorioNomenclaturaMemorando.GetAll().FirstOrDefault();
                folioEncontrado.consecutivo = (short)(folioEncontrado.consecutivo + 1);
                RepositorioNomenclaturaMemorando.SaveChanges();

               
                var resultado = string.Format("{0}{1}{2}{3}",
                              folioEncontrado.prefijo.Trim(), //0
                              folioEncontrado.determinante, //1
                              complementoNomenclatura,//2
                              folioEncontrado.consecutivo.ToString(fmt)//3
                              );
                              //DateTime.Now.Month.ToString("d2"),//3
                              //DateTime.Now.Year); //4
                return resultado;
            }
            catch (Exception ex)
            {

                return string.Empty;
            }
        }



        #region Medios de impugnación 
        public List<CatMediosDeImpugnacion> ObtenerMediosDeImpugnacion()
        {
            try
            {
                var medios = Context.CatMediosDeImpugnacion.ToList();
                return medios;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public List<tblEstatusImpugnacion> ObtenerEstatusImpugnacion()
        {
            try
            {

                var estatus = Context.tblEstatusImpugnacion.ToList();
                return estatus;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public bool ExisteImpugnacionByIdApercibimiento(int idApercibimiento)
        {
            try
            {
                bool resultado = false;
                var Impugnacion = Context.tblImpugnacion.Where(x => x.IdApercibimiento == idApercibimiento && x.Activo == true).ToList();
                if (Impugnacion.Count > 0)
                {
                    resultado = true;
                }
                else
                {
                    resultado = false;
                }
                return resultado;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        public bool ExisteImpugnacionByIdReincidencia(int IdReincidencia)
        {
            try
            {
                bool resultado = false;
                var Impugnacion = Context.tblImpugnacion.Where(x => x.IdReincidencia == IdReincidencia && x.Activo == true).ToList();
                if (Impugnacion.Count > 0)
                {
                    resultado = true;
                }
                else
                {
                    resultado = false;
                }
                return resultado;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public List<tblImpugnacion> ObtenerImpugnacionesByIdApercibimiento(int idApercibimiento)
        {
            try
            {
                var Impugnacion = Context.tblImpugnacion.Where(x => x.IdApercibimiento == idApercibimiento && x.Activo == true).ToList();

                return Impugnacion;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public List<tblImpugnacion> ObtenerImpugnacionesApercibimientoHistoricoById(int Id)
        {
            try
            {
                var Impugnacion = Context.tblImpugnacion.Where(x => x.Id == Id).ToList();

                return Impugnacion;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public List<tblImpugnacion> ObtenerImpugnacionesByIdReincidencia(int IdReincidencia)
        {
            try
            {
                var Impugnacion = Context.tblImpugnacion.Where(x => x.IdReincidencia == IdReincidencia && x.Activo == true).ToList();

                return Impugnacion;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public List<tblImpugnacion> ObtenerImpugnacionesReincidenciaHistoricoById(int Id)
        {
            try
            {
                var Impugnacion = Context.tblImpugnacion.Where(x => x.Id == Id).ToList();

                return Impugnacion;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public List<tblImpugnacion> getAllImpugnacionesByIdReincidencia(int IdReincidencia)
        {
            try
            {
                var Impugnacion = Context.tblImpugnacion.Where(x => x.IdReincidencia == IdReincidencia).ToList();

                return Impugnacion;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public List<tblImpugnacion> getAllImpugnacionesByIdApercibimiento(int idApercibimiento)
        {
            try
            {
                var Impugnacion = Context.tblImpugnacion.Where(x => x.IdApercibimiento == idApercibimiento).ToList();

                return Impugnacion;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public tblImpugnacion ObtenerImpugnacionByIdApercibimiento(int idApercibimiento)
        {
            try
            {
                var Impugnacion = Context.tblImpugnacion.Where(x => x.IdApercibimiento == idApercibimiento && x.Activo == true).First();

                return Impugnacion;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public tblImpugnacion ObtenerImpugnacionByIdReincidencia(int IdReincidencia)
        {
            try
            {
                var Impugnacion = Context.tblImpugnacion.Where(x => x.IdReincidencia == IdReincidencia && x.Activo == true).First();

                return Impugnacion;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public tblImpugnacion ObtenerImpugnacionById(int IdImpugnacion)
        {
            try
            {
                var Impugnacion = Context.tblImpugnacion.Where(x => x.Id == IdImpugnacion && x.Activo == true).First();

                return Impugnacion;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public bool GuardarImpugnacion(int MedioImpugnacion, string FechaImpugnacion, string AnteQuien, int Estatus, string Observaciones, int idApercibimiento, int idReincidencia, int idLaboratorio, string NombreUsuario, string FechaResolucion, string Expediente)
        {
            try
            {

                tblImpugnacion NuevaImpugnacion = new tblImpugnacion();
                NuevaImpugnacion.IdMediosDeImpugnacion = MedioImpugnacion;
                NuevaImpugnacion.Estatus = Estatus;
                NuevaImpugnacion.AnteQuien = AnteQuien;
                NuevaImpugnacion.Observaciones = Observaciones;
                NuevaImpugnacion.FechaImpugnacion = Convert.ToDateTime(FechaImpugnacion);
                NuevaImpugnacion.FechaCaptura = DateTime.Now;
                if (Estatus != (int)EnumEstatusImpugnacion.EnProceso)
                {
                    NuevaImpugnacion.FechaResolucion = Convert.ToDateTime(FechaResolucion);
                }
                else
                {
                    NuevaImpugnacion.FechaResolucion = null;
                }


                NuevaImpugnacion.IdLaboratorio = idLaboratorio;
                if (idApercibimiento != 0)
                {
                    NuevaImpugnacion.IdApercibimiento = idApercibimiento;

                    if (Estatus == (int)EnumEstatusImpugnacion.EnProceso)
                    {
                        NuevaImpugnacion.Activo = true;
                        cambiarEstatusApercibimientoById(idApercibimiento, (int)EnumEstatusApercibidosReincidentes.ApercibidoConImpugnacion);
                    }
                    else
                    {

                        if (Estatus == (int)EnumEstatusImpugnacion.EnContraDeConagua)
                        {
                            cambiarEstatusApercibimientoById(idApercibimiento, (int)EnumEstatusApercibidosReincidentes.Apercibido);
                            desactivaApercibimiento(idApercibimiento);
                        }
                        else if (Estatus == (int)EnumEstatusImpugnacion.AFavorDeConagua)
                        {
                            cambiarEstatusApercibimientoById(idApercibimiento, (int)EnumEstatusApercibidosReincidentes.Apercibido);
                        }
                        else if (Estatus == (int)EnumEstatusImpugnacion.ParaEfectos)
                        {
                            cambiarEstatusApercibimientoById(idApercibimiento, (int)EnumEstatusApercibidosReincidentes.Apercibido);
                            desactivaApercibimiento(idApercibimiento);
                        }
                        NuevaImpugnacion.Activo = false;

                    }
                }
                else
                {
                    NuevaImpugnacion.IdApercibimiento = null;
                }

                if (idReincidencia != 0)
                {
                    NuevaImpugnacion.IdReincidencia = idReincidencia;

                    if (Estatus == (int)EnumEstatusImpugnacion.EnProceso)
                    {
                        NuevaImpugnacion.Activo = true;
                        cambiarEstatusReincidenciaById(idReincidencia, (int)EnumEstatusApercibidosReincidentes.ReincidenciaConImpugnacion);
                    }
                    else
                    {

                        if (Estatus == (int)EnumEstatusImpugnacion.EnContraDeConagua)
                        {
                            cambiarEstatusReincidenciaById(idReincidencia, (int)EnumEstatusApercibidosReincidentes.Reincidencia);
                            desactivaReincidencia(idReincidencia);
                        }
                        else if (Estatus == (int)EnumEstatusImpugnacion.AFavorDeConagua)
                        {
                            cambiarEstatusReincidenciaById(idReincidencia, (int)EnumEstatusApercibidosReincidentes.Reincidencia);
                        }
                        else if (Estatus == (int)EnumEstatusImpugnacion.ParaEfectos)
                        {
                            //Desactiva la reincidencia y se vuelve a activar el apercibimiento
                            cambiarEstatusReincidenciaById(idReincidencia, (int)EnumEstatusApercibidosReincidentes.Reincidencia);
                            desactivaReincidencia(idReincidencia);
                            var idApercibimientoActualiza = BuscarReincidenciaByID(idReincidencia).idApercibimiento;
                            ActivaApercibimiento(idApercibimientoActualiza.Value);
                        }
                        NuevaImpugnacion.Activo = false;

                    }
                }
                else
                {
                    NuevaImpugnacion.IdReincidencia = null;
                }



                NuevaImpugnacion.NombreResponsable = NombreUsuario;


                NuevaImpugnacion.Expediente = Expediente;

                Context.tblImpugnacion.Add(NuevaImpugnacion);
                Context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }

        }


        public bool ActualizarImpugnacion(int MedioImpugnacion, string FechaImpugnacion, string AnteQuien, int Estatus, string Observaciones, int idImpugnacion, string FechaResolucion, string Expediente)
        {
            try
            {
                var ActualizaImpugnacion = ObtenerImpugnacionById(idImpugnacion);

                ActualizaImpugnacion.IdMediosDeImpugnacion = MedioImpugnacion;
                ActualizaImpugnacion.Estatus = Estatus;
                ActualizaImpugnacion.AnteQuien = AnteQuien;
                ActualizaImpugnacion.Observaciones = Observaciones;
                ActualizaImpugnacion.FechaImpugnacion = Convert.ToDateTime(FechaImpugnacion);

                if (Estatus != (int)EnumEstatusImpugnacion.EnProceso)
                {
                    ActualizaImpugnacion.FechaResolucion = Convert.ToDateTime(FechaResolucion);
                }
                else
                {
                    ActualizaImpugnacion.FechaResolucion = null;
                }


                if (ActualizaImpugnacion.IdApercibimiento != null)
                {

                    if (Estatus == (int)EnumEstatusImpugnacion.EnProceso)
                    {
                        ActualizaImpugnacion.Activo = true;
                        //cambiarEstatusApercibimientoById(ActualizaImpugnacion.IdApercibimiento.Value, (int)EnumEstatusApercibidosReincidentes.ApercibidoConImpugnacion);
                    }
                    else
                    {

                        if (Estatus == (int)EnumEstatusImpugnacion.EnContraDeConagua)
                        {
                            cambiarEstatusApercibimientoById(ActualizaImpugnacion.IdApercibimiento.Value, (int)EnumEstatusApercibidosReincidentes.Apercibido);
                            desactivaApercibimiento(ActualizaImpugnacion.IdApercibimiento.Value);
                        }
                        else if (Estatus == (int)EnumEstatusImpugnacion.AFavorDeConagua)
                        {
                            cambiarEstatusApercibimientoById(ActualizaImpugnacion.IdApercibimiento.Value, (int)EnumEstatusApercibidosReincidentes.Apercibido);
                        }
                        else if (Estatus == (int)EnumEstatusImpugnacion.ParaEfectos)
                        {
                            cambiarEstatusApercibimientoById(ActualizaImpugnacion.IdApercibimiento.Value, (int)EnumEstatusApercibidosReincidentes.Apercibido);
                            desactivaApercibimiento(ActualizaImpugnacion.IdApercibimiento.Value);
                        }
                        ActualizaImpugnacion.Activo = false;

                    }
                }


                if (ActualizaImpugnacion.IdReincidencia != null)
                {


                    if (Estatus == (int)EnumEstatusImpugnacion.EnProceso)
                    {
                        ActualizaImpugnacion.Activo = true;
                        //cambiarEstatusReincidenciaById(ActualizaImpugnacion.IdReincidencia.Value, (int)EnumEstatusApercibidosReincidentes.ReincidenciaConImpugnacion);
                    }
                    else
                    {

                        if (Estatus == (int)EnumEstatusImpugnacion.EnContraDeConagua)
                        {
                            cambiarEstatusReincidenciaById(ActualizaImpugnacion.IdReincidencia.Value, (int)EnumEstatusApercibidosReincidentes.Reincidencia);
                            desactivaReincidencia(ActualizaImpugnacion.IdReincidencia.Value);
                        }
                        else if (Estatus == (int)EnumEstatusImpugnacion.AFavorDeConagua)
                        {
                            cambiarEstatusReincidenciaById(ActualizaImpugnacion.IdReincidencia.Value, (int)EnumEstatusApercibidosReincidentes.Reincidencia);
                        }
                        else if (Estatus == (int)EnumEstatusImpugnacion.ParaEfectos)
                        {
                            cambiarEstatusReincidenciaById(ActualizaImpugnacion.IdReincidencia.Value, (int)EnumEstatusApercibidosReincidentes.Reincidencia);
                            desactivaReincidencia(ActualizaImpugnacion.IdReincidencia.Value);
                            var idApercibimientoActualiza = BuscarReincidenciaByID(ActualizaImpugnacion.IdReincidencia.Value).idApercibimiento;
                            ActivaApercibimiento(idApercibimientoActualiza.Value);
                        }
                        ActualizaImpugnacion.Activo = false;

                    }
                }




                ActualizaImpugnacion.Expediente = Expediente;
                Context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }

        }
        public void cambiarEstatusApercibimientoById(int idApercibimiento, int estatus)
        {
            try
            {
                var apercibimiento = BuscarApercibimientoByID(idApercibimiento);

                apercibimiento.Estatus = estatus;
                Context.SaveChanges();
            }
            catch (Exception ex)
            {


            }
        }
        public void cambiarEstatusReincidenciaById(int idReincidencia, int estatus)
        {
            try
            {
                var Reincidencia = BuscarReincidenciaByID(idReincidencia);

                Reincidencia.Estatus = estatus;
                Context.SaveChanges();
            }
            catch (Exception ex)
            {


            }
        }
        public bool desactivaImpugnacion(int idApercibimiento)
        {
            try
            {
                var Impugnacion = Context.tblImpugnacion.Where(x => x.IdApercibimiento == idApercibimiento && x.Activo == true).First();
                Impugnacion.Activo = false;
                Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool desactivaImpugnacionById(int ID)
        {
            try
            {
                var Impugnacion = Context.tblImpugnacion.Where(x => x.Id == ID).First();
                Impugnacion.Activo = false;
                Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<tblImpugnacion> ObtenerListaImpugnacionReincidencia(int idReincidencia)
        {
            var busqueda = RepositorioImpugnacion.Find(x => x.IdReincidencia == idReincidencia).ToList();
            return busqueda;
        }


        public List<usp_BuscaImpugnacionByReincidencia_Result> ObtenerListaImpugnacionReincidencia2(int idReincidencia)
        {
            using (SqlConnection connection = new SqlConnection(cadenaConexion))
            {

                connection.Open();
                var cmd = new SqlCommand("usp_BuscaImpugnacionByReincidencia", connection) { CommandType = CommandType.StoredProcedure };
                cmd.CommandTimeout = 300;
                cmd.Parameters.Add(new SqlParameter("@ID_REINCIDENCIA", idReincidencia));

                var lista = new List<usp_BuscaImpugnacionByReincidencia_Result>();

                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        var datos = new usp_BuscaImpugnacionByReincidencia_Result();
                        datos.MediosImpungnacion = rdr["MediosImpungnacion"].ToString();
                        datos.Estatus = rdr["Estatus"].ToString();
                        datos.Expediente = rdr["Expediente"].ToString();
                        datos.AnteQuien = rdr["AnteQuien"].ToString();
                        datos.FechaImpugnacion = rdr["FechaImpugnacion"].ToString();
                        datos.FechaResolucion = rdr["FechaResolucion"].ToString();
                        datos.Observaciones = rdr["Observaciones"].ToString();
                        datos.IdImpugnacion = (int)rdr["IdImpugnacion"];
                        lista.Add(datos);

                    }
                }
                return lista;

            }
       
        }

        public List<usp_BuscaImpugnacionByApercibimiento_Result> ObtenerListaImpugnacionByIdImpugnacion(int idApercibimiento)
        {
            using (SqlConnection connection = new SqlConnection(cadenaConexion))
            {

                connection.Open();
                var cmd = new SqlCommand("usp_BuscaImpugnacionByApercibimiento", connection) { CommandType = CommandType.StoredProcedure };
                cmd.CommandTimeout = 300;
                cmd.Parameters.Add(new SqlParameter("@ID_APERCIBIMIENTO", idApercibimiento));

                var lista = new List<usp_BuscaImpugnacionByApercibimiento_Result>();

                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        var datos = new usp_BuscaImpugnacionByApercibimiento_Result();
                        datos.MediosImpungnacion = rdr["MediosImpungnacion"].ToString();
                        datos.Estatus = rdr["Estatus"].ToString();
                        datos.Expediente = rdr["Expediente"].ToString();
                        datos.AnteQuien = rdr["AnteQuien"].ToString();
                        datos.FechaImpugnacion = rdr["FechaImpugnacion"].ToString();
                        datos.FechaResolucion = rdr["FechaResolucion"].ToString();
                        datos.Observaciones = rdr["Observaciones"].ToString();
                        datos.IdImpugnacion = (int)rdr["IdImpugnacion"];
                        lista.Add(datos);

                    }
                }
                return lista;

            }

        }

        public bool TieneReincidenciaByIdLaboratorio(int idLaboratorio)
        {
            DiaInhabil_BL diaBLL = new DiaInhabil_BL();
            bool resultado = false;
            var Reincidencia = Context.tbl_Reincidencia.FirstOrDefault(x => x.Id_Laboratorio == idLaboratorio && x.Activo == true && x.Estatus == 3);
            if (Reincidencia != null)
            {
                var diasHabiles = int.Parse(ConfigurationManager.AppSettings["DiasHabilesReincidencia"]);
                diasHabiles = diasHabiles + 1;
                var fechaLimite = diaBLL.SiguienteDiaHabil(Reincidencia.Fecha_Notificacion.Value, diasHabiles);
                //
                if (DateTime.Now >= fechaLimite)
                {
                    resultado = true;
                }
                else
                {
                    resultado = false;
                }
            }
            else
            {
                resultado = false;
            }
            return resultado;
        }
        public bool TieneApercibimientoByIdLaboratorio(int idLaboratorio)
        {
            bool resultado = false;
            var Apercibimiento = Context.Laboratorios_Apercibidos.FirstOrDefault(x => x.Id_Laboratorio == idLaboratorio && x.Activo == true);
            if (Apercibimiento != null)
            {
                resultado = true;
            }
            else
            {
                resultado = false;
            }
            return resultado;
        }


        public bool TieneReincidenciaPreventivaByIdLaboratorio(int idLaboratorio)
        {
            bool resultado = false;
            DiaInhabil_BL diaBLL = new DiaInhabil_BL();
            var Reincidencia = Context.tbl_Reincidencia.FirstOrDefault(x => x.Id_Laboratorio == idLaboratorio && x.Activo == true && x.Estatus == 3);
            
            if (Reincidencia != null)
            {
                var diasHabiles = int.Parse(ConfigurationManager.AppSettings["DiasHabilesReincidencia"]);
                diasHabiles = diasHabiles + 1;
               
                var fechaLimite = diaBLL.SiguienteDiaHabil(Reincidencia.Fecha_Notificacion.Value, diasHabiles);
                var fechaNotificacion = diaBLL.SiguienteDiaHabil(Reincidencia.Fecha_Notificacion.Value, 1);
                //
                if (DateTime.Now >= fechaNotificacion && DateTime.Now < fechaLimite)
                {
                    resultado = true;
                }
                else
                {
                    resultado = false;
                }
               
            }
            else
            {
                var ReincidenciaImpugnada = Context.tbl_Reincidencia.FirstOrDefault(x => x.Id_Laboratorio == idLaboratorio && x.Activo == true && x.Estatus == 4);
                if (ReincidenciaImpugnada != null)
                {
                    resultado = true;
                }
                else
                {
                    resultado = false;
                }

            }
            return resultado;
        }
        #endregion
    }
}
