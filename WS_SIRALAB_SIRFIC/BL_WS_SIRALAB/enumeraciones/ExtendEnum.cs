using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL_WS_SIRALAB
{
    public static class ExtendEnum
    {
        #region Conversiones a entero
        public static int ToInt(this enumTipoCuerpoReceptor tipo_cuerpo)
        {
            return (int)tipo_cuerpo;
        }
        public static int ToInt(this enumSignatario tipo_signatario)
        {
            return (int)tipo_signatario;
        }
        public static int ToInt(this enumNivel nivel)
        {
            return (int)nivel;
        }
        public static int ToInt(this enumOperacion operacion)
        {
            return (int)operacion;
        }
        public static int ToInt(this enumAnalisisEstatus estatus)
        {
            return (int)estatus;
        }
        public static int ToInt(this enumTipoEvaluacion evaluacion)
        {
            return (int)evaluacion;
        } 
        #endregion

        #region Conversiones a enumerador
        public static enumTipoCuerpoReceptor ToEnumTipoCuerpoReceptor(this int id)
        {
            return (enumTipoCuerpoReceptor)id;
        }
        public static enumSignatario ToEnumSignatario(this int id)
        {
            return (enumSignatario)id;
        }
        public static enumNivel ToEnumNivel(this int id)
        {
            return (enumNivel)id;
        }
        public static enumOperacion ToEnumOperacion(this int id)
        {
            return (enumOperacion)id;
        } 
        #endregion
    }
}
