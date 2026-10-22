using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SIRALAB_DAL;
using SIRALAB_BL.common;
using System.Data;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using SIRALAB_BL.negocio.analisis;
using Documento = SIRALAB_DAL.Documento;
using DocumentoAnexo = SIRALAB_BL.common.DocumentoAnexo;

namespace SIRALAB_BL
{
    public class Documento_BL : Base_BL<SIRALAB_DAL.Documento>
    {

        public Documento_BL() : base() { }

        public IEnumerable<SIRALAB_DAL.Documento> ObtenerTodos()
        {
            return Repositorio.GetAll();
        }

        public SIRALAB_DAL.Documento Obtener(int documentoID)
        {
            this.Entidad = Repositorio.Single(a => a.DocumentoID == documentoID);
            return Entidad;
        }

        public void GuardarDocumentos<T>(List<SIRALAB_DAL.Documento> doctosActualizados, T entidad) where T : class
        {
            int id = 0;
            string tipo = string.Empty;
            //List<SIRALAB_DAL.Documento> listaEvidenciasGuardadas = GetEvidencias(entidad);

            List<SIRALAB_DAL.Documento> listaDocumentosAdd = null;
            List<SIRALAB_DAL.Documento> listaDocumentosUpd = null;
            List<SIRALAB_DAL.Documento> listaDocumentosDel = null;
            List<SIRALAB_DAL.Documento> listaDocumentosActual = new List<SIRALAB_DAL.Documento>();
            using (var db = new Siralab_Entities())
            {
                try
                {
                    id = Archivos.GetIdObject(entidad);
                    tipo = entidad.GetType().BaseType.Name;

                    if (id != null && id > 0 && tipo != null && doctosActualizados != null && doctosActualizados.Count > 0)
                    {
                        using (TransactionScope transaction = new TransactionScope())
                        {

                            #region ORDENTRABAJO
                            if (tipo == typeof(OrdenTrabajo).Name)
                            {
                                OrdenTrabajo ordenTrabajo = db.OrdenTrabajoes.FirstOrDefault(a => a.OrdenTrabajoID == id);
                                listaDocumentosActual = ordenTrabajo.DocumentoAnexoes.Select(a => a.Documento).ToList();

                                if (ordenTrabajo != null)
                                {
                                    listaDocumentosAdd = doctosActualizados.Where(a => a.DocumentoID == 0).ToList();
                                    listaDocumentosUpd = doctosActualizados.Intersect(listaDocumentosActual).ToList();
                                    listaDocumentosDel = listaDocumentosActual.Except(doctosActualizados, rel => rel.DocumentoID).ToList();

                                    #region DocumentosEliminados
                                    if (listaDocumentosDel != null && listaDocumentosDel.Count > 0)
                                    {
                                        foreach (SIRALAB_DAL.Documento documentoEliminada in listaDocumentosDel)
                                        {
                                            SIRALAB_DAL.DocumentoAnexo documentoAnexoActual = db.DocumentoAnexoes.FirstOrDefault(x => x.DocumentoID == documentoEliminada.DocumentoID);

                                            if (documentoAnexoActual != null)
                                            {
                                                if (documentoAnexoActual.Documento.DocumentoAnalisis.Any()) //Elimina Documento Analisis.
                                                {
                                                    DocumentoAnalisi documentoAnalisiActual = db.DocumentoAnalisis.FirstOrDefault(x => x.DocumentoID == documentoEliminada.DocumentoID);
                                                    if (documentoAnalisiActual != null)
                                                    {
                                                        db.DocumentoAnalisis.Remove(documentoAnalisiActual);
                                                    }
                                                }
                                                db.DocumentoAnexoes.Remove(documentoAnexoActual); //Elimina anexo.
                                                db.Documentoes.Remove(documentoEliminada); //Elimina documento.
                                            }
                                        }
                                    }

                                    #endregion DocumentosEliminados

                                    #region DocumentosNuevos
                                    if (listaDocumentosAdd != null && listaDocumentosAdd.Count > 0)
                                    {

                                        int DocumentoIDMax = (db.Documentoes.Count() == 0) ? 0 : db.Documentoes.Max(a => a.DocumentoID);
                                        int DocumentoAnexoIDMax = (db.DocumentoAnexoes.Count() == 0) ? 0 : db.DocumentoAnexoes.Max(a => a.DocumentoID);

                                        var numIteracion = 1;
                                        foreach (SIRALAB_DAL.Documento documentoInserta in listaDocumentosAdd)
                                        {
                                            SIRALAB_DAL.Documento documentoNueva = new SIRALAB_DAL.Documento();

                                            DocumentoIDMax += 1;
                                            DocumentoAnexoIDMax += 1;

                                            documentoNueva.DocumentoID = DocumentoIDMax;
                                            documentoNueva.Nombre = documentoInserta.Nombre;
                                            
                                            documentoNueva.Ruta = documentoInserta.Ruta;
                                            documentoNueva.TipoMime = documentoInserta.TipoMime;
                                            documentoNueva.Extension = documentoInserta.Extension;
                                            documentoNueva.FechaRegistro = DateTime.Now;

                                            db.Documentoes.Add(documentoNueva);

                                            SIRALAB_DAL.DocumentoAnexo documentoAnexoNueva = new SIRALAB_DAL.DocumentoAnexo();

                                            documentoAnexoNueva.DocumentoAnexoID = DocumentoAnexoIDMax;
                                            documentoAnexoNueva.DocumentoID = documentoNueva.DocumentoID;
                                            documentoAnexoNueva.TipoDocumentoAnexoID = (int)documentoInserta.TipoDocumentoID;
                                            documentoAnexoNueva.OrdenTrabajoID = ordenTrabajo.OrdenTrabajoID;

                                            db.DocumentoAnexoes.Add(documentoAnexoNueva);


                                            if (documentoInserta.AnalisisID != null)
                                            {
                                                SIRALAB_DAL.DocumentoAnalisi documentoAnalisis = new DocumentoAnalisi()
                                                {
                                                    DocumentoAnalisisID = db.DocumentoAnalisis.Max(da => da.DocumentoAnalisisID) + numIteracion,
                                                    AnalisisID = documentoInserta.AnalisisID.Value,
                                                    DocumentoID = DocumentoIDMax
                                                };
                                                numIteracion++;

                                                db.DocumentoAnalisis.Add(documentoAnalisis);
                                            }
                                        }
                                    }
                                    #endregion DocumentosNuevos
                                    #region DocumentosActualizados
                                    if (listaDocumentosUpd != null && listaDocumentosUpd.Count > 0)
                                    {
                                        foreach (SIRALAB_DAL.Documento documentoActualizada in listaDocumentosUpd)
                                        {
                                            SIRALAB_DAL.DocumentoAnexo documentoAnexoActual = db.DocumentoAnexoes.FirstOrDefault(x => x.DocumentoID == documentoActualizada.DocumentoID);

                                            if (documentoAnexoActual != null)
                                            {
                                                documentoAnexoActual.TipoDocumentoAnexoID = (int)documentoActualizada.TipoDocumentoID;

                                            }
                                        }
                                    }
                                    #endregion DocumentosActualizados

                                }
                            }

                            #endregion ORDENTRABAJO

                            #region MUESTRA
                            else if (tipo == typeof(SIRALAB_DAL.Muestra).Name)
                            {
                                SIRALAB_DAL.Muestra muestra = db.Muestras.Where(a => a.MuestraID == id).FirstOrDefault();

                                listaDocumentosActual = muestra.DocumentoMuestras.Select(a => a.Documento).ToList();

                                if (muestra != null)
                                {
                                    listaDocumentosAdd = doctosActualizados.Where(a => a.DocumentoID == 0).ToList();
                                    listaDocumentosUpd = doctosActualizados.Intersect(listaDocumentosActual).ToList();
                                    listaDocumentosDel = listaDocumentosActual.Except(doctosActualizados, rel => rel.DocumentoID).ToList();


                                    #region DocumentosNuevos
                                    if (listaDocumentosAdd != null && listaDocumentosAdd.Count > 0)
                                    {

                                        int DocumentoIDMax = (db.Documentoes.Count() == 0) ? 0 : db.Documentoes.Max(a => a.DocumentoID);
                                        int DocumentoMuestraIDMax = (db.DocumentoMuestras.Count() == 0) ? 0 : db.DocumentoMuestras.Max(a => a.DocumentoID);

                                        foreach (SIRALAB_DAL.Documento documentoInserta in listaDocumentosAdd)
                                        {
                                            SIRALAB_DAL.Documento documentoNueva = new SIRALAB_DAL.Documento();

                                            DocumentoIDMax += 1;
                                            DocumentoMuestraIDMax += 1;

                                            documentoNueva.DocumentoID = DocumentoIDMax;
                                            documentoNueva.Nombre = documentoInserta.Nombre;
                                            documentoNueva.Ruta = documentoInserta.Ruta ?? "ruta";
                                            documentoNueva.TipoMime = documentoInserta.TipoMime;
                                            documentoNueva.Extension = documentoInserta.Extension;
                                            documentoNueva.FechaRegistro = DateTime.Now;

                                            db.Documentoes.Add(documentoNueva);

                                            SIRALAB_DAL.DocumentoMuestra documentoAnexoNueva = new SIRALAB_DAL.DocumentoMuestra();

                                            documentoAnexoNueva.DocumentoMuestraID = DocumentoMuestraIDMax;
                                            documentoAnexoNueva.DocumentoID = documentoNueva.DocumentoID;
                                            documentoAnexoNueva.TipoDocumentoMuestraID = (int)documentoInserta.TipoDocumentoID;
                                            documentoAnexoNueva.MuestraID = muestra.MuestraID;

                                            db.DocumentoMuestras.Add(documentoAnexoNueva);

                                        }
                                    }
                                    #endregion DocumentosNuevos
                                    #region DocumentosActualizados
                                    if (listaDocumentosUpd != null && listaDocumentosUpd.Count > 0)
                                    {
                                        foreach (SIRALAB_DAL.Documento documentoActualizada in listaDocumentosUpd)
                                        {
                                            SIRALAB_DAL.DocumentoMuestra documentoAnexoActual = db.DocumentoMuestras.Where(x => x.DocumentoID == documentoActualizada.DocumentoID).FirstOrDefault();

                                            if (documentoAnexoActual != null)
                                            {
                                                documentoAnexoActual.TipoDocumentoMuestraID = (int)documentoActualizada.TipoDocumentoID;

                                            }
                                        }
                                    }
                                    #endregion DocumentosActualizados
                                    #region DocumentosEliminados
                                    if (listaDocumentosDel != null && listaDocumentosDel.Count > 0)
                                    {
                                        foreach (SIRALAB_DAL.Documento documentoEliminada in listaDocumentosDel)
                                        {
                                            SIRALAB_DAL.DocumentoAnexo documentoAnexoActual = db.DocumentoAnexoes.Where(x => x.DocumentoID == documentoEliminada.DocumentoID).FirstOrDefault();

                                            if (documentoAnexoActual != null)
                                            {
                                                db.DocumentoAnexoes.Remove(documentoAnexoActual);
                                                db.Documentoes.Remove(documentoEliminada);
                                            }
                                        }
                                    }

                                    #endregion DocumentosEliminados

                                }

                            }

                            #endregion MUESTRA

                            #region ANALISIS
                            else if (tipo == typeof(SIRALAB_DAL.Analisi).Name)
                            {
                                SIRALAB_DAL.Analisi analisis = db.Analisis.Where(a => a.AnalisisID == id).FirstOrDefault();

                                listaDocumentosActual = analisis.DocumentoAnalisis.Select(a => a.Documento).ToList();

                                if (analisis != null)
                                {
                                    listaDocumentosAdd = doctosActualizados.Where(a => a.DocumentoID == 0).ToList();
                                    listaDocumentosDel = listaDocumentosActual.Except(doctosActualizados, rel => rel.DocumentoID).ToList();



                                    #region DocumentosNuevos
                                    if (listaDocumentosAdd != null && listaDocumentosAdd.Count > 0)
                                    {

                                        int DocumentoIDMax = (db.Documentoes.Count() == 0) ? 0 : db.Documentoes.Max(a => a.DocumentoID);
                                        int DocumentoAnexoIDMax = (db.DocumentoAnalisis.Count() == 0) ? 0 : db.DocumentoAnalisis.Max(a => a.DocumentoID);

                                        foreach (SIRALAB_DAL.Documento documentoInserta in listaDocumentosAdd)
                                        {
                                            SIRALAB_DAL.Documento documentoNueva = new SIRALAB_DAL.Documento();

                                            DocumentoIDMax += 1;
                                            DocumentoAnexoIDMax += 1;

                                            documentoNueva.DocumentoID = DocumentoIDMax;
                                            documentoNueva.Nombre = Path.GetFileName(documentoInserta.Nombre);// documentoInserta.Nombre;
                                            documentoNueva.Ruta = documentoInserta.Nombre ?? "ruta";
                                            documentoNueva.TipoMime = documentoInserta.TipoMime;
                                            documentoNueva.Extension = documentoInserta.Extension;
                                            documentoNueva.FechaRegistro = DateTime.Now;

                                            db.Documentoes.Add(documentoNueva);

                                            SIRALAB_DAL.DocumentoAnalisi documentoAnexoNueva = new SIRALAB_DAL.DocumentoAnalisi();

                                            documentoAnexoNueva.DocumentoAnalisisID = DocumentoAnexoIDMax;
                                            documentoAnexoNueva.DocumentoID = documentoNueva.DocumentoID;
                                            documentoAnexoNueva.AnalisisID = analisis.AnalisisID;

                                            db.DocumentoAnalisis.Add(documentoAnexoNueva);

                                        }
                                    }
                                    #endregion DocumentosNuevos     

                                    #region DocumentosEliminados
                                    if (listaDocumentosDel != null && listaDocumentosDel.Count > 0)
                                    {
                                        foreach (SIRALAB_DAL.Documento documentoEliminada in listaDocumentosDel)
                                        {
                                            SIRALAB_DAL.DocumentoAnalisi documentoAnexoActual = db.DocumentoAnalisis.Where(x => x.DocumentoID == documentoEliminada.DocumentoID).FirstOrDefault();

                                            if (documentoAnexoActual != null)
                                            {
                                                db.DocumentoAnalisis.Remove(documentoAnexoActual);
                                                db.Documentoes.Remove(documentoEliminada);
                                            }
                                        }
                                    }

                                    #endregion DocumentosEliminados

                                }

                            }

                            #endregion ANALISIS

                            db.SaveChanges();
                            transaction.Complete();
                        }

                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static List<SIRALAB_DAL.Documento> GetEvidencias<T>(T claseAccion)
        {
            List<SIRALAB_DAL.Documento> listaEvidenciasGuardadas = new List<SIRALAB_DAL.Documento>();
            string tipo = claseAccion.GetType().BaseType.Name;

            if (tipo == typeof(OrdenTrabajo).Name)
            {
                OrdenTrabajo_BL otBL = new OrdenTrabajo_BL();
                OrdenTrabajo ordenTrabajoBusqueda = new OrdenTrabajo();
                OrdenTrabajo ordenTrabajo = (claseAccion as OrdenTrabajo);
                ordenTrabajoBusqueda = otBL.Obten(ordenTrabajo.OrdenTrabajoID);

                if (ordenTrabajoBusqueda != null)
                {
                    List<SIRALAB_DAL.DocumentoAnexo> listaDocumentosAnexo = ordenTrabajoBusqueda.DocumentoAnexoes.ToList();
                    foreach (SIRALAB_DAL.DocumentoAnexo documentoAnexoAux in listaDocumentosAnexo)
                    {
                        documentoAnexoAux.Documento.TipoDocumento = documentoAnexoAux.CatTipoDocumentoAnexo.Nombre;
                        documentoAnexoAux.Documento.TipoDocumentoID = documentoAnexoAux.CatTipoDocumentoAnexo.TipoDocumentoAnexoID;
                        documentoAnexoAux.Documento.AnalisisID = documentoAnexoAux.Documento.DocumentoAnalisis.Any()
                            ? documentoAnexoAux.Documento.DocumentoAnalisis.First().AnalisisID
                            : 0;
                        listaEvidenciasGuardadas.Add(documentoAnexoAux.Documento);
                    }
                }
            }
            else if (tipo == typeof(Muestra).Name)
            {
                Muestra_BL muestraBL = new Muestra_BL();
                Muestra muestraBusqueda = new Muestra();
                Muestra muestra = (claseAccion as Muestra);
                muestraBusqueda = muestraBL.Obten_Muestra(muestra.MuestraID);

                if (muestraBusqueda != null)
                {
                    List<SIRALAB_DAL.DocumentoMuestra> listaDocumentoMuestra = muestraBusqueda.DocumentoMuestras.ToList();

                    foreach (SIRALAB_DAL.DocumentoMuestra documentoMuestraAux in listaDocumentoMuestra)
                    {
                        documentoMuestraAux.Documento.TipoDocumento = documentoMuestraAux.CatTipoDocumentoMuestra.Nombre;
                        listaEvidenciasGuardadas.Add(documentoMuestraAux.Documento);
                    }
                }
            }
            else if (tipo == typeof(Analisi).Name)
            {
                Analisis_BL analisisBL = new Analisis_BL();
                Analisi analisisBusqueda = new Analisi();
                Analisi analisis = (claseAccion as Analisi);
                analisisBusqueda = analisisBL.Obten_Analisis(analisis.AnalisisID);

                if (analisisBusqueda != null)
                {
                    List<DocumentoAnalisi> listaDocumentoAnalisis = analisisBusqueda.DocumentoAnalisis.ToList();

                    foreach (DocumentoAnalisi documentoAnalisisAux in listaDocumentoAnalisis)
                    {
                        documentoAnalisisAux.Documento.TipoDocumento = "Soporte de análisis";
                        listaEvidenciasGuardadas.Add(documentoAnalisisAux.Documento);
                    }
                }
            }
            else
            {
                listaEvidenciasGuardadas = null;
            }

            return listaEvidenciasGuardadas;
        }


        public void EliminarDocumentos(List<Documento> documentos)
        {
            try
            {
                foreach (var doc in documentos)
                {
                    try
                    {
                        Repositorio.Delete(doc);
                    }
                    catch (Exception e) {}
                }
            }
            catch (Exception e){}
        }

        public List<Documento> FindAllByDocumentoID(int documentoId)
        {
            var listaDocumentos = new List<Documento>();
            try
            {
                listaDocumentos = Repositorio.Find(doc => doc.DocumentoID == documentoId).ToList();
            }
            catch (Exception e)
            {

            }

            return listaDocumentos;
        }
    }
}
