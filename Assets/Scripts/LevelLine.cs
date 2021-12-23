using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelLine{
    public int numberEnemies;
    public LevelLineSpacing spacing;
    public float marginLeft;
    public float marginRight;
}

public enum LevelLineSpacing {
    Start,
    End,
    Center,
    SpaceBetween,
    SpaceAround
}