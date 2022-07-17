#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Linq;

[CustomEditor(typeof(LevelData))]
public class LevelDataEditor : Editor
{
    private LevelData level;   
    private LevelBlock brush;   // 팔레트에서 클릭한 블록을 담고있을 브러쉬    
    private List<LevelBlock> prefabs;   // 팔레트에 들어갈 블록들
    private int squareSize = 40;

    public override VisualElement CreateInspectorGUI()
    {
        InitPalette();

        level = target as LevelData;
        // 블록 에디터가 전부 비어있다면 초기화 시켜줌(이미 세팅돼있는걸 날리지 않기위해 확인)
        if(level.blocks.All(i => i == null || i.prefab == null))
            InitBlocks(level.size.x * level.size.y);
        return base.CreateInspectorGUI();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUIBlockPalette();
        GUIBlockEditor();
        GUIBottom();
    }

    private void InitPalette()
    {
        prefabs = new List<LevelBlock>();
        var puzzleBlock = Resources.Load<GameObject>("Blocks/PuzzleBlock");
        int colors = puzzleBlock.GetComponentInChildren<BlockColorSetter>().ColorCount;

        for (int i = 0; i < colors; i++)
        {
            var block = new LevelBlock(Vars.BlockType.PuzzleBlock, puzzleBlock, Vector2Int.one, i);
            prefabs.Add(block);
        }
    }

    private void GUIBlockPalette()
    {
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();

        int i;
        for (i = 0; i < prefabs.Count; i++)
        {
            var lvBlock = prefabs[i];
            if(brush != null && brush.prefab == lvBlock.prefab && brush.color == lvBlock.color)
            {
                GUI.backgroundColor = Color.gray;
            }

            if(GUILayout.Button(new GUIContent("", lvBlock.prefab.name), GUILayout.Width(squareSize), GUILayout.Height(squareSize)))
            {
                brush = lvBlock;
                InitPalette();
            }

            var lastRect = GUILayoutUtility.GetLastRect();
            GUI.DrawTexture(lastRect, SpriteToTexture2D(lvBlock.Img));
            GUI.backgroundColor = Color.white;
        }

        if (brush != null && brush.type == Vars.BlockType.None && brush.prefab == null)
            GUI.backgroundColor = Color.gray;

        if(GUILayout.Button("", GUILayout.Width(squareSize), GUILayout.Height(squareSize)))
        {
            var emptyBlock = new LevelBlock(Vars.BlockType.None, null, Vector2Int.zero);
            brush = emptyBlock;
            InitPalette();
        }

        GUI.backgroundColor = Color.white;

        GUILayout.EndHorizontal();
    }

    private void GUIBlockEditor()
    {
        GUILayout.Space(30);
        EditorGUI.BeginChangeCheck();
        var sizeX = level.size.x;
        var sizeY = level.size.y;

        for (int y = 0; y < sizeY; y++)
        {
            GUILayout.BeginHorizontal();
            {
                for (int x = 0; x < sizeX; x++)
                {
                    var size = level.size.x * level.size.y;
                    if (level.blocks.Length != size)
                        InitBlocks(size);
                    
                    if(GUILayout.Button("", GUILayout.Width(squareSize), GUILayout.Height(squareSize)))
                    {
                        if(brush != null)
                        {                            
                            level.blocks[y * level.size.x + x] = brush;
                        }                                                
                    }

                    if(level.blocks[y * level.size.x + x].Img != null)
                    {
                        var lastRect = GUILayoutUtility.GetLastRect();
                        GUI.DrawTexture(lastRect, SpriteToTexture2D(level.blocks[y * level.size.x + x].Img));
                    }
                }

                GUI.backgroundColor = Color.white;
            }            
            GUILayout.EndHorizontal();
        }

        if (EditorGUI.EndChangeCheck())
            Save();
    }

    private void GUIBottom()
    {
        if(GUILayout.Button("Clear", GUILayout.Width(squareSize * 2)))
        {
            InitBlocks(level.size.x * level.size.y);
            InitPalette();
        }

        EditorGUILayout.Separator();        
    }

    private Texture2D SpriteToTexture2D(Sprite sprite)
    {
        int x = Mathf.FloorToInt(sprite.textureRect.x);
        int y = Mathf.FloorToInt(sprite.textureRect.y);
        int width = Mathf.FloorToInt(sprite.textureRect.width);
        int height = Mathf.FloorToInt(sprite.textureRect.height);

        Texture2D newTex = new Texture2D(width, height);
        Color[] newColors = sprite.texture.GetPixels(x, y, width, height);

        newTex.SetPixels(newColors);
        newTex.Apply();
        return newTex;
    }

    private void InitBlocks(int size)
    {
        level.blocks = new LevelBlock[size];

        for (int i = 0; i < size; i++)
        {
            level.blocks[i] = null;
        }

        Save();
    }

    private void Save()
    {
        EditorUtility.SetDirty(level);
    }
}
#endif
