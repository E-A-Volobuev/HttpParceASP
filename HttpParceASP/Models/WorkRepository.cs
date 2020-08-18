using HtmlAgilityPack;
using HttpParceASP.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace HttpParceASP.Models
{
    //класс репозитория,является промежуточным звеном между методами, непосредственно взаимодействующими с данными, и остальной программой.
    public class WorkRepository:IRepository,IDisposable
    {
        WordDBContext db = new WordDBContext();

        //получаем html страницу, сохраняем на жёсткий диск по указанному пути и возвращаем содержимое в виде строки.
        [ExceptionLogger]
        public string Parse(string name)
        {
            try
            {
                System.Net.WebClient wc = new System.Net.WebClient();
                byte[] bytes = wc.DownloadData($"https://www.{name}");
                string webData = System.Text.Encoding.UTF8.GetString(bytes);

                var pageDoc = new HtmlDocument();
                pageDoc.LoadHtml(webData);
                var pageText = pageDoc.DocumentNode.InnerText;

                if (!String.IsNullOrEmpty(pageText))
                {
                    File.WriteAllText(@"C:\SomeDir\note.txt", pageText);
                }
                return Convert.ToString(pageText);
            }
            catch(Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
           
        }
        // очищаем строку от html тегов, подсчитываем количество повторений слов и сохраняем статистику в бд
        [ExceptionLogger]
        public void Save(string siteName)
        {
            try
            {
                string text = Parse(siteName);
                string[] split = text.Trim().Split(new Char[] { ' ', ',', '.', '!', '?', '"', ';', ':', '[', ']', '(', ')', '\n', '\r', '\t' });

                var result = split.GroupBy(x => x)
                                  .Where(x => x.Count() > 0)
                                  .Select(x => new Word { Name = x.Key, Count = x.Count() }).ToList();
                foreach (Word s in result)
                {

                    db.Words.Add(s);
                    db.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
           
        }
        // просмотр списка объектов бд
        public IEnumerable<Word> List()
        {
            return db.Words;
        }
        //очистка историй парсинга
        public void Clear()
        {
            try
            {
                db.Words.RemoveRange(db.Words);
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
        //журнал ошибок
        public IEnumerable<ExceptionDetail>ExeptionsList()
        {
            return db.ExceptionDetails;
        }

        //очистка неуправляемых ресурсов
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                {
                    db.Dispose();
                    db = null;
                }
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}