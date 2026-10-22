using System;
using System.Linq;
using SIRALAB_BL.common;
using SIRALAB_DAL;

namespace SIRALAB_BL.negocio.analisis
{
    public class AnalisisFaltaPago_BL
    {
        public Siralab_Entities AnalisisContext { get; private set; }
        public AnalisisFaltaPago AnalisisFaltaPago { get; private set; }
        Repository<AnalisisFaltaPago> Repositorio;

        public AnalisisFaltaPago_BL()
        {
            AnalisisContext = new Siralab_Entities();
            Repositorio = new Repository<AnalisisFaltaPago>(AnalisisContext);
        }

        public AnalisisFaltaPago Obten_AnalisisFaltaPago(int AnalisisFaltaPagoID)
        {
            var analisis = Repositorio.Find(x => x.AnalisisFaltaPagoID == AnalisisFaltaPagoID).FirstOrDefault();
            this.AnalisisFaltaPago = analisis;
            return analisis;
        }

        public AnalisisFaltaPago Guarda(AnalisisFaltaPagoArgBase_BL args)
        {
            AnalisisFaltaPago analisisFaltaPago = new AnalisisFaltaPago();
            analisisFaltaPago.AnalisisID = args.AnalisisID;
            analisisFaltaPago.Firma = args.Firma;
            analisisFaltaPago.Cadena = args.Cadena;
            analisisFaltaPago.Contenedor = args.Contenedor;
            analisisFaltaPago.SelloDigital = args.SelloCNA;
            analisisFaltaPago.Marcado = args.Marcado;
            analisisFaltaPago.FechaRegistro = args.FechaRegistro;

            try
            {
                if (args.AnalisisFaltaPagoID != null) //Es una actualización
                {
                    analisisFaltaPago.AnalisisFaltaPagoID = args.AnalisisFaltaPagoID.Value;
                    Repositorio.Edit(analisisFaltaPago);
                }
                else
                {
                    analisisFaltaPago.AnalisisFaltaPagoID = Repositorio.NextNewID(x => x.AnalisisID);
                    Repositorio.Add(analisisFaltaPago);
                }
                Repositorio.SaveChanges();
                this.AnalisisFaltaPago = Repositorio.Single(x => x.AnalisisFaltaPagoID == analisisFaltaPago.AnalisisFaltaPagoID);

                return AnalisisFaltaPago = this.AnalisisFaltaPago;
            }
            catch (Exception ex)
            {
                this.AnalisisFaltaPago = null;
                return this.AnalisisFaltaPago;
            }
        }
    }
}