﻿using DeRoso.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
