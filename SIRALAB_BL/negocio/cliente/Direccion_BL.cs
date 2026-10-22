using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIRALAB_DAL;
using SIRALAB_BL;
using SIRALAB_BL.common;
using System.Web;
using System.Reflection;

namespace SIRALAB_BL
{
    public class Direccion_BL
    {
        public Siralab_Entities DireccionContext { get; private set; }
        public Direccion Direccion { get; private set; }

        public Localidad Localidad { get; private set; }
        public Colonia Colonia { get; private set; }
        public ColoniaTipo ColoniaTipo { get; private set; }

        Repository<Direccion> Repositorio;
        Repository<Localidad> RepositorioLoc;
        Repository<Colonia> RepositorioCol;
        Repository<ColoniaTipo> RepositorioColTipo;
        public Direccion_BL()
        {
            DireccionContext = new Siralab_Entities();
            Repositorio = new Repository<Direccion>(DireccionContext);
            RepositorioLoc = new Repository<Localidad>(DireccionContext);
            RepositorioCol = new Repository<Colonia>(DireccionContext);
            RepositorioColTipo = new Repository<ColoniaTipo>(DireccionContext);
        }


        public void Guarda(DireccionArgBase_BL direccion)
        {
            var dir = new Direccion
            {
                DireccionID = direccion.DireccionID != null ? int.Parse(direccion.DireccionID.ToString()) : 0,
                Calle = direccion.Calle ?? null
            };
            if (direccion.NumeroExteior == null)
                dir.NumeroExterior = null;
            else
            dir.NumeroExterior = direccion.NumeroExteior.Trim().Length > 20 ? direccion.NumeroExteior.Trim().Substring(0, 19).Trim() : direccion.NumeroExteior.Trim();
            if (direccion.NumeroInterior == null)
                dir.NumeroInterior = null;
            else
            dir.NumeroInterior = direccion.NumeroInterior.Trim().Length > 20 ? direccion.NumeroInterior.Trim().Substring(0, 19).Trim() : direccion.NumeroInterior.Trim();
            dir.EstadoID = int.Parse(direccion.EstadoID.ToString());
            dir.MunicipioID = int.Parse(direccion.MunicipioID.ToString());
            dir.LocalidadID = int.Parse(direccion.LocalidadID.ToString());
            dir.ColoniaID = int.Parse(direccion.ColoniaID.ToString());
            if (direccion.DireccionID != null)
            {
                dir.CP = int.Parse(direccion.CP.ToString());
            }
            
            var LocalidadNombre = HttpContext.Current.Session["LocalidadNombre"];
            var NombreColonia = HttpContext.Current.Session["NombreColonia"];


            try
            {
                var Localidadd= RepositorioLoc.Single(x => x.LocalidadID == dir.LocalidadID && x.MunicipioID == dir.MunicipioID && x.EstadoID == dir.EstadoID);
                var nombreLocalidad = "";
                if (Localidadd==null)
                {
                    nombreLocalidad = LocalidadNombre.ToString();

                    var Loc = new Localidad
                    {
                        LocalidadID = dir.LocalidadID,
                        MunicipioID = dir.MunicipioID,
                        EstadoID = dir.EstadoID,
                        Nombre = nombreLocalidad
                    };
                    RepositorioLoc.Add(Loc);
                    RepositorioLoc.SaveChanges();
                    dir.LocalidadID = Loc.LocalidadID;
                }


            }
            catch (Exception ex)
            {
                this.Localidad = null;

            }
            try
            {
                var Coloniaa = RepositorioCol.Single(x => x.ColoniaID== dir.ColoniaID);
                var nombreCol = string.Empty;
                var coltipo = string.Empty;
                if (HttpContext.Current.Session["ColTipo"]!=null)
                {
                    coltipo= HttpContext.Current.Session["ColTipo"].ToString();
                }
                var NombreColTipo = coltipo; //PONER VALIDACION POR SI TRUENA (RGG)
                
                if (Coloniaa == null)
                {
                    var ColoniaTipoo = RepositorioColTipo.Single(x => x.Nombre == NombreColTipo).ColoniaTipoID;
                    if (NombreColonia==null)
                    {
                        NombreColonia = string.Empty;
                    }
                    nombreCol = NombreColonia.ToString();
                    var Col = new Colonia
                    {
                        ColoniaID = RepositorioCol.NextNewID(x => x.ColoniaID),
                        MunicipioID = dir.MunicipioID,
                        EstadoID = dir.EstadoID,
                        CP = dir.CP,
                        Nombre = nombreCol,
                        ColoniaTipoID= ColoniaTipoo

                    };
                    RepositorioCol.Add(Col);
                    RepositorioCol.SaveChanges();
                    dir.ColoniaID = Col.ColoniaID;
                }
            }
            catch (Exception ex)
            {

                this.Colonia = null;
            }
            try
            {
                if (dir.DireccionID != 0) //Es una actualización
                {
                    Repositorio.Edit(dir);
                    Repositorio.SaveChanges();
                    this.Direccion = Repositorio.Single(x => x.DireccionID == dir.DireccionID);
                }
                else//Es un nuevo registro
                {
                    dir.DireccionID = Repositorio.NextNewID(x => x.DireccionID);
                    Repositorio.Add(dir);
                    Repositorio.SaveChanges();
                    this.Direccion = Repositorio.Single(x => x.DireccionID == dir.DireccionID);
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException exdb)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var err in exdb.EntityValidationErrors)
                {
                    sb.Append(string.Format("{0}||{1}", err.Entry.Entity.GetType().Name, err.Entry.State));
                    foreach (var err1 in err.ValidationErrors)
                    {
                        sb.Append(string.Format("Propiedad {0} -- Mensaje {1}", err1.PropertyName, err1.ErrorMessage));
                    }
                }
                Console.Write(sb.ToString());
                this.Direccion = null;
            }
            catch (Exception ex)
            {
                this.Direccion = null;
            }
        }

        public void obtener(int DireccionID)
        {
            this.Direccion = Repositorio.Single(x => x.DireccionID == DireccionID);
        }
        
    }
}
