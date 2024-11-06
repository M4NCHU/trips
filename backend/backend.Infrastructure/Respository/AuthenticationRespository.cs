using backend.Domain.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace backend.Infrastructure.Repository
{
    public class AuthenticationRepository : UserManager<UserModel>, IAuthenticationRepository
    {
        private readonly ILogger<AuthenticationRepository> _logger;

        public AuthenticationRepository(
            IUserStore<UserModel> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<UserModel> passwordHasher,
            IEnumerable<IUserValidator<UserModel>> userValidators,
            IEnumerable<IPasswordValidator<UserModel>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<UserModel>> logger,
            ILogger<AuthenticationRepository> authLogger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _logger = authLogger;
        }

        // Implementacja niestandardowej metody logowania
        public async Task LogCustomInformationAsync(string message)
        {
            _logger.LogInformation(message);
            await Task.CompletedTask;
        }
    }
}