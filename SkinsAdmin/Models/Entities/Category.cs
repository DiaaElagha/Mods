using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SkinsAdmin.Models
{
    public class Category : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Pleace Enter Show Android")]
        [Display(Name = "Show Android")]
        [ScaffoldColumn(false)]
        public bool IsShowAndroid { get; set; }

        [Required(ErrorMessage = "Pleace Enter Show IOS")]
        [Display(Name = "Show IOS")]
        [ScaffoldColumn(false)]
        public bool IsShowIOS { get; set; }

        [Required(ErrorMessage = "Pleace Enter Name")]
        [Display(Name = "Category Name")]
        public string Name { get; set; }
    }
}
