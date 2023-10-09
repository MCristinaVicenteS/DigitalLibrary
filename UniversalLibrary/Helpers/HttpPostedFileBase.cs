using Syncfusion.Pdf.Interactive;

namespace UniversalLibrary.Helpers
{
    public abstract class HttpPostedFileBase
    {
        public int Id { get; set; }

        public HttpPostedFileBase PdfFile { get; set; }

        public string Pdf { get; set; }

        public virtual int ContentLength { get; set; }

        public virtual string FileName { get; }

        public virtual System.IO.Stream InputStream { get;}

        public virtual string ContentType { get; }

    }
}
