using NorthWindExercises.Models;

namespace NorthWindExercises.Modeldto
{
    public class EmployeeDTO
    {
        public int EmployeeId { get; set; }

        public string LastName { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string? FullName => $"{LastName} {FirstName}";

        public string? Title { get; set; }

        public string? TitleOfCourtesy { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime? HireDate { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? Region { get; set; }

        public string? PostalCode { get; set; }

        public string? Country { get; set; }

        public string? HomePhone { get; set; }

        public string? Extension { get; set; }

        public byte[]? Photo { get; set; }

        public string? Notes { get; set; }

        public int? ReportsTo { get; set; }

        public string? PhotoPath { get; set; }

        public EmployeeDTO(Employee employee)
        {
            this.EmployeeId = employee.EmployeeId;
            this.LastName = employee.LastName;
            this.FirstName = employee.FirstName;
            this.Title = employee.Title;
            this.TitleOfCourtesy = employee.TitleOfCourtesy;
            this.BirthDate = employee.BirthDate;
            this.HireDate = employee.HireDate;
            this.Address = employee.Address;
            this.City = employee.City;
            this.Region = employee.Region;
            this.PostalCode = employee.PostalCode;
            this.Country = employee.Country;
            this.HomePhone = employee.HomePhone;
            this.Extension = employee.Extension;
            this.Notes = employee.Notes;
            this.ReportsTo = employee.ReportsTo;
        }
    }
}
