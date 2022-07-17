using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleMap : MonoBehaviour
{
    public Transform blocksHolder;
    public LevelData testLevel;

    public LevelData CurrentLevel { get; private set; }
    public List<PuzzleBlock> SpawnedBlocks { get; private set; }

    private void Start()
    {
        LoadLevel(testLevel);
    }

    public void LoadLevel(LevelData level)
    {
        CurrentLevel = level;
        SpawnedBlocks = new List<PuzzleBlock>();
        PuzzleBlock blockPrefab = Resources.Load<PuzzleBlock>("Blocks/PuzzleBlock");

        for (int i = 0; i < level.blocks.Length; i++)
        {
            LevelBlock levelBlock = level.blocks[i];

            float x = (float)i % level.size.x - level.size.x / 2.6f;
            float y = i / level.size.x * -1 + level.size.y / 2.6f;
            var spawnedBlock = Instantiate(blockPrefab, blocksHolder);
            spawnedBlock.transform.localPosition = new Vector2(x * 0.75f, y * 0.75f);

            if (levelBlock.prefab != null)
                spawnedBlock.myColorSetter.SetColor(level.blocks[i].color);
            else
                spawnedBlock.myColorSetter.RandomizeColor();

            spawnedBlock.Init(i, this);

            SpawnedBlocks.Add(spawnedBlock);
        }

        Camera.main.orthographicSize = level.size.x - 0.5f;
    }
}
