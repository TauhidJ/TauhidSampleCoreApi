using System.Text.RegularExpressions;
using Zero.SeedWorks;
using Zero.SharedKernel.Constants;
using Zero.SharedKernel.Types.Result;

namespace TauhidSampleCoreApi.Domain.Aggregates.CustomerAggregate
{
    public class EmailAddress : ValueObject
    {
        public string Value { get; private set; }

        public static readonly EmailAddress _empty = new(string.Empty);
        public static EmailAddress Empty => _empty;

        private EmailAddress(string value)
        {
            Value = value;
        }

        public static Result<EmailAddress> Create(string? value, bool allowNull = false)
        {
            if (allowNull && value == null)
                return Result.Success(_empty);
            if (string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value, RegexPatterns.EmailAddress))
                return Result.Failure<EmailAddress>("Email address is invalid");

            return Result.Success(new EmailAddress(value));
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
        public static implicit operator string(EmailAddress emailAddress)
        {
            return emailAddress.Value;
        }
        public static explicit operator EmailAddress(string emailAddress)
        {
            return Create(emailAddress).Value;
        }
    }
}
