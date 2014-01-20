using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Teste.Models
{
    public class ProdutoDecorator : Produto
    {
        private Produto _produto;
        public new int Id { get { return (_produto.Id); } set { _produto.Id = value; } }
        public new string Descricao { get { return (_produto.Descricao); } set { _produto.Descricao = value; } }
        public new List<CategoriaDecorator> Categorias { get; set; }

        public ProdutoDecorator()
        {
            _produto = new Produto();
            Categorias = new List<CategoriaDecorator>();
        }
        public ProdutoDecorator(Produto produto)
        {
            _produto = produto;
            Categorias = new List<CategoriaDecorator>();
            foreach (Categoria categoria in produto.Categorias)
            {
                CategoriaDecorator decorator = new CategoriaDecorator(categoria);
                decorator.Selecionado = true;
                Categorias.Add(decorator);
            }
        }

        public override string ToString()
        {
            return string.Format("#{0} - {1}", Id, Descricao);
        }
    }
}