using DeRoso.Core;
using System.Windows.Controls;

namespace DeRoso.Views
{
    class CreatorViewPatientEditor : IContentCreator
    {
        public UserControl CreateContent()
        {
            return new ViewPatientEditor();
        }
    }
}
