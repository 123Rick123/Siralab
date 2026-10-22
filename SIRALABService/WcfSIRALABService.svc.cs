//------------------------------------------------------------------------------
// <copyright file="WebDataService.svc.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;
using SIRALAB_DAL;

namespace SIRALABService
{
    public class WcfSIRALABService : DataService<Siralab_Entities>
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(DataServiceConfiguration config)
        {
            // TODO: set rules to indicate which entity sets and service operations are visible, updatable, etc.
            // Examples:
            // config.SetEntitySetAccessRule("MyEntityset", EntitySetRights.AllRead);
            // config.SetServiceOperationAccessRule("MyServiceOperation", ServiceOperationRights.All);
            config.SetEntitySetAccessRule("Analisis", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("AnalisisFirmas", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("Anexoes", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("CatTipoCuerpoReceptors", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("CatTipoDocumentoAnexoes", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("CatTipoDocumentoMuestras", EntitySetRights.AllRead);//
            config.SetEntitySetAccessRule("CatUnidadMedidas", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("CatUsoAguas", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("Clientes", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("Consecions", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("CuerpoReceptors", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("Estatus", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("EstatusTipoes", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("FrecuenciaMuestreos", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("FrecuenciaReportes", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("Laboratorios", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("LaboratorioAcreditacions", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("LaboratorioAprobacions", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("LaboratorioClientes", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("LaboratorioSignatarios", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("LaboratorioVigencias", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("Metodoes", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("Muestras", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("MuestraValors", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("Muestreadors", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("Normas", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("NormaConfiguracions", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("Operadors", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("OrdenTrabajoes", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("Parametroes", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("ParametroMetodoes", EntitySetRights.AllRead);
            config.SetEntitySetAccessRule("Personas", EntitySetRights.AllRead);

            config.SetServiceOperationAccessRule("GetOrdenTrabajo", ServiceOperationRights.AllRead);

            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3;
        }

        [WebGet]
        public IEnumerable<OrdenTrabajo> GetOrdenTrabajo(int ClienteID)
        {
            Siralab_Entities db = new Siralab_Entities();
            return db.OrdenTrabajoes.Where(p => p.ClienteID == ClienteID);
        }
    }
}
