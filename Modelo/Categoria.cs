using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class Categoria
    {
        public virtual int Id { get; set; }
        public virtual string Descricao { get; set; }
        public virtual int? CategoriaPaiId { get; set; }
        public virtual Categoria CategoriaPai { get; set; }
        public virtual ICollection<Categoria> SubCategorias { get; set; }
        public virtual ICollection<Produto> Produtos { get; set; }

        public virtual bool PossuiProdutos
        {
            get
            {
                return (this.Produtos.Any() || this.SubCategorias.Any(sc => sc.PossuiProdutos));
            }
        }

        public virtual IEnumerable<Produto> TodosProdutos
        {
            get
            {
                List<Produto> todosProdutos = new List<Produto>();
                if (this.Produtos.Any())
                    todosProdutos.AddRange(this.Produtos);

                if (this.SubCategorias.Any())
                {
                    foreach (Categoria categoria in this.SubCategorias)
                    {
                        todosProdutos.AddRange(categoria.TodosProdutos);
                    }
                }
                return (todosProdutos.Distinct().OrderBy(p => p.Descricao));
            }
        }

        public Categoria()
        {
            SubCategorias = new List<Categoria>();
            Produtos = new List<Produto>();
        }
        public override string ToString()
        {
            return string.Format("#{0} - {1}", Id, Descricao);
        }
    }
}
