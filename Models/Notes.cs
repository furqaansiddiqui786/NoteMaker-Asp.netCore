using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NoteMaker.Models
{
    public class Notes
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }

        [NotMapped]
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        
        [Required]
        public string Title { get; set; }

        [Required]
        public string Note { get; set; }

    }
}
