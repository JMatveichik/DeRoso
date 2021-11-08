using DeRoso.Core;
using System.Windows.Controls;

namespace DeRoso.Views
{
    class CreatorViewHome : IContentCreator
    {
        public UserControl CreateContent()
        {
            return new ViewHome();
        }
    }
}
