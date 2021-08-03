using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SkinsAdmin.Models.Entities
{
    public class ModImages : BaseEntity
    {

        [Key]
        public int Id { get; set; }

        [Display(Name = "Image Name")]
        public string ImageName { get; set; }

        [Required(ErrorMessage = "Pleace Enter Image Path")]
        [Display(Name = "Image Path")]
        public string ImagePath { get; set; }

        [Required(ErrorMessage = "Pleace Enter Mods")]
        [Display(Name = "Mods")]
        public int ModsId { get; set; }
        [ForeignKey(nameof(ModsId))]
        public Mods Mods { get; set; }
    }
}
