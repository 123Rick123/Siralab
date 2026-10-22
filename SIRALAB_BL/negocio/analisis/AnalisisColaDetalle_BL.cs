using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRALAB_DAL;

namespace SIRALAB_BL.negocio.analisis
{



    public class AnalisisColaDetalle_BL
    {
        private Siralab_Entities AnalisisContext { get; set; }

        private AnalisisColaDetalle AnalisisColaDetalle { get; set; }
        Repository<AnalisisColaDetalle> Repositorio;


        public AnalisisColaDetalle_BL()
        {
            AnalisisContext = new Siralab_Entities();
            Repositorio = new Repository<AnalisisColaDetalle>(AnalisisContext);
        }


        public AnalisisColaDetalle Guarda(AnalisisColaDetalle cola)
        {
            try
            {
                if (cola.AnalisisColaDetalleID > 0) //Se actualiza el registro.
                {
                    Repositorio.Edit(cola);
                }
                else //Se crea un nuevo registro.
                {
                    cola.AnalisisColaDetalleID = Repositorio.NextNewID(acd => (int)acd.AnalisisColaDetalleID);
                    Repositorio.Add(cola);
                }

                return cola;
            }
            catch (Exception e)
            {
                this.AnalisisColaDetalle = null;
                return this.AnalisisColaDetalle;
            }
        }


        public void DeleteById(int analisisColaDetalleId)
        {
            try
            {
                Repositorio.Delete(acd => acd.AnalisisColaDetalleID == analisisColaDetalleId);
            }
            catch (Exception e)
            {
                //
            }
        }

        public AnalisisColaDetalle FindByID(int AnalisisColaDetalleID)
        {
            try
            {
                return Repositorio.First(acd => acd.AnalisisColaDetalleID == AnalisisColaDetalleID);
            }
            catch (Exception e)
            {
                return null;
            }

        }
    }
}
