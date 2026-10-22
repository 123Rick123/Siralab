using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using SIRALAB_WCF.Modelos;
using SIRALAB_WCF.Implementacion;
using System.Net;
using System.ServiceModel.Web;
using System.Security.Claims;
using System.Security.Permissions;
using System.Web;
using System.ServiceModel.Activation;
using System.Threading;
using System.Web.Security;
using System.ServiceModel.Channels;
using System.Security.Principal;
using System.Web.ApplicationServices;
using System.IO;
using System.Configuration;

namespace SIRALAB_WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "OperacionesLaboratorio" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select OperacionesLaboratorio.svc or OperacionesLaboratorio.svc.cs at the Solution Explorer and start debugging.


    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class OperacionesLaboratorio : IOperacionesLaboratorio
    {


        Result<OrdenTrabajo> result = new Result<Modelos.OrdenTrabajo>();
        IOrdenTrabajo IntOT = new IOrdenTrabajo();
        IAnalisis Ianalisis = new IAnalisis();
        ILogin pobjImplementacion = new ILogin();



        Result<OrdenTrabajo> IOperacionesLaboratorio.InsertaOT(TokenDatamodel Request, InsercionOT IOT)
        {

            try
            {
                string pobjInt = string.Empty;
                bool IsInsert = pobjImplementacion.ComprobarAcceso(Request.Token.Trim(), IOT.RFCLaboratorio, ref pobjInt);
                if (IsInsert == true)
                {
                    if (IOT.RFCLaboratorio != null && IOT.RFCLaboratorio != "")
                    {
                        result = IntOT.AltaOT(IOT);
                    }
                    else
                    {
                        ErrorDetalle error = new ErrorDetalle("Laboratorio no encontrado  verifique  \n RFC:"+ IOT.RFCLaboratorio, "0001");
                        result.Error = error;
                    }
                }
                else
                {
                    ErrorDetalle error = new ErrorDetalle(pobjInt, "0001");
                    result.Error = error;
                }
            }
            catch (Exception ex)
            {
                ErrorDetalle error = new ErrorDetalle("Ocurrio un error al momento de procesar la operacion", ex.HResult.ToString());
                result.Error = error;

            }

            return result;

        }



        Result<OrdenTrabajo> IOperacionesLaboratorio.ObtenerOrdenes(TokenDatamodel Request, BusquedaOT OT)
        {
            try
            {
                string pobjInt = string.Empty;


                bool IsInsert = pobjImplementacion.ComprobarAcceso(Request.Token.Trim(), OT.RFCLaboratorio, ref pobjInt);
                if (IsInsert == true)
                {
                    if (OT.RFCLaboratorio != null && OT.RFCLaboratorio != "")
                    {
                        var filtros = IntOT.ObtenerFiltro(OT);

                        List<OrdenTrabajo> lstOrdenes = new List<Modelos.OrdenTrabajo>();
                        IntOT.Buscar(ref lstOrdenes, filtros);
                        result.ListaResultado = lstOrdenes;

                    }
                    else
                    {
                        ErrorDetalle error = new ErrorDetalle("Laboratorio no encontrado  verifique  \n RFC:" + OT.RFCLaboratorio, "0001");
                        result.Error = error;
                    }
                }
                else
                {
                    ErrorDetalle error = new ErrorDetalle("Laboratorio no encontrado  verifique  \n RFC:" + OT.RFCLaboratorio, "0001");
                    result.Error = error;
                }
            }
            catch (Exception ex)
            {
                ErrorDetalle error = new ErrorDetalle("Ocurrio un error al momento de procesar la operacion", ex.HResult.ToString());
                result.Error = error;

            }

            return result;

        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="IOA"></param>
        /// <returns></returns>
        public Result<string> InsertaAnalisis(TokenDatamodel Request, InsertaAnalisis IOA)
        {
            Result<string> res = new Result<string>();
            try
            {


                string IsError = string.Empty;
                bool IsInsert = Ianalisis.InsertaAnalisis(ref IsError, IOA);
                string pobjInt = string.Empty;
                bool IsValido = pobjImplementacion.ComprobarAcceso(Request.Token, IOA.OrdenTrabajoID, ref pobjInt);
                if (IsValido == true)
                {
                    if (IsInsert)
                    {
                        res.Executenonquery = true;
                        res.ListaResultado = new List<string>() { "Alta exitosa" };
                    }
                    else
                    {
                        res.Executenonquery = false;
                        res.Error = new ErrorDetalle(IsError, "236");

                    }
                }
                else
                {

                    res.Executenonquery = false;
                    res.Error = new ErrorDetalle(pobjInt, "0001");

                }


            }
            catch (Exception ex)
            {
                ErrorDetalle error = new ErrorDetalle("Ocurrio un error al momento de procesar la operacion", ex.HResult.ToString());
                result.Error = error;

            }

            return res;

        }

        public CuerpoMensaje Autenticacion(Cabecera Request)
        {
            HttpContext h = null;
            Result<string> res = new Result<string>();
            StringBuilder MensajeArmado = new StringBuilder();
            try
            {
                string ponbIntRolId = string.Empty;
                string IsError = string.Empty;
                bool IsInsert = pobjImplementacion.ComprobarAcceso(Request.Token, Request.RFC, ref ponbIntRolId);




                if (IsInsert == true)
                {
                    res.Executenonquery = true;
                    res.ListaResultado = new List<string>() { "Autenticacion exitosa" };

                    foreach (string a in res.ListaResultado)
                    {
                        MensajeArmado.Append(a);
                    }
                }
                else
                {
                    res.Executenonquery = false;
                    res.Error = new ErrorDetalle(ponbIntRolId, "236");
                    foreach (string a in res.ListaResultado)
                    {

                        MensajeArmado.Append(a);

                    }
                }



            }
            catch (Exception ex)
            {
                ErrorDetalle error = new ErrorDetalle("Ocurrio un error al momento de procesar la operacion", ex.HResult.ToString());
                result.Error = error;

                MensajeArmado.Append(error.Codigo + "\t" + error.Mensage);


            }

            return new CuerpoMensaje(MensajeArmado.ToString());




        }



    }
}

