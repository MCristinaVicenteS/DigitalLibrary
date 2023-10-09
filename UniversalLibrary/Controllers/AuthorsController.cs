using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversalLibrary.Data;
using UniversalLibrary.Data.Entities;
using UniversalLibrary.Helpers;
using UniversalLibrary.Models;

namespace UniversalLibrary.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IAuthorRepository _authorRepository;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;

        public AuthorsController(DataContext context, IUserHelper userHelper, IAuthorRepository authorRepository, IImageHelper imageHelper, IConverterHelper converterHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _authorRepository = authorRepository;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
        }

        // GET: Authors
        public async Task<IActionResult> Index()
        {
            return View(_authorRepository.GetAll().OrderBy(a => a.FirstName)); //vai ao repositorio e trás td os authors e ordena pelo nome
        }

        // GET: Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            //se o id n existir -> retorno um objecto do tipo NotFoundViewResult -> passa a view AuthorNotFound
            if (id == null)
            {
                return new NotFoundViewResult("AuthorNotFound");
            }

            var author = await _authorRepository.GetByIdAsync(id.Value);

            if (author == null)
            {
                return new NotFoundViewResult("AuthorNotFound");
            }

            return View(author);
        }

        // GET: Authors/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuthorViewModel model) //uso este modelo
        {
            if (ModelState.IsValid)
            {
                //carregar a imagem do author
                var path = string.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0) //se o model tiver imagem
                {
                    //usar este método do imageHelper -> envia o ficheiro e guarda nessa pasta -> authors
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "authors");
                }

                //converter o AuthorViewModel em Author -> para continuar a gravar o Author na BD
                var author = _converterHelper.ToAuthor(model, path, true); //é true pq ainda n tem Id

                //associar o user ao author, antes de ester ser criado
                author.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                await _authorRepository.CreateAsync(author); //se for válido fica guardado em memória
                return RedirectToAction(nameof(Index));      //qd estiver gravado -> redirecciona para a action Index -> mostra  a lista de authors

            }
            return View(model); //se n passar na validação -> deixa os dados nos campos mas n os grava
        }

        // GET: Authors/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("AuthorNotFound");
            }

            //Dupla segurança
            var author = await _authorRepository.GetByIdAsync(id.Value); //verifica na memória se o id existe ou n

            if (author == null)
            {
                return new NotFoundViewResult("AuthorNotFound");
            }

            //converter o author em AuthorViewModel -> para aparecer a imagem
            var model = _converterHelper.ToAuthorViewModel(author);

            return View(model); //se encontrar o author -> mostra-o
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AuthorViewModel model) //uso o modelo
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = model.ImageAuthor; //n é empty como no create -> caso n queira alterar a imagem

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "authors");
                    }

                    var author = _converterHelper.ToAuthor(model, path, false); //é false pq já tem id

                    author.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                    await _authorRepository.UpdateAsync(author);
                }

                catch (DbUpdateConcurrencyException)
                {
                    if (!await _authorRepository.ExistAsync(model.Id)) //se o author n existir ->retorna
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index)); //redireciona para o index do authors
            }
            return View(model); //se alg coisa correr mal -> retorna c o author como estava
        }

        // GET: Authors/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("AuthorNotFound");
            }

            var author = await _authorRepository.GetByIdAsync(id.Value);

            if (author == null)
            {
                return new NotFoundViewResult("AuthorNotFound");
            }

            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            try
            {
                await _authorRepository.DeleteAsync(author);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (true)
                {
                    ViewBag.ErrorTitle = $"{author.FirstName} {author.LastName}, may be in a book that is being used";
                    ViewBag.ErrorMessage = $"{author.FirstName} {author.LastName}, cannot be deleted until the book is borrowed.</br></br> " +
                        $"Once the loan ends, try deleting the author";
                }
            }

            return View("Error");
        }


        public IActionResult AuthorNotFound()
        {
            return View();
        }


        private bool AuthorExists(int id)
        {
            return _context.authors.Any(e => e.Id == id);
        }
    }
}
