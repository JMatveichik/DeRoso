using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRoso.Core
{
    interface IDescribed
    {
        /// <summary>
        /// Название параметра
        /// </summary>        
        string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Описание параметра
        /// </summary>
        string Description
        {
            get;
            set;            
        }        
    }
}
