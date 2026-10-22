namespace SIRALAB_BL.negocio.Contacto
{
    public class ContactoArgs
    {
        public int ContactoID { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Correo { get; set; }
        public bool? RecibeAcuse { get; set; }
        public bool EsLaboratorio { get; set; }
    }
}