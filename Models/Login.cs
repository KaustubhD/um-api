using System.Collections.Generic;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace UserManagement
{
    public class Login
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string UsernameOut { get; set; }

        internal AppDb Db { get; set; }

        public Login()
        {
        }

        internal Login(AppDb db)
        {
            Db = db;
        }
        public string checkAuth(){
            using var cmd = Db.Connection.CreateCommand();
            
            cmd.CommandText = "login";
            cmd.CommandType = CommandType.StoredProcedure;
            BindParams(cmd);
            MySqlParameter outputEmailParam = new MySqlParameter("@username_out", SqlDbType.VarChar)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputEmailParam);
            cmd.ExecuteNonQuery();
            string value = (string)outputEmailParam.Value;
            string value1 = value;
            //System.Diagnostics.Debug.WriteLine("ABCDE----------------------------------------");
            UsernameOut = value1;
            return UsernameOut;
  
            
        }
       

        private void BindParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@username",
                DbType = DbType.String,
                Value = Username,
            });
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@pass",
                DbType = DbType.String,
                Value = Password,
            });
            
        }
    }
}
