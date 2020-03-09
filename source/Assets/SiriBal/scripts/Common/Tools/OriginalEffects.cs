using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OriginalEffects : MonoBehaviour
{
    // Common 
    private float TimeCounter{get; set;}
    // -------

    // Fade in/out effect
    private bool isFadeIn;
    private float fadeTime;
    // --------


    void Start()
    {
        TimeCounter = 0;
        // Initialize Values
        isFadeIn = false;
    }
    void Update()
    {
        TimeCounter += Time.deltaTime;
        if (isFadeIn)
        {
            FadeProcess();
        }
    }


    // FadeIn effect setup (sec)
    public void SetUpFadeIn(float fade = 2)
    {
        isFadeIn = true;
        fadeTime = fade;
        TimeCounter = 0;
    }

    // FadeIn Effect Runner
    private void FadeProcess()
    {
        if (TimeCounter > fadeTime * 3)
        {
            isFadeIn = false;
            return;
        }
        var ratio = TimeCounter / fadeTime;
        var alpha = 0f;
        if(ratio < 1)
        {
            alpha = ratio;
        }
        else if (ratio >= 1 && ratio < 2)
        {
            alpha = 2 - ratio;
        }
        else
        {
            alpha = 0;
        }

        var color = this.gameObject.GetComponent<Text>().color;
        this.gameObject.GetComponent<Text>().color = new Color(color.r, color.g, color.b, alpha); 
    }

}
