using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using StoryMaker.Backend.Software;
using StoryMaker.Models.Software;
using StructureISetting = StoryMaker.Backend.Software.ISetting;
using ModelISetting = StoryMaker.Models.Software.ISetting;
using SaveSetting = StoryMaker.Models.Software.SaveSetting;

namespace StoryMaker.Core.Save_Load
{
    public static class SoftwareData
    {
        public static SoftwareStructure GetData(this SoftwareModel software)
        {
            var settings = new List<StructureISetting>();

            foreach (var setting in software.Setting.SettingsList)
            {
                settings.Add(setting.GetData());
            }

            var structure = new SoftwareStructure
            {
                RecentProjects = software.RecentProject
                    .Select(rp => new KeyValuePair<KeyValuePair<string, string>, DateTime>(rp.Key, rp.Value))
                    .OrderByDescending(s => s.Value.TimeOfDay.TotalSeconds).ToList(),
                    Setting = new Backend.Software.Setting()
                    {
                        SettingsList = settings
                    }
            };

            return structure;
        }

        public static StructureISetting GetData(this ModelISetting setting)
        {
            switch (setting)
            {
                case SaveSetting saveSetting:
                    return new Backend.Software.SaveSetting()
                    {
                        AutoSave = saveSetting.AutoSave,
                        AutoSaveTimer = saveSetting.AutoSaveTimer
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(setting));
            }
        }

        public static SoftwareModel Create(this SoftwareStructure structure)
        {
            var settings = new List<ModelISetting>();

            foreach (var setting in structure.Setting.SettingsList)
            {
                settings.Add(setting.Create());
            }
            var software = new SoftwareModel
            {
                RecentProject =
                    new ObservableCollection<KeyValuePair<KeyValuePair<string, string>, DateTime>>(structure
                        .RecentProjects
                        .Select(s => new KeyValuePair<KeyValuePair<string, string>, DateTime>(s.Key, s.Value))
                        .OrderByDescending(s => s.Value.TimeOfDay.TotalSeconds)),
                Setting = new Models.Software.Setting()
                {
                    SettingsList = settings
                }
            };

            return software;
        }

        public static ModelISetting Create(this StructureISetting setting)
        {
            switch (setting)
            {
                case Backend.Software.SaveSetting saveSetting:
                    return new SaveSetting()
                    {
                        AutoSave = saveSetting.AutoSave,
                        AutoSaveTimer = saveSetting.AutoSaveTimer
                    };
                default:
                    throw new ArgumentOutOfRangeException(nameof(setting));
            }
        }

        public static void Save(this SoftwareModel software, string path)
        {
            var structure = software.GetData();

            //write info & project files
            new Helpers.BinaryStream<SoftwareStructure>().Write($"{path}/Recents.inf", structure);
        }

        public static SoftwareModel Load(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var dirInfo = new DirectoryInfo(path);

            //if path does not exist create new one
            if (!dirInfo.Exists)
                dirInfo.Create();


            if (dirInfo.GetFiles("*.inf").Length != 1) return new SoftwareModel();

            var infoFile = dirInfo.GetFiles("*.inf")[0];

            if (!File.Exists(infoFile.FullName)) return new SoftwareModel();
            try
            {
                var f = File.OpenRead(infoFile.FullName);
                var b = new BinaryFormatter();
                var softwareData = (SoftwareStructure)b.Deserialize(f);
                f.Close();
                return softwareData.Create();
            }
            catch
            {
                return new SoftwareModel();
            }
        }
    }
}