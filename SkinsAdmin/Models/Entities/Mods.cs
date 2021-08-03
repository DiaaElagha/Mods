using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SkinsAdmin.Models
{
    public class Mods : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Pleace Enter Name")]
        [Display(Name = "Mod Name")]
        public string ModName { get; set; }

        [Required(ErrorMessage = "Pleace Enter Description")]
        [Display(Name = "Mod Description")]
        public string ModDescription { get; set; }

        [Display(Name = "Mod Url")]
        public string ModUrl { get; set; }

        [Display(Name = "Thumbnail Image")]
        [Required(ErrorMessage = "Pleace Enter Thumbnail Image")]
        public string ModThumbnailPath { get; set; }

        [Required(ErrorMessage = "Pleace Enter Category")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }

    }
}
