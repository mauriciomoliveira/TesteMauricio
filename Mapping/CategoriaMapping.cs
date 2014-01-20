using FluentNHibernate.Mapping;
using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping
{
    public class CategoriaMapping : ClassMap<Categoria>
    {
        public CategoriaMapping()
        {
            Id(x => x.Id);
            Map(x => x.Descricao);
            //Map(x => x.CategoriaPaiId).Column("[CategoriaPai_id]");
            References(x => x.CategoriaPai).Column("[CategoriaPai_id]").Cascade.SaveUpdate();            
            HasMany(x => x.SubCategorias).Cascade.SaveUpdate();
            HasManyToMany(x => x.Produtos).Cascade.SaveUpdate();

        }
    }
}
