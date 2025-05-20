using NorthWindExercises.Models;

namespace NorthWindExercises.Modeldto
{
    public class CustomerDTO
    {
        public string CustomerId { get; set; } = null!;

        public string CompanyName { get; set; } = null!;

        public string? ContactName { get; set; }

        public string? ContactTitle { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }


        public string? Country { get; set; }

        public CustomerDTO(Customer customer)
        {
            this.CustomerId = customer.CustomerId;
            this.CompanyName = customer.CompanyName;
            this.ContactName = customer.ContactName;
            this.ContactTitle = customer.ContactTitle;
            this.Address = customer.Address;
            this.City = customer.City;
            this.Country = customer.Country;
        }
    }
}
