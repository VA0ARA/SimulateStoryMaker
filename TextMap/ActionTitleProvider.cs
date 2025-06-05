using System;
using System.Collections.Generic;
using StoryMaker.Models;
using StoryMaker.Models.Actions;
using StoryMaker.DataStructure;

namespace StoryMaker.TextMap
{
    public class ActionTitleProvider:ITitleProvider
    {
        static Dictionary<Type, string> _actions = new Dictionary<Type, string>()
        {
            {
                typeof(ChangeImageAction),"عوض کردن تصویر"
            },
            {
                typeof(ExitSceneAction),"خروج از صحنه"
            },
            {
                typeof(PlaySoundAction),"پخش صوت"
            },
            {
                typeof(SetVariableAction),"مقداردهی متغیر"
            },
            {
                typeof(SetVisibilityAction),"نمایش / پنهان"
            },
            {
                typeof(PlayAnimationAction),"نمایش انیمیشن"
            }
        };

        public string GetTitle(Type type)
        {
            return _actions.ContainsKey(type) ? _actions[type] : "?????????????????";
        }
    }
}
