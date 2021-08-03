using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SkinsAdmin.Models
{
    public class Apps : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Pleace Enter Activation")]
        [Display(Name = "Activation")]
        [ScaffoldColumn(false)]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Pleace Enter App Name")]
        [Display(Name = "App Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Pleace Enter AppKey")]
        [Display(Name = "App Key")]
        public string AppKey { get; set; }

        [Required(ErrorMessage = "Pleace Enter Category")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }
    }
}
