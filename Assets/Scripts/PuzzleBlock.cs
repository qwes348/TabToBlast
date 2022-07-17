using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class PuzzleBlock : MonoBehaviour
{
    public BlockColorSetter myColorSetter;
    public Vars.BlockType blockType;

    [ShowNativeProperty]
    public int MyIndex { get; private set; }
    public PuzzleMap MyMap { get; private set; }

    public PuzzleBlock LeftBlock
    {
        get
        {
            if (MyMap == null || MyMap.CurrentLevel == null)
                return null;
            if (MyIndex % MyMap.CurrentLevel.size.x == 0)
                return null;
            if (MyIndex == 0)
                return null;

            return MyMap.SpawnedBlocks[MyIndex - 1];
        }
    }
    public PuzzleBlock RightBlock
    {
        get
        {
            if (MyMap == null || MyMap.CurrentLevel == null)
                return null;
            if (MyIndex % MyMap.CurrentLevel.size.x == 1)
                return null;

            return MyMap.SpawnedBlocks[MyIndex + 1];
        }
    }
    public PuzzleBlock UpBlock
    {
        get
        {
            if (MyMap == null || MyMap.CurrentLevel == null)
                return null;
            if (MyIndex / MyMap.CurrentLevel.size.x == 0)
                return null;

            return MyMap.SpawnedBlocks[MyIndex - MyMap.CurrentLevel.size.x];
        }
    }
    public PuzzleBlock DownBlock
    {
        get
        {
            if (MyMap == null || MyMap.CurrentLevel == null)
                return null;
            if (MyIndex / MyMap.CurrentLevel.size.x == (MyMap.CurrentLevel.size.x - 1))
                return null;

            return MyMap.SpawnedBlocks[MyIndex + MyMap.CurrentLevel.size.x];
        }
    }

    public void Init(int index, PuzzleMap map)
    {
        MyIndex = index;
        MyMap = map;
    }

    public void OnTouched()
    {
        Debug.LogFormat("L:{0}, R{1}, U{2}, D{3}", LeftBlock != null ? LeftBlock.name : "null", RightBlock != null ? RightBlock.name : "null", 
            UpBlock != null ? UpBlock.name : "null", DownBlock != null ? DownBlock.name : "null");
    }
}
