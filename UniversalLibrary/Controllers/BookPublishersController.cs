using System;
using System.Collections.Generic;
using System.Data;
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
    public class BookPublishersController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IBookPublisherRepository _bookPublisherRepository;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;

        public BookPublishersController(DataContext context, IUserHelper userHelper, IBookPublisherRepository bookPublisherRepository, IImageHelper imageHelper, IConverterHelper converterHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _bookPublisherRepository = bookPublisherRepository;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;

        }

        // GET: BookPublishers
        public async Task<IActionResult> Index()
        {
            return View(_bookPublisherRepository.GetAll().OrderBy(p => p.PublisherName)); //vai ao repositorio e trás td as editoras e ordena por nome
        }

        // GET: BookPublishers/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            //se o id n existir -> retorno um objecto do tipo NotFoundViewResult -> e passo a view q quero mostrar -> PublisherNotFound
            if (id == null)
            {
                return new NotFoundViewResult("PublisherNotFound");
            }

            var bookPublisher = await _bookPublisherRepository.GetByIdAsync(id.Value);
                        
            if (bookPublisher == null)
            {
                return new NotFoundViewResult("PublisherNotFound");
            }

            return View(bookPublisher);
        }

        // GET: BookPublishers/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: BookPublishers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookPublisherViewModel model) //usa este modelo
        {          
            if (ModelState.IsValid)
            {
                //vai buscar a imagem do logotipo da editora
                var path = string.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0) //se o model tiver uma imagem
                {
                    //usar este método do imagehelper -> envia o ficheiro e guarda nessa pasta -> bookPublisher                    
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "bookPublishers");
                }

                //converter o BookPublisherViewModel em BookPublisher -> para continuar a gravar o BookPublisher na BD
                var bookPublisher = _converterHelper.ToBookPublisher(model, path, true); //ainda n tem id -> fica true

                //Associar o user à editora, antes de esta ser criada                
                bookPublisher.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                await _bookPublisherRepository.CreateAsync(bookPublisher); //se for válido -> fica guardado em memória
                return RedirectToAction(nameof(Index)); //qd estiver gravado -> redireciona para a action index -> mostra a lista de editoras
            }

            return View(model); //se n passar na validação -> deixa os dados nos campos mas n os grava
        }

        // GET: BookPublishers/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("PublisherNotFound");
            }

            var bookPublisher = await _bookPublisherRepository.GetByIdAsync(id.Value); //vai ver na memória se -> o id colocado no url existe ou n

            if (bookPublisher == null)
            {
                return new NotFoundViewResult("PublisherNotFound");
            }

            //converter o bookPublisher em BookPublisherViewModel -> para aparecer a imagem
            var model = _converterHelper.ToBookPublisherViewModel(bookPublisher);

            return View(model); //se encontrar o bookPublisher -> mostra-o
        }

        // POST: BookPublishers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookPublisherViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = model.ImagLog; //n é empty como no create -> para o caso de n querer alterar a imagem

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "bookPublishers");
                    }

                    var bookPublisher = _converterHelper.ToBookPublisher(model, path, false); //já tem id -> fica false


                    bookPublisher.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                    await _bookPublisherRepository.UpdateAsync(bookPublisher);
                }

                catch (DbUpdateConcurrencyException)
                {
                    if (!await _bookPublisherRepository.ExistAsync(model.Id)) //se o book n existir (ex, alguem apagou) -> retorna
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));  //no final -> redireciona para o index
            }
            return View(model);  //se alg coisa correr mal -> retorna c o bookPublisher como estava
        }

        // GET: BookPublishers/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("PublisherNotFound");
            }

            var bookPublisher = await _bookPublisherRepository.GetByIdAsync(id.Value);

            if (bookPublisher == null)
            {
                return new NotFoundViewResult("PublisherNotFound");
            }

            return View(bookPublisher);
        }

        // POST: BookPublishers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookPublisher = await _bookPublisherRepository.GetByIdAsync(id);

            try
            {
                await _bookPublisherRepository.DeleteAsync(bookPublisher);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (true)
                {
                    ViewBag.ErrorTitle = $"{bookPublisher.PublisherName}, maybe being used.";
                    ViewBag.ErrorMessage = $"{bookPublisher.PublisherName}, cannot be deleted as it's being borrowed a book of this book publisher. </br></br>" +
                        $"Once the loan ends, try deleting the book publisher.";
                }

                return View("Error");
            }
        }


        public IActionResult PublisherNotFound()
        {
            return View();
        }


        private bool BookPublisherExists(int id)
        {
            return _context.BookPublishers.Any(e => e.Id == id);
        }
    }
}
