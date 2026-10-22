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
    public class Anexo_BL
    {
        public Siralab_Entities AnexoContext { get; private set; }
        public Anexo Anexo { get; private set; }

        Repository<Anexo> Repositorio;

        public Anexo_BL()
        {
            AnexoContext = new Siralab_Entities();
            Repositorio = new Repository<Anexo>(AnexoContext);
        }

        public void Guarda(AnexoArgBase_BL anexo)
        {
            var an = new Anexo();
            if (anexo.AnexoID != 0)
            {
                an = Repositorio.Single(x => x.AnexoID == anexo.AnexoID);
            }

            an.AnexoID = anexo.AnexoID != null ? int.Parse(anexo.AnexoID.ToString()) : 0;
            an.PukAnexoID = anexo.PukAnexoID;
            an.ConsecionID = anexo.ConsecionID;
            an.Anexo1 = anexo.Anexo.ToString();
            an.Latitud = decimal.Parse(anexo.Latitud.ToString());
            an.Longitud = decimal.Parse(anexo.Longitud.ToString());
            an.Datum = anexo.Datum;
            an.CuerpoReceptorID = anexo.CuerpoReceptorID;
            an.NombreCuerpoReceptor = anexo.NombreCuerpoReceptor;
            an.Efluente = anexo.Efluente;
            an.CuerpoReceptorID = anexo.CuerpoReceptorID;
            an.UsoAguaID = anexo.UsoAguaID;
            an.Medidor = anexo.NumeroMedidor;
            an.MarcaMedidor = anexo.Marcamedidor;
            an.idUman = anexo.iduman;

            if (an.AnexoID == 0)//Nuevo registro
            {
                an.FuenteAbastecimiento = anexo.FuenteAbastecimiento;
                an.OrigenDescarga = anexo.OrigenDescarga;

                var direccionBl = new Direccion_BL();
                direccionBl.Guarda(anexo.dir);

                if (direccionBl.Direccion != null)
                {
                    an.AnexoID = Repositorio.NextNewID(x => x.AnexoID);
                    an.DireccionID = direccionBl.Direccion.DireccionID;

                    Repositorio.Add(an);
                    Repositorio.SaveChanges();
                    this.Anexo = Repositorio.Single(x => x.AnexoID == an.AnexoID);
                }
            }
            else
            {
                an.DireccionID = int.Parse(anexo.dir.DireccionID.ToString());
                var direccionBl = new Direccion_BL();
                direccionBl.Guarda(anexo.dir);

                //Repositorio.Edit(an);
                Repositorio.SaveChanges();
                this.Anexo = Repositorio.Single(x => x.AnexoID == an.AnexoID);
            }
        }


        public Anexo Guarda_PDES(SIRALAB_DAL.Anexo anexo)
        {
            Anexo an = new Anexo();
            an.AnexoID = anexo.AnexoID != null ? int.Parse(anexo.AnexoID.ToString()) : 0;
            an.ConsecionID = anexo.ConsecionID;
            an.Anexo1 = anexo.Anexo1.ToString();
            an.Latitud = decimal.Parse(anexo.Latitud.ToString());
            an.Longitud = decimal.Parse(anexo.Longitud.ToString());
            an.Datum = anexo.Datum;
            an.CuerpoReceptorID = anexo.CuerpoReceptorID;
            an.NombreCuerpoReceptor = anexo.NombreCuerpoReceptor;
            an.FuenteAbastecimiento = anexo.FuenteAbastecimiento;
            an.OrigenDescarga = anexo.OrigenDescarga;
            an.Efluente = anexo.Efluente;

            an.CuerpoReceptorID = anexo.CuerpoReceptorID;
            an.UsoAguaID = anexo.UsoAguaID;
            an.Medidor = anexo.Medidor;
            an.DireccionID = anexo.DireccionID;

            Repositorio.Edit(an);
            Repositorio.SaveChanges();
            an = Repositorio.Single(x => x.AnexoID == an.AnexoID);
            return an;
        }


        public List<Anexo> Listado_Anexo(int ConsecionID)
        {
            List<Anexo> anexo = new List<Anexo>();
            var an = Repositorio.Find(x => x.ConsecionID == ConsecionID);
            anexo = an.ToList();
            return anexo;
        }

        public List<Anexo> Listado_Anexo(int ConsecionID, bool fuente)
        {

            List<Anexo> resultado = new List<Anexo>();


            resultado = Repositorio.Find(x => x.ConsecionID == ConsecionID && x.AnexoMedidors.Any(an => an.FuenteAbastecimiento == fuente)).ToList();

            return resultado;
        }

        public void Obten(int anexoID)
        {
            this.Anexo = Repositorio.Single(x => x.AnexoID == anexoID);
        }
        public void ObtenAnexos(int consecionID)
        {
            this.Anexo = Repositorio.First(x => x.ConsecionID == consecionID);
        }
        //public void ObtenAnexo(int puuc, string anexo)
        //{
        //    this.Anexo = Repositorio.First(x => x.PukAnexoID == puuc && x.Anexo1 == anexo);
        //}
        public void ObtenAnexo(int consecionID, string anexo, long? iduman)
        {
            this.Anexo = Repositorio.First(x => x.ConsecionID == consecionID && x.Anexo1 == anexo && x.idUman == (iduman ?? x.idUman));
        }
        public void Obtanexnombre(int consecionID, string anexo, string numeroMedidor)
        {
            this.Anexo = Repositorio.First(x => x.ConsecionID == consecionID && x.Anexo1 == anexo && x.Medidor == numeroMedidor);
        }
        public void Obtanexsolo(int consecionID, string anexo)
        {
            this.Anexo = Repositorio.First(x => x.ConsecionID == consecionID && x.Anexo1 == anexo);
        }
    }
}
