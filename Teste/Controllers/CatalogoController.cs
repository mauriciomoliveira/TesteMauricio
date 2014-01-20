using Infraestrutura;
using Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Teste.Controllers
{
    public class CatalogoController : Controller
    {
        //
        // GET: /Catalogo/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ObterTodosProdutos()
        {
            using (IRepositorio<Categoria> repositorio = Fabrica.Fabricar<IRepositorio<Categoria>>())
            {
                IList<Categoria> lista = repositorio.Listar();
                using (IRepositorio<Produto> repositorioProduto = Fabrica.Fabricar<IRepositorio<Produto>>(repositorio.UnidadeTrabalho))
                {
                    Categoria naoCategorizados = new Categoria() { Descricao = "Não categorizados" };
                    foreach (Produto produto in repositorioProduto.Listar().Where(p => !p.Categorias.Any()))
                    {
                        naoCategorizados.Produtos.Add(produto);
                    }
                    lista.Add(naoCategorizados);
                }
                return View(lista);
            }
        }

    }
}