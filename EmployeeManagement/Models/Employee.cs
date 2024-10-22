namespace EmployeeManagement.Models
{
    public class Employee: UserActivity
    {
        public int Id { get; set; }
        public string LectureName { get; set; }  // Made consistent with lowercase 'string'
        public string LectureSurname { get; set; }
        public string EmployeeNo { get; set; }

        public string ContactDetails { get; set; }
        public string Model { get; set; }  // Corrected property name "Modele" to "Model"

        public string Program { get; set; }

        public int NumberOfHours { get; set; }  // Corrected property name "numberOfHouhours" to "NumberOfHours"

        public int HourlyRate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string? IsApproved { get; set; } 

    }
}
