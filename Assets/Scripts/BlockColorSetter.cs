using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class BlockColorSetter : MonoBehaviour
{
    public int color;

    public List<Sprite> sprites;
    public SpriteRenderer spriteRend;
    public bool randomColorOnAwake = false;

    public Action<int> onColorChanged;

    public int ColorCount { get => sprites.Count; }

    private void OnEnable()
    {
        if (randomColorOnAwake)
            RandomizeColor();
    }

    public void SetColor(int colorIndex)
    {
        spriteRend.sprite = sprites[Mathf.Clamp(colorIndex, 0, sprites.Count - 1)];
        color = colorIndex;
        onColorChanged?.Invoke(color);
    }

    public void RandomizeColor()
    {
        color = Random.Range(0, sprites.Count);
        SetColor(color);
    }
}
