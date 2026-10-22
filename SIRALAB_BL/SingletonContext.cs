using SIRALAB_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIRALAB_BL
{
    public class SingletonContext
    {
        private static SingletonContext instance;

        private Siralab_Entities SE_Context;
        public Siralab_Entities Context { get {  return SE_Context; } }
        private SingletonContext() { SE_Context = new Siralab_Entities(); }
        public static SingletonContext Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SingletonContext();
                }

                return instance;
            }
        }
    }
}
