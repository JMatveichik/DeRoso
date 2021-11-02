﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeRoso.Core.Health;

namespace DeRoso.Core.Data
{
    public class XmlResultsSaver : FileOutputResultSaver
    {
        public XmlResultsSaver (string targetPath) : base(targetPath)
        {

        }

        public override bool Save(HealthTestReport report, bool showResults)
        {
            throw new NotImplementedException();
        }
    }
}
