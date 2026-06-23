using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SQLite.CodeFirst;

namespace anbar.Data.Models
{
    public class Person
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PersonId { get; set; }
        [Required]
        [Unique]
        public string PersonIdByUser { get; set; }
        [Required]
        public string PersonName { get; set; }
        public string PersonModel { get; set; }
        public string PersonCity { get; set; }
        public string PersonProvince { get; set; }
        public string PersonMobile { get; set; }
        public string PersonPhone { get; set; }
        public string PersonPhoneCode { get; set; }
        public string PersonAddress { get; set; }
        public string PersonDetail { get; set; }
    }
}
