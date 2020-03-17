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
    public class LocationController : ControllerBase
    {
        public LocationController(AppDb db)
        {
            Db = db;
        }
        [HttpGet]
        [Route("country")]
        public IActionResult Countries()
        {
            
            List<Demograph> countries = new List<Demograph>();
            Db.Connection.Open();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "select country_id, country_name from country";
            var reader = cmd.ExecuteReader();
            
            using (reader)
            {
                while (reader.Read())
                {
                    countries.Add(new Demograph()
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
            List<Demograph> states = new List<Demograph>();
            Db.Connection.Open();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "select state_id, state_name from state s inner join country using(country_id) where country_id=@c_id";
            cmd.Parameters.AddWithValue("@c_id", fields);
            var reader = cmd.ExecuteReader();

            using (reader)
            {
                while (reader.Read())
                {
                    states.Add(new Demograph()
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
            List<Demograph> cities = new List<Demograph>();
            Db.Connection.Open();
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = "select city_id, city_name from city s inner join state using(state_id) where state_id=@c_id";
            cmd.Parameters.AddWithValue("@c_id", fields);
            var reader = cmd.ExecuteReader();

            using (reader)
            {
                while (reader.Read())
                {
                    cities.Add(new Demograph()
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
        public AppDb Db { get; }
    }
    class Demograph
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}