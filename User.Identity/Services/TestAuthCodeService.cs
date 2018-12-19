using System.Threading.Tasks;

namespace User.Identity.Services
{
    public class TestAuthCodeService:IAuthCodeService
    {
        public async Task<bool> Validate(string phone, string authCode)
        {
            //TBD 手机号的验证
            //测试环境默认返回 true
            return await Task.FromResult(true);
        }
    }
}
