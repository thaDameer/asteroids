using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class SpriteHelperClass
{


    public static async void Flash(SpriteRenderer spriteRenderer, Color color, float flashDuration = 0.05f,
        int flashCount = 3, Action onCompleteCallback = null)
    {

        if (spriteRenderer == null)
        {
            Debug.LogWarning("SpriteRenderer is null.");
            return;
        }

        Color originalColor = spriteRenderer.color;

        for (int i = 0; i < flashCount; i++)
        {
            if (spriteRenderer)
                spriteRenderer.color = color;
            await Task.Delay(Mathf.RoundToInt(flashDuration * 1000) / 2);
            if (spriteRenderer)
                spriteRenderer.color = originalColor;
            await Task.Delay(Mathf.RoundToInt(flashDuration * 1000) / 2);
        }
        onCompleteCallback?.Invoke();
    }

}
