using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryMaker.Models.Interfaces
{
    public interface IScenePlayer
    {
        void Play(SceneModel scene);
        void Pause();
        void Resume();
        void Stop();
    }
}
