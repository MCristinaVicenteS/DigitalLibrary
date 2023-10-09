using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using UniversalLibrary.Data;
using UniversalLibrary.Data.Entities;
using UniversalLibrary.Helpers;
using UniversalLibrary.Models;

namespace UniversalLibrary.Controllers
{
    public class ReadersController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IReaderRepository _readerRepository;
        private readonly IImageHelper _imageHelper;
        private readonly IConverterHelper _converterHelper;

        public ReadersController(DataContext context, IUserHelper userHelper, IReaderRepository readerRepository, IImageHelper imageHelper, IConverterHelper converterHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _readerRepository = readerRepository;
            _imageHelper = imageHelper;
            _converterHelper = converterHelper;
        }

        public IActionResult Index()
        {
            return View(_readerRepository.GetAll().OrderBy(p => p.FirstName));
        }


        //Get para o Employee criar os readers -> no momento do Phsysical Loan
        public IActionResult Create() 
        {
            var model = new ReaderViewModel();
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReaderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0) //se o model tiver uma imagem
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "readers");
                }
                                
                var reader = _converterHelper.ToReader(model, path, true); //ainda n tem id -> fica true

                string user = _context.Users.FirstOrDefault()?.Id;
                reader.User = await _userHelper.GetUserByIdAsync(user);

                await _readerRepository.CreateAsync(reader);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ReaderNotFound");
            }

            var reader = await _readerRepository.GetByIdAsync(id.Value);

            if (reader== null)
            {
                return new NotFoundViewResult("ReaderNotFound");
            }

            var model = _converterHelper.ToReaderViewModel(reader);

            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ReaderNotFound");
            }

            var reader = await _readerRepository.GetByIdAsync(id.Value);  //vai ver na memória se -> o id colocado no url existe ou n

            if (reader== null)
            {
                return new NotFoundViewResult("ReaderNotFound");
            }

            var model = _converterHelper.ToReaderViewModel(reader);            

            return View(model);    
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ReaderViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = model.Image; //n é empty como no create -> para o caso de n querer alterar a imagem

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "readers");
                    }


                    var reader = _converterHelper.ToReader(model, path, false); //já tem id -> fica false
         
                    string user = _context.Users.FirstOrDefault()?.Id;
                    reader.User = await _userHelper.GetUserByIdAsync(user);

                    await _readerRepository.UpdateAsync(reader);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _readerRepository.ExistAsync(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model); 
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ReaderNotFound");
            }

            var reader = await _readerRepository.GetByIdAsync(id.Value);
            if (reader == null)
            {
                return new NotFoundViewResult("ReaderNotFound");
            }

            return View(reader);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reader = await _readerRepository.GetByIdAsync(id);

            try
            {
                await _readerRepository.DeleteAsync(reader);
                return RedirectToAction(nameof(Index));
            }

            catch (DbUpdateException ex)       
            {
                if (ex.InnerException != null && ex.InnerException.Message.Contains("DELETE")) //Se o texto q vem no erro é != null e contém a palavra delete -> aparece a msg q foi escrita na viewbag                        
                {
                    ViewBag.ErrorTitle = $"{reader.FullName}, maybe being used.";
                    ViewBag.ErrorMessage = $"{reader.FullName}, cannot be deleted while loaning a book. </br></br>" +
                        $"Once the loan ends, try deleting the reader.";
                    //NOTA: o br faz parágrafo pq na ViewBag da view Error usei o Html.Raw -> senão apareceria br e n faria parágrafo
                }

                return View("Error");  //especificar q o erro é por querer apagar
            }           
        }

        public IActionResult ReaderNotFound()
        {
            return View();
        }
    }
}
