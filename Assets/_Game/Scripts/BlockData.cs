using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BlockData
{
    public Vector3 position;
    public Color color;

    public BlockData(Vector3 position, Color color)
    {
        this.position = position;
        this.color = color;
    }
}

[Serializable]
public class GameState
{
    public int[] blockQuantities;
    public List<BlockData> blockList;

    public GameState(int[] blockQuantities, List<BlockData> blockList)
    {
        this.blockQuantities = blockQuantities;
        this.blockList = blockList;
    }
}
