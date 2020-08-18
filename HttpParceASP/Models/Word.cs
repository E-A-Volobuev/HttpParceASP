using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HttpParceASP.Models
{
    //класс, описывающий слово
    public class Word
    {
        public int Id { get; set; }  // Id для сохранения в бд
        public string Name { get; set; }// слово
        public int Count { get; set; }// количество повторений слова
    }
}