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
        // public int Id { get; set; }
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
        public async Task<string> checkAuth(){
            using var cmd = Db.Connection.CreateCommand();
            
            // MySqlCommand cmd = new MySqlCommand("login", connection);
            cmd.CommandText = "login";
            cmd.CommandType = CommandType.StoredProcedure;
            BindParams(cmd);
            MySqlParameter outputEmailParam = new MySqlParameter("@username_out", SqlDbType.VarChar)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputEmailParam);
            await cmd.ExecuteNonQueryAsync();
            string value = (string)outputEmailParam.Value;
            string value1 = value;
            System.Diagnostics.Debug.WriteLine("ABCDE----------------------------------------");
            UsernameOut = value1;
            // ILog.log.Debug(UsernameOut);
            return UsernameOut;
            // or
            // UsernameOut = OutParam.Value;
            
            
            /*
            using (var cmd = Db.Connection.CreateCommand()){
                cmd.CommandText = @"SELECT f_name, m_name, l_name from users where username=@uname and password=@pass";
                BindParams(cmd);
                var result =  await ReadAllAsync(await cmd.ExecuteReaderAsync());
                return result.Count > 0 ? result : null;
            }
            */
            
        }
        /*
        private async Task<List<User>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<User>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new User(Db)
                    {
                        FirstName = reader.GetString(0),
                        MiddleName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                        LastName = reader.GetString(2),
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }
        */

        // public async Task InsertAsync()
        // {
        //     using var cmd = Db.Connection.CreateCommand();
        //     cmd.CommandText = @"INSERT INTO `BlogPost` (`Title`, `Content`) VALUES (@title, @content);";
        //     BindParams(cmd);
        //     await cmd.ExecuteNonQueryAsync();
        //     Id = (int) cmd.LastInsertedId;
        // }

        // public async Task UpdateAsync()
        // {
        //     using var cmd = Db.Connection.CreateCommand();
        //     cmd.CommandText = @"UPDATE `BlogPost` SET `Title` = @title, `Content` = @content WHERE `Id` = @id;";
        //     BindParams(cmd);
        //     BindId(cmd);
        //     await cmd.ExecuteNonQueryAsync();
        // }

        // public async Task DeleteAsync()
        // {
        //     using var cmd = Db.Connection.CreateCommand();
        //     cmd.CommandText = @"DELETE FROM `BlogPost` WHERE `Id` = @id;";
        //     BindId(cmd);
        //     await cmd.ExecuteNonQueryAsync();
        // }

        // private void BindId(MySqlCommand cmd)
        // {
        //     cmd.Parameters.Add(new MySqlParameter
        //     {
        //         ParameterName = "@id",
        //         DbType = DbType.Int32,
        //         Value = Id,
        //     });
        // }

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
