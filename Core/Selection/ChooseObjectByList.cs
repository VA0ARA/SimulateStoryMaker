using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoryMaker.Helpers;
using StoryMaker.Models.Components;
using StoryMaker.Models;

namespace StoryMaker.Core.Selection
{
    public class ChooseObjectByList<T>:IObjectChooser<T> where T:Component
    {
        public async Task<IEnumerable<T>> Choose()
        {
            var objects = StoryObject.GetObjectsOfType<T>();
            var names = objects.Select(o => o.StoryObject.Name);
            var chooseDialogue=new ChooseFromList(names);
            if(chooseDialogue.ShowDialog()==true)
                return new T[]{objects.ElementAt(chooseDialogue.SelectedIndex)};

            return new T[0];
        }
    }
}