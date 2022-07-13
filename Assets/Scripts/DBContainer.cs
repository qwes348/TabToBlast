using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBContainer : MonoBehaviour
{
    public static DBContainer instance;

    public SpriteDB spriteDB;

    private void Awake()
    {
        if (instance != null)
            DestroyImmediate(gameObject);

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
