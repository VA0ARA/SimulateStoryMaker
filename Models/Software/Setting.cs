using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace StoryMaker.Models.Software
{
    public class Setting
    {
        public List<ISetting> SettingsList { get; set; } = new List<ISetting>()
        {
            new SaveSetting()
        };
    }
}