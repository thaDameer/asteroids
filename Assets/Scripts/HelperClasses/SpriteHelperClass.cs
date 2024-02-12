using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class SpriteHelperClass 
{
    public static Color flashColor = Color.red;
    
    public static async void Flash(SpriteRenderer spriteRenderer,  float flashDuration = 0.05f, int flashCount = 3)
    {
        if (spriteRenderer == null)
        {
            Debug.LogWarning("SpriteRenderer is null.");
            return;
        }

        Color originalColor = spriteRenderer.color;

        for (int i = 0; i < flashCount; i++)
        {
            if(spriteRenderer)
                spriteRenderer.color = flashColor;
            await Task.Delay(Mathf.RoundToInt(flashDuration * 1000) / 2);
            if(spriteRenderer)
                spriteRenderer.color = originalColor;
            await Task.Delay(Mathf.RoundToInt(flashDuration * 1000) / 2);
        }
    }
    
}
