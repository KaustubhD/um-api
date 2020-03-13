using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace UserManagement.Models
{
    public class AddressModel
    {
        public string AddressType { get; set; }
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PIN { get; set; }

        internal AppDb Db { get; set; }

        public AddressModel()
        {
        }

        internal AddressModel(AppDb db)
        {
            Db = db;
        }
        public async Task AddAddress(int id)
        {   
            /*
            -----  No query written
            using (var cmd = Db.Connection.CreateCommand()){
                cmd.CommandText = @"SELECT f_name, m_name, l_name from users where username=@uname and password=@pass";
                BindParams(cmd);
                var result =  await ReadAllAsync(await cmd.ExecuteReaderAsync());
                return result.Count > 0 ? result : null;
            }
            */

        }
        public void BindParams(MySqlCommand cmd, string suffix="")
        {
            cmd.Parameters.Add(new MySqlParameter("addres_type" + suffix, AddressType));
            cmd.Parameters.Add(new MySqlParameter("addres" + suffix, AddressLine));
            cmd.Parameters.Add(new MySqlParameter("city" + suffix, City));
            cmd.Parameters.Add(new MySqlParameter("state" + suffix, State));
            cmd.Parameters.Add(new MySqlParameter("country" + suffix, Country));
            cmd.Parameters.Add(new MySqlParameter("pin" + suffix, PIN));
        }
    }
}
