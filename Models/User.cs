using System.Collections.Generic;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace UserManagement
{
    public class User
    {
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string Email { get; set; }
        public string AltEmail { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DOB { get; set; }
        public string Gender { get; set; }
        public string DOJ { get; set; }
        public string ContactNumberType { get; set; }
        public string Number { get; set; }
      //  public string CountryId { get; set; }
        public string AreaCode { get; set; }
        
        public string AddressType { get; set; }
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public string PIN { get; set; }
        public string AddressType2 { get; set; }
        public string AddressLine2 { get; set; }
        public string City2 { get; set; }
        public string State2 { get; set; }
        public string Country2 { get; set; }
        public string PIN2 { get; set; }
        //public ContactNumber phones;
        //public Address[] addresses;

        internal AppDb Db { get; set; }

        public User()
        {
        }

        internal User(AppDb db)
        {
            Db = db;
        }
        public async Task<string> AddOneUser()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "insert_user_data";
            cmd.CommandType = CommandType.StoredProcedure;
            BindAddProcParams(cmd);
            MySqlParameter outputEmailParam = new MySqlParameter("@username_out", SqlDbType.VarChar)
            {
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(outputEmailParam);
            //cmd.Connection.Open();
            await cmd.ExecuteNonQueryAsync();
            System.Diagnostics.Debug.WriteLine("ABCDE----------------------------------------");
            //cmd.Connection.Close();
            return (string)outputEmailParam.Value;

        }

        public async Task<bool> FindOneAsync(string username)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT user_id FROM user WHERE username = @username";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@username",
                DbType = DbType.String,
                Value = username
            });
            var result = await cmd.ExecuteReaderAsync();
            return await result.ReadAsync();
        }

        public async Task<int> DeleteAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "delete_user";
            cmd.CommandType = CommandType.StoredProcedure;
            BindUsername(cmd);
            
            await cmd.ExecuteNonQueryAsync();
            return 1;
        }
        public async Task<List<User>> GetAllUsersAsync()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT `first_name`,`middle_name`,`last_name`,`email`,`username`  FROM `user`";
            return await ReadAllAsync(await cmd.ExecuteReaderAsync());
        }
        private async Task<List<User>> ReadAllAsync(DbDataReader reader)
        {
            var posts = new List<User>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var post = new User(Db)
                    {
                        //  Id = reader.GetInt32(0),
                        FirstName = reader.GetString(0),
                        MiddleName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        Email = reader.GetString(3),
                        UserName = reader.GetString(4),
                        //DOB = Convert.ToDateTime(reader["date_of_birth"]).ToString("dd/MM/yyyy"),
                        //DOJ = Convert.ToDateTime(reader["date_of_joining"]).ToString("dd/MM/yyyy")

                        //  Content = reader.GetString(2),
                    };
                    posts.Add(post);
                }
            }
            return posts;
        }

        private void BindAddProcParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter("salut", Salutation));
            cmd.Parameters.Add(new MySqlParameter("f_name", FirstName));
            cmd.Parameters.Add(new MySqlParameter("m_name", MiddleName));
            cmd.Parameters.Add(new MySqlParameter("l_name", LastName));
            cmd.Parameters.Add(new MySqlParameter("dept_name", DepartmentName));
            cmd.Parameters.Add(new MySqlParameter("desig", DesignationName));
            cmd.Parameters.Add(new MySqlParameter("email", (AltEmail == null || AltEmail == "") ? Email + "," + AltEmail : Email ));
            cmd.Parameters.Add(new MySqlParameter("username", UserName));
            cmd.Parameters.Add(new MySqlParameter("pass", Password));
            cmd.Parameters.Add(new MySqlParameter("dob", DOB));
            cmd.Parameters.Add(new MySqlParameter("gend", Gender));
            cmd.Parameters.Add(new MySqlParameter("doj", DOJ));
            cmd.Parameters.Add(new MySqlParameter("ph_num_type", ContactNumberType));
            cmd.Parameters.Add(new MySqlParameter("ph_number", Number));
            cmd.Parameters.Add(new MySqlParameter("ph_ext", AreaCode));
            cmd.Parameters.Add(new MySqlParameter("addres_type", AddressType));
            cmd.Parameters.Add(new MySqlParameter("addres", AddressLine));
            cmd.Parameters.Add(new MySqlParameter("city", City));
            cmd.Parameters.Add(new MySqlParameter("state", State));
            cmd.Parameters.Add(new MySqlParameter("country", Country));
            cmd.Parameters.Add(new MySqlParameter("pin", PIN));
            cmd.Parameters.Add(new MySqlParameter("addres_type2", AddressType2));
            cmd.Parameters.Add(new MySqlParameter("addres2", AddressLine2));
            cmd.Parameters.Add(new MySqlParameter("city2", City2));
            cmd.Parameters.Add(new MySqlParameter("state2", State2));
            cmd.Parameters.Add(new MySqlParameter("country2", Country2));
            cmd.Parameters.Add(new MySqlParameter("pin2", PIN2));
            cmd.Parameters.Add(new MySqlParameter("is_ac", 1));
            /* TODO
            Add phone number parameters
            Add address parameters
            */
        }
        private void BindUsername(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter("username", UserName));
        }

    }
}
