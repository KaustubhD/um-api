using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Models
{
    public class JsonModel
    {
        public JsonModel(string s)
        {
            json = s;
        }
        public string json { get; set; }
    }
}
