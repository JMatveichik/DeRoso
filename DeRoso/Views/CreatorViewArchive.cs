using DeRoso.Core;
using System.Windows.Controls;

namespace DeRoso.Views
{
    class CreatorViewArchive : IContentCreator
    {
        public UserControl CreateContent()
        {
            return new ViewArchive();
        }
    }
}
