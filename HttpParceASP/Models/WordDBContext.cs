using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HttpParceASP.Models
{
    //контекст данных для подключения к бд
    public class WordDBContext:DbContext
    {
        //передаем строку подключения в базовый класс
        public WordDBContext() : base("DbConnection")
        { }
        //определяем набор сущностей , хранящихся в бд
        //слова
        public DbSet<Word> Words { get; set; }
        //ошибки
        public DbSet<ExceptionDetail> ExceptionDetails { get; set; }
    }
}