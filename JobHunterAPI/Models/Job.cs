using System.Data.SqlTypes;
using System.Text.Json.Serialization;

namespace JobHunterAPI.Models
{
    public class Job
    {
        public int Id { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyLogo { get; set; }
        public string? JobTitle { get; set; }
        public string? JobType { get; set; }
        public string ?JobDescription { get; set; }
        public string ?JobStatus { get; set; }
        public string? RequiredExperience { get; set; }
        public decimal? Salary{ get; set; }
        public string?  JobLocation { get; set; }

        public DateOnly? ExpiresOn { get; set; }
        public DateOnly? CreatedOn { get; set; }

    



        //Naviation Props
        [JsonIgnore]
        public ICollection<JobCategory> JobCategories { get; set; }
    }
}
