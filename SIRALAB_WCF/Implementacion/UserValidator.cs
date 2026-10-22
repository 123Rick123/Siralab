using SIRALAB_BL.common;
using SIRALAB_DAL;
using SIRALAB_WCF.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIRALAB_WCF.Implementacion
{
    /// <summary>
    /// User Name Password Validator
    /// </summary>
    public class UserValidator
    {
        public bool IsUserValid(string RFC, string token)
        {



            // if (usr.Contrasenia == contrasenia.Encriptar())
            bool BlMensaje = false;
            var contexto = new Siralab_Entities();
            try
            {
                string TokenEncriptar = token.Encriptar();
                var result = (from LAB in contexto.Laboratorios
                              join LABAC in contexto.LaboratorioAcreditacions
                              on LAB.LaboratorioID equals LABAC.LaboratorioID
                              join USR in contexto.Usuarios
                              on LAB.LaboratorioID equals USR.LaboratorioID
                              join CAT in contexto.CatUsuarioRols
                              on USR.UsuarioRolID equals CAT.UsuarioRolID
                              where LAB.RFC == RFC.Trim()
                              && LABAC.Token == TokenEncriptar
                              && CAT.UsuarioRolID == 1
                              && USR.Activo == true
                              && LAB.Activo == true



                              select new UsuarioDataModel
                              {
                                  UsuarioID = USR.UsuarioID,
                                  RFC = LAB.RFC,
                                  LaboratorioID = LAB.LaboratorioID,
                                  PersonaID = USR.PersonaID,
                                  Activo = USR.Activo,
                                  UsuarioRolID = CAT.UsuarioRolID,
                                  Token = LABAC.Token

                              }
                              ).ToList();

                var Consulta = result.Where(consulta => consulta.RFC == RFC.Trim() && consulta.Token == TokenEncriptar && consulta.Activo == true && consulta.UsuarioRolID == 1).ToList();



                foreach (UsuarioDataModel view in Consulta)
                {

                    if (view.RFC == RFC.Trim() && view.Token == TokenEncriptar)
                    {
                        BlMensaje = true;
                        // ponbIntRolId = view.UsuarioRolID;
                       // commaSeparatedRoles = "Laboratorio";
                        
                    }

                    else
                    {

                        BlMensaje = false;
                        throw new Exception("Los datos de laboratorio no son validdos  Verifique");
                    }
                }

                return BlMensaje;
            }
            catch (Exception e)
            {
                BlMensaje = false;
                return BlMensaje;

            }



        }
    }
}