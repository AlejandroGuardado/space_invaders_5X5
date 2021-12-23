using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : GameEntity{
    public GameEntityDissolver dissolver;

    private void Awake() {
        Deactivate();
    }
}