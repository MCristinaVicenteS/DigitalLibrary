using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace UniversalLibrary.Helpers
{

    //inserir esta classe no Details dos controladores
    public class NotFoundViewResult : ViewResult
    {
        //criar o construtor que vai receber o nome da view
        public NotFoundViewResult(string viewName)
        {
            ViewName = viewName; //herda o ViewName do ViewResult

            //criar o código de erro
            StatusCode = (int)HttpStatusCode.NotFound;
        }
    }
}
