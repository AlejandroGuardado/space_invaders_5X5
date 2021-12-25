using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillImage : MonoBehaviour{
    public UnityEngine.UI.Image image;
    public bool reverse;
    private float fillTimer;
    private float fillTime;
    private bool isFilling = false;

    private void Update() {
        if (isFilling) {
            fillTimer += Time.deltaTime;
            float fillAmount = Mathf.Clamp01(fillTimer / fillTime);
            if (reverse) {
                fillAmount = 1 - fillAmount;
            }
            image.fillAmount = fillAmount;
            if (fillTimer > fillTime) {
                fillTimer = 0f;
                isFilling = false;
            }
        }
    }

    public void Fill(float time) {
        if (isFilling || time <= 0f) return;
        fillTime = time;
        image.fillAmount = 0f;
        fillTimer = 0f;
        isFilling = true;
    }

    public void Clear() {
        isFilling = false;
        image.fillAmount = 0f;
        fillTimer = 0f;
        fillTime = 0f;
    }
}
