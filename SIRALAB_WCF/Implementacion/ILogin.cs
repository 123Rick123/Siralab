using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SIRALAB_WCF.Modelos;
using SIRALAB_BL;
using SIRALAB_DAL;
using SIRALAB_BL.common;
using SIRALAB_BL.catalogos;
using SIRALAB.Controles;
using SIRALAB.Controles.AnexoE;

namespace SIRALAB_WCF.Implementacion
{
    public class ILogin
    {

        #region Variables
        int pobjId;
        #endregion

        public bool ComprobarAcceso(string token)
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
                              where LABAC.Token == TokenEncriptar
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

                var Consulta = result.Where(consulta => consulta.Token == TokenEncriptar && consulta.Activo == true && consulta.UsuarioRolID == 1).ToList();



                foreach (UsuarioDataModel view in Consulta)
                {

                    if (view.Token == TokenEncriptar)
                    {

                        BlMensaje = true;
                    }

                    else
                    {

                        BlMensaje = false;
                        throw new Exception("Laboratorio no encontrado  verifique  \n RFC:" + view.RFC);
                    }
                }

                return BlMensaje;
            }
            catch (Exception e)
            {
                return false;

            }
        }


        public bool ComprobarAcceso(string token, string RFC, ref string ponbIntRolId)
        {
            // if (usr.Contrasenia == contrasenia.Encriptar())
            bool BlMensaje = false;
            var contexto = new Siralab_Entities();
            try
            {

                if (string.IsNullOrEmpty(token) && string.IsNullOrEmpty(RFC))
                {

                    throw new Exception("El campo Laboratorio No debe estar Vacio \n RFC:" + RFC);

                }
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
                // IsError += "El parametro " + parametro.Param.ToString() + "no concide con la configuracion de la norma:" + OrdenTrabajo.Norma + "|";

                if (Consulta.Count == 0 && Consulta.Capacity == 0)
                {
                    BlMensaje = false;
                    throw new Exception("Laboratorio no encontrado  verifique E \n RFC:" + RFC);
                }
                foreach (UsuarioDataModel view in Consulta)
                {

                    if (view.RFC == RFC.Trim() && view.Token == TokenEncriptar)
                    {
                        ponbIntRolId = view.UsuarioRolID.ToString();
                        BlMensaje = true;
                    }

                    else
                    {
                        //ponbIntRolId = 0;
                        BlMensaje = false;
                        throw new Exception("LABORATORIO NO ENCONTRADO  VERIFIQUE \n RFC:" + RFC);
                    }
                }

                // ponbIntRolId = 0;


                return BlMensaje;
            }
            catch (Exception e)
            {
                ponbIntRolId = e.Message;
                return false;

            }
        }



        public bool ComprobarAcceso(string token, int OrdenTrabajoID, ref string pobjInt)
        {
            bool BlMensaje = false;
            var contexto = new Siralab_Entities();
            try
            {
                SIRALAB_DAL.OrdenTrabajo pobjOrdenTrabajo = new SIRALAB_DAL.OrdenTrabajo();
                List<ModelNormaConfiguracion> NormaConfiguracion = new List<ModelNormaConfiguracion>();

                var ordenTrabajoBl = new OrdenTrabajo_BL();
                string TokenEncriptar = token.Encriptar();
                if (OrdenTrabajoID == 0)
                {
                    BlMensaje = false;
                    throw new Exception("Verifique     la  orden de trabajo:     " + OrdenTrabajoID);
                }
                else
                {
                    var orden = ordenTrabajoBl.Obten(OrdenTrabajoID);
                    //Obtenemos el laboratorioID
                    pobjId = orden.LaboratorioID;
                    //var aprobacion = usr.Laboratorio.LaboratorioVigencias.Where(p => p.Activo).SingleOrDefault();
                }



                var result = (from LAB in contexto.Laboratorios
                              join LABAC in contexto.LaboratorioAcreditacions
                              on LAB.LaboratorioID equals LABAC.LaboratorioID
                              join USR in contexto.Usuarios
                              on LAB.LaboratorioID equals USR.LaboratorioID
                              join CAT in contexto.CatUsuarioRols
                              on USR.UsuarioRolID equals CAT.UsuarioRolID
                              where LAB.LaboratorioID == pobjId
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

                var Consulta = result.Where(consulta => consulta.LaboratorioID == pobjId && consulta.Token == TokenEncriptar && consulta.Activo == true && consulta.UsuarioRolID == 1).ToList();

                if (Consulta.Count == 0 && Consulta.Capacity == 0)
                {

                    BlMensaje = false;
                    throw new Exception("No se encontró el Laboratorio verifique orden de trabajo  consultado:" + OrdenTrabajoID);
                }


                foreach (UsuarioDataModel view in Consulta)
                {

                    if (view.LaboratorioID == pobjId)
                    {

                        BlMensaje = true;
                    }

                    else
                    {

                        BlMensaje = false;
                        throw new Exception("Laboratorio no encontrado  verifique         orden de trabajo:" + OrdenTrabajoID);
                    }
                }

                return BlMensaje;
            }
            catch (Exception e)
            {
                pobjInt = e.Message;
                return false;

            }
        }



    }
}
