using MediatR;

namespace Inlämningsuppgift_1.Core.Interfaces
{
    public interface ICommand
    {
        public interface ICommand<out TResponse> : IRequest<TResponse>
        {
        }

        public interface ICommand : IRequest
        {
        }
    }
}
