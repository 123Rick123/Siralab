using System.Collections.Generic;
using System.Linq;

namespace SIRALAB_SIRFIC
{
    public class ProcesamientoDatos
    {
        public Contribuyente BuscaContribuyente(string RFC)
        {
            using (var sirficContext = new CNA_SIRFICEntities())
            {
                var lista = sirficContext.SIRFIC_SIRALAB_ANCR.Where(p => p.rfc == RFC).ToList();
                var contribuyente = ConvertToContribuyente(lista);

                return contribuyente;
            }
        }

        public Contribuyente ConvertToContribuyente(List<SIRFIC_SIRALAB_ANCR> listado)
        {
            var cont = new Contribuyente();
            if (listado.Count > 0)
            {
                var direc = new Direccion();
                var titulos = new List<Titulo>();

                #region Contribuyente

                var clienteTemp = listado.Select(p => new Contribuyente
                {
                    ClienteID = p.ID_CONTRIBUYENTE,
                    Nombre = p.nombre,
                    ApellidoPaterno = p.apellidoPaterno ?? "",
                    ApellidoMaterno = p.apellidoMaterno ?? "",
                    RFC = p.rfc,
                    CURP = p.curp ?? "",
                    Correo = (p.email == null || p.email.Trim() == "") ? "correo@contribuyente.com" : p.email.Trim()
                }).Distinct();

                foreach (var clie in clienteTemp)
                {
                    cont.ClienteID = clie.ClienteID;
                    cont.Nombre = clie.Nombre;
                    cont.ApellidoPaterno = clie.ApellidoPaterno;
                    cont.ApellidoMaterno = clie.ApellidoMaterno;
                    cont.RFC = clie.RFC;
                    cont.CURP = clie.CURP;
                    cont.Correo = clie.Correo;
                }

                #endregion Contribuyente

                #region Direccion

                var direccion_Temp = listado.Select(p => new Direccion
                {
                    Calle = p.vialidad ?? "",
                    NumeroExterior = (p.numeroExterior1 ?? "") + (p.numeroExterior2 == null ? "" : " " + p.numeroExterior2),
                    NumeroInterior = p.numeroInterior ?? "",
                    EstadoID = (int)p.CLAVE_ESTADO,
                    Estado = p.ESTADO,
                    MunicipioID = (int)p.CLAVE_MUNICIPIO,
                    Municipio = p.MUNICIPIO,
                    LocalidadID = (int)p.CLAVE_LOCALIDAD,
                    Localidad = p.LOCALIDAD,
                    Asentamiento = p.ASENTAMIENTO_HUMANO,
                    AsentamientoID = (int)p.ID_ASENTAMIENTO_HUMANO,
                    TipoAsentamiento = p.TipoAsentamiento,
                    CodigoPostal = p.codigoPostal
                }).Distinct();
                foreach (var dir in direccion_Temp)
                {
                    direc.Calle = dir.Calle;
                    direc.NumeroExterior = dir.NumeroExterior;
                    direc.NumeroInterior = dir.NumeroInterior;
                    direc.EstadoID = dir.EstadoID;
                    direc.Estado = dir.Estado;
                    direc.MunicipioID = dir.MunicipioID;
                    direc.Municipio = dir.Municipio;
                    direc.LocalidadID = dir.LocalidadID;
                    direc.Localidad = dir.Localidad;
                    direc.Asentamiento = dir.Asentamiento;
                    direc.AsentamientoID = dir.AsentamientoID;
                    direc.TipoAsentamiento = dir.TipoAsentamiento;
                    direc.CodigoPostal = dir.CodigoPostal;
                }

                #endregion Direccion

                #region Titulos

                var titulosTemp = listado.Select(p => new Titulo { TituloID = p.ID_TITULO, Titulo1 = p.titulo }).Distinct();
                var anexoTemp = listado.Select(p => new
                {
                    TituloID = p.ID_TITULO,
                    AprovechamientoID = p.ID_APROVECHAMIENTO,
                    Anexo = p.numeroAnexo,
                    TipoCuerpoReceptorID = p.TipoCuerpoRececptorID,
                    Aprovechamiento = p.NOM_APROVECHAMIENTO,
                    AnexoID = p.ID_PUNTO_DESCARGA,
                    NumeroPuntoDescarga = p.numeroPuntoDescarga,
                    ClasificacionDescarga = p.CLASIFICACION_DESCARGA,
                    CuerpoReceptorID = p.idCuerpoReceptor,
                    CuerpoReceptor = p.NOMBRE_CR,
                    SerieMedidorID = p.ID_SERIE_MEDIDOR,
                    SerieMedidor = p.numeroMedidor,
                    MarcaMedidorID = p.ID_MARCA_MEDIDOR,
                    MarcaMedidor = p.MARCA_MEDIDOR,
                    Latitud = p.latitud,
                    Longitud = p.longitud,
                    Calle = p.vialidad_PD ?? "",
                    NumeroExterior = (p.numeroExterior1_PD ?? "") + (p.numeroExterior2_PD != null ? " " + p.numeroExterior2_PD : ""),
                    NumeroInterior = p.numeroInterior_PD ?? "",
                    EstadoID = (int)p.CLAVE_ESTADO_PD,
                    Estado = p.ESTADO_PD,
                    MunicipioID = (int)p.CLAVE_MUNICIPIO_PD,
                    Municipio = p.MUNICIPIO_PD,
                    LocalidadID = (int)p.CLAVE_LOCALIDAD_PD,
                    Localidad = p.LOCALIDAD_PD,
                    Asentamiento = p.ASENTAMIENTO_HUMANO_PD,
                    AsentamientoID = (int)p.ID_ASENTAMIENTO_HUMANO_PD,
                    TipoAsentamiento = p.TipoAsentamiento,  /*Campo Faltante pero no es ocupado*/
                    CodigoPostal = p.CODIGO_POSTAL_PD
                }).Distinct();

                foreach (var tit in titulosTemp)
                {
                    var titulo = new Titulo {TituloID = tit.TituloID, Titulo1 = tit.Titulo1};

                    #region Anexos

                    var anexoTitulo = anexoTemp.Where(p => p.TituloID == tit.TituloID).Select(q => new Anexo
                    {
                        AprovechamientoID = q.AprovechamientoID,
                        Anexo1 = q.Anexo,
                        TipoCuerpoReceptorID = q.TipoCuerpoReceptorID,
                        Aprovechamiento = q.Aprovechamiento,
                        AnexoID = q.AnexoID,
                        NumeroPuntoDescarga = q.NumeroPuntoDescarga,
                        ClasificacionDescarga = q.ClasificacionDescarga,
                        CuerpoReceptorID = q.CuerpoReceptorID,
                        CuerpoReceptor = q.CuerpoReceptor,
                        SerieMedidorID = q.SerieMedidorID,
                        SerieMedidor = q.SerieMedidor,
                        MarcaMedidorID = q.MarcaMedidorID,
                        MarcaMedidor = q.MarcaMedidor,
                        Latitud = q.Latitud,
                        Longitud = q.Longitud,
                        Domicilio = new Direccion
                        {
                            Calle = q.Calle, /*Campo Faltante*/
                            NumeroExterior = q.NumeroExterior, /*Campo Faltante*/
                            NumeroInterior = q.NumeroInterior, /*Campo Faltante*/
                            EstadoID = q.EstadoID,
                            Estado = q.Estado,
                            MunicipioID = q.MunicipioID,
                            Municipio = q.Municipio,
                            LocalidadID = q.LocalidadID,
                            Localidad = q.Localidad,
                            Asentamiento = q.Asentamiento,
                            AsentamientoID = q.AsentamientoID,
                            TipoAsentamiento = q.TipoAsentamiento,  /*Campo Faltante pero no es ocupado*/
                            CodigoPostal = q.CodigoPostal == null ? 0 : (int)q.CodigoPostal
                        },
                    }).ToList();

                    foreach (var anexo in anexoTitulo)
                    {
                        anexo.AnexoMedidores = listado.Where(p => p.ID_APROVECHAMIENTO == anexo.AprovechamientoID).Select(q => new AnexoMedidor_vw
                    {
                        ID_SERIE_MEDIDOR = q.ID_SERIE_MEDIDOR,
                        numeroMedidor = q.numeroMedidor,
                        ID_MARCA_MEDIDOR = q.ID_MARCA_MEDIDOR,
                        MARCA_MEDIDOR = q.MARCA_MEDIDOR,
                        FuenteAbastecimiento = q.FUENTE_ABASTECIMIENTO
                    }).ToList();
                    }

                    #endregion Anexos

                    titulo.Anexos.AddRange(anexoTitulo);
                    titulos.Add(titulo);
                }

                #endregion Titulos

                cont.Domicilio = direc;
                cont.Titulos = titulos;
            }
            return cont;
        }

        public List<string> GetContribuyentes(string RFC)
        {
            using (CNA_SIRFICEntities sirficContext = new CNA_SIRFICEntities())
            {
                //CNA_SIRFICEntities sirficContext = new CNA_SIRFICEntities(); //Like
                var contribuyentes = sirficContext.SIRFIC_SIRALAB_ANCR.Select(p => p.rfc.ToLower()).Distinct().ToList();

                var returnValues = contribuyentes.Where(p => p.Like(RFC.ToLower().FilterSQLData())).ToList();
                return returnValues.Select(p => p.ToUpper()).ToList();
            }
        }
    }
}