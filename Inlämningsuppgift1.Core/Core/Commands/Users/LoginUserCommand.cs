using Inlämningsuppgift1.Core.Core.Common;
using static Inlämningsuppgift_1.Core.Interfaces.ICommand;

namespace Inlämningsuppgift_1.Core.Commands.Users
{
    public class LoginUserCommand : ICommand<Result<string?>>
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";

    }
}
