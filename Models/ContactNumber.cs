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
        public void BindAndExecuteProcedure(MySqlCommand cmd, string ProcedureName)
        {
            cmd.CommandText = ProcedureName;
            cmd.CommandType = CommandType.StoredProcedure;
            BindParams(cmd);
            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
        }

        public void BindParams(MySqlCommand cmd) { 
        
            cmd.Parameters.Add(new MySqlParameter("ph_num_type", ContactNumberType));
            cmd.Parameters.Add(new MySqlParameter("ph_number", Number));
            cmd.Parameters.Add(new MySqlParameter("ph_ext", AreaCode));
            cmd.Parameters.Add(new MySqlParameter("country_code", CountryCode));
        }
    }
}
