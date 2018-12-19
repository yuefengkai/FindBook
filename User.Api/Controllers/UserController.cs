using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using User.Api.Data;
using User.Api.Dtos;
using User.Api.Models;

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
        
         /// <summary>
        /// 用户更新个人信息
        /// </summary>
        /// <returns></returns>
        [Route("")]
        [HttpPatch]
        public async Task<IActionResult> Patch([FromBody]JsonPatchDocument<AppUser> patch)
        {
            var entity = await _userContext.Users
                                .SingleOrDefaultAsync(b => b.Id == UserIdentity.UserId);

            //将需要跟新的数据复制给对象
            patch.ApplyTo(entity);

            //如果有修改Properties, 不追踪 AppUser 实体的 Properties 属性 单独通过以下的方法进行处理
            if (entity.Properties != null)
            {

                foreach (var item in entity.Properties)
                {
                    _userContext.Entry(item).State = EntityState.Detached;

                }

                //Properties 属性 单独通过以下的方法进行处理
                //获取原来用户所有的Properties, 必须使用 AsNoTracking()，否则会自动附加到用户属性上
                var originProperties = await _userContext.UserPropertys.AsNoTracking().Where(b => b.AppUserId == UserIdentity.UserId).ToListAsync();


                foreach (var item in originProperties)
                {
                    if (!entity.Properties.Exists(b => b.Key == item.Key && b.Value == item.Value))
                    {
                        //如果不存在做删除操作
                        _userContext.Remove(item);
                    }
                }
                foreach (var item in entity.Properties)
                {
                    if (!originProperties.Exists(b => b.Key == item.Key && b.Value == item.Value))
                    {
                        //如果不存在做新增操作
                        _userContext.Add(item);
                    }
                }
            }
           
            using (var transAction = _userContext.Database.BeginTransaction())
            {
                //更新用户信息
                _userContext.Users.Update(entity);
                _userContext.SaveChanges();
                transAction.Commit();
            }

            return Json(entity);
        }

        /// <summary>
        /// 检测或创建用户(当用户手机不存在时创建用户)
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
       [Route("check-or-create")]
        [HttpPost]
        public async Task<IActionResult> CreateOrCreate(string phone)
        {
            //TDB 做手机号码格式验证

            var user = _userContext.Users.SingleOrDefault(u => u.Phone == phone);

            if (user != null) return Ok(user.Id);
            
            user = new AppUser {Phone = phone};
                
            _userContext.Users.Add(user);
                
            await _userContext.SaveChangesAsync();

            return Ok(user.Id);
        }

    }
}
