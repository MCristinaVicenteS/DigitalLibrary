using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using UniversalLibrary.Data;
using UniversalLibrary.Helpers;
using UniversalLibrary.Models;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using MimeKit;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.Extensions.Configuration;

namespace UniversalLibrary.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly DataContext _context;
        private readonly ILibraryFeedbackRepository _libraryFeedbackRepository;
        private readonly ILogger<HomeController> _logger;
        private readonly IBookRepository _bookRepository;
        private readonly IBookOnlineRepository _bookOnlineRepository;
        private readonly IBookPublisherRepository _bookPublisherRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConverterHelper _converterHelper;
        private readonly IUserHelper _userHelper;
        private readonly IConfiguration _configuration; //para aceder ao app.JSON od estão as configurações do Token
        private readonly IMailHelper _mailHelper;
        public HomeController(DataContext context,
            ILogger<HomeController> logger, 
            IBookRepository bookRepository, 
            IBookOnlineRepository bookOnlineRepository, 
            IBookPublisherRepository bookPublisherRepository,
            IAuthorRepository authorRepository, 
            ICategoryRepository categoryRepository, 
            ILibraryFeedbackRepository libraryFeedbackRepository, 
            IWebHostEnvironment webHostEnvironment, 
            IConverterHelper converterHelper, IUserHelper userHelper, 
            IConfiguration configuration, 
            IMailHelper mailHelper)
        {
            _context = context;
            _libraryFeedbackRepository = libraryFeedbackRepository;
            _logger = logger;
            _bookRepository = bookRepository;
            _bookOnlineRepository = bookOnlineRepository;
            _bookPublisherRepository = bookPublisherRepository;
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
            _converterHelper = converterHelper;

            _userHelper = userHelper;
            _configuration = configuration;
            _mailHelper = mailHelper;
        }


        //********************SendEmailFromUser************************************

        // Ação GET para exibir o formulário de envio de email
        public ActionResult Contact() 
        {
            return View();
        }

        // Codigo post para enviar email
        [HttpPost]
        public ActionResult SendEmailContact(EmailViewModel model)    
        {
            if (!ModelState.IsValid) {
                ViewData["message"] = "Information not Valid!";
                return View("Contact", model);
            }

            // Construir e enviar a mensagem por email
            var emailMessage = new MailMessage();
            emailMessage.Subject = "Contact From" +  model.Name;
            emailMessage.From = new MailAddress("UniversalLibrary@hotmail.com");
            emailMessage.To.Add("UniversalLibrary@hotmail.com");
            emailMessage.IsBodyHtml = true;

            //Corpo da Mensagem
            emailMessage.Body = "<p>Name: " + model.Name + "</p><p>E-mail: " + model.Email + "</p>" +
                "<p>Message: " + model.Message;

            // Criar A porta e a forma de entrada da mensagem
            var client = new SmtpClient("smtp.Outlook.com", 587);

            client.Credentials = new NetworkCredential("UniversalLibrary@hotmail.com", "cinel123!");
            client.EnableSsl = true;

            // Enviar a Mensagem

            try
            {
                client.Send(emailMessage);
            }
            catch (Exception ex)
            {

                ViewData["message"] = "The Message has fail:" + ex.Message;
            }


            return View("SendEmailContact");
        }

        
        //********************SendEmailFromUserClose************************************

        public async Task<IActionResult> Index()
        {
            // Recuperar todos os feedbacks da biblioteca usando o repositório
            var feedbacks = await _libraryFeedbackRepository.GetFeedbacksForHomePageAsync();
            return View(feedbacks);
        }

        // ---Codigo que vai mostrar os livros na views do Site//------
        public IActionResult Books()
        {
            var books = _bookRepository.GetAll().OrderBy(p => p.Title);
            return View(books);

        }

        // ---Codigo que vai mostrar os livros online na views do Site //------        
        public IActionResult BookOnline()
        {
            var bookOnlines = _bookOnlineRepository.GetAll().OrderBy(p => p.Title);
            return View(bookOnlines);
        }

        public IActionResult BookPublisher()
        {
            var bookPublisher = _bookPublisherRepository.GetAll().OrderBy(p => p.PublisherName);
            return View(bookPublisher);
        }

        public IActionResult Author()
        {
            var author = _authorRepository.GetAll().OrderBy(p => p.FirstName);
            return View(author);
        }

        public IActionResult Category()
        {
            var category = _categoryRepository.GetAll().OrderBy(p => p.CategoryName);
            return View(category);
        }

        public async Task<IActionResult> PhisicalLibrary()
        {
            return View(await _context.PhisicalLibraries.ToListAsync());
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
                .FirstOrDefaultAsync(m => m.Id == id);

            if (book == null)
            {
                return new NotFoundViewResult("BookNotFound");
            }

            //converter o book em BookViewModel -> para aparecer a imagem
            var model = _converterHelper.ToBookViewModel(book);

            return View(model);
        }

        //GET: BookOnlines/DetailsBookOnline/5
        public async Task<IActionResult> DetailsBookOnline(int ? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("BookOnlineNotFound"); 
            }

            var bookOnline = await _context.BookOnlines
                .Include(l => l.Author)
                .Include(l => l.Category)
                .Include(l => l.BookPublisher)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (bookOnline == null)
            {
                return new NotFoundViewResult("BookOnlineNotFound");
            }

            var model = _converterHelper.ToBookOnlineViewModel(bookOnline);

            return View(model);
        }

        //GET: BookPublishers/DetailsBookPublisher/5
        public async Task<IActionResult> DetailsBookPublisher(int ? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookPublisher = await _bookPublisherRepository.GetByIdAsync(id.Value);

            if (bookPublisher == null)
            {
                return NotFound();
            }

            return View(bookPublisher);
        }

        //GET: Authors/DetailsAuthor/5
        public async Task<IActionResult> DetailsAuthor(int ? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var author = await _authorRepository.GetByIdAsync(id.Value);

            if(author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        //GET: Categories/DetailsCategory/5
        public async Task<IActionResult> DetailsCategory (int ? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryRepository.GetByIdAsync(id.Value);

            if(category == null)
            {
                return NotFound();
            }

            return View(category);
        }


        //GET: PhisicalLibraries/DetailsPhisicalLibrary/5
        public async Task<IActionResult> DetailsPhisicalLibrary(int ? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var library = await _context.PhisicalLibraries.FindAsync(id.Value);

            if (library == null)
            {
                return NotFound();
            }

            return View(library);
        }


        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult BookNotFound()
        {
            return View();
        }

        public IActionResult BookOnlineNotFound()
        {
            return View();
        }

    }
}
