using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using User.Identity.Services;

namespace User.Identity.Authentication
{
    /// <summary>
    /// 自定义扩展IdentityService4 授权模式
    /// </summary>
    public class SmsAuthCodeValidator : IExtensionGrantValidator
    {
        /// <summary>
        /// 定义授权验证名称
        /// </summary>
        public string GrantType => "sms_auth_code";

        private readonly IAuthCodeService _authCodeService;
        private readonly IUserService _userService;

        public SmsAuthCodeValidator(IAuthCodeService iAuthCodeService,IUserService userService)
        {
            _authCodeService = iAuthCodeService;
            _userService = userService;
        }

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            //从请求中获得 手机号和验证码
            var phone = context.Request.Raw["phone"];
            var code = context.Request.Raw["auth_code"];

            //授权失败
            var errorValidationResult = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
            //检查手机号和验证码参数是否符合预期
            if (string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(code) )
            {
                context.Result = errorValidationResult;
                return;
            }
            
            //检测验证码
            if(!await _authCodeService.Validate(phone,code))
            {
                context.Result = errorValidationResult;
                return;
            }
            
//            //构建UserClaims
//            var claims = new Claim[]
//            {             
//                new Claim("name","gzz"),
//                new Claim(phone,code),
//            };
//            context.Result = new GrantValidationResult("gzz_id", GrantType, claims);
            
            //完成用户注册
            var userId = await _userService.CheckOrCreate(phone);
            if (userId<=0)
            {
                context.Result = errorValidationResult;
                return;
            }

            context.Result = new GrantValidationResult(userId.ToString(), GrantType);
        }
    }
}
