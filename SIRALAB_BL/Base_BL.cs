using SIRALAB_DAL;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;

namespace SIRALAB_BL
{
    public class Base_BL<T> where T : class  
    {
        public Siralab_Entities dbContext { get { return SingletonEntity; } }
        public ObjectContext dbObjectContext { get { return ((IObjectContextAdapter)dbContext).ObjectContext; } }
        public Repository<T> Repositorio { get; private set; }
        public T Entidad { get; set; }
        public string Mensaje { get; set; }

        private Siralab_Entities SingletonEntity;
        public Base_BL()
        {
            //SingletonEntity = SingletonContext.Instance.Context;
            SingletonEntity = new Siralab_Entities();
            Repositorio = new Repository<T>(dbContext);
        }
    }
}
