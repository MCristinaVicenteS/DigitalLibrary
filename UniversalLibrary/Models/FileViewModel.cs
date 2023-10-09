using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using NPOI.Util;
using Syncfusion.Pdf;
using UniversalLibrary.Data;
using UniversalLibrary.Helpers;

namespace UniversalLibrary.Models
{
    public class FileViewModel : HttpPostedFileBase, IEntity
    {
        private readonly IFileHelper _fileHelper;

        public FileViewModel(IFileHelper fileHelper)
        {
            _fileHelper = fileHelper;
        }

        public int Id { get; set; }

        public string Name { get; set; }


       

    }
}
