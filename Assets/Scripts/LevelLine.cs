using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelLine{
    public List<float> xPositions;
    public int NumberSpots { get { return xPositions.Count; } }

    public LevelLine() {
        xPositions = new List<float>();
    }
}