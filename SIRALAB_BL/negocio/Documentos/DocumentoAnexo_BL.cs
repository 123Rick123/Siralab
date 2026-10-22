using System;
using System.Collections.Generic;
using System.Linq;
using SIRALAB_DAL;

namespace SIRALAB_BL.negocio.Documentos
{
    public class DocumentoAnexo_BL
    {
        private Siralab_Entities Context { get; set; }

        private DocumentoAnexo DocumentoAnexo { get; set; }
        Repository<DocumentoAnexo> Repositorio;

        public DocumentoAnexo_BL()
        {
            Context = new Siralab_Entities();
            Repositorio = new Repository<DocumentoAnexo>(Context);
        }

        public List<DocumentoAnexo> FindAllByDocumentoID(int documentoID)
        {
            var listaDocuementosAnexos = new List<DocumentoAnexo>();

            try
            {
                listaDocuementosAnexos = Repositorio.Find(docA => docA.DocumentoID == documentoID).ToList();
            }
            catch (Exception e)
            {
                
            }

            return listaDocuementosAnexos;
        }


        public void EliminarDocumentosAnexo(List<DocumentoAnexo> anexos )
        {
            try
            {
                foreach (var docAnexo in anexos)
                {
                    try
                    {
                        Repositorio.Delete(docAnexo);

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