using Inlämningsuppgift_1.Core.Commands.Users;
using Inlämningsuppgift_1.Core.Interfaces;
using Inlämningsuppgift_1.Data.Interfaces;
using Inlämningsuppgift1.Core.Core.Common;
using MediatR;

namespace Inlämningsuppgift_1.Core.Handlers.Commands.Users
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterUserCommandHandler(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
        }

        public Task<Result<bool>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {

            if (string.IsNullOrWhiteSpace(request.Username))
                return Task.FromResult<Result<bool>>(Errors.Validation);
            if (string.IsNullOrWhiteSpace(request.Password))
                return Task.FromResult<Result<bool>>(Errors.Validation);
            if (request.Password.Length < 6)
                return Task.FromResult<Result<bool>>(Errors.Validation);
            if (string.IsNullOrWhiteSpace(request.Email) || !request.Email.Contains("@"))
                return Task.FromResult<Result<bool>>(Errors.Validation);

            var existingUser = _unitOfWork.Users.GetByUsername(request.Username);
            if (existingUser != null)
                return Task.FromResult<Result<bool>>(Errors.DuplicateEntity);

            var hashedPassword = _passwordHasher.HashPassword(request.Password);
            var newUser = new Data.Entities.User
            {
                Username = request.Username,
                Password = hashedPassword,
                Email = request.Email
            };

            _unitOfWork.Users.Add(newUser);

            return Task.FromResult<Result<bool>>(true);
        }
    }
}
