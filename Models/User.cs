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
            MySqlParameter outputEmailParam;

            MySqlCommand cmd = Db.Connection.CreateCommand();
            MySqlTransaction myTrans;
            myTrans = Db.Connection.BeginTransaction();

            cmd.Connection = Db.Connection;
            cmd.Transaction = myTrans;

            try
            {
                cmd.CommandText = "insert_user";
                cmd.CommandType = CommandType.StoredProcedure;
                outputEmailParam = new MySqlParameter("@username_out", SqlDbType.VarChar) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(outputEmailParam);
                BindUsername(cmd);
                BindUserProcParams(cmd);
                cmd.ExecuteNonQuery();

                cmd.CommandText = "insert_contact";
                cmd.CommandType = CommandType.StoredProcedure;
                var num_type = new MySqlParameter("ph_num_type", String.Empty);
                var num = new MySqlParameter("ph_number", String.Empty);
                var code = new MySqlParameter("ph_ext", String.Empty);
                var c_code = new MySqlParameter("country_code", String.Empty);

                cmd.Parameters.Add(num_type);
                cmd.Parameters.Add(num);
                cmd.Parameters.Add(code);
                cmd.Parameters.Add(c_code);
                Console.WriteLine("-----" + addresses.Count);
                foreach (ContactNumberModel phone in phones)
                {
                    //phone.BindParams(cmd);
                    num_type.Value = phone.ContactNumberType;
                    num.Value = phone.Number;
                    code.Value = phone.AreaCode;
                    c_code.Value = phone.CountryCode;
                    cmd.ExecuteNonQuery();
                }


                // send new cmd to avoid complexity
                cmd.CommandText = "insert_address";
                cmd.CommandType = CommandType.StoredProcedure;
                var add_type = new MySqlParameter("addres_type", String.Empty);
                var add = new MySqlParameter("addres", String.Empty);
                var city = new MySqlParameter("city", String.Empty);
                var state = new MySqlParameter("state", String.Empty);
                var pin = new MySqlParameter("pin", String.Empty);
                var country = new MySqlParameter("country", String.Empty);
                cmd.Parameters.Add(add_type);
                cmd.Parameters.Add(add);
                cmd.Parameters.Add(city);
                cmd.Parameters.Add(state);
                cmd.Parameters.Add(pin);
                cmd.Parameters.Add(country);
                Console.WriteLine(addresses.Count);
                foreach (AddressModel address in addresses)
                {
                    Console.WriteLine(address.AddressType + address.AddressLine + address.City + address.State + address.Country + address.PIN);
                    add_type.Value = address.AddressType;
                    add.Value = address.AddressLine;
                    city.Value = address.City;
                    state.Value = address.State;
                    country.Value = address.Country;
                    pin.Value = address.PIN;
                    cmd.ExecuteNonQuery();


                }
                myTrans.Commit();
                Console.WriteLine("Both records are written to database.");
                return "User inserted";
            }
            catch (Exception e)
            {
                try
                {
                    myTrans.Rollback();
                }
                catch (MySqlException ex)
                {
                    if (myTrans.Connection != null)
                    {
                        Console.WriteLine("An exception of type " + ex.GetType() +
                        " was encountered while attempting to roll back the transaction.");
                    }
                }

                Console.WriteLine("An exception of type " + e.Message +
                " was encountered while inserting the data.");
                Console.WriteLine("Neither record was written to database.");
            }
            finally
            {
                Db.Connection.Close();
            }
            /*
                catch (Exception e)
                {
                    return e.Message;
                }
                */


            return "0";

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

       
        public string UpdateUser()
        {
            MySqlParameter outputEmailParam;

            MySqlCommand cmd = Db.Connection.CreateCommand();
            MySqlTransaction myTrans;

            myTrans = Db.Connection.BeginTransaction();
            cmd.Connection = Db.Connection;
            cmd.Transaction = myTrans;

            try {
                cmd.CommandText = "update_user";
                cmd.CommandType = CommandType.StoredProcedure;
                outputEmailParam = new MySqlParameter("@username_out", SqlDbType.VarChar) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(outputEmailParam);
                BindUsername(cmd);
                BindUserProcParams(cmd);
                cmd.ExecuteNonQuery();

                cmd.CommandText = "update_contact";
                cmd.CommandType = CommandType.StoredProcedure;
                var num_type = new MySqlParameter("ph_num_type", String.Empty);
                var num = new MySqlParameter("ph_number", String.Empty);
                var code = new MySqlParameter("ph_ext", String.Empty);
                var c_code = new MySqlParameter("country_code", String.Empty);
                
                cmd.Parameters.Add(num_type);
                cmd.Parameters.Add(num);
                cmd.Parameters.Add(code);
                cmd.Parameters.Add(c_code);

                foreach (ContactNumberModel phone in phones)
                {
                    //phone.BindParams(cmd);
                    num_type.Value = phone.ContactNumberType;
                    num.Value = phone.Number;
                    code.Value = phone.AreaCode;
                    c_code.Value = phone.CountryCode;
                    cmd.ExecuteNonQuery();
                }   
                // send new cmd to avoid complexity
                cmd.CommandText = "update_address";
                cmd.CommandType = CommandType.StoredProcedure;
                var add_type = new MySqlParameter("addres_type", String.Empty);
                var add = new MySqlParameter("addres", String.Empty);
                var city = new MySqlParameter("city", String.Empty);
                var state = new MySqlParameter("state", String.Empty);
                var pin = new MySqlParameter("pin", String.Empty);
                var country = new MySqlParameter("country", String.Empty);
                cmd.Parameters.Add(add_type);
                    cmd.Parameters.Add(add);
                    cmd.Parameters.Add(city);
                    cmd.Parameters.Add(state);
                    cmd.Parameters.Add(pin);
                cmd.Parameters.Add(country);

                foreach (AddressModel address in addresses)
                {
                    add_type.Value = address.AddressType;
                    add.Value = address.AddressLine;
                    city.Value = address.City;
                    state.Value = address.State;
                    country.Value = address.Country;
                    pin.Value = address.PIN;
                    cmd.ExecuteNonQuery();
                }
                myTrans.Commit();
                Console.WriteLine("Record Updated");
                return "Record Updated";
            }
            catch (Exception e)
            {
                try
                {
                    myTrans.Rollback();
                }
                catch (MySqlException ex)
                {
                    if (myTrans.Connection != null)
                    {
                        Console.WriteLine("An exception of type " + ex.GetType() +
                        " was encountered while attempting to roll back the transaction.");
                    }
                }

                Console.WriteLine("An exception of type " + e.Message +
                " was encountered while inserting the data.");
                Console.WriteLine("Neither record was written to database.");
            }
            finally
            {
                Db.Connection.Close();
            }



            return "Update failed";


        }
            
        

        private void BindUserProcParams(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter("salut", Salutation));
            cmd.Parameters.Add(new MySqlParameter("f_name", FirstName));
            cmd.Parameters.Add(new MySqlParameter("m_name", MiddleName));
            cmd.Parameters.Add(new MySqlParameter("l_name", LastName));
            cmd.Parameters.Add(new MySqlParameter("dept_name", DepartmentName));
            cmd.Parameters.Add(new MySqlParameter("desig", DesignationName));
            cmd.Parameters.Add(new MySqlParameter("email", (AltEmail == null || AltEmail == "") ? Email + "," + AltEmail : Email ));
            cmd.Parameters.Add(new MySqlParameter("pass", Password));
            cmd.Parameters.Add(new MySqlParameter("dob", DateTime.Parse(DOB).ToString("yyyy-MM-dd")));
            cmd.Parameters.Add(new MySqlParameter("gend", Gender));
            cmd.Parameters.Add(new MySqlParameter("doj", DateTime.Parse(DOJ).ToString("yyyy-MM-dd")));
            cmd.Parameters.Add(new MySqlParameter("is_ac", 1));

        }
        private void BindContactProcParams(MySqlCommand cmd)
        {
            foreach (ContactNumberModel phone in phones)
            {
                phone.BindParams(cmd);
            }
        }

        private void BindAddressProcParams(MySqlCommand cmd)
        {
            foreach (AddressModel address in addresses)
            {
                address.BindParams(cmd);
            }
        }

        private void BindUsername(MySqlCommand cmd)
        {
            cmd.Parameters.Add(new MySqlParameter("username", UserName));
        }

     
    }
}
