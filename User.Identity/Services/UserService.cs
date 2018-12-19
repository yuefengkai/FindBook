using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace User.Identity.Services
{
    public class UserService:IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly string _userServiceUrl = "http://localhost";

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<int> CheckOrCreate(string phone)
        {
            var url = _userServiceUrl + "/api/users/check-or-create";
            var form = new Dictionary<string, string>
            {
                {
                    "phone", phone
                }
            };

            var content =new FormUrlEncodedContent(form);

            var response = await _httpClient.PostAsync(url, content);

            if (response.StatusCode!= HttpStatusCode.OK)
            {
                return 0;
            }

            var userId = await response.Content.ReadAsStringAsync();

            int.TryParse(userId, out var intUserId);

            return intUserId;

        }
    }
}
