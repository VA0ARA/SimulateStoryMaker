using System.Collections;
using System.Collections.Generic;
using StoryMaker.DataStructure.Assets;
using UnityEngine;

public class SimpleAssetFactory : IAssetFactory
{
    public IAsset CreateAsset<T>(string path) where T : IAsset
    {
        if(typeof(T)==typeof(ImageAsset))
            return new ImageAsset(path);
        
        if(typeof(T)==typeof(SoundAsset))
            return new SoundAsset(path);

        return null;
    }
}
