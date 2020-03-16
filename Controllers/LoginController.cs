using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace UserManagement.Controllers{
    [Route("api/[controller]")]
    public class LoginController : ControllerBase{ 
    
        public LoginController(AppDb db){
            Db = db;
        }

        [HttpPost]
        public IActionResult Post([FromBody]Login body){
            Db.Connection.Open();
            body.Db = Db;
            var result = body.checkAuth();
            if(result == null){
                return Forbid();
            }
 
            return new OkObjectResult(result);
        }
        public AppDb Db { get; }
    }

}
