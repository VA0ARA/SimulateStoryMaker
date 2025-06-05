using StoryMaker.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoryMaker.Models.Actions;
using StoryMaker.Helpers;


namespace StoryMaker.Models.Builder
{
    public class ProvideActionFromList : IActionProvider
    {
        static List<Type> _actionTypes = new List<Type>()
        {
            typeof(ChangeImageAction),
            typeof(ExitSceneAction),
            typeof(PlayAnimationAction),
            typeof(PlaySoundAction),
            typeof(SetVariableAction),
            typeof(SetVisibilityAction),
            //typeof(TransformAction)
        };

        TextMap.ActionTitleProvider _actionTitleProvider = new TextMap.ActionTitleProvider();
        IActionBuilder _actionBuilder;

        public ProvideActionFromList(IActionBuilder actionBuilder)
        {
            _actionBuilder = actionBuilder;
        }
        public async Task<IAction> GetAction()
        {
            var actionTitles = _actionTypes.Select(a => _actionTitleProvider.GetTitle(a));
            var chooseFromList = new ChooseFromList(actionTitles);
            if(chooseFromList.ShowDialog()==true)
            {
                return CreateAction(chooseFromList.SelectedIndex);
            }

            return null;
        }

        IAction CreateAction(int index)
        {
            switch(index)
            {
                case 0:
                    return _actionBuilder.Create<ChangeImageAction>();

                case 1:
                    return _actionBuilder.Create<ExitSceneAction>();

                case 2:
                    return _actionBuilder.Create<PlayAnimationAction>();

                case 3:
                    return _actionBuilder.Create<PlaySoundAction>();

                case 4:
                    return _actionBuilder.Create<SetVariableAction>();

                case 5:
                    return _actionBuilder.Create<SetVisibilityAction>();
            }

            return null;
        }
    }
}
