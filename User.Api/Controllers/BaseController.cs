using Microsoft.AspNetCore.Mvc;
using User.Api.Dtos;

namespace User.Api.Controllers
{
    public class BaseController:Controller
    {
        protected UserIdentity UserIdentity => new UserIdentity {UserId = 1, Name = "gzz"};
    }
}
