using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;

public class PuzzleBlock : MonoBehaviour
{
    public bool debugLog;
    public BlockColorSetter myColorSetter;
    public Vars.BlockType blockType;

    [ShowNativeProperty]
    public int MyIndex { get; private set; }
    public PuzzleMap MyMap { get; private set; }

    [ShowNativeProperty]
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

    [ShowNativeProperty]
    public PuzzleBlock RightBlock
    {
        get
        {
            if (MyMap == null || MyMap.CurrentLevel == null)
                return null;
            if (MyIndex % MyMap.CurrentLevel.size.x == MyMap.CurrentLevel.size.x - 1)
                return null;

            return MyMap.SpawnedBlocks[MyIndex + 1];
        }
    }

    [ShowNativeProperty]
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

    [ShowNativeProperty]
    public PuzzleBlock DownBlock
    {
        get
        {
            if (MyMap == null || MyMap.CurrentLevel == null)
                return null;
            if (IsBottomHorizontalLine)
                return null;

            return MyMap.SpawnedBlocks[MyIndex + MyMap.CurrentLevel.size.x];
        }
    }

    public bool IsBottomHorizontalLine { 
        get
        {
            return MyIndex / MyMap.CurrentLevel.size.x == (MyMap.CurrentLevel.size.x - 1);
        }
    }

    public void Init(int index, PuzzleMap map)
    {
        MyIndex = index;
        MyMap = map;
    }

    public void OnTouched()
    {
        if(debugLog)
            Debug.LogFormat("L:{0}, R{1}, U{2}, D{3}", LeftBlock != null ? LeftBlock.name : "null", RightBlock != null ? RightBlock.name : "null", 
            UpBlock != null ? UpBlock.name : "null", DownBlock != null ? DownBlock.name : "null");

        MyMap.OnBlockTouched(this);
    }

    public List<PuzzleBlock> GetMatchedNeighborBlocks()
    {
        MyMap.recursionCallList.Add(this);

        int myColorIndex = GetColorIndex();

        if (myColorIndex < 0)
            return null;

        List<PuzzleBlock> matchedNeighborBlocks = new List<PuzzleBlock>();
        List<PuzzleBlock> allNeighborBlocks = new List<PuzzleBlock>
        {
            UpBlock,
            LeftBlock,
            RightBlock,
            DownBlock
        };

        foreach(var neighbor in allNeighborBlocks)
        {
            if (neighbor == null || neighbor.GetColorIndex() < 0)
                continue;
            if (MyMap.recursionCallList.Contains(neighbor))
                continue;

            if (neighbor.GetColorIndex() == myColorIndex)
            {
                matchedNeighborBlocks.Add(neighbor);

                var thisLineMatchedBlocks = neighbor.GetMatchedNeighborBlocks();
                if (thisLineMatchedBlocks != null)
                    matchedNeighborBlocks.AddRange(thisLineMatchedBlocks);
            }
        }

        return matchedNeighborBlocks;
    }

    public void Pang()
    {
        MyMap.SpawnedBlocks[MyIndex] = null;
        Destroy(gameObject);
    }

    public int GetColorIndex() => myColorSetter == null ? -1 : myColorSetter.color;

    public void FallBlock()
    {
        if (IsBottomHorizontalLine)
            return;

        int prevMyIndex;

        while (DownBlock == null && !IsBottomHorizontalLine)
        {
            prevMyIndex = MyIndex;
            MyIndex += MyMap.CurrentLevel.size.x;

            MyMap.SpawnedBlocks[prevMyIndex] = null;
            MyMap.SpawnedBlocks[MyIndex] = this;
        }

        //transform.localPosition = MyMap.GetNewBlockPos(MyIndex);
        transform.DOLocalMove(MyMap.GetNewBlockPos(MyIndex), 0.25f);
    }
}
