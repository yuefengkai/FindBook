using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using User.Api.Data;
using User.Api.Dtos;

namespace User.Api.Controllers
{
    [Route("api/users")]
    public class UserController : BaseController
    {
        private UserContext _userContext;
        private ILogger<UserController> _logger;
        
        public UserController(UserContext userContext,ILogger<UserController> logger)
        {
            _userContext = userContext;
            _logger = logger;
        }
        
        // GET api/values
        [Route("")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = await _userContext.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == UserIdentity.UserId);

            if (user == null)
            {
                var ex = new UserOperationException($"错误的用户上下文Id {UserIdentity.UserId}");

                throw ex;
            }

            return Json(user);
        }
        
        // GET api/values
        [Route("")]
        [HttpPatch]
        public async Task<IActionResult> Patch()
        {
            return Json(new
            {
                message = "welcome to gitlab ci build",
                user = await _userContext.Users.SingleOrDefaultAsync(u => u.Name == "gzz")
            });
        }
    }
}
