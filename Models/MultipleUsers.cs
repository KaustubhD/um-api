using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace UserManagement
{
    public class MultipleUsers
    {
        public User[] users { get; set; }

        internal AppDb Db { get; set; }

        public MultipleUsers()
        {
        }

        internal MultipleUsers(AppDb db)
        {
            Db = db;
        }
        public async Task AddAllUsers()
        {
            for(int i = 0; i < users.Length; i++){
                users[i].AddOneUser();
            }
        }

    }
}
