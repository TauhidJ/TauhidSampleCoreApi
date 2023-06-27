using System.Text.RegularExpressions;
using Zero.SeedWorks;
using Zero.SharedKernel.Constants;
using Zero.SharedKernel.Types.Result;

namespace TauhidSampleCoreApi.Domain.Aggregates.CustomerAggregate
{
    public class MobileNumber : ValueObject
    {
        public string Value { get; private set; }

        //Empty Value Condition Incase Value Not Passed From User
        private static readonly MobileNumber _empty = new(string.Empty);
        public static MobileNumber Empty => _empty;

        private MobileNumber(string value)
        {
            Value = value;
        }

        public static Result<MobileNumber> Create(string? value, bool allowNull = false)
        {   //Check if value of mobilenumber Is Null, 
            if (allowNull && value == null)
                return Result.Success(_empty);  //Result is success if value is null or empty
            //Check if Mobilenumber contains any space or if it does not match to our regex pattern
            if (string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value, RegexPatterns.MobileNumber))
                return Result.Failure<MobileNumber>("Mobile number is Invalid"); //result failure if space present on mobile no or not match to our regex pattern 

            return Result.Success(new MobileNumber(value));//if all above conditions are false value passed to the private constructor
        }
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
        public static implicit operator string(MobileNumber mobileNumber)
        {
            return mobileNumber.Value;
        }
        public static explicit operator MobileNumber(string mobilenumber)
        {
            return Create(mobilenumber).Value;
        }
    }
}
