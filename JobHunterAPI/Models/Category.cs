using System.Text.Json.Serialization;

namespace JobHunterAPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryCode { get; set; }

        [JsonIgnore]
        public ICollection<JobCategory> JobCategories { get; set; }
    }
}
