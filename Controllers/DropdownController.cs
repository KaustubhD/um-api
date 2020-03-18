using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DropdownController : ControllerBase
    {
        public DropdownController(AppDb db)
        {
            Db = db;
        }
        [HttpGet]
        [Route("country")]
        public IActionResult Countries()
        {
            
            List<DropdownModel> countries = new List<DropdownModel>();
            Db.Connection.Open();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "select country_id, country_name from country";
            var reader = cmd.ExecuteReader();
            
            using (reader)
            {
                while (reader.Read())
                {
                    countries.Add(new DropdownModel()
                    {
                        id = reader.GetInt32(0),
                        name = reader.GetString(1)
                    });
                }
            }
            Db.Connection.Close();
            if (countries.Count > 0)
            {
                return Ok(countries);
            }
            else
                return NoContent();
        }

        [HttpGet]
        [Route("state")]
        public IActionResult States()
        {
            String fields = HttpContext.Request.Query["id"];
            List<DropdownModel> states = new List<DropdownModel>();
            Db.Connection.Open();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "select state_id, state_name from state s inner join country using(country_id) where country_id=@c_id";
            cmd.Parameters.AddWithValue("@c_id", fields);
            var reader = cmd.ExecuteReader();

            using (reader)
            {
                while (reader.Read())
                {
                    states.Add(new DropdownModel()
                    {
                        id = reader.GetInt32(0),
                        name = reader.GetString(1)
                    });
                }
            }
            Db.Connection.Close();
            if (states.Count > 0)
            {
                return Ok(states);
            }
            else
                return NoContent();
        }

        [HttpGet]
        [Route("city")]
        public IActionResult Cities()
        {
            String fields = HttpContext.Request.Query["id"];
            List<DropdownModel> cities = new List<DropdownModel>();
            Db.Connection.Open();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "select city_id, city_name from city s inner join state using(state_id) where state_id=@c_id";
            cmd.Parameters.AddWithValue("@c_id", fields);
            var reader = cmd.ExecuteReader();

            using (reader)
            {
                while (reader.Read())
                {
                    cities.Add(new DropdownModel()
                    {
                        id = reader.GetInt32(0),
                        name = reader.GetString(1)
                    });
                }
            }
            Db.Connection.Close();
            if (cities.Count > 0)
            {
                return Ok(cities);
            }
            else
                return NoContent();
        }

        [Route("salutation")]
        public IActionResult Salutations()
        {

            List<DropdownModel> salutations = new List<DropdownModel>();
            Db.Connection.Open();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "select * from salutation;";
            var reader = cmd.ExecuteReader();

            using (reader)
            {
                while (reader.Read())
                {
                    salutations.Add(new DropdownModel()
                    {
                        id = reader.GetInt32(0),
                        name = reader.GetString(1)
                    });
                }
            }
            Db.Connection.Close();
            if (salutations.Count > 0)
            {
                return Ok(salutations);
            }
            else
                return NoContent();
        }

        [Route("addressType")]
        public IActionResult addressTypes()
        {

            List<DropdownModel> addTypes = new List<DropdownModel>();
            Db.Connection.Open();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "select * from address_type;";
            var reader = cmd.ExecuteReader();

            using (reader)
            {
                while (reader.Read())
                {
                    addTypes.Add(new DropdownModel()
                    {
                        id = reader.GetInt32(0),
                        name = reader.GetString(1)
                    });
                }
            }
            Db.Connection.Close();
            if (addTypes.Count > 0)
            {
                return Ok(addTypes);
            }
            else
                return NoContent();
        }

        [Route("contactType")]
        public IActionResult contactTypes()
        {

            List<DropdownModel> phoneTypes = new List<DropdownModel>();
            Db.Connection.Open();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "select * from contact_type;";
            var reader = cmd.ExecuteReader();

            using (reader)
            {
                while (reader.Read())
                {
                    phoneTypes.Add(new DropdownModel()
                    {
                        id = reader.GetInt32(0),
                        name = reader.GetString(1)
                    });
                }
            }
            Db.Connection.Close();
            if (phoneTypes.Count > 0)
            {
                return Ok(phoneTypes);
            }
            else
                return NoContent();
        }


        public AppDb Db { get; }
    }

  
    class DropdownModel
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}