using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Infraestrutura;
using Modelo;
using System.Linq;

namespace PersistenciaUnitTestProject
{
    [TestClass]
    public class PersistenciaUnitTest
    {
        private static int _categoriaTestePrincipalId;
        [ClassInitialize]
        public static void Popular(TestContext context)
        {
            Guid guid = Guid.NewGuid();
            using (IRepositorio<Categoria> repositorio = Fabrica.Fabricar<IRepositorio<Categoria>>())
            {
                Categoria categoria = new Categoria() { Descricao = "Categoria de Teste Principal" + guid.ToString() };
                repositorio.Criar(categoria);
                _categoriaTestePrincipalId = categoria.Id;
            }
        }

        [TestMethod]
        public void IncluirNovaCategoria()
        {
            Guid guid = Guid.NewGuid();
            using (IRepositorio<Categoria> repositorio = Fabrica.Fabricar<IRepositorio<Categoria>>())
            {
                Categoria categoria = new Categoria() { Descricao = "Subcategoria de Teste " + guid.ToString() };
                repositorio.Criar(categoria);
            }
            using (IRepositorio<Categoria> repositorio = Fabrica.Fabricar<IRepositorio<Categoria>>())
            {
                Categoria categoria = repositorio.Listar().FirstOrDefault(c => c.Descricao.Equals("Subcategoria de Teste " + guid.ToString()));
                Assert.IsTrue(categoria != null);
            }
        }

        [TestMethod]
        public void IncluirSubCategoria()
        {
            Guid guid = Guid.NewGuid();
            using (IRepositorio<Categoria> repositorio = Fabrica.Fabricar<IRepositorio<Categoria>>())
            {
                Categoria subcategoria = new Categoria() { Descricao = "Categoria de Teste " + guid.ToString(), CategoriaPai = repositorio.Obter(_categoriaTestePrincipalId) };
                repositorio.Criar(subcategoria);
            }
            using (IRepositorio<Categoria> repositorio = Fabrica.Fabricar<IRepositorio<Categoria>>())
            {
                Categoria subcategoria = repositorio.Listar().FirstOrDefault(c => c.Descricao.Equals("Categoria de Teste " + guid.ToString()));
                Assert.IsTrue(subcategoria != null);
                Assert.IsTrue(subcategoria.CategoriaPai.Id == _categoriaTestePrincipalId);
            }
        }

        [TestMethod]
        public void ObterCategoriaExistente()
        {
            using (IRepositorio<Categoria> repositorio = Fabrica.Fabricar<IRepositorio<Categoria>>())
            {
                Categoria categoria = repositorio.Obter(_categoriaTestePrincipalId);
                Assert.IsTrue(categoria != null);
            }
        }

        [TestMethod]
        public void ObterCategoriaInexistente()
        {
            using (IRepositorio<Categoria> repositorio = Fabrica.Fabricar<IRepositorio<Categoria>>())
            {
                Categoria categoria = repositorio.Obter(Int32.MaxValue);
                Assert.IsTrue(categoria == null);
            }
        }
    }
}
