using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFader : MonoBehaviour
{
    public enum fadeDirection 
    {
        IN, //alpha to 1
        OUT//alpha to 0
    }

    public Image imageToFade;
    public float fadeSpeed = 0.8f;
    [HideInInspector]
    public bool fadeDone = false;

    void Start()
    {
        
    }

    IEnumerator Fade(fadeDirection dir)
    {
        fadeDone = false;
        float alpha = (dir == fadeDirection.OUT) ? 1 : 0;
        float fadeEndValue = 0;
        if (dir == fadeDirection.IN) fadeEndValue = 1;

        if (dir == fadeDirection.OUT)
        {
            while (alpha >= fadeEndValue)
            {

                yield return null;
            }
            fadeDone = true;
        }

        if (dir == fadeDirection.IN)
        {
            while(alpha <= fadeEndValue)
            {

                yield return null;
            }
            fadeDone = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
