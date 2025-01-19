using System.ComponentModel.DataAnnotations;

namespace ProjektDb.Models
{
    public class Log
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string TableName { get; set; }
        public Guid ObjectId { get; set; }

        public DateTime DeleteDate { get; set; } = DateTime.Now;


    }
}
