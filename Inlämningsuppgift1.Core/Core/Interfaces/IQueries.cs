using MediatR;

namespace Inlämningsuppgift_1.Core.Interfaces
{
    public interface IQueries
    {
        public interface IQuery<out TResponse> : IRequest<TResponse>
        {
        }
    }
}
