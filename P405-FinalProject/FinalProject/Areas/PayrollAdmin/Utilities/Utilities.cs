using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.Utilities
{
    public static class Utilities
    {
        public static bool RemoveImage(string root, string image)
        {
            string path = Path.Combine(root, "img", image);

            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }

            return false;
        }
    }
}
