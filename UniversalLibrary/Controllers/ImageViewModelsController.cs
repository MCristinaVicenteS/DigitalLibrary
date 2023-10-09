using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversalLibrary.Data;
using UniversalLibrary.Data.Entities;
using UniversalLibrary.Helpers;
using UniversalLibrary.Models;
using static System.Net.Mime.MediaTypeNames;

namespace UniversalLibrary.Controllers
{
    public class ImageViewModelsController : Controller
    {
        private readonly IImageHelper _imageHelper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ImageViewModelsController(IImageHelper imageHelper, IWebHostEnvironment webHostEnvironment)
        {
            _imageHelper = imageHelper;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: ImageViewModels
        public IActionResult Index()
        {
            return View(_imageHelper.GetAll().OrderBy(i=> i.Title));
        }

        // GET: ImageViewModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ImageViewModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ImageViewModel imageViewModel)
        {
            if (ModelState.IsValid)
            {

                //save image to wwwroot/image
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(imageViewModel.ImageFile.FileName);
                string extension = Path.GetExtension(imageViewModel.ImageFile.FileName);
                imageViewModel.Image = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/images/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await imageViewModel.ImageFile.CopyToAsync(fileStream);
                }





                await _imageHelper.CreateAsync(imageViewModel);
                return RedirectToAction(nameof(Index));
            }
            return View(imageViewModel);
        }

        // GET: ImageViewModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _imageHelper.GetByIdAsync(id.Value);
              

            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        

        // GET: ImageViewModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imageViewModel = await _imageHelper.GetByIdAsync(id.Value);
            if (imageViewModel == null)
            {
                return NotFound();
            }
            return View(imageViewModel);
        }

        // POST: ImageViewModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,ImageViewModel imageViewModel)
        {
            if (id != imageViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //-----save image to wwwroot/image----
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(imageViewModel.ImageFile.FileName);
                string extension = Path.GetExtension(imageViewModel.ImageFile.FileName);
                imageViewModel.Image = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/images/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await imageViewModel.ImageFile.CopyToAsync(fileStream);
                }



                try
                {
                    await _imageHelper.UpdateAsync(imageViewModel);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _imageHelper.ExistAsync(imageViewModel.Id))
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
            return View(imageViewModel);
        }

        // GET: ImageViewModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imageViewModel = await _imageHelper.GetByIdAsync(id.Value);
              
            if (imageViewModel == null)
            {
                return NotFound();
            }

            return View(imageViewModel);
        }

        // POST: ImageViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var imageViewModel = await _imageHelper.GetByIdAsync(id);

            //------ Delete image from wwwroot/image----
            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "image", imageViewModel.Image);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);

            await _imageHelper.DeleteAsync(imageViewModel);
            return RedirectToAction(nameof(Index));
        }

    }
}
