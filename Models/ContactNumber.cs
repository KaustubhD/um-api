using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace UserManagement
{
    public class ContactNumber
    {
        public string ContactNumberType { get; set; }
        public string Number { get; set; }
        public string CountryId { get; set; }
        
        

        internal AppDb Db { get; set; }

        public ContactNumber()
        {
        }

        internal ContactNumber(AppDb db)
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

    }
}
