using System;
using System.Collections.Generic;
using System.Text;

namespace WikiGameBot.Data.Loaders
{
    public enum LoaderResponse
    {
        Error = 0,
        InvalidRequest = 1,
        RequestDenied = 2,
        Success = 3
    }
}
