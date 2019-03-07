using System;
using System.IO;

namespace theorbo.Services
{
    public static class PathService
    {
        static PathService()
        {
            AppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "theorbo");
        }

        public static string AppData { get; }

        public static void EnsurePathExists()
        {
            if (!Directory.Exists(AppData))
                Directory.CreateDirectory(AppData);
        }
    }
}