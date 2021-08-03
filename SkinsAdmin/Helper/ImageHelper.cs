using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SkinsAdmin.Helper
{
    public class ImageHelper
    {
        public static async Task<string> SaveImage(IFormFile img, IHostingEnvironment environment, String Foldername)
        {
            String fileName = null;
            if (img != null && img.Length > 0)
            {
                var uploads = Path.Combine(environment.WebRootPath, Foldername);
                if (img.Length > 0)
                {
                    fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(img.FileName);
                    using (var fileStream = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                    {
                        await img.CopyToAsync(fileStream);
                    }
                }
            }
            return fileName;
        }
    }
}
