using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Animation;
using StoryMaker.Editor.Utility;
using StoryMaker.Helpers;
using StoryMaker.Models;
using StoryMaker.Models.AssetModels;
using StoryMaker.Models.Components;
using Timeline.Model;
using Transform = StoryMaker.Models.Components.Transform;

namespace StoryMaker.Core.Save_Load
{
    public static class AnimationVersion1
    {
        public static AnimationPack Create(string path)
        {
            if (Directory.Exists(Paths.DocumentTemp))
                Directory.Delete(Paths.DocumentTemp, true);
            Directory.CreateDirectory(Paths.DocumentTemp);

            ZipFile.ExtractToDirectory(path, Paths.DocumentTemp);
            var dirInfo = new DirectoryInfo(Paths.DocumentTemp);
            var anim = dirInfo.GetFiles().FirstOrDefault(f => f.FullName.EndsWith(".anim"));
            if (anim == null) return null;
            try
            {
                return new BinaryStream<AnimationPack>().Read(anim.FullName);
            }
            catch
            {
                return null;
            }
        }

        public static List<string> GetClips(this AnimationPack pack)
        {
            return pack.Clips.Select(c => c.Name).ToList();
        }


        public static List<StoryObject> Load(this AnimationPack pack, string name)
        {
            #region Import Assets

            foreach (var asset in pack.Assets)
            {
                asset.ReplacePath(Paths.DocumentTemp);
            }

            AssetsController.AddAsset(Paths.ProjectPath, name, pack.Assets.Select(i => i.Path).ToList(),
                AssetTypes.Character);

            AssetsController.RefreshFolders(Paths.ProjectPath);

            var folder = AssetsController.CharacterFolders.FirstOrDefault(f => f.Name.ToLower() == name.ToLower());
            folder?.RefreshFolder();

            foreach (var asset in pack.Assets)
            {
                var a = folder?.Assets.FirstOrDefault(i =>
                    Path.GetFileNameWithoutExtension(i.Name) == Path.GetFileNameWithoutExtension(asset.Path));
                if (string.IsNullOrEmpty(a.FilePath)) continue;
                asset.ReplacePath(folder?.Path);
            }

            #endregion

            var storyObjects = new List<StoryObject>();
            var dic = new Dictionary<Element, StoryObject>();
            var dicId = new Dictionary<int, StoryObject>();

            foreach (var clip in pack.Clips)
            {
                foreach (var element in clip.Elements)
                {
                    if (dicId.ContainsKey(element.ID))
                    {
                        dic.Add(element, dicId[element.ID]);
                        continue;
                    }

                    var sO = new StoryObject {Name = element.Name};
                    //sO.UpdateStates(true);

                    storyObjects.Add(sO);
                    dicId.Add(element.ID, sO);
                    dic.Add(element, sO);
                }
            }

            foreach (var storyObject in dicId)
            {
                var e = dic.Keys.First(el => el.ID == storyObject.Key);
                if (e.Parent != null)
                {
                    storyObject.Value.transform.SetParent(dic[e.Parent].transform);
                }
            }

            var rootObject = storyObjects.Single(s => s.transform.Parent == null);
            var animator = rootObject.AddComponent<Animator>();
            var addressDic = AnimationUtility.CollectStoryObjects(animator)
                .ToDictionary(c => c.Key, c => c.Value);

            foreach (var clip in pack.Clips)
            {
                var c = new Clip(clip.Name);
                animator.AnimatorController.AddClip(c);

                foreach (var element in clip.Elements)
                {
                    var sObj = dic[element];
                    var objAddress = addressDic[sObj];

                    // var transformRecordData = new RecordData(objAddress, typeof(Transform));
                    // c.Channels.Add(transformRecordData);

                    #region Transform Keys

                    sObj.transform.LocalPosition = element.Transform.Position.GetVector2();
                    sObj.transform.LocalScale = element.Transform.Scale.GetVector2();
                    sObj.transform.Angle = element.Transform.Rotation;

                    //-----------------------
                    if (element.Transform.Positions.Count > 0)
                    {
                        var localPosChannel = new Channel(objAddress, nameof(sObj.transform.LocalPosition),
                            typeof(Transform));

                        foreach (var keyFrame in element.Transform.Positions.KeyFrames)
                        {
                            localPosChannel.Add((int) (keyFrame.Key * clip.FPS), keyFrame.Value.GetVector2());
                        }


                        c.Channels.Add(localPosChannel);
                    }

                    //------------------------
                    if (element.Transform.Scales.Count > 0)
                    {
                        var localScaleChannel = new Channel(objAddress, nameof(sObj.transform.LocalScale),
                            typeof(Transform));

                        foreach (var keyFrame in element.Transform.Scales.KeyFrames)
                        {
                            localScaleChannel.Add((int) (keyFrame.Key * clip.FPS), keyFrame.Value.GetVector2());
                        }

                        c.Channels.Add(localScaleChannel);
                    }

                    //------------------------
                    if (element.Transform.Rotations.Count > 0)
                    {
                        var localAngleChannel = new Channel(objAddress, nameof(sObj.transform.LocalAngle),
                            typeof(Transform));

                        foreach (var keyFrame in element.Transform.Rotations.KeyFrames)
                        {
                            localAngleChannel.Add((int) (keyFrame.Key * clip.FPS), keyFrame.Value);
                        }

                        c.Channels.Add(localAngleChannel);
                    }

                    #endregion

                    var imageComponent = sObj.GetComponent<ImageRenderer>();

                    if (element?.Renderer?.Image != null)
                    {
                        if (imageComponent == null)
                            imageComponent = sObj.AddComponent<ImageRenderer>();

                        var asset = AssetsController.GetAsset<DrawableAssetModel>(Paths.ProjectPath,
                            element.Renderer.Image.Path);

                        imageComponent.CurrentAsset = asset;

                        imageComponent.Visibility = element.Renderer.Enabled;

                        imageComponent.FlipX = element.Renderer.FlipX;

                        imageComponent.FlipY = element.Renderer.FlipY;

                        imageComponent.SortOrder = element.Renderer.Order;

                        imageComponent.Pivot = element.Transform.Pivot.GetVector2();
                    }

                    if (element?.Renderer?.ImageChannel.Count > 0)
                    {
                        if (imageComponent == null)
                            imageComponent = sObj.AddComponent<ImageRenderer>();

                        var assetChannel = new Channel(objAddress, nameof(imageComponent.CurrentAsset),
                            typeof(ImageRenderer));

                        foreach (var keyFrame in element.Renderer.ImageChannel.KeyFrames)
                        {
                            var img = AssetsController.GetAsset<DrawableAssetModel>(Paths.ProjectPath,
                                keyFrame.Value.Path);

                            assetChannel.Add((int) (keyFrame.Key * clip.FPS), img);
                        }

                        c.Channels.Add(assetChannel);
                    }

                    //-------------------------
                    if (imageComponent != null && element.Transform.PivotChannel.Count > 0)
                    {
                        var localPivotChannel = new Channel(objAddress, nameof(imageComponent.Pivot),
                            typeof(Transform));

                        foreach (var keyFrame in element.Transform.PivotChannel.KeyFrames)
                        {
                            localPivotChannel.Add((int)(keyFrame.Key * clip.FPS), keyFrame.Value.GetVector2());
                        }

                        c.Channels.Add(localPivotChannel);
                    }

                    if (element?.Renderer?.EnableChannel.Count > 0)
                    {
                        if (imageComponent == null)
                            imageComponent = sObj.AddComponent<ImageRenderer>();

                        var visibilityChannel = new Channel(objAddress, nameof(imageComponent.Visibility),
                            typeof(ImageRenderer));

                        foreach (var keyFrame in element.Renderer.EnableChannel.KeyFrames)
                        {
                            visibilityChannel.Add((int) (keyFrame.Key * clip.FPS), keyFrame.Value);
                        }

                        c.Channels.Add(visibilityChannel);
                    }

                    if (element?.Renderer?.FlipXChannel.Count > 0)
                    {
                        if (imageComponent == null)
                            imageComponent = sObj.AddComponent<ImageRenderer>();

                        var flipXChannel = new Channel(objAddress, nameof(imageComponent.FlipX), typeof(ImageRenderer));

                        foreach (var keyFrame in element.Renderer.FlipXChannel.KeyFrames)
                        {
                            flipXChannel.Add((int)(keyFrame.Key * clip.FPS), keyFrame.Value);
                        }

                        c.Channels.Add(flipXChannel);
                    }

                    if (element?.Renderer?.FlipYChannel.Count > 0)
                    {
                        if (imageComponent == null)
                            imageComponent = sObj.AddComponent<ImageRenderer>();

                        var flipYChannel = new Channel(objAddress, nameof(imageComponent.FlipY), typeof(ImageRenderer));

                        foreach (var keyFrame in element.Renderer.FlipYChannel.KeyFrames)
                        {
                            flipYChannel.Add((int)(keyFrame.Key * clip.FPS), keyFrame.Value);
                        }

                        c.Channels.Add(flipYChannel);
                    }

                    if (element?.Renderer?.OrderChannel.Count > 0)
                    {
                        if (imageComponent == null)
                            imageComponent = sObj.AddComponent<ImageRenderer>();

                        var orderChannel = new Channel(objAddress, nameof(imageComponent.SortOrder), typeof(ImageRenderer));

                        foreach (var keyFrame in element.Renderer.OrderChannel.KeyFrames)
                        {
                            orderChannel.Add((int) (keyFrame.Key * clip.FPS), keyFrame.Value);
                        }

                        c.Channels.Add(orderChannel);
                    }
                }
            }

            return storyObjects;
        }

        private static Vector2 GetVector2(this Animation.Point point)
        {
            return new Vector2(point.X, point.Y);
        }
    }
}