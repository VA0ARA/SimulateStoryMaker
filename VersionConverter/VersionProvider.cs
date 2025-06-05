using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace StoryMaker.VersionConverter
{
    public class VersionProvider : IVersionProvider
    {
        public string GetVersion(string path)
        {
            DirectoryInfo directory = new DirectoryInfo(path);

            FileInfo[] oldVersions = directory.GetFiles("*.inf");
            if (oldVersions.Length == 1)
                return "ورژنهای قدیمی پویا نو!";

            FileInfo[] versions= directory.GetFiles("*.txt");
            return versions.Length == 1 ? File.ReadAllText(versions[0].FullName) : "ورژن ناشناخته!";
        }
    }
}
