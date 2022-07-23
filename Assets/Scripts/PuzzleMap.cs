using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PuzzleMap : MonoBehaviour
{
    public Transform blocksHolder;
    public LevelData testLevel;

    public LevelData CurrentLevel { get; private set; }
    public PuzzleBlock[] SpawnedBlocks { get; private set; }

    public List<PuzzleBlock> recursionCallList = new List<PuzzleBlock>();

    public float blockDistance = 0.75f;
    public float lineOffset = 2.6f;

    private void Start()
    {
        LoadLevel(testLevel);
    }

    public void LoadLevel(LevelData level)
    {
        CurrentLevel = level;
        SpawnedBlocks = new PuzzleBlock[level.size.x * level.size.y];
        PuzzleBlock blockPrefab = Resources.Load<PuzzleBlock>("Blocks/PuzzleBlock");

        for (int i = 0; i < level.blocks.Length; i++)
        {
            LevelBlock levelBlock = level.blocks[i];

            float x = (float)i % level.size.x - level.size.x / lineOffset;
            float y = i / level.size.x * -1 + level.size.y / lineOffset;
            var spawnedBlock = Instantiate(blockPrefab, blocksHolder);
            spawnedBlock.transform.localPosition = new Vector2(x * blockDistance, y * blockDistance);

            if (levelBlock.prefab != null)
                spawnedBlock.myColorSetter.SetColor(level.blocks[i].color);
            else
                spawnedBlock.myColorSetter.RandomizeColor();

            spawnedBlock.Init(i, this);

            //SpawnedBlocks.Add(spawnedBlock);
            SpawnedBlocks[i] = spawnedBlock;
        }

        Camera.main.orthographicSize = level.size.x - 0.5f;
    }

    public void OnBlockTouched(PuzzleBlock block)
    {
        if (block.myColorSetter == null)
            return;

        recursionCallList.Add(block);

        List<PuzzleBlock> pangBlocks = block.GetMatchedNeighborBlocks();
        pangBlocks.Add(block);
        recursionCallList.Clear();

        if(pangBlocks != null && pangBlocks.Count > 1)
        {
            foreach (var b in pangBlocks)
                b.Pang();

            FallBlocks();
            FillBlocks();
        }
    }

    private void FallBlocks()
    {
        for (int x = 0; x < CurrentLevel.size.x; x++)
        {
            for (int y = CurrentLevel.size.y - 1; y >= 0; y--)
            {
                var block = SpawnedBlocks[y * CurrentLevel.size.x + x];
                if (block == null)
                    continue;
                if (block.DownBlock != null)
                    continue;

                block.FallBlock();                
            }
        }
    }

    private void FillBlocks()
    {
        PuzzleBlock blockPrefab = Resources.Load<PuzzleBlock>("Blocks/PuzzleBlock");

        for (int x = 0; x < CurrentLevel.size.x; x++)
        {
            for (int y = CurrentLevel.size.y - 1; y >= 0; y--)
            {
                int index = y * CurrentLevel.size.x + x;
                if (SpawnedBlocks[index] != null)
                    continue;

                float posX = (float)index % CurrentLevel.size.x - CurrentLevel.size.x / lineOffset;
                float posY = index / CurrentLevel.size.x * -1 + CurrentLevel.size.y / lineOffset * 5f;
                var spawnedBlock = Instantiate(blockPrefab, blocksHolder);
                spawnedBlock.transform.localPosition = new Vector2(posX * blockDistance, posY * blockDistance);

                spawnedBlock.myColorSetter.RandomizeColor();
                spawnedBlock.Init(index, this);
                SpawnedBlocks[index] = spawnedBlock;

                spawnedBlock.transform.DOLocalMove(GetNewBlockPos(index), 0.25f);
            }
        }
    }

    public Vector2 GetNewBlockPos(int index)
    {
        float posX = (float)index % CurrentLevel.size.x - CurrentLevel.size.x / lineOffset;
        float posY = index / CurrentLevel.size.x * -1 + CurrentLevel.size.y / lineOffset;

        return new Vector2(posX * blockDistance, posY * blockDistance);
    }
}
