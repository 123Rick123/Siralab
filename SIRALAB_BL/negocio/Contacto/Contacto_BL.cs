using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SIRALAB_DAL;

namespace SIRALAB_BL.negocio.Contacto
{
    public class Contacto_BL
    {
        public Siralab_Entities ContactoContext { get; private set; }
        public SIRALAB_DAL.Contacto Contacto { get; private set; }

        Repository<SIRALAB_DAL.Contacto> Repositorio;

        public Contacto_BL()
        {
            ContactoContext = new Siralab_Entities();
            Repositorio = new Repository<SIRALAB_DAL.Contacto>(ContactoContext);
        }


        public List<ContactoArgs> ObtenContactosCorreoAcuse(int laboratorioId)
        {
            var laboratorioContactoBl = new LaboratorioContacto_BL();

            var contactosId = laboratorioContactoBl.ObtenContactosByLaboratorioId(laboratorioId).Select(persona => persona.PersonaID);

            return Repositorio.Find(contacto => contacto.Activo && contacto.TipoContactoID == 4 && contactosId.Contains(contacto.PersonaID)).Select(ctc => new ContactoArgs()
            {
                ContactoID = ctc.ContactoID,
                Nombre = ctc.Persona.Nombre,
                ApellidoPaterno = ctc.Persona.ApellidoPaterno,
                ApellidoMaterno = ctc.Persona.ApellidoMaterno,
                Correo = ctc.Contacto1,
                RecibeAcuse = ctc.RecibeAcuse,
                EsLaboratorio = false
            }).ToList() ;
        }

        public SIRALAB_DAL.Contacto ObtenContacto(int contactoId)
        {
            return Repositorio.Find(contacto => contacto.ContactoID == contactoId).FirstOrDefault();
        }

        public bool UpdateContacto(SIRALAB_DAL.Contacto contacto)
        {
            try
            {
                if (contacto != null)
                {
                    Repositorio.Edit(contacto);
                    Repositorio.SaveChanges();
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}