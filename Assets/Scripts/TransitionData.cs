using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TransitionData", menuName = "SpaceInvaders/TransitionData")]
public class TransitionData : ScriptableObject{
    public float splashScreenTransitionTime;
    public float splashScreenHoldTime;
    public float menuDelayTime;
    public float menuTransitionTime;
    public float creditsDelayTime;
    public float creditsTransitionTime;
    public float creditsScrollDelayTime;
    public float sessionDelayTime;
    public float sessionTransitionTime;
}
