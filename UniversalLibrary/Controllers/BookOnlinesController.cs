using System;
using System.Web;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversalLibrary.Data;
using UniversalLibrary.Data.Entities;
using UniversalLibrary.Helpers;
using UniversalLibrary.Models;
using Syncfusion.Pdf;
using iTextSharp.text.pdf.parser;
using Org.BouncyCastle.Bcpg;
using MathNet.Numerics;

namespace UniversalLibrary.Controllers
{    
    public class BookOnlinesController : Controller
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IBookOnlineRepository _bookOnlineRepository;
        private readonly IImageHelper _imageHelper;
        private readonly IFileHelper _fileHelper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConverterHelper _converterHelper;


        public BookOnlinesController(DataContext context, IUserHelper userHelper, IBookOnlineRepository bookOnlineRepository, IImageHelper imageHelper, IFileHelper fileHelper,IWebHostEnvironment webHostEnvironment, IConverterHelper converterHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _bookOnlineRepository = bookOnlineRepository;
            _imageHelper = imageHelper;
            _fileHelper = fileHelper;
            _webHostEnvironment = webHostEnvironment;
            _converterHelper = converterHelper;
        }

        // GET: BookOnlines
        public async Task<IActionResult> Index()
        {
            return View(_bookOnlineRepository.GetAll().OrderBy(p => p.Title));
        }

        // GET: BookOnlines/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("BookOnlineNotFound");
            }

            var bookOnline = await _context.BookOnlines
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.BookPublisher)
                .FirstOrDefaultAsync(m => m.Id == id);

            //var bookOnline = await _bookOnlineRepository.GetByIdAsync(id.Value);

            if (bookOnline == null)
            {
                return new NotFoundViewResult("BookOnlineNotFound");
            }

            var model = _converterHelper.ToBookOnlineViewModel(bookOnline);

            return View(model);
        }

        // GET: BookOnlines/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var model = new BookOnlineViewModel();
            
            model.AvailableAuthors = _bookOnlineRepository.GetComboAuthors();
            model.AvailablePublishers = _bookOnlineRepository.GetComboPublisher();
            model.AvailableCategories = _bookOnlineRepository.GetComboCategories();

            return View(model);
        }

        // POST: BookOnlines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookOnlineViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;
                var pathFile = string.Empty;
                                    

                if(model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    path = await _imageHelper.UploadImageAsync(model.ImageFile, "booksOnline");
                }

                if (model.PdfFile != null && model.PdfFile.Length > 0)
                {
                    if (model.PdfFile.Length > 5 * 1024 * 1024 && !model.PdfFile.ContentType.Equals("application/pdf"))
                    {
                        ViewBag.ErrorTitle = $"{model.Title}, cannot be added";
                        ViewBag.ErrorMessage = $"{model.Title}, The document need to be a .Pdf </br></br>" +
                            $"and the pdf document can only contain 5MB. </br></br> +" +
                            $"Try again.";
                    }

                    pathFile = await _fileHelper.UploadFileAsync(model.PdfFile, "booksOnlineFiles");
                }

                var book = _converterHelper.ToBookOnline(model, path, pathFile, true);

                book.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                string user = _context.Users.FirstOrDefault()?.Id;
                book.User = await _userHelper.GetUserByIdAsync(user);

                await _bookOnlineRepository.CreateAsync(book);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }



        // GET: BookOnlines/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("BookOnlineNotFound");
            }

            var bookOnline = await _bookOnlineRepository.GetByIdAsync(id.Value);

            if (bookOnline == null)
            {
                return new NotFoundViewResult("BookOnlineNotFound");
            }

            var model = _converterHelper.ToBookOnlineViewModel(bookOnline);
            model.AvailableAuthors = _bookOnlineRepository.GetComboAuthors();
            model.AvailablePublishers = _bookOnlineRepository.GetComboPublisher();
            model.AvailableCategories = _bookOnlineRepository.GetComboCategories();

            return View(model);
        }

        // POST: BookOnlines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookOnlineViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = model.Image;
                    var pathFile = model.Pdf;

                    if(model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        path = await _imageHelper.UploadImageAsync(model.ImageFile, "booksOnline");
                    }


                    if (model.PdfFile != null && model.PdfFile.Length > 0)
                    {
                        if (model.PdfFile.Length > 5 * 1024 * 1024 && !model.PdfFile.ContentType.Equals("application/pdf"))
                        {
                            ViewBag.ErrorTitle = $"{model.Title}, cannot be added";
                            ViewBag.ErrorMessage = $"{model.Title}, The document need to be a .Pdf </br></br>" +
                                $"and the pdf document can only contain 5MB. </br></br> +" +
                                $"Try again.";
                        }

                        pathFile = await _fileHelper.UploadFileAsync(model.PdfFile, "booksOnlineFiles");
                    }


                    var book = _converterHelper.ToBookOnline(model, path, pathFile, false);

                    book.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

                    string user = _context.Users.FirstOrDefault()?.Id;
                    book.User = await _userHelper.GetUserByIdAsync(user);

                    await _bookOnlineRepository.UpdateAsync(book);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await _bookOnlineRepository.ExistAsync(model.Id))
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

        // GET: BookOnlines/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("BookOnlineNotFound");
            }

            var bookOnline = await _bookOnlineRepository.GetByIdAsync(id.Value);

            if (bookOnline == null)
            {
                return new NotFoundViewResult("BookOnlineNotFound");
            }

            return View(bookOnline);
        }

        // POST: BookOnlines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _bookOnlineRepository.GetByIdAsync(id);

            try
            {
                //------ Delete image from wwwroot/image-->"DELETE"----
                var imagePath = System.IO.Path.Combine(_webHostEnvironment.WebRootPath, "image", book.Image);
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
                //------ Delete image from wwwroot/image-->"DELETE"----

                await _bookOnlineRepository.DeleteAsync(book);
                return RedirectToAction(nameof(Index));
            }

            catch (DbUpdateException ex)
            {
                if(ex.InnerException != null && ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"{book.Title}, maybe being used.";
                    ViewBag.ErrorMessage = $"{book.Title}, cannot be deleted as it's being borrowed. </br></br>" +
                        $"Once the loan ends, try deleting the book.";
                }

                return View("Error");
            }
        }


        public async Task<IActionResult> ShowPdf(int ? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("BookOnlineNotFound");
            }

            var bookOnline = await _bookOnlineRepository.GetByIdAsync(id.Value);

            if (bookOnline == null)
            {
                return new NotFoundViewResult("BookOnlineNotFound");
            }

            string filePath = bookOnline.Pdf;
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return File(fileStream, "application/pdf");

            //string filePath = bookOnline.Pdf;
            //return File(filePath, "application/pdf");


            //string filePath = bookOnline.Pdf;
            //byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            //return File(fileBytes, "application/pdf");


            //var model = _converterHelper.ToBookOnlineViewModel(bookOnline);

            //return View(model);
        }




        //public ActionResult MergePDF()
        //{
        //    //Creates the new PDF document.
        //    PdfDocument finalDoc = new PdfDocument();

        //    //Creates a string array of source files to be merged.
        //    //string[] source = System.IO.Directory.GetFiles(Server.MapPath("~/App_Data/"), "*.pdf");
        //    string[] source = Directory.GetFiles(@"c:\", "*.pdf");

        //    //Merges PDFDocument.
        //    PdfDocument.Merge(finalDoc, source);

        //    //Open the document in browser after saving it.
        //    finalDoc.Save("Output.pdf", HttpContext.ApplicationInstance.Response, HttpReadType.Save);


        //    //Closes the document
        //    finalDoc.Close(true);

        //    return View();
        //}


        //private string videoAddress = "~/App_Data/Videos";

        //[HttpPost]
        //public string MultiUpload(string id, string fileName)
        //{
        //    var chunkNumber = id;
        //    var chunks = Request.InputStream;
        //    string path = Server.MapPath(videoAddress + "/Temp");
        //    string newpath = Path.Combine(path, fileName + chunkNumber);
        //    using (FileStream fs = System.IO.File.Create(newpath))
        //    {
        //        byte[] bytes = new byte[3757000];
        //        int bytesRead;
        //        while ((bytesRead = Request.InputStream.Read(bytes, 0, bytes.Length)) > 0)
        //        {
        //            fs.Write(bytes, 0, bytesRead);
        //        }
        //    }
        //    return "done";
        //}

        //[HttpPost]
        //public string UploadComplete(string fileName, string complete)
        //{
        //    string tempPath = Server.MapPath(videoAddress + "/Temp");
        //    string videoPath = Server.MapPath(videoAddress);
        //    string newPath = Path.Combine(tempPath, fileName);
        //    if (complete == "1")
        //    {
        //        string[] filePaths = Directory.GetFiles(tempPath).Where(p => p.Contains(fileName)).OrderBy(p => Int32.Parse(p.Replace(fileName, "$").Split('$')[1])).ToArray();
        //        foreach (string filePath in filePaths)
        //        {
        //            MergeFiles(newPath, filePath);
        //        }
        //    }
        //    System.IO.File.Move(Path.Combine(tempPath, fileName), Path.Combine(videoPath, fileName));
        //    return "success";
        //}

        //private static void MergeFiles(string file1, string file2)
        //{
        //    FileStream fs1 = null;
        //    FileStream fs2 = null;
        //    try
        //    {
        //        fs1 = System.IO.File.Open(file1, FileMode.Append);
        //        fs2 = System.IO.File.Open(file2, FileMode.Open);
        //        byte[] fs2Content = new byte[fs2.Length];
        //        fs2.Read(fs2Content, 0, (int)fs2.Length);
        //        fs1.Write(fs2Content, 0, (int)fs2.Length);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message + " : " + ex.StackTrace);
        //    }
        //    finally
        //    {
        //        if (fs1 != null) fs1.Close();
        //        if (fs2 != null) fs2.Close();
        //        System.IO.File.Delete(file2);
        //    }
        //}




        //********


        public IActionResult BookOnlineNotFound()
        {
            return View();
        }


        private bool BookOnlineExists(int id)
        {
            return _context.BookOnlines.Any(e => e.Id == id);
        }
    }
}
