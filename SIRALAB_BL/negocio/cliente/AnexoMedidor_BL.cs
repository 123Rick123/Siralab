using SIRALAB_BL.common;
using SIRALAB_DAL;
using System.Collections.Generic;
using System.Linq;

namespace SIRALAB_BL
{
    public class AnexoMedidor_BL
    {
        public Siralab_Entities AnexoContext { get; private set; }

        public AnexoMedidor AnexoMedidor { get; private set; }

        private Repository<AnexoMedidor> Repositorio;

        public AnexoMedidor_BL()
        {
            AnexoContext = new Siralab_Entities();
            Repositorio = new Repository<AnexoMedidor>(AnexoContext);
        }

        public void Guarda(AnexoMedidorArgBase_BL anexoMedidor)
        {
            AnexoMedidor an = new AnexoMedidor();

            if (anexoMedidor.AnexoMedidorID != null && anexoMedidor.AnexoMedidorID != 0)
            {
                an = Repositorio.Single(x => x.AnexoMedidorID == anexoMedidor.AnexoMedidorID);
            }

            an.AnexoMedidorID = anexoMedidor.AnexoMedidorID != null ? int.Parse(anexoMedidor.AnexoMedidorID.ToString()) : 0;

            an.AnexoID = anexoMedidor.AnexoID ?? 0;
            an.ID_SERIE_MEDIDOR = anexoMedidor.ID_SERIE_MEDIDOR;
            an.numeroMedidor = anexoMedidor.numeroMedidor;
            an.ID_MARCA_MEDIDOR = anexoMedidor.ID_MARCA_MEDIDOR;
            an.MARCA_MEDIDOR = anexoMedidor.MARCA_MEDIDOR;
            an.FuenteAbastecimiento = anexoMedidor.FuenteAbastecimiento;

            if (an.AnexoMedidorID == 0)//Nuevo registro
            {
                an.AnexoMedidorID = Repositorio.NextNewID(x => x.AnexoMedidorID);
                Repositorio.Add(an);
                Repositorio.SaveChanges();
            }
            else
            {
                Repositorio.SaveChanges();
            }
        }

        /// <summary>
        /// Obtiene una lista de elementos
        /// </summary>
        /// <param name="AnexoID"></param>
        /// <returns></returns>
        public List<AnexoMedidor> Listado_Anexo(int AnexoID)
        {
            List<AnexoMedidor> anexo = new List<AnexoMedidor>();
            var an = Repositorio.Find(x => x.AnexoID == AnexoID);
            anexo = an.ToList();
            return anexo;
        }

        /// <summary>
        /// Obtienen un elemento
        /// </summary>
        /// <param name="AnexoMedidorID"></param>
        public void Obten(int AnexoID, long ID_SERIE_MEDIDOR)
        {
            this.AnexoMedidor = Repositorio.Single(x => x.AnexoID == AnexoID && x.ID_SERIE_MEDIDOR == ID_SERIE_MEDIDOR);
        }

        /// <summary>
        /// Obtienen un elemento
        /// </summary>
        /// <param name="AnexoMedidorID"></param>
        public void Obten(int AnexoMedidorID)
        {
            this.AnexoMedidor = Repositorio.Single(x => x.AnexoMedidorID == AnexoMedidorID);
        }
        public List<AnexoMedidor> Obten(int AnexoID, bool esFuente)
        {
            List<AnexoMedidor> lst = Repositorio.Find(x => x.AnexoID == AnexoID && x.FuenteAbastecimiento == esFuente && x.numeroMedidor!=null ).ToList();
            return lst;
        }
    }
}