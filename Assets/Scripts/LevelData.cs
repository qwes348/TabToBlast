using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObject/New LevelData")]
public class LevelData : ScriptableObject
{
    public Vector2Int size;
    public LevelBlock[] blocks = new LevelBlock[36];
}

[Serializable]
public class LevelBlock
{
    public Vars.BlockType type;
    public GameObject prefab;
    public int color;
    public Vector2Int size = Vector2Int.one;
    public Sprite Img { get; private set; }

    public LevelBlock(Vars.BlockType type, GameObject prefab, Vector2Int size, int col = 0)
    {
        this.type = type;
        this.prefab = prefab;
        this.color = col;
        this.size = size;

        BlockColorSetter colorSetter = null;

        if(prefab != null)
        {
            colorSetter = prefab.GetComponentInChildren<BlockColorSetter>();
        }

        if(colorSetter != null)
            Img = prefab.GetComponentInChildren<BlockColorSetter>().sprites[col];
    }
}
