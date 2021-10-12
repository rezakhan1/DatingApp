using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController:BaseController
    {
        DataContext _datacontext;
       public BuggyController(DataContext datacontext)
       {
           _datacontext=datacontext;
       } 

       [HttpGet("auth")]
       public ActionResult<string> getSecret(){
        return Unauthorized("Not Authorized !");
       }
       [HttpGet("not-found")]
       public ActionResult<string> notFound(){
          var thing=_datacontext.User.Find(-1);
          if(thing ==null)return NotFound();
          return Ok(thing);
       }
       [HttpGet("server-error")]
       public ActionResult<string> getServerError(){
          var thing=_datacontext.User.Find(-1);
          var thingsToReturn=thing.ToString();
          return thingsToReturn;
       }
       [HttpGet("bad-request")]
       public ActionResult<string> getBadRequest(){
        return BadRequest("Bad Request");
       }
    }
}