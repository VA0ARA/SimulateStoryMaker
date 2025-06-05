using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoryMaker.Models.Components;

namespace StoryMaker.Models.Builder
{
    public class CameraBuilder
    {
        public Camera Build()
        {
            var camera = new StoryObject()
            {
                Name = "Camera",
            };

            var cameraComponent = camera.AddComponent<Camera>();

            return cameraComponent;
        }
    }
}
