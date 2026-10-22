using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRALAB_BL.common
{
    public enum enumUsuarioRol
    {
        Laboratorio = 1,
        Recaudacion,
        Calidad,
        Administrador_PALA,
        Administrador_SIRALAB,
        x,
        y,
        Habilitar
    }
    public static class enumUsuarioRolExtend
    {
        /// <summary>
        /// Convierte la representación numérica a un valor del tipo enumUsuarioRol
        /// </summary>
        /// <param name="_UsuarioRol">Valor numérico que representa un rol de usuario (se localiza dentro del intervalo de enteros [1,3])</param>
        /// <returns>Devuelve la representación de un rol de usuario</returns>
        public static enumUsuarioRol ParseToEnumUsuarioRol(this int _UsuarioRol)
        {
            try
            {
                enumUsuarioRol returnValue = (enumUsuarioRol)_UsuarioRol;
                return returnValue;
            }
            catch
            {
                throw new Exception("No es posible hacer la conversión, la enumeración solo contempla 5 valores dentro del rango [1,5]");
            }
        }

        /// <summary>
        /// Convierte la representación de un valor del tipo enumUsuarioRol a su valor numérico
        /// </summary>
        /// <param name="_UsuarioRol">Valor de tipo enumUsuarioRol</param>
        /// <returns>Devuelve un valor entero que pertenece al valor representado como un valor de tipo enumUsuarioRol</returns>
        public static int ParseToInt(this enumUsuarioRol _UsuarioRol)
        {
            int returnValue = (int)_UsuarioRol;
            return returnValue;
        }
    }

    public enum enumOperacionesWUC { 
        Consulta,
        Edicion_Cosulta
    }
    public static class enumOperacionesWUCExtend
    {
        public static enumOperacionesWUC ParseToEnum(this int operacion)
        {
            try
            {
                enumOperacionesWUC returnValue = (enumOperacionesWUC)operacion;

                return returnValue;
            }
            catch
            {
                throw new Exception("No es posible hacer la conversión, la enumeración solo contempla 2 valores dentro del rango [1,2]");
            }
        }

        public static int ParseToInt(this enumOperacionesWUC operacion)
        {
            int returnValue = (int)operacion;
            return returnValue;
        }
    }

    public enum enumGridNorma
    {
        Nombre,
        Descripcion,
        FechaInicio,
        Parametro,
        HorasProceso,
        Municipal,
        Evaluacion,
        Accion
    }
    public static class enumGridNormaExtend
    {
        public static enumGridNorma ParseToEnum(this int operacion)
        {
            try
            {
                enumGridNorma returnValue = (enumGridNorma)operacion;

                return returnValue;
            }
            catch
            {
                throw new Exception("No se encuentra dentro de las definiciones");
            }
        }

        public static int ParseToInt(this enumGridNorma operacion)
        {
            int returnValue = (int)operacion;
            return returnValue;
        }
    }

    public enum enumCatalogo
    {
        Norma,
        NormaConfiguracion
    }
    public static class enumCatalogoExtend
    {
        public static enumCatalogo ParseToEnum(this int operacion)
        {
            try
            {
                enumCatalogo returnValue = (enumCatalogo)operacion;

                return returnValue;
            }
            catch
            {
                throw new Exception("No se encuentra dentro de las definiciones");
            }
        }

        public static int ParseToInt(this enumCatalogo operacion)
        {
            int returnValue = (int)operacion;
            return returnValue;
        }
    }

    public enum enumTipoPersonaBusqueda
    {
        Nombre,
        RFC
    }
    
    public enum enumDocumento
    {
        OrdenTrabajo,
        Muestra,
        Analisis
    }
    public static class enumDocumentoExtend
    {
        public static enumDocumento ParseToEnum(this int operacion)
        {
            try
            {
                enumDocumento returnValue = (enumDocumento)operacion;

                return returnValue;
            }
            catch
            {
                throw new Exception("No se encuentra dentro de las definiciones");
            }
        }

        public static int ParseToInt(this enumDocumento opcion)
        {
            int returnValue = (int)opcion;
            return returnValue;
        }
    }

    public enum enumEstatusOrdenTrabajo
    {
        Activo = 1,
        Cancelado
    }
    public static class enumEstatusOrdenTrabajoExtend
    {
        public static enumEstatusOrdenTrabajo ParseToEnum(this int estatusCancelacion)
        {
            enumEstatusOrdenTrabajo returnValue;
            if (estatusCancelacion.TryParseEnum<enumEstatusOrdenTrabajo>(out returnValue))
            {
                return returnValue;
            }
            else
            {
                throw new Exception("No se encuentra dentro de las definiciones");
            }
        }
        public static int ParseToInt(this enumEstatusOrdenTrabajo opcion)
        {
            int returnValue = (int)opcion;
            return returnValue;
        }
    }
    public enum enumEstatusAnalisis
    {
        Activo = 3,
        Cancelado = 4,
        EnProceso = 9,
        Firmado = 10,
        Finalizado = 11,
        FirmaFallida = 12,
        EnvioFallido = 13
    }
    public static class enumEstatusAnalisisExtend
    {
        public static enumEstatusAnalisis ParseToEnum(this int estatusCancelacion)
        {
            enumEstatusAnalisis returnValue;
            if (estatusCancelacion.TryParseEnum<enumEstatusAnalisis>(out returnValue))
            {
                return returnValue;
            }
            else
            {
                throw new Exception("No se encuentra dentro de las definiciones");
            }
        }
        public static int ParseToInt(this enumEstatusAnalisis opcion)
        {
            int returnValue = (int)opcion;
            return returnValue;
        }
    }
    [Serializable]
    public enum enumEstatusCancelacion
    {
        Capturada = 5,
        Enviada,
        Aprobada,
        Rechazada
    }
    public static class enumEstatusCancelacionExtend
    {
        public static enumEstatusCancelacion ParseToEnum(this int estatusCancelacion)
        {
            enumEstatusCancelacion returnValue;
            if (estatusCancelacion.TryParseEnum<enumEstatusCancelacion>(out returnValue))
            {
                return returnValue;
            }
            else
            {
                throw new Exception("No se encuentra dentro de las definiciones");
            }
        }
        public static int ParseToInt(this enumEstatusCancelacion opcion)
        {
            int returnValue = (int)opcion;
            return returnValue;
        }
    }
    public enum enumEstatusTipo
    {
        OrdenTrabajo = 1,
        Analisis = 2,
        SolicitudCancelacion = 3
    }
    public static class enumEstatusTipoExtend
    {
        public static enumEstatusTipo ParseToEnum(this int estatusTipo)
        {
            enumEstatusTipo returnValue;
            if(estatusTipo.TryParseEnum<enumEstatusTipo>(out returnValue))
                return returnValue;
            else
                throw new Exception("No se encuentra dentro de las definiciones");
        }
        public static int ParseToInt(this enumEstatusTipo opcion)
        {
            int returnValue = (int)opcion;
            return returnValue;
        }
    }
    public static class enumExtend
    {
        public static bool TryParseEnum<TEnum>(this int enumValue, out TEnum retVal) where TEnum : struct
        {
            retVal = default(TEnum);
            bool success = Enum.IsDefined(typeof(TEnum), enumValue);
            if (success)
            {
                retVal = (TEnum)Enum.ToObject(typeof(TEnum), enumValue);
            }
            return success;
        }
    }

    #region Clases de Apoyo
    public enum enumNivel
    {
        Simple = 1,
        Compuesta,
        Mensual
        //ValorInstantaneo
    }

    public static class enumConverter
    {
        public static enumNivel ConvertToNivel(this int Value)
        {
            enumNivel resultado = enumNivel.Simple;
            try
            {
                resultado = (enumNivel)Value;
            }
            catch
            {

            }
            return resultado;
        }

        public static int ConvertToInt(this enumNivel Value)
        {
            return (int)Value;
        }
    }
    #endregion
}
