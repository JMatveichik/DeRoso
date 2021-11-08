using DeRoso.Core;
using System.Windows.Controls;

namespace DeRoso.Views
{
    class CreatorViewTestsEditor : IContentCreator
    {
        public UserControl CreateContent()
        {
            return new ViewTestsEditor();
        }
    }
}
