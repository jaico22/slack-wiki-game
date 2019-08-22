using System;
using System.Collections.Generic;
using System.Text;

namespace WikiGameBot.Core.PathValidation
{
    public class ValidationData
    {
        public bool IsValid { get; set; }

        public int PathLength { get; set; }

        public string ValidationMessage { get; set; }
    }
}
