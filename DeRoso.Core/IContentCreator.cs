using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DeRoso.Core
{
    public interface IContentCreator
    {
        /// <summary>
        /// Функция создания нового экземпляра контента
        /// </summary>
        /// <returns>Новый контетн в виде UserControl
        /// </returns>
        UserControl CreateContent();
    }
}
