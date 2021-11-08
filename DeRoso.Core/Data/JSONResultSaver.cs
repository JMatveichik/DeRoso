using System;
using DeRoso.Core.Health;

namespace DeRoso.Core.Data
{
    public class JSONResultSaver : FileOutputResultSaver
    {
        public JSONResultSaver(string targetPath) : base(targetPath)
        {

        }

        public override bool Save(HealthTestReport report, bool showResults)
        {
            throw new NotImplementedException();
        }
    }
}
