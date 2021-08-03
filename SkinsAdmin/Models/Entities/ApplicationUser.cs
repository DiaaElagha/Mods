using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SkinsAdmin.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            CreateAt = DateTime.Now;
        }
        public string FullName { get; set; }



        [ScaffoldColumn(false)]
        public DateTime? CreateAt { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? UpdateAt { get; set; }
    }
}
