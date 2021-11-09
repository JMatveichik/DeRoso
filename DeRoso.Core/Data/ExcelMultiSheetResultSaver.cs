using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRoso.Core.Data
{
    public class ExcelMultiSheetResultSaver : ExcelResultsSaver
    {
        public ExcelMultiSheetResultSaver(string targetPath) : base(targetPath)
        {

        }
    }
}
