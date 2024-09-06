using System.Text.Json.Serialization;

namespace JobHunterAPI.Models
{
    public class JobCategory
    {
        public int JobId { get; set; }
        public Job Job { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

      

    }

}
