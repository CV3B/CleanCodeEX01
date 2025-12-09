using Inlämningsuppgift_1.Core.Commands.Users;
using Inlämningsuppgift_1.Core.Interfaces;
using Inlämningsuppgift_1.Data.Constants;
using Inlämningsuppgift_1.Data.Interfaces;
using Inlämningsuppgift1.Core.Core.Common;
using MediatR;

namespace Inlämningsuppgift_1.Core.Handlers.Commands.Users
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<string?>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public LoginUserCommandHandler(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;   
        }

        public Task<Result<string?>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return Task.FromResult<Result<string?>>(Errors.Validation);

            var user = _unitOfWork.Users.GetByUsername(request.Username);
            if (user == null || !_passwordHasher.VerifyPassword(user.Password, request.Password))
                return Task.FromResult<Result<string?>>(Errors.Unauthorized);

            var token = TokenSettings.TokenPrefix + Guid.NewGuid().ToString();

            _unitOfWork.Tokens.Add(user.Id, token);
            
            return Task.FromResult<Result<string?>>(token);
        }
    }
}
