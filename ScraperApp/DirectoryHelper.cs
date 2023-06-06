using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ScraperApp;

internal static class DirectoryHelper
{
    internal static string GetProjectFolderPath()
    {
        string baseDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        return Directory.GetParent(baseDirectory).Parent.Parent.FullName;
    }

    internal static string GetJsonFolderPath()
    {
        return Path.Combine(GetProjectFolderPath(), "Json");
    }
}