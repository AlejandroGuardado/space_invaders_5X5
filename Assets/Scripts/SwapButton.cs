using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapButton : MonoBehaviour{
    public UnityEngine.UI.Image image;
    public Sprite holdSprite;
    public Sprite releaseSprite;

    public void OnHold() {
        if(image.sprite != holdSprite) {
            image.sprite = holdSprite;
        } 
    }

    public void OnRelease() {
        if (image.sprite != releaseSprite) {
            image.sprite = releaseSprite;
        }
    }
}