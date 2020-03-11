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
        public async Task<IActionResult> Post([FromBody]Login body){
            await Db.Connection.OpenAsync();
            body.Db = Db;
            var result = await body.checkAuth();
            //if(result == null){
            //    Context.Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
            //    Context.Response.End();
            //}
 
            return new OkObjectResult(result);
            
            // return Ok();
        }
        public AppDb Db { get; }
    }

}
