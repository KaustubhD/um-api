using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace UserManagement.Models
{
    public class ContactNumberModel
    {
        public string ContactNumberType { get; set; }
        public string Number { get; set; }
        public string CountryCode { get; set; }
        public string AreaCode { get; set; }
        
        

        internal AppDb Db { get; set; }

        public ContactNumberModel()
        {
        }

        internal ContactNumberModel(AppDb db)
        {
            Db = db;
        }
        public async Task AddContactNumber(int id)
        {
            /*
            ------- No Query written
            
            using (var cmd = Db.Connection.CreateCommand()){
                cmd.CommandText = @"SELECT f_name, m_name, l_name from users where username=@uname and password=@pass";
                BindParams(cmd);
                var result =  await ReadAllAsync(await cmd.ExecuteReaderAsync());
                return result.Count > 0 ? result : null;
            }
            */

        }
        public void BindParams(MySqlCommand cmd) { 
        
            cmd.Parameters.Add(new MySqlParameter("ph_num_type", ContactNumberType));
            cmd.Parameters.Add(new MySqlParameter("ph_number", Number));
            cmd.Parameters.Add(new MySqlParameter("ph_ext", AreaCode));
            cmd.Parameters.Add(new MySqlParameter("country", CountryCode));
        }
    }
}
