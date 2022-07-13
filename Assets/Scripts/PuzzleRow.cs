using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleRow : MonoBehaviour
{
    [SerializeField]
    private int _myColumnCount;
    public int MyColumnCount { get => _myColumnCount; }

    public Vars.AlignmentType myAlignmentType;

    public PuzzleBlock blockPrefab;    

    public void Init()
    {
        for (int i = 0; i < MyColumnCount; i++)
        {

        }
    }
}
