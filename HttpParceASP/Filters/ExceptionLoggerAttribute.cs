using HttpParceASP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HttpParceASP.Filters
{
    //фильтр исключений
    public class ExceptionLoggerAttribute:FilterAttribute, IExceptionFilter
    {
        //С помощью переданного в метод OnException() объекта ExceptionContext получаем всю необходимую информацию об исключении, формируем объект ExceptionDetail и сохраняем его в базу данных
        public void OnException(ExceptionContext filterContext)
        {
            ExceptionDetail exceptionDetail = new ExceptionDetail()
            {
                ExceptionMessage = filterContext.Exception.Message,
                StackTrace = filterContext.Exception.StackTrace,
                ControllerName = filterContext.RouteData.Values["controller"].ToString(),
                ActionName = filterContext.RouteData.Values["action"].ToString(),
                Date = DateTime.Now
            };

            using (WordDBContext db = new WordDBContext())
            {
                db.ExceptionDetails.Add(exceptionDetail);
                db.SaveChanges();
            }

            filterContext.Result =
                new RedirectResult("~/Content/ErrorPage.html");

            filterContext.ExceptionHandled = true;
        }
    }
}