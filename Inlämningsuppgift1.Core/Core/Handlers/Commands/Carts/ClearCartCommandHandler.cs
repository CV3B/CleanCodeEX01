using Inlämningsuppgift_1.Core.Commands.Carts;
using Inlämningsuppgift_1.Data.Interfaces;
using Inlämningsuppgift1.Core.Core.Common;
using MediatR;

namespace Inlämningsuppgift_1.Core.Handlers.Commands.Carts
{
    public class ClearCartCommandHandler : IRequestHandler<ClearCartCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClearCartCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<Result<bool>> Handle(ClearCartCommand request, CancellationToken cancellationToken)
        {
            var user = _unitOfWork.Users.GetById(request.UserId);

            if (user == null)
                return Task.FromResult<Result<bool>>(Errors.NotFound);

            var cart = _unitOfWork.Carts.GetByUserId(user.Id);

            if (cart.Count == 0)
                return Task.FromResult<Result<bool>>(Errors.Validation);


            _unitOfWork.Carts.Clear(request.UserId);
            return Task.FromResult<Result<bool>>(true);

        }
    }
}
