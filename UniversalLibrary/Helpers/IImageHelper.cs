using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using UniversalLibrary.Data;
using UniversalLibrary.Models;

namespace UniversalLibrary.Helpers
{
    public interface IImageHelper: IGenericRepository<ImageViewModel>
    {
        //método para o upload
        //esta string vai ser o caminho para a BD
        Task<string> UploadImageAsync(IFormFile imageFile, string folder);
    }
}
