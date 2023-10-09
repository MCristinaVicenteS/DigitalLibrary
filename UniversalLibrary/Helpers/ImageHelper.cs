using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using UniversalLibrary.Data;
using UniversalLibrary.Models;

namespace UniversalLibrary.Helpers
{
    public class ImageHelper :GenericRepository<ImageViewModel>, IImageHelper
    {
        private readonly DataContext _context;
        public ImageHelper(DataContext context) : base(context)
        {
            _context = context;
        }
        public async Task<string> UploadImageAsync(IFormFile imageFile, string folder)
        {
            //inserir o guid para aceder à imagem q está guardada
            string guid = Guid.NewGuid().ToString();
            string file = $"{guid}.jpg";

            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\images\\{folder}", file);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"~/images/{folder}/{file}";
        }
    }
}
