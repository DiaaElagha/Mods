using System;
using System.Collections.Generic;

namespace SkinsAdmin.Models
{
    public class HomeVM 
    {
        public List<Mods> Mods { get; set; }
        public int? totalSkins { get; set; } = 0;
        public int? totalCategory { get; set; } = 0;
    }
}
