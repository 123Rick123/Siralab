using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIRALAB.FIEL
{
    /// <summary>
    /// Clase que contiene una lista de objetos de la clase "Contenedor"
    /// contiene metodos para enviar contenedoresa a firma y obtener los contedores con la informacion firmada
    /// </summary>
    public class FirmaElectronica
    {

        private char _Separador { get; set; }
        private List<Contenedor> _ListaFirmas { get; set; }
        private Exception _Exception { get; set; }

        /// <summary>
        /// Separador de las cadenas concatenadas
        /// </summary>
        public char separador { get { return _Separador; } set { _Separador = value; } }

        /// <summary>
        /// Lista de contenedores que se van a firmar
        /// </summary>
        public List<Contenedor> listaContenedores
        {
            set { _ListaFirmas = value; }
            get { return _ListaFirmas; }
        }

        /// <summary>
        /// Inicializa la lista contenedores que se van a trabajar
        /// y asigna " separador = ',' "
        /// </summary>
        public FirmaElectronica()
        {
            this.separador = ',';
            listaContenedores = new List<Contenedor>();
        }

        /// <summary>
        /// Agrega un contendor nuevo para firmar, a la lista de contenedores
        /// </summary>
        /// <param name="primaryKey">Llave primaria que identificara la informacions</param>

        public void AddCadena(string primaryKey, string cadena)
        {
            Contenedor nuevo = _ListaFirmas.Where(a => a.primaryKey == primaryKey).SingleOrDefault();
            if (nuevo != null)
            {
                _ListaFirmas.Remove(nuevo);
                _ListaFirmas.Add(new Contenedor(primaryKey, cadena));
            }
            else
            {
                _ListaFirmas.Add(new Contenedor(primaryKey, cadena));
            }
        }

        public void AddCadena(string primaryKey, string cadena, string parametroExtra)
        {
            Contenedor nuevo = _ListaFirmas.Where(a => a.primaryKey == primaryKey && a.parametroExtra == parametroExtra).SingleOrDefault();
            if (nuevo != null)
            {
                _ListaFirmas.Remove(nuevo);
                _ListaFirmas.Add(new Contenedor(primaryKey, cadena, parametroExtra));
            }
            else
            {
                _ListaFirmas.Add(new Contenedor(primaryKey, cadena, parametroExtra));
            }
        }

        /// <summary>
        /// Si el metodo obtenerListaFirmada genera una Exception regresara la propiedad "exception" llena
        /// </summary>
        public Exception exception { get { return _Exception; } set { _Exception = value; } }

        /// <summary>
        /// Obtiene la lista de contenedores firmada y validada
        /// </summary>
        /// <param name="colecionAFirmar">Objeto que contiene la lista a firmar </param>
        /// <param name="cadenaResult"> Cadenas a firmar, concatenadas y separadas por un char </param>
        /// <param name="separador">char para separar las cadenas </param>
        /// <returns>Objeto con la lista de contenedores firmados </returns>
        public static FirmaElectronica obtenerListaFirmada(FirmaElectronica colecionAFirmar, string cadenaResult, char separador)
        {
            try
            {
                string[] cadenas = cadenaResult.Split(separador);
                FirmaElectronica aux = colecionAFirmar;
                int i = 0;
                foreach (string cms in cadenas)
                {
                    Contenedor contenedor = aux.listaContenedores[i];
                    contenedor.cadenaCMS = cms;
                    aux.listaContenedores[i] = contenedor;
                    i++;
                }
                colecionAFirmar = aux;
            }
            catch (Exception ex)
            {
                colecionAFirmar.exception = ex;
            }
            return colecionAFirmar;
        }



    }
}