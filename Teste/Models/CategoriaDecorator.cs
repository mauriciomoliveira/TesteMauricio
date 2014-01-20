using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Teste.Models
{
    public class CategoriaDecorator : Categoria
    {
        private Categoria _categoria;
        public new int Id { get { return (_categoria.Id); } set { _categoria.Id = value; } }
        public new string Descricao { get { return (_categoria.Descricao); } set { _categoria.Descricao = value; } }
        public new int? CategoriaPaiId { get { return (_categoria.CategoriaPaiId); } set { _categoria.CategoriaPaiId = value; } }
        public new Categoria CategoriaPai { get { return (_categoria.CategoriaPai); } set { _categoria.CategoriaPai = value; } }
        public new ICollection<Categoria> SubCategorias { get { return (_categoria.SubCategorias); } set { _categoria.SubCategorias = value; } }
        public new ICollection<Produto> Produtos { get { return (_categoria.Produtos); } set { _categoria.Produtos = value; } }
        public bool Selecionado { get; set; }

        public CategoriaDecorator()
        {
            _categoria = new Categoria();
        }
        public CategoriaDecorator(Categoria categoria)
        {
            _categoria = categoria;
        }

        public override string ToString()
        {
            return string.Format("#{0} - {1}", Id, Descricao);
        }
    }
}