using Inlämningsuppgift_1.Core.Queries.Users;
using Inlämningsuppgift_1.Data.Entities;
using Inlämningsuppgift_1.Data.Interfaces;
using Inlämningsuppgift1.Core.Core.Common;
using MediatR;

namespace Inlämningsuppgift_1.Core.Handlers.Queries.Users
{
    public class GetUserByTokenQueryHandler : IRequestHandler<GetUserByTokenQuery, Result<User?>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserByTokenQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<Result<User?>> Handle(GetUserByTokenQuery request, CancellationToken cancellationToken)
        {
            var userId = _unitOfWork.Tokens.GetUserIdByToken(request.Token);

            if (userId == null)
                return Task.FromResult<Result<User?>>(Errors.Unauthorized);

            var user = _unitOfWork.Users.GetById(userId.Value);

            return Task.FromResult<Result<User?>>(user);

        }
    }
}
