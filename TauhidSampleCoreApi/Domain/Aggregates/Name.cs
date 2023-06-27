using Zero.SeedWorks;
using Zero.SharedKernel.Types.Result;

namespace TauhidSampleCoreApi.Domain.Aggregates
{
    public class Name : ValueObject
    {
        public string Value { get; private set; }
        public static readonly char[] _notAllowedCharacters = new char[] { '$', '^', '`', '<', '>', '+', '/', '=', '~' };

        private Name(string value)
        {
            Value = value;
        }
        public static Result<Name> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Result.Failure<Name>("Name Can't Be Blank.");
            if (value.Length > 100)
                return Result.Failure<Name>("Name Is Too Long.");
            if (value.IndexOfAny(_notAllowedCharacters) != -1)
                return Result.Failure<Name>("Some special characters are not allowed in the name.");

            return Result.Success(new Name(value));
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
        public static implicit operator string(Name name)
        {
            return name.Value;
        }
        public static explicit operator Name(string name)
        {
            return Create(name).Value;
        }
    }
}
