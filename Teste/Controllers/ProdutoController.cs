using Infraestrutura;
using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Teste.Models;

namespace Teste.Controllers
{
    public class ProdutoController : Controller
    {
        //
        // GET: /Produto/
        public ActionResult Index()
        {
            using (IRepositorio<Produto> repositorio = Fabrica.Fabricar<IRepositorio<Produto>>())
            {
                IList<Produto> lista = repositorio.Listar();
                return View(lista);
            }
        }

        //
        // GET: /Produto/Details/5
        public ActionResult Details(int id)
        {
            using (IRepositorio<Produto> repositorio = Fabrica.Fabricar<IRepositorio<Produto>>())
            {
                return View(repositorio.Obter(id));
            }
        }

        //
        // GET: /Produto/Create
        public ActionResult Create()
        {
            ProdutoDecorator produto = new ProdutoDecorator(new Produto());
            using (IRepositorio<Categoria> repositorio = Fabrica.Fabricar<IRepositorio<Categoria>>())
            {
                foreach (var categoria in repositorio.Listar())
                {
                    produto.Categorias.Add(new CategoriaDecorator(categoria));
                }
            }
            produto.Categorias = produto.Categorias.OrderBy(c => c.Descricao).ToList();
            return View(produto);
        }

        //
        // POST: /Produto/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection, ProdutoDecorator produto)
        {
            try
            {
                if (produto != null)
                {
                    Produto novoProduto = new Produto() { Descricao = produto.Descricao };
                    using (IRepositorio<Categoria> repositorio = Fabrica.Fabricar<IRepositorio<Categoria>>())
                    {
                        foreach (Categoria categoria in repositorio.Listar().Where(c => produto.Categorias.Any(pc => (pc.Selecionado) && (pc.Id == c.Id))))
                        {
                            novoProduto.Categorias.Add(categoria);
                        }

                        using (IRepositorio<Produto> repositorioProduto = Fabrica.Fabricar<IRepositorio<Produto>>(repositorio.UnidadeTrabalho))
                        {
                            repositorioProduto.Criar(novoProduto);
                        }
                    }

                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Produto/Edit/5
        public ActionResult Edit(int id)
        {
            using (IRepositorio<Produto> repositorio = Fabrica.Fabricar<IRepositorio<Produto>>())
            {
                ProdutoDecorator produto = new ProdutoDecorator(repositorio.Obter(id));
                using (IRepositorio<Categoria> repositorioCategoria = Fabrica.Fabricar<IRepositorio<Categoria>>())
                {
                    foreach (Categoria categoria in repositorioCategoria.Listar().Where(c => !produto.Categorias.Any(pc => pc.Id == c.Id)))
                    {
                        produto.Categorias.Add(new CategoriaDecorator(categoria));
                    }
                }
                produto.Categorias = produto.Categorias.OrderBy(c => c.Descricao).ToList();
                return View(produto);
            }
        }

        //
        // POST: /Produto/Edit/5
        [HttpPost]
        public ActionResult Edit(FormCollection collection, ProdutoDecorator produto)
        {
            try
            {
                using (IRepositorio<Produto> repositorio = Fabrica.Fabricar<IRepositorio<Produto>>())
                {
                    Produto produtoPersistido = repositorio.Obter(produto.Id);
                    produtoPersistido.Descricao = produto.Descricao;
                    using (IRepositorio<Categoria> repositorioCategoria = Fabrica.Fabricar<IRepositorio<Categoria>>(repositorio.UnidadeTrabalho))
                    {
                        produtoPersistido.Categorias = new List<Categoria>(repositorioCategoria.Listar().Where(c => produto.Categorias.Any(pc => (pc.Selecionado) && (pc.Id == c.Id))));
                    }
                    repositorio.Atualizar(produtoPersistido);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Produto/Delete/5
        public ActionResult Delete(int id)
        {
            using (IRepositorio<Produto> repositorio = Fabrica.Fabricar<IRepositorio<Produto>>())
            {
                return View(repositorio.Obter(id));
            }
        }

        //
        // POST: /Produto/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                using (IRepositorio<Produto> repositorio = Fabrica.Fabricar<IRepositorio<Produto>>())
                {
                    repositorio.Remover(repositorio.Obter(id));
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
