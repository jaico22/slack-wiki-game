using System;
using System.Collections.Generic;
using System.Text;

namespace WikiGameBot.Data.Loaders
{
    public class SQLServerCredentials
    {
        public string username { get; set; }
        public string password { get; set; }
        public string engine { get; set; }
        public string host { get; set; }
        public string port { get; set; }
        public string dbInstanceIdentifier { get; set; }
    }
}
