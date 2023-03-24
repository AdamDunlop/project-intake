using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Smartsheetsproject.Models;

namespace SmartsheetsPIF2.Services
{
    public class SecurityDAO
    {
        bool success = false;


        string connectionString = @"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = 'User Database'; Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

    public bool FindUser(UserModel user)
    { 

        string sqlQuery = "Select * From dbo.Users Where username = @username AND password = @password";

        using (var connection = new SqlConnection(connectionString))
        {
            SqlCommand command = new SqlCommand(sqlQuery, connection);

            command.Parameters.Add("@username", System.Data.SqlDbType.VarChar, 30).Value = user.Username;
            command.Parameters.Add("@password", System.Data.SqlDbType.VarChar, 30).Value = user.Password;

            try 
                {
                   connection.Open();
                   SqlDataReader reader = command.ExecuteReader();
            
                if (reader.HasRows)
                    {
                       success = true;
                    }
                }
            catch(Exception e)
                 {
                    Console.WriteLine(e.Message);
                 }
            return success;

            }

        }
    }
}