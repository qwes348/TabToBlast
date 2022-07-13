using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "SpriteDB", menuName = "ScriptableObject / SpriteDB")]
public class SpriteDB : ScriptableObject
{
    [ShowAssetPreview]
    public Sprite block_red;
    [ShowAssetPreview]
    public Sprite block_blue;
    [ShowAssetPreview]
    public Sprite block_green;
    [ShowAssetPreview]
    public Sprite block_yellow;
}
