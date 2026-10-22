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
    public class Laboratorio_BL
    {
        public Siralab_Entities LaboratorioContext { get; private set; }
        public Laboratorio  Analisis { get; private set; }

        Repository<Laboratorio> Repositorio;

        public Laboratorio_BL()
        {
            LaboratorioContext = new Siralab_Entities();
            Repositorio = new Repository<Laboratorio>(LaboratorioContext);
        }
              

        public Laboratorio Obten_Laboratorio(string Laboratorio)
        {
            var lab = Repositorio.Find(x => x.Nombre ==Laboratorio).FirstOrDefault();
            return lab;
        }
        public Laboratorio Obten(string rfc)
        {
            var lab = Repositorio.Find(x => x.RFC == rfc).FirstOrDefault();
            return lab;
        }
        public Laboratorio Obten_Laboratorio(int LaboratorioID)
        {
            var lab = Repositorio.Find(x => x.LaboratorioID == LaboratorioID).FirstOrDefault();
            return lab;
        }

        public List<Laboratorio> Obten_Laboratorio(int NormaID, int ParametroID)
        {
            var list = LaboratorioContext.usp_LaboratorioParametroNorma(NormaID, ParametroID).OrderBy(p => p.Nombre);
            List<Laboratorio> lista_laboratorios = new List<Laboratorio>();

            lista_laboratorios = list.Select(p => new Laboratorio { LaboratorioID = p.LaboratorioID, Nombre = p.Nombre }).ToList();
            /*foreach (var i in list)
            {
                lista_laboratorios.Add(new Laboratorio
                {
                    LaboratorioID = i.LaboratorioID,
                    Nombre = i.Nombre,
                    
                });
                
            }*/
            return lista_laboratorios;
        }

        public List<Laboratorio> ObtenerLaboratorios()
        {
            return Repositorio.Find(x => x.Activo).ToList();
        }
    }
}