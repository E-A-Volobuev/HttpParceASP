using HtmlAgilityPack;
using HttpParceASP.Filters;
using HttpParceASP.Models;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HttpParceASP.Controllers
{
    public class HomeController : Controller
    {
        IRepository repo;

        
        public HomeController()
        {
            //Чтобы управлять зависимостями через Ninject, вначале надо создать объект Ninject.IKernel с помощью встроенной реализации этого интерфейса - класса StandardKernel:
            IKernel ninjectKernel = new StandardKernel();
            //Далее нужно установить отношения между интерфейсами и их реализациями:
            ninjectKernel.Bind<IRepository>().To<WorkRepository>();
            //И в конце создается объект интерфейса через метод Get:
            repo = ninjectKernel.Get<IRepository>();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Info()
        {
            return View();
        }

        
        [HttpGet]
        //гет метод принимающий через представление название сайта и передаёт его в пост метод 
        public ActionResult Parse()
        {
            return View();
        }
        [HttpPost]
        [ExceptionLogger]
        //пост метод, принимает из представления название сайта и передает его в метод интерфейса репозитория
        public string Parse(string siteName)
        {
            if( siteName!=null)
            {
                repo.Save(siteName);
                return "Выполнено!";
            }
            else
            {
                throw new Exception("Некорректный ввод");
            }
            
        }
        // метод действия, выводящий в представления объекты бд
        public ActionResult Display()
        {
            ViewBag.Words = repo.List();
            return View();
        }
        //метод действия, вызывающий метод очистки бд интерфейса репозитория
        public string Clear()
        {
            repo.Clear();
            return "Очищено!";
        }
        //журнал ошибок
        public ActionResult Exeptions()
        {
            
            return View(repo.ExeptionsList());
        }
       
    }
}