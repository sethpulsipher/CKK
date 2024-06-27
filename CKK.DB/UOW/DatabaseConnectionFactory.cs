using CKK.DB.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Data.SqlClient;

namespace CKK.DB.UOW
{
    public class DatabaseConnectionFactory : IConnectionFactory
    {
        //retrieves a connection string based on a provided name, useful when you have multiple connection strings
        public static string CnnVal(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        //the default ConnectionString
        public readonly string connectionString = "Server=.;Database=StructuredProjectDB;Trusted_Connection=True";

        //returns IDbConnection as a new SqlConnection using the connectionString. Deeply inheritted in SqlConnection is IDbConnection 
        public IDbConnection GetConnection
        {
            get
            {
                return new SqlConnection(connectionString);
            }
        }
    }
}
