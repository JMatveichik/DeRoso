using DeRoso.Core.Health;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeRoso.Core.Converters
{
    public static class EnumItemSourceProvider
    {
        // Provides the bindable enumeration of descriptions
        public static IEnumerable<EnumCalculationType> EnumCalculationTypeTemplateValues
        {
            get { return Enum.GetValues(typeof(EnumCalculationType)).Cast<EnumCalculationType>(); }
        }
    }
}
