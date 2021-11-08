using DeRoso.Core;
using System.Windows.Controls;

namespace DeRoso.Views
{
    class CreatorViewHelp : IContentCreator
    {
        public UserControl CreateContent()
        {
            return new ViewHelp();
        }
    }
}
