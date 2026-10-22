using SIRALAB_WCF.Modelos;
using System;
using System.Linq;
using System.Web;
using SIRALAB_BL;
using SIRALAB_DAL;
using SIRALAB_BL.common;
using SIRALAB_BL.catalogos;
using SIRALAB.Controles;
using System.Collections.Generic;

namespace SIRALAB_WCF.Implementacion
{
    public class IAnalisis
    {
        #region Variables
        int pobjOrdenId;
        #endregion
        public IAnalisis()
        {
                      
             
             
             
            ///va
        }
        public List<Analisis> CargaInformacion(int OrdenTrabajoID)
        {
            ObtenerOrdenTrabajo(OrdenTrabajoID);
            int a = 0;

            Analisis pobjAnalisist = new Analisis();
            List<Analisis> pobjAnalisis = new List<Analisis>();
            var contexto = new Siralab_Entities();
            var q = from j in contexto.Analisis
                    where j.OrdenTrabajoID == 1
                    select new { j.AnalisisID };


            using (Siralab_Entities sdb = new Siralab_Entities())
            {

                // var Q = sdb.Analisis.Where(x => x.OrdenTrabajoID == 395040);

                var A = from x in sdb.Analisis
                        where x.OrdenTrabajoID == OrdenTrabajoID
                        select new Analisis
                        {

                            AnalisisID = x.AnalisisID,
                            AnexoID = x.AnexoID,
                            AnalisisPadreID = x.AnalisisPadreID,
                            LaboratorioID = x.LaboratorioID,
                            EstatusID = x.EstatusID,
                            FechaFirma = x.FechaFirma,
                            FechaProceso = x.FechaProceso,
                            FechaRegistro = x.FechaRegistro,
                            Observaciones = x.Observaciones,
                            ConsecionID = x.ConsecionID
                        };


                return A.ToList();


            }


        }
        private List<SIRALAB_DAL.OrdenTrabajo> ObtenerOrdenTrabajo(int OrdenTrabajoID)
        {

            SIRALAB_DAL.OrdenTrabajo pobjOrdenTrabajo = new SIRALAB_DAL.OrdenTrabajo();
            List<ModelNormaConfiguracion> NormaConfiguracion = new List<ModelNormaConfiguracion>();
            var ordenTrabajoBl = new OrdenTrabajo_BL();
            var orden = ordenTrabajoBl.Obten(OrdenTrabajoID);
            int Norma = 0;
            pobjOrdenTrabajo.NormaID = orden.NormaID;
            pobjOrdenTrabajo.CuerpoReceptorID = orden.CuerpoReceptorID;
            pobjOrdenTrabajo.UsoAguaID = orden.UsoAguaID;
            Norma = Convert.ToInt32(pobjOrdenTrabajo.NormaID);

            int CuerpoReceptorID = Convert.ToInt32(pobjOrdenTrabajo.CuerpoReceptorID);
            int UsoAguaID = Convert.ToInt32(pobjOrdenTrabajo.UsoAguaID);
            pobjOrdenId = orden.LaboratorioID;
            //var aprobacion = usr.Laboratorio.LaboratorioVigencias.Where(p => p.Activo).SingleOrDefault();
            NormaConfiguracion = ValidaRelacionNormaCuerpoUsoAguaID(Norma, CuerpoReceptorID, UsoAguaID);


            return null;
        }
        private List<ModelNormaConfiguracion> ValidaRelacionNormaCuerpoUsoAguaID(int NormaID, int CuerpoReceptorID, int usoAguaID)
        {
            var SiralabContext = new Siralab_Entities();

            List<ModelNormaConfiguracion> NormaConfiguracion = new List<ModelNormaConfiguracion>();
            ModelNormaConfiguracion Nn = new ModelNormaConfiguracion();
            //var aprobacion = usr.Laboratorio.LaboratorioVigencias.Where(p => p.Activo).SingleOrDefault();
            var A = from x in SiralabContext.NormaConfiguracions
                    where x.NormaID == NormaID
                    && x.CuerpoReceptorID == CuerpoReceptorID
                    && x.UsoAguaID == usoAguaID


                    select new ModelNormaConfiguracion
                    {
                        NormaID = x.NormaID,
                        ParametroID = x.ParametroID,
                        CuerpoReceptorID = x.CuerpoReceptorID,
                        UsoAguaID = x.UsoAguaID,
                        NivelID = x.NivelID

                    };


            A.ToList();

            NormaConfiguracion = A.ToList();



            var FiltroNormas = NormaConfiguracion.Where(LNivelId => LNivelId.NormaID == NormaID).Distinct();

            return FiltroNormas.ToList();
        }
        //TODO: Este metodo verifica si la fecha del registro de la muestra
        //cumple con la fecha vigencia dada por el laboratorio
        private bool ValidarFechaLaboratorioVigencia(DateTime? fechaRegistroMuestra)
        {

            Modelos.LaboratorioVigencia ass = new Modelos.LaboratorioVigencia();
            var contexto = new Siralab_Entities();
            var q = from j in contexto.LaboratorioVigencias
                    where j.LaboratorioID == pobjOrdenId
                    && j.Activo == true
                    select new Modelos.LaboratorioVigencia
                    {
                        LaboratorioID = j.LaboratorioID,
                        LaboratorioVigenciaID = j.LaboratorioVigenciaID,
                        Numero = j.Numero,
                        FechaInicio = j.FechaInicio,
                        FechaFin = j.FechaFin,
                        Activo = j.Activo
                    };
            foreach (Modelos.LaboratorioVigencia a in q)
            {
                ass.LaboratorioID = a.LaboratorioID;
                ass.LaboratorioVigenciaID = a.LaboratorioVigenciaID;
                ass.Numero = a.Numero;
                ass.FechaInicio = a.FechaInicio;
                ass.FechaFin = a.FechaFin;
                ass.Activo = a.Activo;
            }
            string Mensaje = string.Empty;
            if (ass.FechaFin < DateTime.Now)
            {


                Mensaje = "Existe un problema, su aprobación ha expirado, unicamente tendrá disponible capturar análisis del periodo en el que su aprobación se encontraba vigente.";

                throw new Exception(Mensaje);
            }


            if (ass == null)
            {
                Mensaje = "Existe un problema, no tiene aprobación activa para su laboratorio.";
                throw new Exception("Existe un problema, no tiene aprobación activa para su laboratorio.");
            }

            if (fechaRegistroMuestra < ass.FechaFin)
            {
                return true;
            }
            else
            {

                return false;
                throw new Exception("Existe un problema, no tiene aprobación activa para su laboratorio.");
            }
        }
        private bool ValidarDatosLaboratorioSignatario(ref string IsError, string RFC, string nombreSignatario, ref int PersonaId, ref int laboratorioId)
        {
            try
            {
                var contexto = new Siralab_Entities();
                List<Modelos.LaboratorioSignatario> listaDatosLaboratorioSignatario = new List<Modelos.LaboratorioSignatario>();
                bool BlMensaje = false;

                var r = (from c in contexto.Laboratorios
                         join c1 in contexto.LaboratorioSignatarios
                         on c.LaboratorioID equals c1.LaboratorioID
                         join P in contexto.Personas
                         on c1.PersonaID equals P.PersonaID
                         where c.RFC.ToUpper() == RFC.Trim().ToUpper()
                         && c1.Activo == true
                         select new Modelos.LaboratorioSignatario
                         {
                             LaboratorioID = c.LaboratorioID,
                             PersonaId = P.PersonaID,
                             NombrePersonaSignatario = P.Nombre,
                             ApellidoPaternoPersonaSignatario = P.ApellidoPaterno,
                             ApellidoMaternoPersonaSignatario = P.ApellidoMaterno,
                             Activo = c1.Activo,
                             RFC = c.RFC

                         }).ToList();

                if (r.Count > 0 || r != null)
                {
                    var FiltroNorma = r.Where(consulta => consulta.RFC == RFC && consulta.NombreCompleto.ToUpper() == nombreSignatario.ToUpper() && consulta.Activo == true).ToList();
                    if (FiltroNorma != null && FiltroNorma.Count > 0)
                    {
                        foreach (Modelos.LaboratorioSignatario v in FiltroNorma)
                        {
                            if (v.NombreCompleto == nombreSignatario)
                            {
                                PersonaId = v.PersonaId;
                                laboratorioId = v.LaboratorioID;
                                BlMensaje = true;
                            }
                            else
                            {
                                PersonaId = 0;
                                laboratorioId = 0;

                                BlMensaje = false;
                                IsError = "Los datos del signatario no son validdos  Verifique";
                            }
                        }
                    }
                    else
                    {
                        IsError = "Los datos del signatario no son validos  Verifique";
                    }
                }
                else
                {

                    BlMensaje = false;
                    IsError = "Los datos del signatario no son validos  Verifique";
                }

                return BlMensaje;
            }
            catch (Exception ex)
            {
                IsError = ex.Message;
                return false;
            }
        }
        private bool ValidarDatosConformanDireccion(int CP, string Estado, string Municipio, string Localidad, string Colonia)
        {
            bool BlMensaje = false;
            var contexto = new Siralab_Entities();
            var consulta = (from c in contexto.Estadoes
                            join c1 in contexto.Municipios
                            on c.EstadoID equals c1.EstadoID
                            join P in contexto.Localidads
                            on c1.MunicipioID equals P.MunicipioID
                            join Col in contexto.Colonias
                            on c1.MunicipioID equals Col.MunicipioID
                            where c.Nombre == Estado
                            && c1.Nombre == Municipio
                            && P.Nombre == Localidad
                            && P.MunicipioID ==c1.MunicipioID
                            && Col.Nombre == Colonia
                            select new Modelos.DireccionJoin
                            {
                                EstadoID = c.Nombre,
                                MunicipioID = c1.Nombre,
                                LocalidadID = P.Nombre,
                                ColoniaID = Col.Nombre,
                                cp = Col.CP


                            }

                ).ToList();

            var filtro = consulta.Where(c => c.EstadoID == Estado && c.MunicipioID == Municipio && c.LocalidadID == Localidad && c.ColoniaID == Colonia);

            foreach (Modelos.DireccionJoin busqueda in filtro)
            {
                if (busqueda.cp == CP)
                {

                    BlMensaje = true;
                }
                else
                {

                    BlMensaje = false;
                    throw new Exception("El codigo postal no corresponde: " + CP + " Codigo postal sugerido: " + filtro.First().cp);
                }
            }
            return BlMensaje;
        }
        /// <summary>
        /// Funcion que Guarda las direcciones de una muestra y genera los parametros para la muestra simple.
        /// </summary>
        /// <param name="IsError"></param>
        /// <param name="dir"></param>
        /// <param name="an"></param>
        /// <param name="coordenada"></param>
        /// <param name="datum"></param>
        /// <returns></returns>
        private bool AgregaDireccion(ref string IsError, Modelos.Direccion dir, Analisi an, Coordenadas_BL coordenada, string datum)
        {
            try
            {
                bool IsInsert = false;
                var SiralabContext = new Siralab_Entities();
                var muestraBl = new Muestra_BL();
                var muestraArgs = new MuestraCapturaArgs();
                if (an != null)
                {
                    //SE REALIZA UN CICLO CON EL NUMERO DE MUESTRAS CONFIGURADO EN LA ORDEN DE TRABAJO
                    //int Frecuencia = SiralabContext.OrdenTrabajoes.Where(x => x.OrdenTrabajoID == an.OrdenTrabajoID).Select(x => x.FrecuenciaMuestreo).First().MuestrasSimples - 1;
                    //for (int i = 0; i <= Frecuencia; i++)
                    //{
                    var OrdenTrabajo = SiralabContext.OrdenTrabajoes.Where(x => x.OrdenTrabajoID == an.OrdenTrabajoID);
                    muestraArgs.AnalisisID = an.AnalisisID;
                    //Se llena el objeto de dirección
                    var direccion = new DireccionCapturaArgs();
                    direccion.DireccionID = 0;
                    direccion.Calle = dir.Calle;
                    direccion.NumeroExteior = dir.Numero_exterior;
                    direccion.NumeroInterior = dir.Numero_interior;
                    int CP = int.Parse(dir.CP);
                    if (ValidarDatosConformanDireccion(CP, dir.Estado, dir.Municipio, dir.Localidad, dir.Colonia))
                    {
                        int EstadoID = SiralabContext.Estadoes.Where(x => x.Nombre == dir.Estado).Select(x => x.EstadoID).First();
                        int MunicipioID = SiralabContext.Municipios.Where(x => x.Nombre == dir.Municipio && x.EstadoID == EstadoID).Select(x => x.MunicipioID).First();
                        int LocalidadID = SiralabContext.Localidads.Where(x => x.Nombre == dir.Localidad && x.MunicipioID == MunicipioID && x.EstadoID == EstadoID).Select(x => x.LocalidadID).First();
                        int ColoniaID = SiralabContext.Colonias.Where(x => x.Nombre == dir.Colonia && x.MunicipioID == MunicipioID && x.EstadoID == EstadoID).Select(x => x.ColoniaID).First();
                        direccion.EstadoID = EstadoID;
                        direccion.MunicipioID = MunicipioID;
                        direccion.LocalidadID = LocalidadID;
                        direccion.ColoniaID = ColoniaID;
                        direccion.CP = int.Parse(dir.CP);
                        muestraArgs.dir = direccion;
                        //se termina de llenar el objeto dirección
                        muestraArgs.Numero = OrdenTrabajo.First().NumeroInforme;
                        muestraArgs.Descripcion = "";
                        muestraArgs.Identificacion = "";
                        muestraArgs.Caudal = 0;
                        muestraArgs.Latitud = coordenada.Latitude;
                        muestraArgs.Longitud = coordenada.Longitude;
                        muestraArgs.Datum = datum;
                        muestraArgs.Fecha = null;
                        muestraArgs.EsCompuesta = false;
                        muestraArgs.NivelID = 1;
                        muestraArgs.FechaRecepcion = null;
                        muestraArgs.Cancelado = false;
                        muestraArgs.Observaciones = "";
                        SIRALAB_DAL.Muestra muestra = muestraBl.Guarda(muestraArgs);
                        if (muestra.MuestraID != 0)
                        {
                            int muestraParametros = SiralabContext.MuestraValors.Where(x => x.MuestraID == muestra.MuestraID).Count();
                            if (muestraParametros == 0)
                            {
                                var orden = SiralabContext.OrdenTrabajoes.Where(x => x.OrdenTrabajoID == an.OrdenTrabajoID);
                                using (Siralab_Entities db = new Siralab_Entities())
                                {
                                    db.usp_MuestraValores(orden.First().NormaID, muestra.MuestraID, 1, false);
                                    IsInsert = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        IsError = "La direccion es incorrecta favor de validar";
                        IsInsert = false;
                        //break;
                    }

                    //Guardar(this, muestraArgs);
                    //}

                    //FIN DEL CICLO DEL NUMERO DE MUESTARS CONFIGURADO EN LA ORDEN DE TRABAJO
                }
                return IsInsert;

            }
            catch (Exception ex)
            {
                IsError = ex.Message;
                return false;
            }

        }
        /// <summary>
        /// Funcion para generar la incercion de 
        /// </summary>
        /// <param name="IsError"></param>
        /// <param name="IA"></param>
        /// <returns>Verdadeo o falso en caso de ver un error IsError carga la informacion del error</returns>
        public bool InsertaAnalisis(ref string IsError, InsertaAnalisis IA)
        {
            try
            {
                var SiralabContext = new Siralab_Entities();
                bool IsInsertDireccion = false;
                bool IsInsert = false;
                var analisisBl = new Analisis_BL();

                if (ConfiguracionNorma(ref IsError, IA.OrdenTrabajoID, IA.Analisis))
                {
                    foreach (Modelos.Analisis analisis in IA.Analisis)
                    {

                        int TotalNumEXtMuestras = analisis.MuestrasSimple.Where(x => x.Direccion.Numero_exterior != "").Count();
                        int TotalCalleMuestras = analisis.MuestrasSimple.Where(x => x.Direccion.Calle != "").Count();
                        int TotalCodigopostal = analisis.MuestrasSimple.Where(x => x.Direccion.CP != "").Count();
                        bool IsDeleteAnalisis = false;
                        if (TotalNumEXtMuestras < analisis.MuestrasSimple.Count() || TotalCalleMuestras < analisis.MuestrasSimple.Count() || TotalCodigopostal < analisis.MuestrasSimple.Count())
                        {
                            IsError = "DatosFaltantes";
                            IsInsert = false;
                            break;
                        }
                        else
                        {
                            SIRALAB_DAL.Analisi an = new SIRALAB_DAL.Analisi();
                            GeneraAnalisis(ref IsError, IA.OrdenTrabajoID, ref an);
                            analisisBl.ActualizaObservaciones(an.AnalisisID, analisis.Observaciones);
                            Coordenadas_BL coordenada = new Coordenadas_BL(
                              analisis.LatitudGrados,
                              analisis.LatitudMinutos,
                              analisis.LatitudSeg,
                              (EnumCoordinatesLat)Enum.Parse(typeof(EnumCoordinatesLat), Convert.ToInt32(analisis.Latitud).ToString()),
                              analisis.LongitudGrados,
                              analisis.LongitudMinutos,
                              analisis.LongitudSeg,
                              (EnumCoordinatesLng)Enum.Parse(typeof(EnumCoordinatesLng), Convert.ToInt32(analisis.Longitud).ToString()));

                            foreach (Modelos.Muestra muestra in analisis.MuestrasSimple)
                            {
                                IsInsertDireccion = AgregaDireccion(ref IsError, muestra.Direccion, an, coordenada, Convert.ToInt32(analisis.Datum).ToString());

                            }
                            if (!IsInsertDireccion)
                            {
                                IsInsert = false;
                                break;
                            }
                            else
                            {

                                List<SIRALAB_DAL.Muestra> Muestradal = new List<SIRALAB_DAL.Muestra>();
                                SIRALAB_DAL.LaboratorioSignatario LBSIG = new SIRALAB_DAL.LaboratorioSignatario();
                                bool GuardoSimple = false;
                                bool GeneroMuestraSimple = GeneraMuestras(ref IsError, ref Muestradal, analisis.MuestrasSimple, an, 1);
                                bool IsSignatario = GeneraSignatario(ref IsError, ref LBSIG, analisis.Signatario);

                                if (GeneroMuestraSimple && IsSignatario)
                                {
                                    GuardoSimple = GuardarSimples(ref IsError, Muestradal, an.AnalisisID, LBSIG);
                                }
                                else
                                {
                                    analisisBl.Eliminar(an.AnalisisID);
                                    IsDeleteAnalisis = true;
                                }
                                var Requierecompuesta = from orden in SiralabContext.OrdenTrabajoes
                                                        join norma in SiralabContext.Normas on orden.NormaID equals norma.NormaID
                                                        where orden.OrdenTrabajoID == IA.OrdenTrabajoID
                                                        select norma.RequiereCompuesta;
                                if (GuardoSimple && Requierecompuesta.First())
                                {
                                    if (GeneraCompuesta(ref IsError, an))
                                    {
                                        List<SIRALAB_DAL.Muestra> Muestradalc = new List<SIRALAB_DAL.Muestra>();
                                        bool GeneroMuestraCompuesta = GeneraMuestras(ref IsError, ref Muestradalc, analisis.MuestrasCompuesta, an, 2);
                                        if (GeneroMuestraCompuesta)
                                        {
                                            if (GuardarCompuesta(ref IsError, Muestradalc, an))
                                            {
                                                IsInsert = true;
                                            }
                                            else
                                            {
                                                if (!IsDeleteAnalisis)
                                                {
                                                    analisisBl.Eliminar(an.AnalisisID);
                                                }
                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    if (GuardoSimple)
                                    {
                                        IsInsert = true;
                                    }
                                    else
                                    {
                                        IsInsert = false;
                                        if (!IsDeleteAnalisis)
                                        {
                                            analisisBl.Eliminar(an.AnalisisID);
                                        }
                                    }

                                }

                            }
                        }

                    }
                }
                return IsInsert;
            }
            catch (Exception ex)
            {
                IsError = ex.Message;
                return false;
            }
        }
        /// <summary>
        /// Funcion que genera el alta de un analisis nuevo
        /// </summary>
        /// <param name="IsError">parametro referencia para devolver un error</param>
        /// <param name="OrdenTrabajoID"></param>
        /// <param name="an"></param>
        /// <returns></returns>
        private bool GeneraAnalisis(ref string IsError, int OrdenTrabajoID, ref Analisi an)
        {
            try
            {
                var SiralabContext = new Siralab_Entities();
                var Anexo = from orden in SiralabContext.OrdenTrabajoes

                            where orden.OrdenTrabajoID == OrdenTrabajoID
                            select new
                            {
                                AnexoID = orden.AnexoID,
                                ConsecionID = orden.ConsecionID
                            };
                var OrdenTrabajo = from orden in SiralabContext.OrdenTrabajoes

                                   where orden.OrdenTrabajoID == OrdenTrabajoID
                                   select new
                                   {
                                       FechaEmision = orden.FechaEmision,
                                       LaboratorioID = orden.LaboratorioID
                                   };


                AnalisisCapturaArgs analisiscaptura = new AnalisisCapturaArgs();
                analisiscaptura.AnalisisPadreID = null;
                analisiscaptura.OrdenTrabajoID = OrdenTrabajoID;
                analisiscaptura.ConsecionID = Convert.ToInt32(Anexo.First().ConsecionID);
                analisiscaptura.AnexoID = Convert.ToInt32(Anexo.First().AnexoID);
                analisiscaptura.LaboratorioID = OrdenTrabajo.First().LaboratorioID;
                analisiscaptura.EstatusID = 3;
                analisiscaptura.Firmada = false;
                analisiscaptura.FechaFirmada = null;
                analisiscaptura.Enviada = false;
                analisiscaptura.Procesado = false;
                analisiscaptura.FechaProceso = null;
                analisiscaptura.FechaRegistro = OrdenTrabajo.First().FechaEmision;

                Analisis_BL analisis_bl = new Analisis_BL();
                an = analisis_bl.Guarda(analisiscaptura);
                return true;
            }
            catch (Exception ex)
            {
                IsError = ex.Message;
                return false;
            }
        }
        private bool ConfiguracionNorma(ref string IsError, int OrdenTrabajoID, List<Modelos.Analisis> lstAnalisis)
        {
            try
            {
                OrdenTrabajo_BL ot = new OrdenTrabajo_BL();
                var OrdenTrabajo = ot.Obten(OrdenTrabajoID);
                NormaConfiguracion_BL nc = new NormaConfiguracion_BL();
                foreach (Analisis analisis in lstAnalisis)
                {
                    if (analisis.MuestrasSimple.Count != OrdenTrabajo.FrecuenciaMuestreo.MuestrasSimples)
                    {
                        IsError += "La norma " + OrdenTrabajo.Norma.Nombre + " requiere " + OrdenTrabajo.FrecuenciaMuestreo.MuestrasSimples.ToString() + " muestras simples|";
                    }
                    else
                    {
                        if (OrdenTrabajo.Norma.RequiereCaudal && analisis.MuestrasSimple.Where(x => x.Caudal != 0).Count() == 0)
                        {
                            IsError += "La norma " + OrdenTrabajo.Norma + " requiere caudal|";
                        }
                        if (OrdenTrabajo.Norma.RequiereCompuesta && analisis.MuestrasCompuesta.Count() == 0)
                        {
                            IsError += "La norma " + OrdenTrabajo.Norma + "por lo menos una muestra compuesta|";
                        }
                        var ConfiguracionSimple = nc.ObtenerNormaConfiguracion(Convert.ToInt32(OrdenTrabajo.NormaID), Convert.ToInt32(OrdenTrabajo.CuerpoReceptorID), Convert.ToInt32(OrdenTrabajo.UsoAguaID), 1);
                        var ConfiguracionComp = nc.ObtenerNormaConfiguracion(Convert.ToInt32(OrdenTrabajo.NormaID), Convert.ToInt32(OrdenTrabajo.CuerpoReceptorID), Convert.ToInt32(OrdenTrabajo.UsoAguaID), 2);
                        foreach (Modelos.Muestra muestra in analisis.MuestrasSimple)
                        {
                            if (ConfiguracionSimple.Count() == muestra.Parametros.Count())
                            {
                                foreach (Modelos.Parametro parametro in muestra.Parametros)
                                {
                                    if (!ConfiguracionSimple.Any(x => x.ParametroID == Convert.ToInt32(parametro.Param)))
                                    {
                                        IsError += "El parametro " + parametro.Param.ToString() + "no concide con la configuracion de la norma:" + OrdenTrabajo.Norma + "|";
                                    }
                                }
                            }
                            else
                            {
                                IsError += "La muestra simple no cumple con el numero de parametros establecidos:" + ConfiguracionSimple.Count() + "|";
                            }

                        }
                        if (OrdenTrabajo.Norma.RequiereCompuesta)
                        {
                            foreach (Modelos.Muestra muestra in analisis.MuestrasCompuesta)
                            {
                                if (ConfiguracionComp.Count() == muestra.Parametros.Count())
                                {
                                    foreach (Modelos.Parametro parametro in muestra.Parametros)
                                    {
                                        if (!ConfiguracionComp.Any(x => x.ParametroID == Convert.ToInt32(parametro.Param)))
                                        {
                                            IsError += "El parametro del la muestra compuesta " + parametro.Param.ToString() + "no concide con la configuracion de la norma:" + OrdenTrabajo.Norma.Nombre + "|";
                                        }
                                    }
                                }
                                else
                                {
                                    IsError += "La muestra compuesta no cumple con el numero de parametros establecidos:" + ConfiguracionSimple.Count() + "|";
                                }

                            }
                        }
                    }

                }
                if (IsError == "")
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
                IsError = ex.Message;
                return false;
            }

        }


        #region MuestraSimple
        private bool GeneraMuestras(ref string IsError, ref List<SIRALAB_DAL.Muestra> MuestraDAL, List<Modelos.Muestra> Muestras, SIRALAB_DAL.Analisi AN, int IdNivel)
        {

            try
            {

                var SiralabContext = new Siralab_Entities();
                bool GeneroLista = false;
                List<SIRALAB_DAL.Muestra> AnalisiMuestras = SiralabContext.Muestras.Where(x => x.AnalisisID == AN.AnalisisID && x.NivelID == IdNivel).ToList();
                for (int i = 0; i <= AnalisiMuestras.Count - 1; i++)
                {  //secaptura el id laboratorio
                    pobjOrdenId = AN.LaboratorioID;
                    ValidarFechaLaboratorioVigencia(Muestras[i].Fecha_RecepcionCompuesta);
                    AnalisiMuestras[i].Descripcion = Muestras[i].Descripcion;
                    AnalisiMuestras[i].Caudal = Convert.ToDecimal(Muestras[i].Caudal);
                    AnalisiMuestras[i].Cancelado = Convert.ToBoolean(Muestras[i].Cancelado);
                    AnalisiMuestras[i].Fecha = (Muestras[i].Fecha_Muestra.Value- Muestras[i].Fecha_Muestra.Value.TimeOfDay)+Muestras[i].Hora_Muestra.Value;
                    AnalisiMuestras[i].FechaRecepcion =( Muestras[i].Fecha_RecepcionCompuesta.Value - Muestras[i].Fecha_RecepcionCompuesta.Value.TimeOfDay) +Muestras[i].Hora_RecepcionCompuesta.Value;
                    AnalisiMuestras[i].Identificacion = Muestras[i].Identificacion;
                    AnalisiMuestras[i].Observaciones = Muestras[i].Observaciones;
                    if (IdNivel == 1)
                    {
                        AnalisiMuestras[i].ValMuestraC = Muestras[i].Volumen_Muestra_compuesta;
                        var Requierecompuesta = from orden in SiralabContext.OrdenTrabajoes
                                                join norma in SiralabContext.Normas on orden.NormaID equals norma.NormaID
                                                where orden.OrdenTrabajoID == AN.OrdenTrabajoID
                                                select norma.RequiereCompuesta;
                        if (!Requierecompuesta.First())
                        {
                            AnalisiMuestras[i].FlagCompuesta = true;
                        }
                    }
                    else
                    {
                        AnalisiMuestras[i].FlagCompuesta = true;
                        AnalisiMuestras[i].ValMuestraC = AnalisiMuestras[i].ValMuestraC;
                    }
                    AnalisiMuestras[i].NivelID = IdNivel;
                    foreach (MuestraValor mv in AnalisiMuestras[i].MuestraValors)
                    {
                        var mu = Muestras[i].Parametros.Where(x => Convert.ToInt32(x.Param) == mv.ParametroID);

                        if (Convert.ToInt32(mu.First().Accion) == 1 || Convert.ToInt32(mu.First().Accion) == 2)
                        {
                            mv.Exento = Convert.ToInt32(mu.First().Accion) == 1;
                            mv.NoAnalizado = Convert.ToInt32(mu.First().Accion) == 2;
                        }
                        else
                        {
                            string MetodoLA = mu.First().Metodo.ToUpper();
                            string rfclabaora = mu.First().RFCLaboratorioMuentra.ToUpper();
                            var LaboratorioM = SiralabContext.Laboratorios.Where(x => x.RFC == rfclabaora && x.Activo == true).Select(x => x.LaboratorioID);
                            if (LaboratorioM.Count() > 0 && LaboratorioM != null)
                            {
                                int LaboratorioID = LaboratorioM.First();
                                var metodoprametro = from a in SiralabContext.LaboratorioAprobacions
                                                     join b in SiralabContext.ParametroMetodoes
                                                     on a.ParametroMetodoID equals b.ParametroMetodoID
                                                     join c in SiralabContext.Metodoes
                                                     on b.MetodoID equals c.MetodoID
                                                     join d in SiralabContext.LaboratorioVigencias
                                                      on a.LaboratorioVigenciaID equals d.LaboratorioVigenciaID
                                                     where a.LaboratorioID == LaboratorioID
                                                    && b.ParametroID == mv.ParametroID
                                                    && c.Nombre.Replace(" ", "") == MetodoLA
                                                    && d.Activo == true
                                                    && a.Activo == true
                                                    && c.Activo == true
                                                     select c;
                                if (metodoprametro.Count() > 0)
                                {
                                    mv.Valor = Convert.ToDecimal(mu.First().Valor);
                                    mv.ValorValidacion = mu.First().ValorValidacion;
                                    mv.Exento = Convert.ToInt32(mu.First().Accion) == 1;
                                    mv.NoAnalizado = Convert.ToInt32(mu.First().Accion) == 2;
                                    mv.ParametroMetodoID = metodoprametro.First().MetodoID;
                                    mv.LaboratorioID = LaboratorioID;
                                    mv.LC_MC = mu.First().LC_MC;
                                }
                                else
                                {
                                    throw new ApplicationException("El metodo no corresponde al laboratorio o no existe Metodo: " + mu.First().Metodo + " " + mu.First().RFCLaboratorioMuentra + " Parametro: " + mu.First().Param.ToString());
                                }
                            }
                            else
                            {
                                throw new ApplicationException("El laboratorio no esta activo en PALA: " + mu.First().RFCLaboratorioMuentra + "parametro: " + mu.First().Param.ToString());
                            }
                        }
                    }
                }
                GeneroLista = true;
                MuestraDAL = AnalisiMuestras;
                return GeneroLista;

            }
            catch (Exception ex)
            {

                IsError = ex.Message;
                return false;
            }
        }
        private bool GuardarSimples(ref string IsError, List<SIRALAB_DAL.Muestra> muestras, int analisisId, SIRALAB_DAL.LaboratorioSignatario laboratorioSignatario)
        {



            try
            {
                foreach (var muestra in muestras)
                {
                    Muestreador_BL muestreadorBl = new Muestreador_BL();
                    LaboratorioSignatario_BL signatarioBl = new LaboratorioSignatario_BL();
                    var signatario = signatarioBl.Obten_LaboratorioSignatario(laboratorioSignatario.LaboratorioID, laboratorioSignatario.PersonaID);
                    var signatarioSaved = signatarioBl.Obten_LaboratorioSignatario(muestra.MuestraID);
                    var muestreadorArgs = new MuestreadorCapturaArgs();
                    if (signatario != null || signatarioSaved != null)
                    {
                        muestreadorArgs.MuestraID = muestra.MuestraID;
                        muestreadorArgs.LaboratorioSignatarioID = signatario == null ? signatarioSaved.LaboratorioSignatarioID : signatario.LaboratorioSignatarioID;
                        muestreadorBl.Guarda(muestreadorArgs);
                    }

                    Muestra_BL muestraBl = new Muestra_BL();
                    var muestraTemp = muestraBl.Obten_Muestra(muestra.MuestraID);
                    muestraTemp.Numero = muestraTemp.Numero;
                    //muestraTemp.Descripcion = muestraTemp.Descripcion;
                    //muestraTemp.Identificacion = muestraTemp.Identificacion;
                    muestraTemp.Descripcion = muestra.Descripcion;
                    muestraTemp.Identificacion = muestra.Identificacion;
                    muestraTemp.Caudal = muestra.Caudal;
                    muestraTemp.Fecha = muestra.Fecha;
                    muestraTemp.FechaRecepcion = muestra.FechaRecepcion;
                    muestraTemp.Cancelado = muestra.Cancelado;
                    muestraTemp.Observaciones = muestra.Observaciones;
                    muestraTemp.NivelID = muestraTemp.NivelID;
                    muestraTemp.ValMuestraC = muestra.ValMuestraC ?? 0;
                    muestraTemp.ValHojaCampo = muestra.ValHojaCampo ?? 0;
                    muestraTemp.FlagCompuesta = muestra.FlagCompuesta;


                    muestraBl.ActualizaMuestra(muestraTemp);

                    foreach (var muestraMuestraValor in muestra.MuestraValors)
                    {
                        MuestraValor_BL muestraValorBl = new MuestraValor_BL();
                        ParametroMetodo_BL parametroMetodoBl = new ParametroMetodo_BL();
                        MuestraValorArgBase_BL mvabBl = new MuestraValorArgBase_BL();
                        var mv = muestraValorBl.Obten(muestraMuestraValor.MuestraValorID);
                        var mp = parametroMetodoBl.Obten(muestraMuestraValor.ParametroID, muestraMuestraValor.ParametroMetodoID ?? 0);
                        mvabBl.MuestraValorID = (int)mv.MuestraValorID;
                        mvabBl.MuestraID = mv.MuestraID;
                        mvabBl.ParametroID = (int)mv.ParametroID;
                        mvabBl.OperacionID = mv.OperacionID;
                        mvabBl.Exenta = (bool)mv.Exenta;
                        mvabBl.Evalua = (bool)mv.Evalua;
                        mvabBl.AceptaValor = mv.AceptaValor;
                        mvabBl.LFD = (bool)mv.LFD;
                        mvabBl.LimiteInferior = mv.LimiteInferior;
                        mvabBl.LimiteSuperior = mv.LimiteSuperior;
                        mvabBl.LimiteInferiorLFD = mv.LimiteInferiorLFD;
                        mvabBl.LimiteSuperiorLFD = mv.LimiteSuperiorLFD;
                        mvabBl.Grupo = (bool)mv.Grupo;
                        mvabBl.EscalaDecimal = mv.EscalaDecimal;
                        mvabBl.LCMC = mv.LCMC;
                        mvabBl.GeneradoSP = false;
                        mvabBl.Valor = muestraMuestraValor.Valor ?? 0;
                        mvabBl.ValorValidacion = muestraMuestraValor.ValorValidacion;
                        mvabBl.LaboratorioID = muestraMuestraValor.LaboratorioID;
                        mvabBl.ParametroMetodoID = mp?.ParametroMetodoID;
                        mvabBl.LaboratorioID = muestraMuestraValor.LaboratorioID;
                        mvabBl.Exento = (bool)muestraMuestraValor.Exento;
                        mvabBl.NoAnalizado = (bool)muestraMuestraValor.NoAnalizado;
                        mvabBl.LC_MC = muestraMuestraValor.LC_MC;
                        if (mp != null) mvabBl.ParametroMetodoID = mp.ParametroMetodoID;
                        muestraValorBl.Guardar(mvabBl);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                IsError = ex.Message;
                return false;
            }



        }
        private bool GeneraSignatario(ref string IsError, ref SIRALAB_DAL.LaboratorioSignatario LBSIG, Signatario sig)
        {
            try
            {
                bool GeneraSig = false;
                var SiralabContext = new Siralab_Entities();
                int UltimoIDPersona = 0;
                int UltimoIDLaboratorio = 0;
                ValidarDatosLaboratorioSignatario(ref IsError, sig.RFCLaboratorio, sig.NombreSignatario, ref UltimoIDPersona, ref UltimoIDLaboratorio);
                LBSIG.LaboratorioID = UltimoIDLaboratorio;
                LBSIG.PersonaID = UltimoIDPersona;
                if (IsError == "")
                {
                    GeneraSig = true;
                }
                return GeneraSig;
            }
            catch (Exception ex)
            {
                IsError = ex.Message;
                return false;
            }

        }
        #endregion
        #region Muestracompuesta
        private bool GeneraCompuesta(ref string IsError, SIRALAB_DAL.Analisi an)
        {
            try
            {
                var SiralabContext = new Siralab_Entities();
                bool GeneroCompuesta = false;
                decimal CompuestaCaudal = 0;
                //Metodo para generar la muestra compuesta
                Muestra_BL muestra_bl = new Muestra_BL();
                Norma_BL ncBL = new Norma_BL();
                MuestraCapturaArgs muestraArgs = new MuestraCapturaArgs();
                SIRALAB_DAL.Muestra m = null;

                MuestraValor_BL muestravalor_bl = new MuestraValor_BL();

                if (an != null)
                {
                    var lista_muestra = muestra_bl.Lista_Muestra(an.AnalisisID);
                    if (lista_muestra.Where(p => p.Caudal != 0).Count() > 0)
                    {
                        CompuestaCaudal = lista_muestra.Where(p => p.Caudal != 0).Average(x => x.Caudal);
                    }
                    var unicamuestra = muestra_bl.Lista_Muestra(an.AnalisisID).FirstOrDefault();

                    muestraArgs.AnalisisID = an.AnalisisID;
                    muestraArgs.DireccionID = unicamuestra.DireccionID;
                    muestraArgs.Numero = unicamuestra.Numero;
                    muestraArgs.Descripcion = "";
                    muestraArgs.Identificacion = "";
                    muestraArgs.Caudal = CompuestaCaudal;
                    muestraArgs.Latitud = unicamuestra.Latitud;
                    muestraArgs.Longitud = unicamuestra.Longitud;
                    muestraArgs.Datum = unicamuestra.Datum;
                    muestraArgs.Fecha = null;
                    muestraArgs.EsCompuesta = true;
                    muestraArgs.FechaRecepcion = null;
                    muestraArgs.Cancelado = false;
                    muestraArgs.Observaciones = "";
                    muestraArgs.NivelID = 2;
                    m = muestra_bl.Guarda(muestraArgs);
                    if (m.MuestraID != 0)
                    {
                        int muestraParametros = SiralabContext.MuestraValors.Where(x => x.MuestraID == m.MuestraID).Count();
                        if (muestraParametros == 0)
                        {
                            var orden = SiralabContext.OrdenTrabajoes.Where(x => x.OrdenTrabajoID == an.OrdenTrabajoID);
                            using (Siralab_Entities db = new Siralab_Entities())
                            {
                                db.usp_MuestraValores(orden.First().NormaID, m.MuestraID, 2, true);
                                GeneroCompuesta = true;
                            }
                        }
                    }

                }
                return GeneroCompuesta;
            }
            catch (Exception ex)
            {
                IsError = ex.Message;
                return false;
            }

        }
        private bool GuardarCompuesta(ref string IsError, List<SIRALAB_DAL.Muestra> muestras, SIRALAB_DAL.Analisi an)
        {
            try
            {
                foreach (var muestra in muestras)
                {
                    Muestreador_BL muestreadorBl = new Muestreador_BL();
                    Muestra_BL muestraBl = new Muestra_BL();
                    var muestraTemp = muestraBl.Obten_Muestra(muestra.MuestraID);
                    muestraTemp.Numero = muestraTemp.Numero;
                    muestraTemp.Descripcion = muestraTemp.Descripcion;
                    muestraTemp.Identificacion = muestra.Identificacion;
                    muestraTemp.Caudal = muestraTemp.Caudal;
                    muestraTemp.Fecha = muestra.Fecha;
                    muestraTemp.FechaRecepcion = muestra.FechaRecepcion;
                    muestraTemp.Cancelado = muestra.Cancelado;
                    muestraTemp.Observaciones = muestra.Observaciones;
                    muestraTemp.NivelID = muestraTemp.NivelID;
                    muestraTemp.ValMuestraC = muestra.ValMuestraC ?? 0;
                    muestraTemp.ValHojaCampo = muestra.ValHojaCampo ?? 0;
                    muestraTemp.FlagCompuesta = muestra.FlagCompuesta;
                    muestraBl.ActualizaMuestra(muestraTemp);

                    foreach (var muestraMuestraValor in muestra.MuestraValors)
                    {
                        MuestraValor_BL muestraValorBl = new MuestraValor_BL();
                        ParametroMetodo_BL parametroMetodoBl = new ParametroMetodo_BL();
                        MuestraValorArgBase_BL mvabBl = new MuestraValorArgBase_BL();
                        var mv = muestraValorBl.Obten(muestraMuestraValor.MuestraValorID);
                        var mp = parametroMetodoBl.Obten(muestraMuestraValor.ParametroID, muestraMuestraValor.ParametroMetodoID ?? 0);
                        mvabBl.MuestraValorID = (int)mv.MuestraValorID;
                        mvabBl.MuestraID = mv.MuestraID;
                        mvabBl.ParametroID = (int)mv.ParametroID;
                        mvabBl.OperacionID = mv.OperacionID;
                        mvabBl.Exenta = (bool)mv.Exenta;
                        mvabBl.Evalua = (bool)mv.Evalua;
                        mvabBl.AceptaValor = mv.AceptaValor;
                        mvabBl.LFD = (bool)mv.LFD;
                        mvabBl.LimiteInferior = mv.LimiteInferior;
                        mvabBl.LimiteSuperior = mv.LimiteSuperior;
                        mvabBl.LimiteInferiorLFD = mv.LimiteInferiorLFD;
                        mvabBl.LimiteSuperiorLFD = mv.LimiteSuperiorLFD;
                        mvabBl.Grupo = (bool)mv.Grupo;
                        mvabBl.EscalaDecimal = mv.EscalaDecimal;
                        mvabBl.LCMC = mv.LCMC;
                        mvabBl.GeneradoSP = false;
                        mvabBl.Valor = muestraMuestraValor.Valor ?? 0;
                        mvabBl.ValorValidacion = muestraMuestraValor.ValorValidacion;
                        mvabBl.LaboratorioID = muestraMuestraValor.LaboratorioID;
                        mvabBl.ParametroMetodoID = mp?.ParametroMetodoID;
                        mvabBl.LaboratorioID = muestraMuestraValor.LaboratorioID;
                        mvabBl.Exento = (bool)muestraMuestraValor.Exento;
                        mvabBl.NoAnalizado = (bool)muestraMuestraValor.NoAnalizado;
                        mvabBl.LC_MC = muestraMuestraValor.LC_MC;
                        if (mp != null) mvabBl.ParametroMetodoID = mp.ParametroMetodoID;
                        muestraValorBl.Guardar(mvabBl);

                    }
                    GuardaGrupos(an, muestra.MuestraID);
                    Muestra_BL muestra_bl = new Muestra_BL();
                    muestra_bl.RecalculaSimple(muestra.MuestraID);
                }
                return true;
            }
            catch (Exception ex)
            {
                IsError = ex.Message;
                return false;
            }



        }

        private void GuardaGrupos(SIRALAB_DAL.Analisi an, int MuestraID)
        {
            /* Acá creamos la sumatoria para todos los parámetros que sean de grupo */

            Muestra_BL mBL = new Muestra_BL();
            OrdenTrabajo_BL otbl = new OrdenTrabajo_BL();
            var muestra = mBL.Obten_Muestra(MuestraID);
            var ot = otbl.Obten(an.OrdenTrabajoID);
            var ListaGrupos = muestra.MuestraValors.Where(p => p.Grupo);
            foreach (var item in ListaGrupos)
            {
                Norma_BL nBL = new Norma_BL();
                var NormaGrupo = nBL.Obtener(Convert.ToInt32(ot.NormaID)).NormaConfiguracions.Where(p =>
                    p.NivelID == 2
                    && p.ParametroID == item.ParametroID
                    && p.CuerpoReceptorID == ot.CuerpoReceptorID
                    && p.UsoAguaID == ot.UsoAguaID).FirstOrDefault();

                if (NormaGrupo != null)
                {
                    var itemsGrupo = NormaGrupo.NormaConfiguracion1.Select(p => p.ParametroID);
                    var ValorGrupo = muestra.MuestraValors.Where(p => itemsGrupo.Contains(p.ParametroID) && p.Valor != 0 && !p.NoAnalizado && !p.Exento && !p.Grupo).Sum(q => q.Valor);

                    /* ahora ya tenemos el valor para guardarlo en el grupo */
                    MuestraValor_BL mvBL = new MuestraValor_BL();
                    mvBL.Guardar(item.MuestraValorID, ValorGrupo);
                }
            }
        }
        #endregion




    }
}