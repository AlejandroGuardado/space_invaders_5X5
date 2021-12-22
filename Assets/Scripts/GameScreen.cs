using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameScreen : MonoBehaviour{
    public abstract IEnumerator OnEnter();
    public abstract void OnUpdate();
    public abstract IEnumerator OnExit();
    public abstract float GetExitTime();
}
