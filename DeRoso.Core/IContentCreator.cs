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
