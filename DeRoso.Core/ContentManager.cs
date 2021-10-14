using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DeRoso.Core
{
    public class ContentManager
    {
        private static readonly ContentManager instance = new ContentManager();

        public static ContentManager Instance => instance;

        private ContentManager()
        {

        }

        /// <summary>
        /// Зарегестрировать создателя контента.
        /// </summary>
        /// <param name="id">Строковый идентификатор контента</param>
        /// <param name="creator">Создатель экземпляра контента</param>
        public void RegisterCreator(string id, IContentCreator creator)
        {
            if (!_creators.Keys.Contains(id))
                _creators.Add(id, creator);
        }

        /// <summary>
        /// Получить экземпляр контента
        /// </summary>
        /// <param name="id">Идентификатор контента</param>
        /// <returns></returns>
        public UserControl GetContent(string id)
        {
            if (_contents.Keys.Contains(id))
                return _contents[id];

            if (_creators.Keys.Contains(id))
            {
                UserControl content = _creators[id].CreateContent();
                _contents.Add(id, content);
                return content;
            }
            else {
                throw new NullReferenceException("Не найдено подходящего эккземпляра контента или подходящего издателя контента.");
            }
        }       

        private  Dictionary<string, UserControl> _contents = new Dictionary<string, UserControl>();

        private Dictionary<string, IContentCreator> _creators = new Dictionary<string, IContentCreator>();
    }

}
