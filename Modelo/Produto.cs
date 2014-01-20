﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class Produto
    {
        public virtual int Id { get; set; }
        public virtual string Descricao { get; set; }
        public virtual ICollection<Categoria> Categorias { get; set; }

        public Produto()
        {
            Categorias = new List<Categoria>();
        }

        public override string ToString()
        {
            return string.Format("#{0} - {1}", Id, Descricao);
        }
    }
}
