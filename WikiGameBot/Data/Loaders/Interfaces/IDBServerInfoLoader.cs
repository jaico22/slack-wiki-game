using System;
using System.Collections.Generic;
using System.Text;

namespace WikiGameBot.Data.Loaders.Interfaces
{
    public interface IDBServerInfoLoader
    {
        /// <summary>
        /// Returns the connection string for the SQL Server
        /// </summary>
        /// <returns></returns>
        string GetConnectionString();
    }
}
