using System.Collections.Generic;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using UserManagement.Models;

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
        public List<ContactNumberModel> phones { get; set; }
        public List<AddressModel> addresses { get; set; }

        internal AppDb Db { get; set; }

        public User()
        {
            phones = new List<ContactNumberModel>();
            addresses = new List<AddressModel>();
        }

        internal User(AppDb db)
        {
            Db = db;
        }
        public string AddOneUser()
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
            cmd.ExecuteNonQuery();
            //cmd.Connection.Close();
            return (string)outputEmailParam.Value;

        }

        public int Delete()
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "delete_user";
            cmd.CommandType = CommandType.StoredProcedure;
            BindUsername(cmd);
            
            cmd.ExecuteNonQuery();
            return 1;
        }
        public List<User> GetAllUsers(string names="")
        {
            using var cmd = Db.Connection.CreateCommand();
            Console.WriteLine("------------------" + names);
            if(names != "" || names != null || names != String.Empty)
            {
                cmd.CommandText = "call get_users_by_name(@namee)";
                cmd.Parameters.AddWithValue("@namee", names);
            }
            else
                cmd.CommandText = "call get_all_active_user()";
            return ReadAll(cmd.ExecuteReader());
        }

         public void Update()
          {
              using var cmd = Db.Connection.CreateCommand();
              cmd.CommandText = @"UPDATE `user` SET `is_active`=0 WHERE `username` = @username;";
              BindUsername(cmd);
              cmd.ExecuteNonQuery();
          }

        public static string GetSafeString(MySqlDataReader reader, string colName)
        {
            return reader[colName] != System.DBNull.Value ? (string)reader[colName] : "";
        }

        private AddressModel AddAddress(MySqlDataReader reader,string prefix)
        {
            var address1 = new AddressModel();
            address1.AddressLine = GetSafeString(reader, prefix + "_address");
            address1.AddressType = prefix;
            address1.City = GetSafeString(reader, prefix +"_city");
            address1.State = GetSafeString(reader, prefix + "_state");
            address1.Country = GetSafeString(reader, prefix + "_country");
            address1.PIN = GetSafeString(reader, prefix + "_pin");
           

            return address1;
        }
        private ContactNumberModel AddContact(MySqlDataReader reader, string prefix)
        {
            var contact1 = new ContactNumberModel();
            contact1.ContactNumberType = prefix;
            contact1.CountryCode = GetSafeString(reader, prefix + "_country_code");
            contact1.AreaCode = GetSafeString(reader, prefix + "_area_code");
            contact1.Number = GetSafeString(reader, prefix + "_number");

            return contact1;
        }

        public User getUserById(int id)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "call get_user(@id)";
            cmd.Parameters.AddWithValue("@id", id);
            
            cmd.ExecuteNonQuery();
            return ReadUser(cmd.ExecuteReader());
        }
        private List<User> ReadAll(MySqlDataReader reader)
        {
            var posts = new List<User>();
            using (reader)
            {
                while (reader.Read())
                {
                    var post = new User();
                    post.Salutation = GetSafeString(reader,"salutation");
                    post.FirstName = GetSafeString(reader, "first_name");
                    post.MiddleName = GetSafeString(reader, "middle_name");
                    post.LastName = GetSafeString(reader, "last_name");
                    post.UserName = GetSafeString(reader, "username");
                    post.DepartmentName = GetSafeString(reader, "department_name");
                    post.DesignationName = GetSafeString(reader, "designation_name");
                    post.Email = GetSafeString(reader, "email");
                    post.Gender = GetSafeString(reader, "gender");
                    post.DOB = Convert.ToDateTime(reader["date_of_birth"]).ToString("dd/MM/yyyy");
                    post.DOJ = Convert.ToDateTime(reader["date_of_joining"]).ToString("dd/MM/yyyy");
                    post.addresses.Add(AddAddress(reader, "current"));
                    post.addresses.Add(AddAddress(reader, "permanant"));
                    post.phones.Add(AddContact(reader,"mobile"));
                    post.phones.Add(AddContact(reader, "work"));
                    post.phones.Add(AddContact(reader, "home"));

                    posts.Add(post);
                }
            }
            return posts;
        }


        private User ReadUser(MySqlDataReader reader)
        {
            var post = new User();
            using (reader)
            {
                while (reader.Read())
                {
                    
                    post.Salutation = GetSafeString(reader, "salutation");
                    post.FirstName = GetSafeString(reader, "first_name");
                    post.MiddleName = GetSafeString(reader, "middle_name");
                    post.LastName = GetSafeString(reader, "last_name");
                    post.UserName = GetSafeString(reader, "username");
                    post.DepartmentName = GetSafeString(reader, "department_name");
                    post.DesignationName = GetSafeString(reader, "designation_name");
                    post.Email = GetSafeString(reader, "email");
                    post.Gender = GetSafeString(reader, "gender");
                    post.DOB = Convert.ToDateTime(reader["date_of_birth"]).ToString("dd/MM/yyyy");
                    post.DOJ = Convert.ToDateTime(reader["date_of_joining"]).ToString("dd/MM/yyyy");
                    post.addresses.Add(AddAddress(reader, "current"));
                    post.addresses.Add(AddAddress(reader, "permanant"));
                    post.phones.Add(AddContact(reader, "mobile"));
                    post.phones.Add(AddContact(reader, "work"));
                    post.phones.Add(AddContact(reader, "home"));

                    
                }
            }
            return post;
        }

        /*
        public void UpdateUser(String uname, User model)
        {
            string[] names = model.name.Split(" ");
            Console.WriteLine("----------" + names[0]);

            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"update user set first_name=@f,middle_name=@m,last_name=@l where username=@u;";
            cmd.Parameters.Add(new MySqlParameter("@f", names[0]));
            cmd.Parameters.Add(new MySqlParameter("@u", uname));
            var midName = new MySqlParameter("@m", DBNull.Value);

            switch (names.Length)
            {
                case 1:
                    break;
                case 2:
                    cmd.Parameters.Add(new MySqlParameter("@l", names[1]));
                    break;
                case 3:
                    midName.Value = names[1];
                    cmd.Parameters.AddWithValue("@l", names[2]);
                    break;
                default:
                    string mid = "";
                    for (int i = 1; i < names.Length - 1; i++) { mid += names[i] + " "; }
                    mid = mid.Remove(mid.LastIndexOf(" "), " ".Length).Insert(mid.LastIndexOf(" "), ""); // Remove last space
                    cmd.Parameters.Add(new MySqlParameter("@m", mid));
                    cmd.Parameters.AddWithValue("@l", names[names.Length - 1]);
                    break;
            }
            cmd.Parameters.Add(midName);
            cmd.ExecuteNonQuery();


            cmd.CommandText = @"update contact_number c inner join user_to_contact using(contact_id) inner join user using(user_id) inner join contact_type using(contact_type_id) set number=@num where username=@u and contact_type=@typ";
            var number = new MySqlParameter("@num", model.phones[0].Number);
            var numberType = new MySqlParameter("@typ", model.phones[0].ContactNumberType);

            cmd.Parameters.Add(number);
            cmd.Parameters.Add(numberType);
            cmd.ExecuteNonQuery();

            if (!string.IsNullOrEmpty(model.phones[1].ContactNumberType))
            {
                number.Value = model.phones[1].Number;
                numberType.Value = model.phones[1].ContactNumberType;
                cmd.ExecuteNonQuery();
            }

            //string[] address = model.address.Split(",");

            //string add = "";
            cmd.CommandText = @"call update_address(@u,@address,@city,@state,@coun,@pin)";

            cmd.Parameters.AddWithValue("@pin", model.addresses[0].PIN);
            cmd.Parameters.AddWithValue("@state", model.address.State);
            cmd.Parameters.AddWithValue("@city", model.address.City);
            cmd.Parameters.AddWithValue("@coun", model.address.Country);

            cmd.Parameters.AddWithValue("@address", model.address.AddressLine);
            cmd.ExecuteNonQuery();


        }

        */



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
            foreach(ContactNumberModel phone in phones)
            {
                phone.BindParams(cmd);
            }

            foreach (ContactNumberModel phone in phones)
            {
                phone.BindParams(cmd);
            }
            addresses[0].BindParams(cmd);
            addresses[1].BindParams(cmd, "2");
            cmd.Parameters.Add(new MySqlParameter("is_ac", 1));
        }
        private void BindUsername(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter("username", UserName));
        }

     
    }
}
