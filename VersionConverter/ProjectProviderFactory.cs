using System;
using System.IO;
using StoryMaker.Injectors;

namespace StoryMaker.VersionConverter
{
    public class ProjectProviderFactory
    {
        public IProjectProvider GetProvider(string path)
        {
            string versionPath = path + "/version.txt";
            // if (!File.Exists(versionPath))
            //     return new OldVersionToversion1Converter.OldProjectProvider()
            //     {
            //         UniqueAnimator = true
            //     };

            string version = File.ReadAllText(versionPath);

            if (version.Contains("0.0.1"))
                return MainInjector.Singleton.Resolve<ProjectProvider>();

            throw new Exception("There's not appropriate version of project provider!");
        }
    }
}
