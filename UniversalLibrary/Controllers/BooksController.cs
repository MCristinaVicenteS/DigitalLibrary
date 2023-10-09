using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Bcpg;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UniversalLibrary.Data;
using UniversalLibrary.Data.Entities;
using UniversalLibrary.Helpers;
using UniversalLibrary.Models;

namespace UniversalLibrary.Controllers
{
    public class BooksController : Controller
    {

        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IImageHelper _imageHelper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;

        public BooksController(IBookRepository bookRepository, IAuthorRepository authorRepository, IImageHelper imageHelper, IWebHostEnvironment webHostEnvironment, DataContext context, IUserHelper userHelper, IConverterHelper converterHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _imageHelper = imageHelper;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Books--> Nao precisa de POST porque so vai mostrar a lista
        public IActionResult Index()
        {
            return View(_bookRepository.GetAll().OrderBy(p => p.Title));
        }


        // GET: Books/Create
        [Authorize(Roles = "Admin")]        //só o Admin é que pode criar books
        public IActionResult Create()
        {
            var model = new BookViewModel();
            model.AvailableAuthors = _bookRepository.GetComboAuthors();
            model.AvailablePublishers = _bookRepository.GetComboPublisher();
            model.AvailableCategories = _bookRepository.GetComboCategories();
            model.AvailableLibrary = _bookRepository.GetComboLibrary();
            return View(model);
        }

        // POST: Books/Create ---> Adicionar o book na lista
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookViewModel model)
        {
            string userName = HttpContext.User.Identity.Name;
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0) //se o model tiver uma imagem
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "books");

                }

                //converter o BookViewModel em Book -> para continuar a gravar o Book na BD
                var book = _converterHelper.ToBook(model, path, true); //ainda n tem id -> fica true

                //Associar o user ao livro, antes de este ser criado -> pelo email             
                book.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                //Associar o user ao livro, antes de este ser criado -> pelo id
                string user = _context.Users.FirstOrDefault()?.Id;
                book.User = await _userHelper.GetUserByIdAsync(user);

                await _bookRepository.CreateAsync(book);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Books/Details/5 ---> Mostrar o detalhe de um Book
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("BookNotFound");
            }

            var book = await _context.Books
                .Include(l => l.Author)
                .Include(l => l.Category)
                .Include(l => l.BookPublisher)
                .Include(l => l.PhisicalLibrary)
                .FirstOrDefaultAsync(m => m.Id == id);

            //book = await _bookRepository.GetBookWithAuthorsAsync(id.Value);

            if (book == null)
            {
                return new NotFoundViewResult("BookNotFound");
            }
            
            //converter o book em BookViewModel -> para aparecer a imagem
            var model = _converterHelper.ToBookViewModel(book);

            return View(model);
        }


        // GET: Books/Edit/5 ---> Aqui usamos GetByIdAsync, porque para mostrar apenas um item por editar
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)  // o ? -> para o user ter de inserir um id no url
        {                        
            if (id == null)
            {
                return new NotFoundViewResult("BookNotFound");
            }

            var book = await _bookRepository.GetByIdAsync(id.Value);  //vai ver na memória se -> o id colocado no url existe ou n

            if (book == null)
            {
                return new NotFoundViewResult("BookNotFound");
            }

            //converter o book em BookViewModel -> para aparecer a imagem
            var model = _converterHelper.ToBookViewModel(book);

            model.AvailableAuthors = _bookRepository.GetComboAuthors();
            model.AvailablePublishers = _bookRepository.GetComboPublisher();
            model.AvailableCategories = _bookRepository.GetComboCategories();
            model.AvailableLibrary = _bookRepository.GetComboLibrary();

            return View(model);    //se encontrar o book -> mostra-o
        }

        // POST: Books/Edit/5 --> Aqui vamos usar o Update porque ja estamos a atualizar 
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = model.Image; //n é empty como no create -> para o caso de n querer alterar a imagem

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "books");
                    }
                                       

                    var book = _converterHelper.ToBook(model, path, false); //já tem id -> fica false

                    //Associar o user ao livro, antes de este ser criado              
                    book.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                    string user = _context.Users.FirstOrDefault()?.Id;
                    book.User = await _userHelper.GetUserByIdAsync(user);

                    await _bookRepository.UpdateAsync(book);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _bookRepository.ExistAsync(model.Id)) //se o book n existir (ex, alguem apagou) -> retorna
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index)); //no final -> redireciona para o index
            }
            return View(model); //se alg coisa correr mal -> retorna c o book como estava
        }

        // GET: Books/Delete/5 ---> Aqui vamos usar o GetByIdAsync, porque queremos mostrar que vamos apagar apenas um livro
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("BookNotFound");
            }

            var book = await _bookRepository.GetByIdAsync(id.Value);
            if (book == null)
            {
                return new NotFoundViewResult("BookNotFound");
            }

            return View(book);
        }

        // POST: Books/Delete/5 ---> Aqui é onde realmente vamos apagar o livro
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);

            try
            {
                //------ Delete image from wwwroot/image-->"DELETE"----
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "image", book.Image);
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
                //------ Delete image from wwwroot/image-->"DELETE"----

                await _bookRepository.DeleteAsync(book);
                return RedirectToAction(nameof(Index));
            }

            catch (DbUpdateException ex)        //dá erro no uptadte da BD -> se tentar apagar um livro q já está a ser emprestado
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE")) //Se o texto q vem no erro é != null e contém a palavra delete -> aparece a msg q foi escrita na viewbag                        
                {
                    ViewBag.ErrorTitle = $"{book.Title}, maybe being used.";
                    ViewBag.ErrorMessage = $"{book.Title}, cannot be deleted as it's being borrowed. </br></br>" +
                        $"Once the loan ends, try deleting the book.";
                    //NOTA: o br faz parágrafo pq na ViewBag da view Error usei o Html.Raw -> senão apareceria br e n faria parágrafo
                }

                return View("Error");  //especificar q o erro é por querer apagar
            }
        }


        public IActionResult BookNotFound()
        {
            return View();
        }

    }
}