using StoryMaker.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoryMaker.Models.Actions;

namespace StoryMaker.Models.Builder
{
    public class ActionBuilder : IActionBuilder
    {
        public IAction Create<T>() where T : IAction
        {
            if (typeof(T) == typeof(ChangeImageAction))
                return new ChangeImageAction() { Duration = 30};

            if (typeof(T) == typeof(ExitSceneAction))
                return new ExitSceneAction();

            if (typeof(T) == typeof(PlayAnimationAction))
                return new PlayAnimationAction();

            if (typeof(T) == typeof(PlaySoundAction))
                return new PlaySoundAction();

            if (typeof(T) == typeof(SetVariableAction))
                return new SetVariableAction();

            if (typeof(T) == typeof(SetVisibilityAction))
                return new SetVisibilityAction() { Duration = 30 };

            throw new Exception("No more action types available!");
        }
    }
}
