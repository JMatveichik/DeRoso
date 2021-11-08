using DeRoso.Core;
using System.Windows.Controls;

namespace DeRoso.Views
{
    class CreatorViewSectionEditor : IContentCreator
    {
        public UserControl CreateContent()
        {
            return new ViewSectionsEditor();
        }
    }
}
