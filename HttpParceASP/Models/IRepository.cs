using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpParceASP.Models
{
    // интерфейс репозитория, контроллер будет работать с методами интерфейса,а не класса, это нужно для управлениями зависимостей,через Ninject
    public interface IRepository
    {
        void Save(string siteName);
        void Clear();
        IEnumerable<ExceptionDetail> ExeptionsList();
        IEnumerable<Word> List();

    }
}
