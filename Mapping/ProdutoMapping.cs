using FluentNHibernate.Mapping;
using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping
{
    public class ProdutoMapping: ClassMap<Produto>
    {
        public ProdutoMapping()
        {
            Id(x => x.Id);
            Map(x => x.Descricao);

            HasManyToMany(x => x.Categorias).Cascade.SaveUpdate();
        }
    }
}
