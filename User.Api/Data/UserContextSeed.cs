using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace User.Api.Data {
    public class UserContextSeed {
        private ILogger<UserContextSeed> _logger;

        public UserContextSeed (ILogger<UserContextSeed> logger) {
            _logger = logger;
        }

        public static async Task SeedAsync (IApplicationBuilder applicationBuilder, ILoggerFactory loggerFactory, int? retry = 0) {
            var retryForAviability = retry.Value;

            try {

                using (var scope = applicationBuilder.ApplicationServices.CreateScope ()) {
                    var userContext = (UserContext) scope.ServiceProvider.GetService (typeof (UserContext));
                    var logger = (ILogger<UserContextSeed>) scope.ServiceProvider.GetService (typeof (ILogger<UserContextSeed>));

                    logger.LogDebug ("UserContextSeed Start.....");

                    userContext.Database.Migrate ();

                    if (!userContext.Users.Any ()) {
                        userContext.Users.Add (new Models.AppUser { Name = $@"gzz" });

                        userContext.SaveChanges ();
                    }
                }
            } catch (Exception ex) {
                if (retryForAviability < 10) {
                    var logger = loggerFactory.CreateLogger (typeof (UserContextSeed));
                    logger.LogError ($"重试第{retryForAviability}次，{ex.Message}");

                    retryForAviability++;

                    await SeedAsync (applicationBuilder, loggerFactory, retryForAviability);
                }

            }
        }
    }
}