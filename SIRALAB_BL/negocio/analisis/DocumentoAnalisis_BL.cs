using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRALAB_DAL;

namespace SIRALAB_BL.negocio.analisis
{
    class DocumentoAnalisis_BL
    {


        private Siralab_Entities Context { get; set; }

        private DocumentoAnalisi DocumentoAnalisi { get; set; }
        Repository<DocumentoAnalisi> Repositorio;

        public DocumentoAnalisis_BL()
        {
            Context = new Siralab_Entities();
            Repositorio = new Repository<DocumentoAnalisi>(Context);
        }


        public DocumentoAnalisi SaveOrUpdate(DocumentoAnalisi documentoAnalisi)
        {
            try
            {
                if (documentoAnalisi.DocumentoAnalisisID > 0)
                {
                    Repositorio.Edit(documentoAnalisi);
                }
                else
                {
                    documentoAnalisi.DocumentoAnalisisID = Repositorio.NextNewID(da => da.DocumentoAnalisisID);
                    Repositorio.Add(documentoAnalisi);
                }

                return documentoAnalisi;
            }
            catch (Exception e)
            {
                return this.DocumentoAnalisi;
            }
        }

        public List<DocumentoAnalisi> FindAllById(int analisisId)
        {
            var documentosAnalisisList = new List<DocumentoAnalisi>();

            try
            {
                documentosAnalisisList = Repositorio.Find(da => da.AnalisisID == analisisId).ToList();
            }
            catch (Exception e)
            {
                
            }

            return documentosAnalisisList;

        }

        public void Eliminar(List<DocumentoAnalisi> listaDocumentosAnalisis)
        {
            try
            {
                foreach (var documentoAnalisi in listaDocumentosAnalisis)
                {
                    try
                    {
                        Repositorio.Delete(documentoAnalisi);
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
            catch (Exception e)
            {

            }
            
        }
    }
}
