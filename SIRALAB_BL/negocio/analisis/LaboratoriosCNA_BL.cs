using System;
using System.Collections.Generic;
using System.Linq;
using SIRALAB_BL.common;
using SIRALAB_DAL;

namespace SIRALAB_BL.negocio.analisis
{
    public class LaboratoriosCNA_BL
    {
        public Siralab_Entities LaboratorioContext { get; private set; }
        public Laboratorio Analisis { get; private set; }

        readonly Repository<LaboratoriosCNA> _repositorio;
        public LaboratoriosCNA_BL()
        {
            LaboratorioContext = new Siralab_Entities();
            _repositorio = new Repository<LaboratoriosCNA>(LaboratorioContext);
        }
        public LaboratoriosCNA ObtenLaboratorioCna(int laboratorioId)
        {
            var hoy = DateTime.Now;
            var lab = _repositorio.Find(x => x.IdLaboratorio == laboratorioId).Where(y => y.FechaInicio <= hoy).FirstOrDefault(z => z.FechaFin>= hoy);
            return lab;
        }
        public LaboratoriosCNA Guarda(filtrolaboratoriocnaArgBase args)
        {
            var ordentrabajo = new LaboratoriosCNA();
            try
            {
                ordentrabajo.IdLaboratorio = args.idlaboratorio;
                ordentrabajo.RfcLaboratorio = args.RFCLaboratorio;
                ordentrabajo.ValCorreo = args.correo;
                ordentrabajo.ValExtemporaneo = args.extemporaneo;
                ordentrabajo.FechaInicio = args.fechainicial;
                ordentrabajo.FechaFin = args.fechafinal;
               _repositorio.Add(ordentrabajo);
                _repositorio.SaveChanges();
            }
            catch (Exception ex)
            {
            }
            return ordentrabajo;
        }
    }

}
