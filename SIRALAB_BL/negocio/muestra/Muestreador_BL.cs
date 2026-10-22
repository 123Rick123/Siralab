using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRALAB_DAL;
using SIRALAB_BL;
using SIRALAB_BL.common;

namespace SIRALAB_BL
{
    public class Muestreador_BL : Base_BL<Muestreador>
    {   
        //public Siralab_Entities MuestreadorContext { get; private set; }
        public Muestreador Muestreador_ { get; private set; }

        //Repository<Muestreador> Repositorio;

        public Muestreador_BL()
        {
            //MuestreadorContext = new Siralab_Entities();
            //Repositorio = new Repository<Muestreador>(MuestreadorContext);
        }

        public List<Muestreador> Lista_Muestreador(int MuestraID)
        {
            var listamuestreador = Repositorio.Find(x => x.MuestraID == MuestraID).ToList();
            return listamuestreador;
        }

        public Muestreador Obten_Muestra(int MuestreadorID)
        {
            var muestreador = Repositorio.Find(x => x.MuestreadorID == MuestreadorID).FirstOrDefault();
            return muestreador;
        }
        /// <summary>
        /// Se obtiene el registro del muestreador, realizando la busqueda por muestraID
        /// </summary>
        /// <param name="MuestraID"></param>
        /// <returns></returns>
        public Muestreador Obten_Muestra_Muestra(int MuestraID)
        {
            var muestreador = Repositorio.Find(x => x.MuestraID == MuestraID && (x.Activo ?? false)).LastOrDefault();
            return muestreador;
        }

        public Muestreador Obten_Muestreador_muestra(int MuestraID)
        {
            var muestreador = Repositorio.Find(x => x.MuestraID == MuestraID && (x.Activo ?? false)).FirstOrDefault();
            return muestreador;
        }

        public List<Muestreador> ObtenLista_Muestrador_muestra(int muestraId)
        {
            return Repositorio.Find(x => x.MuestraID == muestraId).ToList();
        }

        public void EliminaMuestreador(int MuestraID)
        {
            foreach (var muestreador in ObtenLista_Muestrador_muestra(MuestraID))
            {
                Repositorio.Delete(muestreador);
            }
        }

        public Muestreador Guarda(MuestreadorArgBase_BL args)
        {
            try
            {
                var agregar = true;
                var muestreadorTemp = Repositorio.Find(x => x.MuestraID == args.MuestraID && (x.Activo ?? false));

                foreach (var muestreador1 in muestreadorTemp) {
                    if(muestreador1.LaboratorioSignatarioID != args.LaboratorioSignatarioID)
                        muestreador1.Activo = false;
                    else
                    {
                        muestreador1.Activo = true;
                        agregar = false;
                    }
                }
                Repositorio.SaveChanges();

                if (agregar)
                {
                    Muestreador muestreador = new Muestreador();
                    muestreador.MuestreadorID = Repositorio.NextNewID(x => x.MuestreadorID);
                    muestreador.MuestraID = args.MuestraID;
                    muestreador.LaboratorioSignatarioID = args.LaboratorioSignatarioID;
                    muestreador.Activo = true;

                    Repositorio.Add(muestreador);
                    muestreador = Repositorio.Single(x => x.MuestreadorID == muestreador.MuestreadorID);

                    Repositorio.SaveChanges();
                }

                return this.Muestreador_;
            }

            catch (Exception ex)
            {
                this.Muestreador_ = null;
                return this.Muestreador_;
            }
        }

        public void ClonarMuestreador(int muestraID, int? laboratorioSignatarioId)
        {
            if (laboratorioSignatarioId != null)
            {
                try
                {
                    var muestreador = new Muestreador()
                    {
                        MuestreadorID = Repositorio.NextNewID(m => m.MuestreadorID),
                        MuestraID = muestraID,
                        LaboratorioSignatarioID = laboratorioSignatarioId.Value,
                        Activo = true
                    };

                    Repositorio.Add(muestreador);
                    Repositorio.SaveChanges();
                }
                catch (Exception e)
                {

                }

            }
        }
    }
}