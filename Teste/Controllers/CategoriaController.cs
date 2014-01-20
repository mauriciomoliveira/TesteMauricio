using Infraestrutura;
using Modelo;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Teste.Controllers
{
    public class CategoriaController : Controller
    {
        //
        // GET: /Categoria/
        public ActionResult Index()
        {
            using (IRepositorio<Categoria> repositorio = Fabrica.Fabricar<IRepositorio<Categoria>>())
            {
                IList<Categoria> lista = repositorio.Listar();
                return View(lista);
            }
        }

        //
        // GET: /Categoria/Details/5
        public ActionResult Details(int id)
        {
            using (IRepositorio<Categoria> repositorio = Fabrica.Fabricar<IRepositorio<Categoria>>())
            {
                return View(repositorio.Obter(id));
            }
        }

        //
        // GET: /Categoria/Create
        public ActionResult Create()
        {            
            using (IRepositorio<Categoria> repositorio = Fabrica.Fabricar<IRepositorio<Categoria>>())
            {
                ViewBag.ListaCategorias = new List<Categoria>(repositorio.Listar());
            }
            return View();
        }

        //
        // POST: /Categoria/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection, Categoria categoria)
        {
            try
            {
                if (categoria != null)
                {
                    using (IRepositorio<Categoria> repositorio = Fabrica.Fabricar<IRepositorio<Categoria>>())
                    {
                        if ((categoria.CategoriaPaiId.HasValue) && (categoria.CategoriaPaiId.Value != 0))
                            categoria.CategoriaPai = repositorio.Obter(categoria.CategoriaPaiId.Value);
                        else
                            categoria.CategoriaPai = null;
                        repositorio.Criar(categoria);
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
        // GET: /Categoria/Edit/5
        public ActionResult Edit(int id)
        {
            using (IRepositorio<Categoria> repositorio = Fabrica.Fabricar<IRepositorio<Categoria>>())
            {
                ViewBag.ListaCategorias = new List<Categoria>(repositorio.Listar());
                ViewBag.ListaCategorias.Insert(0, new Categoria() { Id = 0, Descricao = "" });
                Categoria categoria = repositorio.Obter(id);
                categoria.CategoriaPaiId = (categoria.CategoriaPai != null) ? categoria.CategoriaPai.Id : (int?)null;
                return View(categoria);
            }
        }

        //
        // POST: /Categoria/Edit/5
        [HttpPost]
        public ActionResult Edit(FormCollection collection, Categoria categoria)
        {
            try
            {
                using (IRepositorio<Categoria> repositorio = Fabrica.Fabricar<IRepositorio<Categoria>>())
                {
                    Categoria categoriaPersistida = repositorio.Obter(categoria.Id);
                    categoriaPersistida.Descricao = categoria.Descricao;
                    if ((categoria.CategoriaPaiId.HasValue) && (categoria.CategoriaPaiId.Value != 0))
                        categoriaPersistida.CategoriaPai = repositorio.Obter(categoria.CategoriaPaiId.Value);
                    else
                        categoriaPersistida.CategoriaPai = null;
                    repositorio.Atualizar(categoriaPersistida);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Categoria/Delete/5
        public ActionResult Delete(int id)
        {
            using (IRepositorio<Categoria> repositorio = Fabrica.Fabricar<IRepositorio<Categoria>>())
            {
                return View(repositorio.Obter(id));
            }
        }

        //
        // POST: /Categoria/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                using (IRepositorio<Categoria> repositorio = Fabrica.Fabricar<IRepositorio<Categoria>>())
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
