using Zero.SeedWorks;
using Zero.SharedKernel.Types.Result;

namespace TauhidSampleCoreApi.Domain.Aggregates
{
    public class Quantity : ValueObject
    {
        public int Value { get; private set; }

        private Quantity(int value)
        {
            Value = value;
        }
        public static Result<Quantity> Create(int value)
        {

            if (value < 0)
                return Result.Failure<Quantity>("Quantity Should be greater than 0");

            return Result.Success(new Quantity(value));
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
        public static implicit operator int(Quantity quantity)
        {
            return quantity.Value;
        }
        public static explicit operator Quantity(int quantity)
        {
            return Create(quantity).Value;
        }
    }
}
