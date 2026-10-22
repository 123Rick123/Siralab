using SIRALAB_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRALAB_BL.negocio.Comun
{
    public class DiaInhabil_BL
    {
        private Siralab_Entities Contex { get; set; }

        public DiaInhabil_BL()
        {
            Contex = new Siralab_Entities();
        }

        // Valida un rango de fecha tomando la fecha base y los dias que quiere contar despues de la fecha
        public DateTime SiguienteDiaHabil(DateTime fechaBase, int dias)
        {
            var dia = 0;
            var fecha = fechaBase;
            DateTime fechaTemporal = fechaBase;
            var diasInhabiles = Contex.CatDiaInhabil.Where(a => a.DiaInhabil > fecha).ToList();

            IEnumerable<CatDiaInhabil> ordQuery =
             from ord in diasInhabiles
             orderby ord.DiaInhabil ascending
             select ord;
            //suma los sabados y domingos 
            while (dia < dias)
            {
                fechaTemporal = fechaTemporal.AddDays(1);
                if (fechaTemporal.DayOfWeek != DayOfWeek.Sunday && fechaTemporal.DayOfWeek != DayOfWeek.Saturday)
                    if (!ordQuery.Any(b => b.DiaInhabil == fechaTemporal))
                    {
                        dia++;
                    }
            }

            return fechaTemporal;
        }
    }
}
