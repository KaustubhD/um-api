using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace UserManagement
{
    public class Address
    {
        public string AddressType { get; set; }
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        internal AppDb Db { get; set; }

        public Address()
        {
        }

        internal Address(AppDb db)
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

    }
}
