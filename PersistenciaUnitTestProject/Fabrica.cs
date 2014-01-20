using Infraestrutura;
using Microsoft.Practices.Unity;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistenciaUnitTestProject
{
    public class Fabrica
    {
        public static T Fabricar<T>(IUnidadeTrabalho unidadeTrabalho = null) where T : class
        {
            using (UnityContainer container = new UnityContainer())
            {
                T fabricado = null;
                container.RegisterType<IUnidadeTrabalho, UnidadeTrabalho>();
                if (unidadeTrabalho == null)
                {
                    container.RegisterType(typeof(IRepositorio<>), typeof(RepositorioBase<>), new InjectionProperty("UnidadeTrabalho"));

                }
                else
                {
                    container.RegisterType(typeof(IRepositorio<>), typeof(RepositorioBase<>), new InjectionProperty("UnidadeTrabalho", unidadeTrabalho));
                }
                fabricado = container.Resolve<T>();

                return (fabricado);
            }
        }
    }
}
