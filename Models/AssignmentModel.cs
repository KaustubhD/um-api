using System.Collections.Generic;
using System;
using MySql.Data.MySqlClient;

namespace UserManagement.Models
{
    public class AssignmentModel : BaseEntity
    {
        public String name { get; set; }
        public AddressModel? address { get; set; }
        public AgePrams? currentCompanyExp { get; set; }
        public AssignmentContactModel? contactDetail {get; set; }
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
            this.address = u.addresses[0];
            this.contactDetail = new AssignmentContactModel()
            {
                Primary = u.phones[0].Number,
                Secondary = u.phones[1].Number
            };
            this.currentCompanyExp = calc(Convert.ToDateTime(u.DOJ));
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
            Console.WriteLine("----------" + names[0]);

            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"update user set first_name=@f,middle_name=@m,last_name=@l where username=@u;";
            cmd.Parameters.Add(new MySqlParameter("@f", names[0]));
            cmd.Parameters.Add(new MySqlParameter("@u", uname));
            var midName = new MySqlParameter("@m", DBNull.Value);

            switch (names.Length){
                case 1:
                    break;
                case 2:
                    cmd.Parameters.Add(new MySqlParameter("@l", names[1]));
                    break;
                case 3:
                    midName.Value = names[1];
                    cmd.Parameters.AddWithValue("@l", names[2]);
                    break;
                default:
                    string mid = "";
                    for (int i = 1; i < names.Length - 1; i++) { mid += names[i] + " "; }
                    mid = mid.Remove(mid.LastIndexOf(" "), " ".Length).Insert(mid.LastIndexOf(" "), ""); // Remove last space
                    cmd.Parameters.Add(new MySqlParameter("@m", mid));
                    cmd.Parameters.AddWithValue("@l", names[names.Length - 1]);
                    break;
            }
            cmd.Parameters.Add(midName);
            cmd.ExecuteNonQuery();

            
            cmd.CommandText = @"update contact_number c inner join user_to_contact using(contact_id) inner join user using(user_id) inner join contact_type using(contact_type_id) set number=@num where username=@u and contact_type=@typ";
            var number = new MySqlParameter("@num", model.contactDetail.Primary);
            var numberType = new MySqlParameter("@typ", "Mobile");

            cmd.Parameters.Add(number);
            cmd.Parameters.Add(numberType);
            cmd.ExecuteNonQuery();

            if (!string.IsNullOrEmpty(model.contactDetail.Secondary))
            {
                number.Value =  model.contactDetail.Secondary;
                numberType.Value = "Work";
                cmd.ExecuteNonQuery();
            }
            
            //string[] address = model.address.Split(",");

            //string add = "";
            cmd.CommandText = @"call update_address(@u,@address,@city,@state,@coun,@pin)";

            cmd.Parameters.AddWithValue("@pin", model.address.PIN);
            cmd.Parameters.AddWithValue("@state", model.address.State);
            cmd.Parameters.AddWithValue("@city", model.address.City);
            cmd.Parameters.AddWithValue("@coun", model.address.Country);
            
            cmd.Parameters.AddWithValue("@address", model.address.AddressLine);
            cmd.ExecuteNonQuery();


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

    public class AssignmentContactModel
    {
        public String Primary { get; set; }
        public String Secondary { get; set; }
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
