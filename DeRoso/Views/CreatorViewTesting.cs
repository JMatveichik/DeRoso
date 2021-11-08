using DeRoso.Core;
using System.Windows.Controls;

namespace DeRoso.Views
{
    class CreatorViewTesting : IContentCreator
    {
        public UserControl CreateContent()
        {
            return new ViewTesting();
        }
    }
}
