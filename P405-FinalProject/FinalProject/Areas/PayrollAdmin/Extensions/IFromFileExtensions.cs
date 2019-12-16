using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.Extensions
{
    public static class IFromFileExtensions
    {
        public static bool IsImage(this IFormFile file)
        {
            return file.ContentType.Contains("image/");
        }

        public static bool IsLessThan(this IFormFile file, int mb)
        {
            return file.Length / 1024 / 1024 < mb;
        }

        public async static Task<string> SaveAsync(this IFormFile file, string root, string folder)
        {

            string path = Path.Combine(root, "img");

            string fileName = Path.Combine(folder, Guid.NewGuid().ToString() + file.FileName);

            string resultName = Path.Combine(path, fileName);

            using (FileStream fileStream = new FileStream(resultName, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return fileName;
        }
    }
}
