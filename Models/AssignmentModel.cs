using System.Collections.Generic;
using System;

namespace UserManagement.Models
{
    public class AssignmentModel : BaseEntity
    {
        public String name { get; set; }
        public String? address { get; set; }
        public String? currentCompanyExp { get; set; }
        public Object? contactDetail {get; set; }
        public AgePrams? age { get; set; }
        public bool? isIndian { get; set; }
        internal AppDb Db { get; set; }
        public AssignmentModel() { }
        internal AssignmentModel(AppDb db) {
            Db = db;
        }
        internal AssignmentModel(User u)
        {
            this.name = u.FirstName + (u.MiddleName != "" ? " " + u.MiddleName : "") + " " + u.LastName;
            this.isIndian = u.addresses[0].Country == "India";
            this.address = u.addresses[0].AddressLine + "," + u.addresses[0].City + "," + u.addresses[0].State + "," + u.addresses[0].PIN;
            this.contactDetail = new
            {
                Primary = u.phones[0].Number,
                Secondary = u.phones[1].Number
            };
            this.currentCompanyExp = calc(Convert.ToDateTime(u.DOJ)).ToString();
            this.age = calc(Convert.ToDateTime(u.DOB));

        }

        public List<AssignmentModel> getAllUsersInCustomFormat(String names = "") {
            var ls = new List<AssignmentModel>();
            
            var users = new User(Db).GetAllUsers(names);
            foreach(User u in users)
            {
                var post = new AssignmentModel(u);

                 
                ls.Add(post);
            }


            return ls;
        }
        public AssignmentModel getUserInCustomFormat(int id)
        {
            var user = new User(Db).getUserById(id);
            return new AssignmentModel(user);

        }

        public void Update(String uname, AssignmentModel model)
        {
            string[] names = model.name.Split(" ");
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"update user set first_name=@f,middle_name=@m,last_name=@l where username=@u";
            cmd.Parameters.AddWithValue("@f", names[0]);

            switch (names.Length){
                case 1:
                    break;
                case 2:
                    cmd.Parameters.AddWithValue("@l", names[1]);
                    break;
                case 3:
                    cmd.Parameters.AddWithValue("@m", names[1]);
                    cmd.Parameters.AddWithValue("@l", names[2]);
                    break;
                default:
                    string mid = "";
                    for (int i = 1; i < names.Length - 1; i++) { mid += names[i] + " "; }
                    string result = mid.Remove(mid.LastIndexOf(" "), " ".Length).Insert(mid.LastIndexOf(" "), "");
                    System.Diagnostics.Debug.Write("---------------"+result);
                    cmd.Parameters.AddWithValue("@m", mid);
                    cmd.Parameters.AddWithValue("@l", names[names.Length - 1]);
                    break;
            }

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
            String res = "";
            res += this.Years == 0 ? "" : (this.Years > 1 ? this.Years + " years " : this.Years + " year ");
            res += this.Months == 0 ? "" : (this.Months > 1 ? this.Months + " months" : this.Months + " month");
            return res;
        }

        
    }
}
