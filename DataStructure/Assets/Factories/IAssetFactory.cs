using System.Collections;
using System.Collections.Generic;
using StoryMaker.DataStructure.Assets;
using UnityEngine;

public interface IAssetFactory
{
    IAsset CreateAsset<T>(string path) where T:IAsset;
}
