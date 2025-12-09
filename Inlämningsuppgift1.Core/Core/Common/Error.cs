namespace Inlämningsuppgift1.Core.Core.Common
{
    public enum ErrorType { NotFound, Validation, Unauthorized, DuplicateEntity, InsufficientStock }

    public record Error(string Id, ErrorType Type, string Description);

    public static class Errors
    {
        public static Error NotFound { get; } = new("NotFound", ErrorType.NotFound, $"This was not found.");
        public static Error Validation { get; } = new("Validation", ErrorType.Validation, "Wrong validation.");
        public static Error Unauthorized { get; } = new("Unauthorized", ErrorType.Unauthorized, "You are not authorized to perform this action.");
        public static Error DuplicateEntity { get; } = new("DuplicateEntity", ErrorType.DuplicateEntity, "This entity already exists.");
        public static Error InsufficientStock { get; } = new("InsufficientStock", ErrorType.InsufficientStock, "There is not enough stock for this product.");
    }
}
