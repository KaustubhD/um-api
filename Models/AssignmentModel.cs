using System.Collections.Generic;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace UserManagement.Models
{
    public class AssignmentModel
    {
        public String Name { get; set; }
        public String Address { get; set; }
        // public DateTime DOJ  { get; set; }
        public AgePrams currentCompanyExp { get; set; }
        public Object ContactDetail {get; set;}
        public AgePrams age { get; set; }
        internal AppDb Db { get; set; }
        public AssignmentModel() { }
        internal AssignmentModel(AppDb db) {
            Db = db;
        }

        public List<AssignmentModel> getAllUsers() {
            var ls = new List<AssignmentModel>();
            
            var users = new User(Db).GetAllUsers();
            foreach(User u in users)
            {
                var post = new AssignmentModel();

                //  Id = reader.GetInt32(0),
                post.Name = u.FirstName + " " + u.MiddleName + " " + u.LastName;
                post.Address = u.addresses[0].AddressLine + "," + u.addresses[0].City + "," + u.addresses[0].State + "," + u.addresses[0].PIN;
                
               // post.ContactDetail = new List<Object>();
                post.ContactDetail = new {
                    Primary = u.phones[0].Number,
                    Secondary = u.phones[1].Number
                };

                post.currentCompanyExp = CalculateDate(Convert.ToDateTime(u.DOJ));
                post.age = CalculateDate(Convert.ToDateTime(u.DOB));
                ls.Add(post);
            }


            return ls;
        }


        public AgePrams CalculateDate(DateTime b)
        {

            // a =  DateTime.Now();
            String CurrentDay = DateTime.Now.ToString("dd");
            String CurrentMonth = DateTime.Now.ToString("MM");
            String CurrentYear = DateTime.Now.ToString("yyyy");
            String DOJDay = b.ToString("dd");
            String DOJMonth = b.ToString("MM");
            String DOJYear = b.ToString("yyyy");
            int Months = (Convert.ToInt32(CurrentMonth) - Convert.ToInt32(DOJMonth));
            int Years = (Convert.ToInt32(CurrentYear) - Convert.ToInt32(DOJYear));
            if (Convert.ToInt32(CurrentDay) < Convert.ToInt32(DOJDay))
            {
                Months--;
            }
            if (Convert.ToInt32(Months) < 0)
            {
                Years--;
                Months += 12;
            }
            // int offs=b.AddMonths(   (Years*12)  +Months     ).Days;
            //  int Days=(int)((today.Ticks-offs.Ticks)/TimeSpan.TicksPerday);
            //int Days = offs;
            return new AgePrams(0, Months, Years);
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
    }
}
