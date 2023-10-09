using Microsoft.AspNetCore.Http;
using Syncfusion.Pdf;
using System.Threading.Tasks;
using UniversalLibrary.Data;
using UniversalLibrary.Models;

namespace UniversalLibrary.Helpers
{
    public interface IFileHelper: IGenericRepository<FileHelper>
    {
        //Método para o upload
        //caminho para a BD
        Task<string> UploadFileAsync (IFormFile pdfFile, string folder);
    }
}
