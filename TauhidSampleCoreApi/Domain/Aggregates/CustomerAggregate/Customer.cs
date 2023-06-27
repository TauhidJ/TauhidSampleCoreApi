using TauhidSampleCoreApi.Shared.Errors.CustomerErrors;
using Zero.SeedWorks;
using Zero.SharedKernel.Types.Result;

namespace TauhidSampleCoreApi.Domain.Aggregates.CustomerAggregate
{
    public class Customer : Entity, IAggregateRoot
    {
        public int CustomerId { get; private set; }
        public Name Name { get; private set; }
        public MobileNumber? MobileNumber { get; private set; }
        public EmailAddress? EmailAddress { get; private set; }
        public bool IsDeleted { get; private set; }

        public Customer()
        {   
        }

        /// <summary>
        /// Creates new customer
        /// </summary>
        /// <param name="name">Name of the customer</param>
        /// <param name="mobileNumber">Mobile number of the customer</param>
        /// <param name="emailAddress">Email address of the customer</param>
        /// <exception cref="ArgumentNullException"></exception>
        public Customer(Name name, MobileNumber? mobileNumber, EmailAddress? emailAddress)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));

            MobileNumber = mobileNumber;
            EmailAddress = emailAddress;
            IsDeleted = false;
        }

        /// <summary>
        /// Updates customer's details
        /// <para>
        /// Errors:
        /// <list type="bullet">
        /// <item><term><see cref="DeletedCustomerError"/></term><description>If the customer is deleted.</description></item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="name">Name of the customer</param>
        /// <param name="mobileNumber">Mobile number of the customer</param>
        /// <param name="emailAddress">Email address of the customer</param>
        public Result Update(Name name, MobileNumber? mobileNumber, EmailAddress? emailAddress)
        {
            if (IsDeleted) return Result.Failure(new DeletedCustomerError("Deleted customer can not be updated."));

            Name = name;
            MobileNumber = mobileNumber;
            EmailAddress = emailAddress;
            return Result.Success();
        }
        /// <summary>
        /// Deletes a customer.
        /// </summary>
        public void Delete()
        {
            IsDeleted = true;
        }
    }
}
