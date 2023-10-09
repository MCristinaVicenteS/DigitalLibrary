using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.X509;
using Syncfusion.Pdf;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UniversalLibrary.Data;
using UniversalLibrary.Models;

namespace UniversalLibrary.Helpers
{
    public class FileHelper : GenericRepository<FileViewModel>, IFileHelper
    {
        public readonly DataContext _context;

        public FileHelper(DataContext context) : base(context)
        {
            _context = context;
        }

       
        public async Task<string> UploadFileAsync(IFormFile pdfFile, string folder)
        {
            string guid = Guid.NewGuid().ToString();
            string file = $"{guid}.pdf";

            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\files\\{folder}", file);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await pdfFile.CopyToAsync(stream);
            }

            return $"wwwroot/files/{folder}/{file}";
        }

        public Task CreateAsync(FileHelper entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(FileHelper entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(FileHelper entity)
        {
            throw new NotImplementedException();
        }

        IQueryable<FileHelper> IGenericRepository<FileHelper>.GetAll()
        {
            throw new NotImplementedException();
        }

        Task<FileHelper> IGenericRepository<FileHelper>.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
