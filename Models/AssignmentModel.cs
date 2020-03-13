using System.Collections.Generic;
using System;

namespace UserManagement.Models
{
    public class AssignmentModel
    {
        public String Name { get; set; }
        public String Address { get; set; }
        public String currentCompanyExp { get; set; }
        public Object ContactDetail {get; set; }
        public AgePrams age { get; set; }
        public bool isIndian { get; set; }
        internal AppDb Db { get; set; }
        public AssignmentModel() { }
        internal AssignmentModel(AppDb db) {
            Db = db;
        }

        public List<AssignmentModel> getAllUsersInCustomFormat() {
            var ls = new List<AssignmentModel>();
            
            var users = new User(Db).GetAllUsers();
            foreach(User u in users)
            {
                var post = new AssignmentModel();

                //  Id = reader.GetInt32(0),
                post.Name = u.FirstName + " " + u.MiddleName + " " + u.LastName;
                post.isIndian = u.addresses[0].Country == "India";
                post.Address = u.addresses[0].AddressLine + "," + u.addresses[0].City + "," + u.addresses[0].State + "," + u.addresses[0].PIN;
                post.ContactDetail = new {
                    Primary = u.phones[0].Number,
                    Secondary = u.phones[1].Number
                };
                post.currentCompanyExp = calc(Convert.ToDateTime(u.DOJ)).ToString();
                post.age = calc(Convert.ToDateTime(u.DOB));
                 
                ls.Add(post);
            }


            return ls;
        }


        public AgePrams calc(DateTime b)
        {
            DateTime zeroTime = new DateTime(1, 1, 1);
            DateTime olddate = b;
            DateTime curdate = DateTime.Now.ToLocalTime();
            TimeSpan span = curdate - olddate;
            int years = (zeroTime + span).Year - 1;
            int months = (zeroTime + span).Month - 1;
            int days = (zeroTime + span).Day - 1;
            return new AgePrams(days, months, years);
        }
    }
    public class AgePrams
    {
        public int Years { get; set; }
        public int Months { get; set; }
        public int Days { get; set; }
        public AgePrams(int a, int b, int c)
        {
            Days = a;
            Months = b;
            Years = c;
        }
        public string ToString()
        {
            return this.Years + " years " + Months + " months";
        }

        
    }
}
