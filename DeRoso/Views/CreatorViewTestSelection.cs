using DeRoso.Core;
using System.Windows.Controls;

namespace DeRoso.Views
{
    class CreatorViewTestSelection : IContentCreator
    {
        public UserControl CreateContent()
        {
            return new ViewTestSelection();
        }
    }
}
