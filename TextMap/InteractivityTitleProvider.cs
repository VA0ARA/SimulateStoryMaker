using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoryMaker.Models.Components.Interactivities;

namespace StoryMaker.TextMap
{
    public class InteractivityTitleProvider : ITitleProvider
    {
        static Dictionary<Type, string> _titles = new Dictionary<Type, string>()
        {
            {typeof(Tap),"ضربه زدن - Tap" },
            {typeof(Drag),"کشیدن - Drag" },
            {typeof(CompareValues),"مقایسه - Compare" },
            {typeof(DropCondition),"افتادن - Drop" }
        };
        public string GetTitle(Type type)
        {
            return _titles.ContainsKey(type)?_titles[type]:"??????????????????????";
        }
    }
}
