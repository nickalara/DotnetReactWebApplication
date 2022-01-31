namespace WebApplication1.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int DepartmentID { get; set; }
        public string HireDate { get; set; }
        public string Picture { get; set; }
        public bool IsActive { get; set; }
        public string ByCreated { get; set; }
        public string ByUpdated { get; set; }
        public string DateCreated { get; set; }
        public string DateUpdated { get; set; }
    }
}
